import { open, readFile, unlink } from 'node:fs/promises';
import { tmpdir } from 'node:os';
import path from 'node:path';
import { createHash, randomUUID } from 'node:crypto';
import { fail } from './errors.mjs';

export async function acquireLock() {
  // All loopback spellings/transports may refer to the same Affinity instance.
  const userKey = createHash('sha256').update(process.env.USERPROFILE ?? process.env.HOME ?? tmpdir()).digest('hex').slice(0,16);
  const filename = path.join(tmpdir(), `codex-affinity-cli-${userKey}.lock`);
  const nonce = randomUUID();
  for (let attempt = 0; attempt < 2; attempt++) {
    let file;
    try { file = await open(filename, 'wx', 0o600); }
    catch (error) {
      if (error.code !== 'EEXIST') throw error;
      let owner;
      try { owner = JSON.parse(await readFile(filename, 'utf8')); }
      catch { fail('BUSY', `Cannot verify the CLI lock owner: ${filename}`); }
      if (!Number.isInteger(owner.pid) || owner.pid <= 0) fail('BUSY', 'Invalid CLI lock; inspect it before removal');
      try { process.kill(owner.pid, 0); }
      catch (probe) {
        if (probe.code === 'ESRCH') { await unlink(filename).catch(() => {}); continue; }
      }
      fail('BUSY', 'Another Affinity CLI operation is running', { pid: owner.pid });
    }
    try { await file.writeFile(JSON.stringify({ pid: process.pid, nonce })); }
    finally { await file.close(); }
    return async () => {
      const owner = JSON.parse(await readFile(filename, 'utf8'));
      if (owner.nonce === nonce) await unlink(filename);
    };
  }
  fail('BUSY', 'Could not acquire the Affinity CLI lock');
}
