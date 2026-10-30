import {createHash} from 'node:crypto';
import {WORKFLOW} from './assets/report-contract.mjs';

// Reviewed historical metadata is separate so original posts and their hashes remain intact.
export function historicalWorkflow(meta, raw, postPath, decisions) {
 const decision=decisions.find(entry=>entry.post===postPath);
 if(!decision)return meta;
 if(meta.type!=='버그'||meta.workflow)throw new Error('Unexpected historical workflow target: '+postPath);
 if(createHash('sha256').update(raw).digest('hex')!==decision.sourceSha256)throw new Error('Historical workflow needs re-review: '+postPath);
 if(!WORKFLOW.some(([id])=>id===decision.workflow)||!decision.reason||!decision.evidence.length||!decision.evidence.every(quote=>raw.includes(quote)))throw new Error('Missing historical workflow evidence: '+postPath);
 return {...meta,workflow:decision.workflow,workflowReview:decision};
}
