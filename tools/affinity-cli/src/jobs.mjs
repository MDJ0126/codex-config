import path from 'node:path';
import { access, stat } from 'node:fs/promises';
import { fail } from './errors.mjs';
import { validate } from './schema.mjs';

const text = { type: 'string', minLength: 1, maxLength: 100000 };
const number = { type: 'number', minimum: -1000000, maximum: 1000000 };
const positive = { type: 'number', exclusiveMinimum: 0, maximum: 100000 };
const colour = { type: 'string', pattern: '^#[0-9a-fA-F]{6}([0-9a-fA-F]{2})?$' };
const object = (properties, required = []) => ({ type: 'object', properties, required, additionalProperties: false });
const rect = { x: number, y: number, width: positive, height: positive };
const step = (type, fields, required = []) => object({ op: { const: type }, ...fields }, ['op', ...required]);
const operations = [
  step('shape', { ...rect, shape: { enum: ['rectangle', 'ellipse'] }, fill: colour, name: text }, ['x','y','width','height','fill']),
  step('text', { ...rect, text, size: positive, font: text, fill: colour, name: text }, ['x','y','text','size','font','fill']),
  step('image', { ...rect, path: text, name: text }, ['x','y','width','height','path']),
  step('rename', { layer: text, name: text }, ['layer','name']),
  step('set-text', { layer: text, text }, ['layer','text']),
  step('fill', { layer: text, fill: colour }, ['layer','fill']),
  step('transform', { layer: text, x: number, y: number, scale: positive, rotation: number }, ['layer']),
  step('save', { path: text, overwrite: { type: 'boolean' } }, ['path']),
  step('export', { path: text, preset: text, overwrite: { type: 'boolean' }, width: positive, height: positive }, ['path','preset']),
  step('inspect', {}, [])
];
export const jobSchema = object({
  version: { const: 1 },
  document: { oneOf: [
    object({ create: object({ width: positive, height: positive, dpi: positive, transparent: { type: 'boolean' } }, ['width','height']) }, ['create']),
    object({ open: text }, ['open']),
    object({ id: text }, ['id'])
  ] },
  steps: { type: 'array', maxItems: 500, items: { oneOf: operations } }
}, ['version','document','steps']);

export async function prepareJob(input, base = process.cwd()) {
  validate(jobSchema, input);
  const job = structuredClone(input);
  for (const [i, operation] of job.steps.entries()) {
    if (operation.op === 'text' && ((operation.width === undefined) !== (operation.height === undefined))) fail('INPUT', `Text step ${i} must specify both width and height for frame text, or neither for artistic text`);
    if (operation.op === 'export' && ((operation.width === undefined) !== (operation.height === undefined))) fail('INPUT', `Export step ${i} must specify both width and height, or neither`);
  }
  const paths = new Set();
  const inputFile = async value => {
    const filename = path.resolve(base, value);
    let info;
    try { info = await stat(filename); } catch { fail('INPUT', `Input file does not exist: ${filename}`); }
    if (!info.isFile()) fail('INPUT', `Input is not a file: ${filename}`);
    return filename;
  };
  if (job.document.open) job.document.open = await inputFile(job.document.open);
  for (const operation of job.steps) {
    if (operation.op === 'image') operation.path = await inputFile(operation.path);
    if (['save','export'].includes(operation.op)) {
      operation.path = path.resolve(base, operation.path);
      const key = process.platform === 'win32' ? operation.path.toLowerCase() : operation.path;
      if (paths.has(key)) fail('INPUT', 'A job cannot write the same output path twice');
      paths.add(key);
      await access(path.dirname(operation.path)).catch(() => fail('INPUT', `Output directory does not exist: ${path.dirname(operation.path)}`));
      const existing = await stat(operation.path).catch(error => { if (error.code !== 'ENOENT') throw error; return null; });
      if (existing && (!existing.isFile() || !operation.overwrite)) fail('INPUT', `Output already exists (explicit overwrite required): ${operation.path}`);
    }
  }
  return job;
}

