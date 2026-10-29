#!/usr/bin/env node
import { readFile, open } from 'node:fs/promises';
import path from 'node:path';
import { fileURLToPath } from 'node:url';
import { McpClient } from '../src/transport.mjs';
import { Affinity } from '../src/affinity.mjs';
import { CliError, fail } from '../src/errors.mjs';
import { readJson, inspectFile, saveImages } from '../src/files.mjs';
import { prepareJob, scriptForJob, parseJobResult, jobSchema } from '../src/jobs.mjs';
import { acquireLock } from '../src/lock.mjs';

const HELP = `Affinity CLI 1.0.0 — local SDK/MCP automation without mouse or keyboard control

Usage: node tools/affinity-cli/bin/affinity.mjs COMMAND [arguments] [options]

  doctor                       Handshake, server version and available tools
  status                       Application version and open document session IDs
  tools                        Tool names, descriptions and actual JSON schemas
  docs [topic]                 SDK topic (default: preamble)
  presets                      Installed export preset names
  fonts                        Installed PostScript font names
  inspect DOCUMENT_ID          Inspect the explicitly selected document/layers
  job FILE.json                Execute a validated design job
  job FILE.json --dry-run      Validate input/output paths without connecting
  run FILE.js                  Execute a trusted Affinity script
  call TOOL --args JSON        Call a discovered MCP tool with validated arguments
  render spread|selection      Call native rendering tool; pass its schema's --args
  verify FILE                  Inspect output bytes/hash and PNG/JPEG dimensions
  schema                       Print the design job JSON schema
  version                      Print version and runtime

Options:
  --endpoint URL               Loopback MCP URL (env AFFINITY_MCP_URL)
  --transport sse|http         Default sse; http for Streamable HTTP
  --timeout MS                 Default 30000; 100..600000
  --args JSON|@FILE            Tool arguments / additional execute_script arguments
  --artifacts DIR              Save rendered image blocks (never overwrite)
  --out FILE                   Write JSON result to a new file as well as stdout
  --script-out FILE            Save compiled job script to a new file
  --width N --height N         Expected pixel dimensions for verify
  --dry-run                    Job validation only, no Affinity connection

All output is JSON. Exit: 0 success, 2 input/config, 3 connection, 4 remote/job,
5 timeout (operation outcome may be unknown), 6 busy, 1 other error.
Affinity must be running with its local MCP server enabled. No app launch,
focus change, dialog dismissal or automatic retry of edits is performed.
`;

function parse(argv) {
  const positionals = [], options = {};
  const flags = new Set(['dry-run','help']);
  const values = new Set(['endpoint','transport','timeout','args','artifacts','out','script-out','width','height']);
  for (let i = 0; i < argv.length; i++) {
    const value = argv[i];
    if (value === '-h') { options.help = true; continue; }
    if (!value.startsWith('--')) { positionals.push(value); continue; }
    const [key, ...parts] = value.slice(2).split('=');
    if (Object.hasOwn(options, key)) fail('INPUT', `Duplicate option: --${key}`);
    if (flags.has(key)) {
      if (parts.length) fail('INPUT', `--${key} does not take a value`);
      options[key] = true;
    } else if (values.has(key)) {
      options[key] = parts.length ? parts.join('=') : argv[++i];
      if (!options[key] || options[key].startsWith('--')) fail('INPUT', `--${key} requires a value`);
    } else fail('INPUT', `Unknown option: --${key}`);
  }
  return { command: positionals.shift() ?? 'help', args: positionals, options };
}
async function exclusiveWrite(filename, contents) {
  const file = await open(path.resolve(filename), 'wx', 0o600);
  try { await file.writeFile(contents); } finally { await file.close(); }
}
async function toolArgs(value) {
  if (!value) return {};
  let result;
  if (value.startsWith('@')) result = await readJson(value.slice(1));
  else try { result = JSON.parse(value); } catch { fail('INPUT', '--args must be JSON or @FILE'); }
  if (!result || typeof result !== 'object' || Array.isArray(result)) fail('INPUT', '--args must contain a JSON object');
  return result;
}
const counts = { help:0, version:0, schema:0, doctor:0, status:0, tools:0, docs:[0,1], presets:0, fonts:0, inspect:1, job:1, run:1, call:1, render:1, verify:1 };

