import { fail } from './errors.mjs';
import { validate } from './schema.mjs';

export class Affinity {
  constructor(client) { this.client = client; }
  async tools() {
    if (this.toolList) return this.toolList;
    const tools = [], seen = new Set();
    let cursor;
    do {
      const result = await this.client.request('tools/list', cursor ? { cursor } : {});
      if (!Array.isArray(result.tools)) fail('PROTOCOL', 'tools/list did not return an array');
      tools.push(...result.tools);
      cursor = result.nextCursor;
      if (cursor && seen.has(cursor)) fail('PROTOCOL', 'Repeated tools/list cursor');
      if (cursor) seen.add(cursor);
      if (tools.length > 1000 || seen.size > 100) fail('PROTOCOL', 'Too many MCP tools/pages');
    } while (cursor);
    this.toolList = tools;
    return tools;
  }
  async tool(name) {
    const tool = (await this.tools()).find(t => t.name === name);
    if (!tool) fail('CAPABILITY', `Affinity does not expose tool: ${name}. Run tools to inspect this version.`);
    return tool;
  }
  async call(name, args = {}) {
    const tool = await this.tool(name);
    validate(tool.inputSchema, args);
    const result = await this.client.request('tools/call', { name, arguments: args });
    if (result?.isError) fail('REMOTE', `Affinity tool failed: ${name}`, { result });
    return result;
  }
  async docs(topic) {
    return this.call('read_sdk_documentation_topic', { filename: topic });
  }
  async preamble() {
    if (!this.preambleRead) {
      await this.docs('preamble');
      this.preambleRead = true;
    }
  }
  async script(source, extra = {}) {
    await this.preamble();
    const tool = await this.tool('execute_script');
    const props = tool.inputSchema?.properties ?? {};
    const sourceKey = ['script', 'code', 'source', 'javascript'].find(key => props[key]?.type === 'string');
    if (!sourceKey) fail('CAPABILITY', 'Cannot identify execute_script source argument; inspect tools output');
    return this.call('execute_script', { ...extra, [sourceKey]: source });
  }
}
