import { fail } from './errors.mjs';

// Validate the JSON Schema constructs used by the server's tool arguments.
// Schemas remain discoverable via `tools`; unsupported assertions fail closed.
export function validate(schema, value, at = '$') {
  if (schema === true || schema === undefined) return;
  if (schema === false) fail('INPUT', `${at} is not allowed`);
  const supported = new Set(['$schema','$id','$comment','$defs','definitions','title','description','default','examples','deprecated','readOnly','writeOnly',
    'type','properties','required','additionalProperties','items','allOf','anyOf','oneOf','enum','const',
    'minimum','maximum','exclusiveMinimum','exclusiveMaximum','minLength','maxLength','pattern','minItems','maxItems']);
  if (schema.$ref) fail('SCHEMA', `${at}: referenced schemas are not supported; inspect the tool schema`);
  for (const key of Object.keys(schema)) if (!supported.has(key)) fail('SCHEMA', `${at}: unsupported schema keyword ${key}`);
  for (const key of ['allOf']) if (schema[key]) for (const item of schema[key]) validate(item, value, at);
  for (const key of ['anyOf', 'oneOf']) if (schema[key]) {
    const matches = schema[key].filter(s => { try { validate(s, value, at); return true; } catch { return false; } });
    if (!matches.length || (key === 'oneOf' && matches.length !== 1)) fail('INPUT', `${at} does not match ${key}`);
  }
  if (schema.enum && !schema.enum.some(x => JSON.stringify(x) === JSON.stringify(value))) fail('INPUT', `${at} must be one of the declared values`);
  if (Object.hasOwn(schema, 'const') && JSON.stringify(schema.const) !== JSON.stringify(value)) fail('INPUT', `${at} must match const`);
  const type = value === null ? 'null' : Array.isArray(value) ? 'array' : typeof value;
  const types = schema.type === undefined ? [] : Array.isArray(schema.type) ? schema.type : [schema.type];
  if (types.length && !types.some(t => t === type || (t === 'integer' && Number.isInteger(value)))) fail('INPUT', `${at}: expected ${types.join('|')}, received ${type}`);
  if (typeof value === 'number') {
    if (!Number.isFinite(value)) fail('INPUT', `${at} must be finite`);
    if (schema.minimum !== undefined && value < schema.minimum) fail('INPUT', `${at} is below minimum`);
    if (schema.maximum !== undefined && value > schema.maximum) fail('INPUT', `${at} is above maximum`);
    if (schema.exclusiveMinimum !== undefined && value <= schema.exclusiveMinimum) fail('INPUT', `${at} is below exclusiveMinimum`);
    if (schema.exclusiveMaximum !== undefined && value >= schema.exclusiveMaximum) fail('INPUT', `${at} is above exclusiveMaximum`);
  }
  if (typeof value === 'string') {
    if (schema.minLength !== undefined && [...value].length < schema.minLength) fail('INPUT', `${at} is too short`);
    if (schema.maxLength !== undefined && [...value].length > schema.maxLength) fail('INPUT', `${at} is too long`);
    if (schema.pattern && !new RegExp(schema.pattern, 'u').test(value)) fail('INPUT', `${at} does not match pattern`);
  }
  if (Array.isArray(value)) {
    if (schema.minItems !== undefined && value.length < schema.minItems) fail('INPUT', `${at} has too few items`);
    if (schema.maxItems !== undefined && value.length > schema.maxItems) fail('INPUT', `${at} has too many items`);
    value.forEach((item, i) => validate(schema.items, item, `${at}[${i}]`));
  } else if (value && typeof value === 'object') {
    for (const key of schema.required ?? []) if (!Object.hasOwn(value, key)) fail('INPUT', `${at}.${key} is required`);
    for (const [key, item] of Object.entries(value)) {
      if (Object.hasOwn(schema.properties ?? {}, key)) validate(schema.properties[key], item, `${at}.${key}`);
      else if (schema.additionalProperties === false) fail('INPUT', `${at}.${key} is unknown`);
      else if (typeof schema.additionalProperties === 'object') validate(schema.additionalProperties, item, `${at}.${key}`);
    }
  }
}