export async function execute(argv) {
  const { command, args, options } = parse(argv);
  if (options.help || command === 'help') return { help: HELP };
  if (!Object.hasOwn(counts, command)) fail('INPUT', `Unknown command: ${command}`);
  const count = counts[command], [min,max] = Array.isArray(count) ? count : [count,count];
  if (args.length < min || args.length > max) fail('INPUT', `${command} expects ${min === max ? min : `${min}..${max}`} positional arguments`);
  if (options['dry-run'] && command !== 'job') fail('INPUT', '--dry-run is only valid for job');
  if (options['script-out'] && command !== 'job') fail('INPUT', '--script-out is only valid for job');
  if ((options.width || options.height) && command !== 'verify') fail('INPUT', '--width/--height are only valid for verify');
  if (options.args && !['run','call','render','job','status','presets','fonts','inspect'].includes(command)) fail('INPUT', '--args is only valid for script/tool commands');
  if (command === 'version') return { version:'1.0.0', node:process.versions.node, cli:fileURLToPath(import.meta.url) };
  if (command === 'schema') return { schema:jobSchema };
  if (command === 'verify') {
    const result = await inspectFile(args[0]);
    for (const dimension of ['width','height']) if (options[dimension] !== undefined) {
      const expected = Number(options[dimension]);
      if (!Number.isInteger(expected) || expected <= 0) fail('INPUT', `Expected ${dimension} must be a positive integer`);
      if (result[dimension] !== expected) fail('VERIFY', `Output ${dimension} differs from expected`, { actual:result[dimension], expected });
    }
    return result;
  }
  let job, source;
  if (command === 'job') {
    job = await prepareJob(await readJson(args[0]), path.dirname(path.resolve(args[0])));
    source = scriptForJob(job);
    if (options['script-out']) await exclusiveWrite(options['script-out'], source);
    if (options['dry-run']) return { dryRun:true, job, ...(options['script-out'] ? { script:path.resolve(options['script-out']) } : {}) };
  }
  if (command === 'run') {
    source = await readFile(args[0], 'utf8');
    if (Buffer.byteLength(source) > 2 * 1024 * 1024) fail('INPUT', 'Script exceeds 2 MiB');
  }
  if (command === 'render' && !['spread','selection'].includes(args[0])) fail('INPUT', 'render expects spread or selection');
  const suppliedArgs = await toolArgs(options.args);
  const client = new McpClient({ endpoint:options.endpoint ?? process.env.AFFINITY_MCP_URL,
    transport:options.transport ?? process.env.AFFINITY_MCP_TRANSPORT ?? 'sse',
    timeout:options.timeout === undefined ? 30000 : Number(options.timeout) });
  const release = await acquireLock();
  try {
    const initialization = await client.connect();
    const affinity = new Affinity(client);
    if (command === 'doctor') return { connected:true, endpoint:client.url.href, transport:client.transport,
      server:initialization.serverInfo, protocol:initialization.protocolVersion,
      tools:(await affinity.tools()).map(t=>t.name), runtimeValidation:'Use status and an actual test job to verify SDK operations.' };
    if (command === 'tools') return { tools:await affinity.tools() };
    if (command === 'docs') return { response:await affinity.docs(args[0] ?? 'preamble') };
    if (command === 'call') {
      if (args[0] === 'execute_script') await affinity.preamble();
      return { response:await affinity.call(args[0], suppliedArgs) };
    }
    if (command === 'render') return { response:await affinity.call(`render_${args[0]}`, suppliedArgs) };
    if (command === 'run') return { response:await affinity.script(source, suppliedArgs) };
    if (['status','presets','fonts'].includes(command)) source = scriptForJob({ action:command });
    if (command === 'inspect') source = scriptForJob({ version:1, document:{ id:args[0] }, steps:[{ op:'inspect' }] });
    const response = await affinity.script(source, suppliedArgs);
    const result = parseJobResult(response);
    // Also read saved files from the CLI side. Native success alone is not a byte check.
    if (job) {
      result.files = [];
      for (const operation of result.completed ?? []) {
        const filenames = operation.op === 'export' ? operation.files.map(f=>f.path) : operation.op === 'save' ? [operation.path] : [];
        for (const filename of filenames) result.files.push(await inspectFile(filename));
      }
    }
    return result;
  } finally { await client.close(); await release(); }
}

export async function main(argv = process.argv.slice(2)) {
  const started = Date.now();
  let envelope, outputFile;
  try {
    const { options } = parse(argv);
    // Reserve the result destination BEFORE any remote action. An existing
    // --out must not cause an edit followed by a misleading local input error.
    if (options.out) outputFile = await open(path.resolve(options.out), 'wx', 0o600);
    const output = await execute(argv);
    const cleaned = await saveImages(output, options.artifacts);
    envelope = { ok:true, ...cleaned.result, ...(cleaned.artifacts.length ? { artifacts:cleaned.artifacts } : {}), elapsedMs:Date.now()-started };
  } catch (error) {
    const code = error instanceof CliError ? error.code : error.code === 'EEXIST' ? 'INPUT' : 'INTERNAL';
    const cleaned = await saveImages(error.details, undefined);
    envelope = { ok:false, error:{ code, message:error.message, ...(cleaned.result ? { details:cleaned.result } : {}) }, elapsedMs:Date.now()-started };
    process.exitCode = ['INPUT','CONFIG','SCHEMA'].includes(code) ? 2
      : ['CONNECTION','HTTP','PROTOCOL','CAPABILITY'].includes(code) ? 3
      : ['REMOTE','RPC','JOB','VERIFY'].includes(code) ? 4 : code === 'TIMEOUT' ? 5 : code === 'BUSY' ? 6 : 1;
  }
  if (outputFile) {
    try { await outputFile.writeFile(JSON.stringify(envelope,null,2)+'\n'); }
    catch (error) {
      envelope = { ok:false, error:{code:'OUTPUT',message:error.message,details:{ remoteResult:envelope, retry:'The remote operation may already have completed. Do not blindly repeat it.' }}, elapsedMs:Date.now()-started };
      process.exitCode = 1;
    } finally { await outputFile.close(); }
  }
  process.stdout.write(JSON.stringify(envelope,null,2)+'\n');
  return envelope;
}
if (process.argv[1] && path.resolve(process.argv[1]) === fileURLToPath(import.meta.url)) await main();
