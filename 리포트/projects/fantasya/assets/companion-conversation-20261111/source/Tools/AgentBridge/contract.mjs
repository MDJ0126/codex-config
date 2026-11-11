export const PROFILES = [
  { id: 'luna', name: '루나', personality: '신중하고 관찰력이 좋은 정찰 동료. 짧고 차분하게 말하며 위험과 약속을 먼저 확인한다.' },
  { id: 'bram', name: '브람', personality: '듬직하고 솔직한 수호 동료. 동료 보호를 좋아하고 실천할 수 있는 약속만 한다.' },
  { id: 'sera', name: '세라', personality: '호기심 많고 다정한 탐험 동료. 함께 겪은 일을 기억하며 다음 모험을 제안한다.' }
];
const property = (type, description) => ({ type, description });
const descriptor = (name, description, properties = {}, required = [], readOnlyHint = false) => ({
  name, description,
  inputSchema: { type: 'object', properties, required, additionalProperties: false },
  annotations: { readOnlyHint, destructiveHint: false, idempotentHint: readOnlyHint, openWorldHint: false }
});
const id = property('string', 'Stable companion ID, for example luna.');
export const TOOLS = [
  descriptor('list_companions', 'List persistent companions, their personalities, game presence and controller owners.', {}, [], true),
  descriptor('observe_companion', 'Read actual game state, current action, reason, allowed targets, recent dialogue, memories and promises. Observe before acting.', { companionId: id }, ['companionId'], true),
  descriptor('wait_for_companion_events', 'Wait up to 20 seconds for an in-game player message addressed to the companion you control. Renew your lease and answer pendingPlayerMessages using say_as_companion with replyToMessageId.', { companionId: id, seconds: property('integer','Wait duration, 1 to 20 seconds.') }, ['companionId'], true),
  descriptor('update_companion_profile', 'Change this companion name and personality when requested by the player, preserving the same identity and memories.', { companionId: id, name: property('string','Visible name, maximum 40 characters.'), personality: property('string','Player-approved personality description, maximum 400 characters.') }, ['companionId','name','personality']),
  descriptor('talk_to_companion', 'Talk with the SAME persistent companion from outside the game. Player instructions can create explicit behavioral promises; responses appear in game too.', { companionId: id, text: property('string', 'Player message, maximum 1000 characters.') }, ['companionId', 'text']),
  descriptor('claim_companion', 'Take temporary control as this agent. Only one agent controls a companion at a time; player promises and emergency responses remain authoritative.', { companionId: id, agentName: property('string', 'Your visible identity, e.g. Codex or Claude.'), seconds: property('integer', 'Lease duration, 5 to 120 seconds.') }, ['companionId', 'agentName']),
  descriptor('act_companion', 'Issue one permitted game action using a fresh observation. Requires your own active lease; promises and current game rules are checked again in Unity.', { companionId: id, action: { type: 'string', enum: ['follow','guard','hold','retreat','attack'] }, targetId: property('integer', 'Actual target ID from observation, only for attack; otherwise 0.'), seconds: property('number', 'Action duration 1 to 6 seconds.'), reason: property('string', 'Short explanation shown to the player, maximum 160 characters.') }, ['companionId','action','reason']),
  descriptor('say_as_companion', 'Speak as the companion you currently control using your OWN model. When answering a pending player message, include its replyToMessageId; newer unseen messages remain pending. Do not invent game outcomes.', { companionId: id, text: property('string', 'Companion dialogue, maximum 1000 characters.'), replyToMessageId: property('string','ID of the latest player message you observed and are answering.') }, ['companionId','text']),
  descriptor('set_promise', 'Set a visible player-authorized standing instruction. It persists across sessions until changed. Use only when the player asked for this behavior.', { companionId: id, mode: {type:'string',enum:['auto','follow','guard','hold','retreat']}, allowAttack: property('boolean','Whether attacks are allowed.'), text: property('string','Exact instruction or promise the player authorized, maximum 200 characters.') }, ['companionId','mode','allowAttack','text']),
  descriptor('remember_companion', 'Store a player-authorized memory for this companion, shared between in-game and external conversations.', { companionId: id, text: property('string','A factual memory the player authorized, maximum 200 characters.') }, ['companionId','text']),
  descriptor('forget_companion_memory', 'Delete one memory by its ID when requested by the player.', { companionId: id, memoryId: property('string','Memory ID from observe_companion.') }, ['companionId','memoryId']),
  descriptor('release_companion', 'Release your lease and return to the local model and player promises.', { companionId: id }, ['companionId'])
];
const forbidden = new Set(['__proto__','constructor','prototype']);
export function validateObject(value, depth = 0) {
  if (depth > 12) throw new Error('Object nesting limit exceeded.');
  if (!value || typeof value !== 'object') return;
  for (const key of Object.keys(value)) {
    if (forbidden.has(key)) throw new Error('Reserved property is not allowed.');
    validateObject(value[key], depth + 1);
  }
}
export function validateArguments(name, value) {
  validateObject(value);
  const tool = TOOLS.find(tool => tool.name === name);
  if (!tool || !value || Array.isArray(value) || typeof value !== 'object') throw new Error('Unknown tool or invalid arguments.');
  for (const key of Object.keys(value)) if (!Object.hasOwn(tool.inputSchema.properties,key)) throw new Error('Unexpected argument: ' + key);
  for (const key of tool.inputSchema.required) if (!Object.hasOwn(value,key)) throw new Error('Missing argument: ' + key);
  for (const [key,item] of Object.entries(value)) {
    const schema=tool.inputSchema.properties[key];
    if (schema.type==='integer' ? !Number.isSafeInteger(item) : typeof item!==schema.type) throw new Error('Invalid argument type: '+key);
    if (schema.enum && !schema.enum.includes(item)) throw new Error('Invalid option: '+key);
    if (typeof item==='number' && !Number.isFinite(item)) throw new Error('Non-finite number.');
    if (typeof item==='string' && (item.length>1000 || !item.trim())) throw new Error('Empty or oversized text.');
  }
  return value;
}
