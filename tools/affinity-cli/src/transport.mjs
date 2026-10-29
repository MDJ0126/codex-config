import { CliError, fail } from './errors.mjs';

// Legacy HTTP+SSE is used by Affinity. Streamable HTTP is also supported for
// compatible future servers. No UI events, process launches or remote endpoints.
export function localUrl(value, base) {
  let url;
  try { url = new URL(value, base); } catch { fail('CONFIG', 'Invalid MCP endpoint'); }
  if (url.protocol !== 'http:' || !['localhost', '127.0.0.1', '[::1]'].includes(url.hostname)
      || url.username || url.password || url.hash) {
    fail('CONFIG', 'MCP endpoint must be an unauthenticated loopback HTTP URL');
  }
  return url;
}

export async function readSse(body, onEvent, maxBytes = 32 * 1024 * 1024) {
  const reader = body.getReader();
  const decoder = new TextDecoder();
  let buffer = '', data = [], event = '', bytes = 0;
  const line = async text => {
    if (!text) {
      if (data.length) await onEvent({ event: event || 'message', data: data.join('\n') });
      data = []; event = ''; bytes = 0;
    } else if (!text.startsWith(':')) {
      const i = text.indexOf(':');
      const field = i < 0 ? text : text.slice(0, i);
      let value = i < 0 ? '' : text.slice(i + 1);
      if (value.startsWith(' ')) value = value.slice(1);
      if (field === 'event') event = value;
      if (field === 'data') data.push(value);
    }
  };
  try {
    while (true) {
      const chunk = await reader.read();
      if (chunk.done) break;
      bytes += chunk.value.byteLength;
      if (bytes > maxBytes) fail('PROTOCOL', 'SSE event exceeds the size limit');
      buffer += decoder.decode(chunk.value, { stream: true });
      let match;
      while ((match = /\r\n|\r(?!$)|\n/.exec(buffer))) {
        const text = buffer.slice(0, match.index);
        buffer = buffer.slice(match.index + match[0].length);
        await line(text);
      }
    }
    buffer += decoder.decode();
    if (buffer.endsWith('\r')) buffer = buffer.slice(0, -1);
    if (buffer) await line(buffer);
    if (data.length) await line('');
  } finally { reader.releaseLock(); }
}

export class McpClient {
  constructor({ endpoint = 'http://localhost:6767/sse', timeout = 30000, transport = 'sse' } = {}) {
    this.url = localUrl(endpoint);
    if (!['sse', 'http'].includes(transport)) fail('CONFIG', 'Transport must be sse or http');
    if (!Number.isInteger(timeout) || timeout < 100 || timeout > 600000) fail('CONFIG', 'Timeout must be 100..600000 ms');
    this.timeout = timeout; this.transport = transport;
    this.controller = new AbortController(); this.pending = new Map(); this.nextId = 0;
    this.closed = false; this.session = null;
  }

  async fetch(url, init = {}) {
    const target = localUrl(url);
    if (target.origin !== this.url.origin) fail('PROTOCOL', 'Cross-origin MCP request refused');
    return fetch(target, { ...init, redirect: 'error', signal: init.signal ?? this.controller.signal });
  }

  headers() {
    return { 'Content-Type': 'application/json', Accept: 'application/json, text/event-stream',
      ...(this.session ? { 'Mcp-Session-Id': this.session } : {}),
      ...(this.protocol ? { 'MCP-Protocol-Version': this.protocol } : {}) };
  }

  async connect() {
    try {
      if (this.transport === 'sse') await this.openSse();
      const result = await this.request('initialize', {
        protocolVersion: '2025-11-25', capabilities: {},
        clientInfo: { name: 'codex-affinity-cli', version: '1.0.0' }
      });
      if (!result?.protocolVersion || !result?.serverInfo) fail('PROTOCOL', 'Invalid MCP initialize response');
      this.protocol = result.protocolVersion; this.info = result.serverInfo;
      await this.notify('notifications/initialized');
      return result;
    } catch (error) {
      await this.close();
      if (error instanceof CliError) throw error;
      throw new CliError('CONNECTION', 'Cannot connect to the local Affinity MCP server. Check that Affinity is running and MCP is enabled.', { reason: error.cause?.code ?? error.message });
    }
  }

