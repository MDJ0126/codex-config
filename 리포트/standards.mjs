import {Marked} from './vendor/marked/marked.esm.js';
export const sections=['작업 결과','변경 내용','검증','남은 사항','작업 시간'];
export const states=['complete','partial','progress','verification','blocked','archived'];
export function validatePost(meta,body,file,{legacyAllowed=false}={}){
 if(meta.format==='legacy-v1'){if(!legacyAllowed)throw new Error(`Unregistered legacy exception: ${file}`);return;}
 if(!['tracefolio-v1','tracefolio-v2'].includes(meta.format))throw new Error(`Missing format: tracefolio-v1 in ${file}`);
 if(meta.format==='tracefolio-v2'){if(!['result','bug'].includes(meta.report_kind))throw new Error('Missing report_kind');if(meta.report_kind==='bug'&&!['new','review','in_progress','resolved','closed'].includes(meta.workflow))throw new Error('Missing/invalid workflow');}
 if(!states.includes(meta.state))throw new Error(`Invalid state in ${file}`);
 if(!/^\d{4}-\d{2}-\d{2}T.*(?:Z|[+-]\d{2}:\d{2})$/.test(meta.date))throw new Error(`Timezone required in ${file}`);
 if(meta.summary.length>180)throw new Error(`Summary exceeds 180 characters in ${file}`);
 const tokens=new Marked().lexer(body);
 const headings=tokens.filter(t=>t.type==='heading'&&t.depth===2).map(t=>t.text);
 let previous=-1;for(const section of sections){const index=headings.indexOf(section);if(index<=previous)throw new Error(`Missing/out-of-order section ${section}: ${file}`);previous=index;}
 for(const label of ['제안 시간:','실제로 완료된 시간:','XERJ:'])if(!body.includes(label))throw new Error(`Missing ${label} in ${file}`);
 if(meta.type==='버그')for(const label of ['재현 조건','기대 동작','실제 동작','원인'])if(!body.includes(label))throw new Error(`Missing bug field ${label}: ${file}`);
 if(tokens.some(t=>t.type==='heading'&&t.depth===1))throw new Error(`Body H1 duplicates metadata title: ${file}`);
}