// Runs INSIDE Affinity. This function deliberately has no Node dependencies.
// API names were checked against Affinity's official 3.3.1 JSLib.
export function affinityRuntime(job) {
  const { Application } = require('/application.js');
  const { Document, NewDocumentOptions, FileExportOptions, FileExportArea, UnitType, RasterFormat } = require('/document.js');
  const { ShapeNodeDefinition, ArtTextNodeDefinition, FrameTextNodeDefinition, ImageNodeDefinition } = require('/nodes.js');
  const { ShapeRectangle, ShapeEllipse } = require('/shapes.js');
  const { Colour } = require('/colours.js');
  const { Point, Rectangle, Transform, Size } = require('/geometry.js');
  const { StoryBuilder } = require('/storybuilder.js');
  const { Font } = require('/fonts.js');
  const { FillDescriptor } = require('/fills.js');
  const { Bitmap } = require('/rasterobject.js');
  const { AddChildNodesCommandBuilder, DocumentCommand } = require('/commands.js');
  const { fs } = require('/fs.js');
  let doc = null, completed = [];
  const info = document => ({ id: String(document.sessionUuid), title: document.title, path: document.path,
    width: document.widthPixels, height: document.heightPixels, dpi: document.dpi,
    dirty: document.isDirty, readOnly: document.isReadOnly });
  const colourFrom = hex => {
    const value = hex.slice(1);
    return Colour.createRGBA8({ r: parseInt(value.slice(0,2),16), g: parseInt(value.slice(2,4),16),
      b: parseInt(value.slice(4,6),16), alpha: value.length === 8 ? parseInt(value.slice(6,8),16) : 255 });
  };
  const check = (code, action) => {
    const value = typeof code === 'object' && code !== null ? code.value : code;
    if (value !== undefined && value !== null && value !== 0 && value !== true) throw new Error(action + ' failed: ' + String(code));
  };
  const listLayers = () => Array.from(doc.layers.all).map(n => ({ name: n.userDescription || n.description,
    type: Object.prototype.toString.call(n), text: n.isTextNode ? n.text : undefined,
    box: { x: n.spreadBaseBox.x, y: n.spreadBaseBox.y, width: n.spreadBaseBox.width, height: n.spreadBaseBox.height } }));
  const layer = name => {
    const matches = Array.from(doc.layers.all).filter(n => (n.userDescription || n.description) === name);
    if (matches.length !== 1) throw new Error('Layer name must match exactly one layer: ' + name + ' (' + matches.length + ' matches)');
    return matches[0];
  };
  const insert = (definition, name) => {
    if (name) definition.userDescription = name;
    const builder = AddChildNodesCommandBuilder.create();
    builder.setInsertionTarget(doc.currentSpread); builder.addNode(definition);
    const command = builder.createCommand(false);
    check(doc.executeCommand(command), 'insert');
    if (command.newNodes.length !== 1) throw new Error('Insert did not create exactly one node');
    return command.newNodes[0];
  };
  const ensureOutput = operation => {
    if (!operation.overwrite && fs.exists(operation.path)) throw new Error('Refusing existing output: ' + operation.path);
  };
  try {
    if (job.action === 'status') return { ok: true, application: { version: Application.version, build: Application.buildVersion,
      platform: Application.platformName }, documents: Document.all.map(info) };
    if (job.action === 'presets') return { ok: true, presets: FileExportOptions.allPresetNames };
    if (job.action === 'fonts') return { ok: true, fonts: Font.all.map(f => ({ family: f.familyName, postscript: f.postscriptName })) };
    // Resolve every existing document by its session UUID; never follow a moving active tab.
    if (job.document.id) {
      doc = Document.all.find(d => String(d.sessionUuid) === job.document.id);
      if (!doc) throw new Error('Target document is not open; inspect status for its session id');
    } else if (job.document.open) {
      if (Document.all.some(d => d.path === job.document.open)) throw new Error('File is already open; use its session id');
      doc = Document.load(job.document.open);
    } else {
      const spec = job.document.create;
      const options = NewDocumentOptions.createDefault();
      options.units = UnitType.Pixel; options.width = spec.width; options.height = spec.height;
      options.dpi = spec.dpi ?? 96; options.rasterFormat = RasterFormat.RGBA8;
      options.isTransparentBackground = spec.transparent ?? false;
      doc = Document.create(options);
    }
    if (!doc || !doc.isOpen) throw new Error('Document could not be opened or created');
    if (doc.isReadOnly && job.steps.some(s => s.op !== 'inspect' && s.op !== 'export')) throw new Error('Target document is read-only');
    // Preflight fonts, export presets, layers, and every output before any edit.
    const fonts = job.steps.some(s => s.op === 'text') ? Font.all : [];
    const presets = job.steps.some(s => s.op === 'export') ? FileExportOptions.allPresetNames : [];
    const availableNames = Array.from(doc.layers.all).map(n => n.userDescription || n.description);
    for (const step of job.steps) {
      if (step.op === 'text' && !fonts.some(f => f.postscriptName === step.font)) throw new Error('Exact PostScript font not installed: ' + step.font + '. Run fonts.');
      if (step.op === 'export' && !presets.includes(step.preset)) throw new Error('Unknown export preset: ' + step.preset + '. Run presets.');
      if (step.path && ['save','export'].includes(step.op)) ensureOutput(step);
      if (step.layer && availableNames.filter(n => n === step.layer).length !== 1) throw new Error('Ambiguous or missing layer: ' + step.layer);
      if (step.op === 'rename') availableNames[availableNames.indexOf(step.layer)] = step.name;
      else if (['shape','text','image'].includes(step.op) && step.name) availableNames.push(step.name);
    }
    for (const [index, operation] of job.steps.entries()) {
      if (operation.op === 'shape') {
        const shape = operation.shape === 'ellipse' ? ShapeEllipse.create() : ShapeRectangle.create();
        insert(ShapeNodeDefinition.create(shape, new Rectangle(operation.x, operation.y, operation.width, operation.height), colourFrom(operation.fill)), operation.name);
      } else if (operation.op === 'text') {
        const story = StoryBuilder.create();
        if (operation.width !== undefined) story.setToFrameTextDefaultStyle(doc.dpi, doc.format);
        else story.setToArtisticTextDefaultStyle(doc.dpi, doc.format);
        const atts = story.glyphAtts;
        atts.height = operation.size;
        atts.font = fonts.find(f => f.postscriptName === operation.font);
        atts.brushFill = FillDescriptor.createSolid(colourFrom(operation.fill));
        story.setGlyphAtts(atts); story.addText(operation.text);
        const definition = operation.width !== undefined
          ? FrameTextNodeDefinition.createFromStoryBuilder(new Rectangle(operation.x,operation.y,operation.width,operation.height), story)
          : ArtTextNodeDefinition.createFromStoryBuilder(new Point(operation.x,operation.y), story);
        insert(definition, operation.name);
      } else if (operation.op === 'image') {
        const bitmap = Bitmap.loadFromFile(operation.path);
        if (!(bitmap.width > 0 && bitmap.height > 0)) throw new Error('Image could not be loaded');
        const definition = ImageNodeDefinition.create(bitmap.format);
        definition.bitmap = bitmap;
        definition.transform = Transform.createScale(operation.width / bitmap.width, operation.height / bitmap.height).translate(operation.x, operation.y);
        insert(definition, operation.name);
      } else if (operation.op === 'rename') layer(operation.layer).userDescription = operation.name;
      else if (operation.op === 'set-text') {
        const node = layer(operation.layer);
        if (!node.isTextNode) throw new Error('Target layer is not text');
        node.setText(operation.text);
      } else if (operation.op === 'fill') check(doc.setBrushFillDescriptor(colourFrom(operation.fill), layer(operation.layer).selfSelection), 'fill');
      else if (operation.op === 'transform') {
        const node = layer(operation.layer);
        const transform = new Transform().setIdentity();
        if (operation.scale !== undefined) transform.scale(operation.scale);
        if (operation.rotation !== undefined) transform.rotate(operation.rotation * Math.PI / 180);
        transform.translate(operation.x ?? 0, operation.y ?? 0);
        check(doc.executeCommand(DocumentCommand.createTransform(node.selfSelection, transform)), 'transform');
      } else if (operation.op === 'save') {
        ensureOutput(operation); check(doc.saveAs(operation.path), 'saveAs');
        if (!fs.exists(operation.path)) throw new Error('Saved file is not visible to Affinity');
      } else if (operation.op === 'export') {
        ensureOutput(operation);
        const size = operation.width !== undefined && operation.height !== undefined ? new Size(operation.width, operation.height) : undefined;
        const records = doc.export(operation.path, FileExportOptions.createWithPresetName(operation.preset), FileExportArea.createForWholeDocument(), size).all;
        if (!records.length || records.some(r => !r.isSuccess)) throw new Error('Export failed: ' + records.map(r => r.errorMessage).join('; '));
        completed.push({ index, op: operation.op, files: records.map(r => ({ path: r.path, warning: r.warningMessage })) });
        continue;
      } else if (operation.op === 'inspect') {
        completed.push({ index, op: 'inspect', document: info(doc), layers: listLayers() }); continue;
      }
      completed.push({ index, op: operation.op, ...(operation.path ? { path: operation.path } : {}) });
    }
    return { ok: true, document: info(doc), completed, layers: listLayers() };
  } catch (error) {
    // Never claim a batch was atomic: native file exports and earlier edits may remain.
    return { ok: false, error: String(error.message ?? error), document: doc?.isOpen ? info(doc) : null,
      completed, partialChangesPossible: true, retry: 'Inspect the document and outputs before retrying; no automatic rollback was attempted.' };
  }
}

