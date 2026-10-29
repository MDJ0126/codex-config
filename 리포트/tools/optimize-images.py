"""Lossless archive optimization. Never changes URLs, dimensions, color profiles or pixels."""
from pathlib import Path
from concurrent.futures import ThreadPoolExecutor, as_completed
from PIL import Image, PngImagePlugin
from io import BytesIO
from hashlib import sha256
import json, struct, time, zlib

ROOT = Path(__file__).resolve().parents[1]
MANIFEST = ROOT / 'migration/image-compression-2026-10-30.json'
digest = lambda b: sha256(b).hexdigest()

def png_chunks(raw):
    offset = 8
    while offset + 12 <= len(raw):
        length = int.from_bytes(raw[offset:offset+4], 'big')
        kind = raw[offset+4:offset+8]
        yield kind, raw[offset+8:offset+8+length]
        offset += length + 12

def jpeg_metadata_only(raw, orientation):
    out = bytearray(raw[:2]); offset = 2
    while offset < len(raw):
        start = offset
        if raw[offset] != 255: raise ValueError('Invalid JPEG segment')
        while raw[offset] == 255: offset += 1
        marker = raw[offset]; offset += 1
        if marker in (0xda, 0xd9): out.extend(raw[start:]); break
        if marker in (0x01, *range(0xd0, 0xd8)): out.extend(raw[start:offset]); continue
        length = int.from_bytes(raw[offset:offset+2], 'big')
        end = offset + length
        if length < 2 or end > len(raw): raise ValueError('Invalid JPEG length')
        if marker != 0xfe and not (marker in (0xe1, 0xed) and orientation in (None, 1)):
            out.extend(raw[start:end])
        offset = end
    return bytes(out)

def preserve_png_display(candidate, original):
    kinds={b'gAMA',b'cHRM',b'sRGB',b'sBIT',b'pHYs',b'cICP'}
    def chunk(kind,data):
        return struct.pack('>I',len(data))+kind+data+struct.pack('>I',zlib.crc32(kind+data)&0xffffffff)
    preserved=b''.join(chunk(k,v) for k,v in png_chunks(original) if k in kinds)
    result=bytearray(candidate[:8])
    for kind,data in png_chunks(candidate):
        if kind in kinds: continue
        result.extend(chunk(kind,data))
        if kind==b'IHDR': result.extend(preserved)
    return bytes(result)

