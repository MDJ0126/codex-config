import { open, mkdir, readFile, realpath, stat } from 'node:fs/promises';
import { createReadStream } from 'node:fs';
import path from 'node:path';
import { createHash } from 'node:crypto';
import { fail } from './errors.mjs';

export async function readJson(filename) {
  try { return JSON.parse((await readFile(filename, 'utf8')).replace(/^\uFEFF/, '')); }
  catch (error) { fail('INPUT', `Cannot read JSON file: ${filename}`, { reason: error.message }); }
}
export function imageInfo(buffer, mime) {
  const info = { mime, bytes: buffer.length, sha256: createHash('sha256').update(buffer).digest('hex') };
  if (buffer.length >= 24 && buffer.subarray(0, 8).equals(Buffer.from([137,80,78,71,13,10,26,10]))) {
    return { ...info, mime: 'image/png', width: buffer.readUInt32BE(16), height: buffer.readUInt32BE(20) };
  }
  if (buffer.length > 4 && buffer[0] === 255 && buffer[1] === 216) {
    let i = 2;
    while (i + 9 < buffer.length) {
      if (buffer[i] !== 255) break;
      const marker = buffer[i + 1];
      if (marker === 217 || marker === 218) break;
      const length = buffer.readUInt16BE(i + 2);
      if (length < 2 || i + 2 + length > buffer.length) break;
      if ([192,193,194,195,197,198,199,201,202,203,205,206,207].includes(marker)) {
        return { ...info, mime: 'image/jpeg', width: buffer.readUInt16BE(i + 7), height: buffer.readUInt16BE(i + 5) };
      }
      i += 2 + length;
    }
  }
  return info;
}
export async function inspectFile(filename) {
  const resolved = await realpath(filename);
  const meta = await stat(resolved);
  if (!meta.isFile()) fail('INPUT', 'Expected a regular file');
  const hash = createHash('sha256');
  let header = Buffer.alloc(0), bytes = 0;
  for await (const chunk of createReadStream(resolved)) {
    hash.update(chunk); bytes += chunk.length;
    if (header.length < 65536) header = Buffer.concat([header,chunk.subarray(0,65536-header.length)]);
  }
  const after = await stat(resolved);
  if (bytes !== meta.size || after.size !== meta.size || after.mtimeMs !== meta.mtimeMs) fail('VERIFY', 'Output changed while it was being verified');
  return { path: resolved, ...imageInfo(header, 'application/octet-stream'), bytes, sha256:hash.digest('hex') };
}

export async function saveImages(result, directory) {
  const files = [];
  async function visit(value) {
    if (!value || typeof value !== 'object') return value;
    if (value.type === 'image' && typeof value.data === 'string') {
      const { data, ...rest } = value;
      if (!directory) return { ...rest, data: '[omitted; pass --artifacts DIR to save]', base64Length: data.length };
      if (!/^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?$/.test(data)) fail('PROTOCOL', 'Invalid image base64');
      const buffer = Buffer.from(data, 'base64');
      const info = imageInfo(buffer, value.mimeType);
      const extension = { 'image/png': 'png', 'image/jpeg': 'jpg', 'image/webp': 'webp' }[info.mime] ?? 'bin';
      const root = path.resolve(directory);
      await mkdir(root, { recursive: true });
      // Unique name plus exclusive create: never overwrite an existing artifact.
      const filename = path.join(root, `affinity-${Date.now()}-${files.length}-${info.sha256.slice(0, 12)}.${extension}`);
      const file = await open(filename, 'wx');
      try { await file.writeFile(buffer); } finally { await file.close(); }
      files.push({ path: filename, ...info });
      return { ...rest, artifact: filename, ...info };
    }
    if (Array.isArray(value)) return Promise.all(value.map(visit));
    const entries = await Promise.all(Object.entries(value).map(async ([k, v]) => [k, await visit(v)]));
    return Object.fromEntries(entries);
  }
  return { result: await visit(result), artifacts: files };
}