export function scriptForJob(job) {
  // Data is a JavaScript literal, never interpolated into source-code fragments.
  const data = JSON.stringify(job).replace(/\u2028/g, '\\u2028').replace(/\u2029/g, '\\u2029');
  return `console.log('__AFFINITY_CLI_RESULT__' + JSON.stringify((${affinityRuntime.toString()})(${data})));`;
}

export function parseJobResult(result) {
  const strings = [];
  const collect = (value, depth = 0) => {
    if (depth > 10) return;
    if (typeof value === 'string') {
      strings.push(value);
      try { const parsed = JSON.parse(value); if (parsed !== value) collect(parsed, depth + 1); } catch { /* plain console output */ }
    } else if (Array.isArray(value)) value.forEach(item => collect(item, depth + 1));
    else if (value && typeof value === 'object') Object.values(value).forEach(item => collect(item, depth + 1));
  };
  for (const content of result?.content ?? []) if (content.type === 'text') collect(content.text);
  if (result?.structuredContent) collect(result.structuredContent);
  const texts = strings.join('\n');
  const marker = '__AFFINITY_CLI_RESULT__';
  const index = texts.lastIndexOf(marker);
  if (index < 0) fail('PROTOCOL', 'Affinity did not return a CLI result marker. Inspect the script response before retrying.', { result });
  const tail = texts.slice(index + marker.length);
  // Some versions wrap console output in JSON log entries. Parse the marker's
  // balanced JSON rather than assuming the entire tool response is plain text.
  let depth = 0, quoted = false, escaped = false, start = tail.indexOf('{');
  for (let i = start; start >= 0 && i < tail.length; i++) {
    const c = tail[i];
    if (quoted) { if (escaped) escaped = false; else if (c === '\\') escaped = true; else if (c === '"') quoted = false; }
    else if (c === '"') quoted = true;
    else if (c === '{') depth++;
    else if (c === '}' && --depth === 0) {
      let value;
      try { value = JSON.parse(tail.slice(start, i + 1)); } catch { break; }
      if (!value.ok) fail('JOB', value.error || 'Affinity job failed', value);
      return value;
    }
  }
  fail('PROTOCOL', 'Could not parse the Affinity job result', { result });
}