def optimize(file):
    raw = file.read_bytes(); before = len(raw)
    rel = file.relative_to(ROOT).as_posix()
    with Image.open(BytesIO(raw)) as im:
        im.load()
        if getattr(im, 'n_frames', 1) > 1: return {'source':rel,'reason':'animation-preserved'}
        pixels = im.convert('RGBA').tobytes()
        profile = im.info.get('icc_profile')
        output = BytesIO()
        if im.format == 'PNG':
            info = PngImagePlugin.PngInfo()
            for kind, data in png_chunks(raw):
                if kind in (b'gAMA', b'cHRM', b'sRGB', b'sBIT', b'pHYs'): info.add(kind, data)
            options = dict(optimize=True, compress_level=9, pnginfo=info)
            for key in ('icc_profile', 'transparency', 'exif'):
                if key in im.info: options[key] = im.info[key]
            im.save(output, format='PNG', **options); candidate = preserve_png_display(output.getvalue(),raw)
        elif im.format == 'JPEG':
            candidate = jpeg_metadata_only(raw, im.getexif().get(274))
        elif im.format == 'WEBP':
            options = dict(lossless=True, method=6, exact=True)
            if profile: options['icc_profile'] = profile
            if im.info.get('exif'): options['exif'] = im.info['exif']
            im.save(output, format='WEBP', **options); candidate = output.getvalue()
        else: return {'source':rel,'reason':'format-preserved'}
        if len(candidate) >= before: return {'source':rel,'reason':'already-smaller'}
        with Image.open(BytesIO(candidate)) as check:
            check.load()
            if check.size != im.size or check.mode != im.mode or check.tobytes() != im.tobytes():
                raise ValueError('Pixel/mode mismatch: '+rel)
            if check.convert('RGBA').tobytes() != pixels or check.info.get('icc_profile') != profile:
                raise ValueError('Color/pixel mismatch: '+rel)
            if im.format == 'PNG':
                colors = lambda b: {k:v for k,v in png_chunks(b) if k in (b'gAMA',b'cHRM',b'sRGB',b'sBIT',b'pHYs',b'cICP')}
                if colors(raw) != colors(candidate): raise ValueError('PNG display metadata mismatch: '+rel)
        parts = Path(rel).parts
        backup = (ROOT/'projects'/parts[1]/'originals/image-compression'/Path(*parts[2:])
                  if parts[0]=='projects' else ROOT/'originals/image-compression'/Path(rel))
        backup.parent.mkdir(parents=True,exist_ok=True)
        if backup.exists() and backup.read_bytes()!=raw: raise ValueError('Backup conflict: '+rel)
        backup.write_bytes(raw)
        if digest(backup.read_bytes())!=digest(raw): raise ValueError('Backup verification failed')
        # Reject concurrent source edits instead of replacing someone else's file.
        if file.read_bytes()!=raw: raise ValueError('Concurrent edit: '+rel)
        file.write_bytes(candidate)
        return dict(source=rel,backup=backup.relative_to(ROOT).as_posix(),beforeSha256=digest(raw),
                    afterSha256=digest(candidate),beforeBytes=before,afterBytes=len(candidate),
                    width=im.width,height=im.height,pixelsSha256=digest(pixels),method='lossless')

def main():
    previous=json.loads(MANIFEST.read_text(encoding='utf-8')) if MANIFEST.exists() else None
    folders=[ROOT/'assets']+list((ROOT/'projects').glob('*/assets'))
    files=sorted(p for base in folders for p in base.rglob('*') if p.is_file() and p.suffix.lower() in ('.png','.jpg','.jpeg','.webp','.gif'))
    if previous:
        recorded={r['source'].replace('\\','/') for key in ('entries','skipped','errors') for r in previous[key]}
        if recorded!={p.relative_to(ROOT).as_posix() for p in files}:
            raise SystemExit('Image set changed: preserve this manifest and start a new dated batch')
    started=time.time(); before=previous['beforeBytes'] if previous else sum(p.stat().st_size for p in files)
    records=previous['entries'] if previous else []; skipped=[]; errors=[]
    known={entry['source'] for entry in records}
    for record in records:
        if digest((ROOT/record['source']).read_bytes())!=record['afterSha256'] or digest((ROOT/record['backup']).read_bytes())!=record['beforeSha256']:
            raise SystemExit('Previously optimized files changed; refusing to overwrite')
    pending_files=[p for p in files if p.relative_to(ROOT).as_posix() not in known]
    with ThreadPoolExecutor(max_workers=4) as pool:
        pending={pool.submit(optimize,p):p for p in pending_files}
        for i,future in enumerate(as_completed(pending),1):
            try:
                record=future.result()
                (records if 'afterSha256' in record else skipped).append(record)
            except Exception as error: errors.append({'source':str(pending[future].relative_to(ROOT)),'error':str(error)})
            if i%250==0: print(json.dumps({'processed':i,'total':len(files),'optimized':len(records),'errors':len(errors)}),flush=True)
    result=dict(schemaVersion=1,scanned=len(files),beforeBytes=before,afterBytes=sum(p.stat().st_size for p in files),
                elapsedSeconds=round(time.time()-started,2),entries=sorted(records,key=lambda r:r['source']),skipped=skipped,errors=errors)
    MANIFEST.write_text(json.dumps(result,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
    print(json.dumps({k:v for k,v in result.items() if k not in ('entries','skipped','errors')}|{'optimized':len(records),'errors':errors},ensure_ascii=False),flush=True)
    if errors: raise SystemExit(1)

if __name__=='__main__': main()