  async openSse() {
    let resolveEndpoint, rejectEndpoint;
    const ready = new Promise((resolve, reject) => { resolveEndpoint = resolve; rejectEndpoint = reject; });
    const timer = setTimeout(() => {
      const error = new CliError('TIMEOUT', 'Timed out waiting for the MCP SSE endpoint');
      rejectEndpoint(error); this.controller.abort();
    }, this.timeout);
    // Start the reader before awaiting its first endpoint event.
    this.streamTask = (async () => {
      const response = await this.fetch(this.url, { headers: { Accept: 'text/event-stream' } });
      if (!response.ok || !response.headers.get('content-type')?.includes('text/event-stream')) {
        await response.body?.cancel();
        fail('CONNECTION', `Expected an SSE stream, received HTTP ${response.status}`);
      }
      await readSse(response.body, async message => {
        if (message.event === 'endpoint') {
          const endpoint = localUrl(message.data, this.url);
          if (endpoint.origin !== this.url.origin) fail('PROTOCOL', 'Cross-origin SSE endpoint refused');
          this.postUrl = endpoint; resolveEndpoint();
        } else if (message.event === 'message') {
          let parsed;
          try { parsed = JSON.parse(message.data); } catch { fail('PROTOCOL', 'Invalid JSON in MCP SSE message'); }
          await this.receive(parsed);
        }
      });
      if (!this.closed) fail('CONNECTION', 'MCP event stream ended');
    })().catch(error => {
      rejectEndpoint(error);
      this.rejectAll(error);
      this.streamError = error;
    });
    try { await ready; } finally { clearTimeout(timer); }
  }

  async receive(message) {
    if (message.jsonrpc !== '2.0') fail('PROTOCOL', 'Invalid JSON-RPC version');
    if (message.method) {
      // Do not open interactive sampling/elicitation dialogs. Reject requests.
      if (message.id !== undefined) await this.post({ jsonrpc: '2.0', id: message.id,
        error: { code: -32601, message: 'Client-side interactive requests are not supported' } });
      return;
    }
    const pending = this.pending.get(message.id);
    if (!pending) return;
    this.pending.delete(message.id); clearTimeout(pending.timer);
    if (message.error) pending.reject(new CliError('RPC', message.error.message || 'MCP request failed', { rpcCode: message.error.code }));
    else if (Object.hasOwn(message, 'result')) pending.resolve(message.result);
    else pending.reject(new CliError('PROTOCOL', 'JSON-RPC response has no result'));
  }

  async post(message) {
    const response = await this.fetch(this.transport === 'sse' ? this.postUrl : this.url, {
      method: 'POST', headers: this.headers(), body: JSON.stringify(message)
    });
    if (!response.ok) {
      await response.body?.cancel();
      fail('HTTP', `MCP returned HTTP ${response.status}`, { status: response.status });
    }
    const session = response.headers.get('mcp-session-id');
    if (session) this.session = session;
    if (response.status === 202 || response.status === 204) { await response.body?.cancel(); return; }
    const type = response.headers.get('content-type') || '';
    if (type.includes('application/json')) await this.receive(await response.json());
    else if (type.includes('text/event-stream')) await readSse(response.body, async event => {
      if (event.event === 'message') {
        try { await this.receive(JSON.parse(event.data)); } catch (error) {
          if (error instanceof SyntaxError) fail('PROTOCOL', 'Invalid JSON in MCP response');
          throw error;
        }
      }
    });
    else { await response.body?.cancel(); if (message.id !== undefined && this.transport === 'http') fail('PROTOCOL', 'MCP returned an unsupported content type'); }
  }

  request(method, params = {}) {
    if (this.closed || this.streamError) return Promise.reject(this.streamError ?? new CliError('CONNECTION', 'MCP client is closed'));
    const id = ++this.nextId;
    return new Promise((resolve, reject) => {
      const timer = setTimeout(() => {
        this.pending.delete(id);
        reject(new CliError('TIMEOUT', `MCP request timed out: ${method}`, {
          outcome: method === 'tools/call' ? 'unknown; do not blindly retry a modifying operation' : 'no response'
        }));
        // Abort outstanding HTTP readers too. No automatic retries of mutations.
        this.controller.abort();
      }, this.timeout);
      this.pending.set(id, { resolve, reject, timer });
      this.post({ jsonrpc: '2.0', id, method, params }).catch(error => {
        const entry = this.pending.get(id);
        if (entry) { this.pending.delete(id); clearTimeout(timer); reject(error); }
      });
    });
  }

  async notify(method, params) {
    const message = { jsonrpc: '2.0', method, ...(params === undefined ? {} : { params }) };
    // Notifications have no pending response, so bound their network wait.
    const timer = setTimeout(() => this.controller.abort(), this.timeout);
    try { await this.post(message); } finally { clearTimeout(timer); }
  }

  rejectAll(error) {
    for (const pending of this.pending.values()) { clearTimeout(pending.timer); pending.reject(error); }
    this.pending.clear();
  }

  async close() {
    this.closed = true; this.controller.abort();
    this.rejectAll(new CliError('CONNECTION', 'MCP connection closed'));
    await this.streamTask;
  }
}
