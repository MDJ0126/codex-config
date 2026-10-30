import {validateDiscussion,DISCUSSION_STATES,TOPICS} from './discussion-contract.mjs';
import {validateManual,manualMarkdown} from './manual-contract.mjs';
export const WORKFLOW=[['new','접수'],['review','검토'],['in_progress','진행'],['resolved','해결'],['closed','종료']];
export const PRIORITY=[['low','낮음'],['normal','보통'],['high','높음'],['urgent','긴급']];
export const SEVERITY=[['minor','경미'],['normal','보통'],['major','주요'],['critical','치명']];
const resultTypes=['작업 결과','수정사항','검토','설정'];
const fields=['title','summary','reporter','assignee','environment','steps','expected','actual','cause','changes','verification','remaining','outcome','proposedTime','actualTime','xerjEvidence'];
export function validateReport(input,projects){
 if(!input||input.schemaVersion!==2)throw new Error('schemaVersion은 2여야 합니다.');
 if(!Object.hasOwn(projects,input.project))throw new Error('등록된 프로젝트를 선택하세요.');
 if(!['result','bug','discussion','manual'].includes(input.kind))throw new Error('결과·버그·토론·작업 메뉴얼 중 하나를 선택하세요.');
 const r={schemaVersion:2,project:input.project,kind:input.kind};
 for(const key of fields){r[key]=String(input[key]??'').trim();if(r[key].length>(['title','summary'].includes(key)?180:5000))throw new Error(`${key} 입력이 너무 깁니다.`);}
 for(const key of ['title','summary','outcome'])if(!r[key])throw new Error(`${key} 항목을 입력하세요.`);
 if(/[\r\n]/.test(r.title+r.summary+r.reporter+r.assignee))throw new Error('제목·요약·작성자·담당자는 한 줄로 입력하세요.');
 r.priority=input.priority||'normal';r.severity=input.severity||'normal';
 if(!PRIORITY.some(([id])=>id===r.priority)||!SEVERITY.some(([id])=>id===r.severity))throw new Error('우선순위 또는 심각도 값이 잘못됐습니다.');
 r.xerjUsage=input.xerjUsage||'미사용';if(!['사용','미사용','실패'].includes(r.xerjUsage))throw new Error('XERJ 사용 상태를 확인하세요.');
 if(r.xerjUsage!=='미사용'&&!r.xerjEvidence)throw new Error('XERJ 사용·실패 근거를 입력하세요.');
 if(r.kind==='bug'){
  r.type='버그';r.workflow=input.workflow||'new';
  if(!WORKFLOW.some(([id])=>id===r.workflow))throw new Error('버그 단계는 접수·검토·진행·해결·종료 중 하나여야 합니다.');
  for(const key of ['environment','steps','expected','actual'])if(!r[key])throw new Error(`버그의 ${key} 항목을 입력하세요.`);
  if(['resolved','closed'].includes(r.workflow)&&(!r.changes||!r.verification))throw new Error('해결·종료 단계에는 수정 내용과 검증 근거가 필요합니다.');
  r.state=r.workflow==='closed'?'complete':r.workflow==='resolved'?'verification':'progress';
 }else if(r.kind==='manual'){
  validateManual(input,r);
 }else if(r.kind==='discussion'){
  validateDiscussion(input,r);
 }else{
  r.type=input.type||'작업 결과';if(!resultTypes.includes(r.type))throw new Error('결과 보고서 세부 유형을 확인하세요.');
  r.state=input.state||'verification';if(!['complete','partial','progress','verification','blocked'].includes(r.state))throw new Error('결과 상태를 확인하세요.');
  if(r.state==='complete'&&!r.verification)throw new Error('완료된 결과에는 검증 근거가 필요합니다.');
 }
 if(/(?:ghp_[a-zA-Z0-9]{20,}|github_pat_[a-zA-Z0-9_]{25,}|-----BEGIN .*PRIVATE KEY)/.test(JSON.stringify(r)))throw new Error('인증정보로 보이는 내용은 공개 보고서에 포함할 수 없습니다.');
 if(JSON.stringify(r).length>(r.kind==='manual'?55000:35000))throw new Error('전체 내용 제한을 초과했습니다. 이전 기록을 보존하고 후속 버전으로 이어가세요.');
 return r;
}
export function issueBody(r){return '<!-- tracefolio:v2 -->\n\n이 보고서는 Tracefolio 공통 작성 양식으로 등록되었습니다. 단계 변경은 아래 JSON의 workflow를 수정합니다.\n\n```json\n'+JSON.stringify(r,null,2)+'\n```';}
export function parseIssueBody(body){
 if(!String(body).includes('<!-- tracefolio:v2 -->'))return null;
 const match=/^```json[ \t]*\r?\n([\s\S]*?)^```[ \t]*$/m.exec(body);if(!match)throw new Error('Tracefolio JSON 블록이 없습니다.');
 if(match[1].length>62000)throw new Error('보고서 크기 제한을 초과했습니다.');return JSON.parse(match[1]);
}
const missing='기록되지 않았습니다.';
export function toMarkdown(r,{date=new Date().toISOString(),version='v1.0.0',issueNumber,issueURL,payloadPath}={}){
 if(r.kind==='manual')return manualMarkdown(r,{date,version,issueNumber,issueURL,payloadPath});
 if(r.kind==='discussion'){
  const label=DISCUSSION_STATES.find(([id])=>id===r.discussionState)[1];
  const lines=['---',`title: ${r.title}`,`date: ${date}`,`version: ${version}`,`status: ${label}`,`state: ${r.state}`,`project: ${r.project}`,'type: 토론','report_kind: discussion',`category: ${TOPICS.find(([id])=>id===r.topic)[1]}`,`summary: ${r.summary}`,'format: tracefolio-v2',`priority: ${r.priority}`,`topic: ${r.topic}`,`discussion_state: ${r.discussionState}`,`reporter: ${r.reporter||'미기록'}`,`assignee: ${r.assignee||'미지정'}`];
  if(r.relatedReport)lines.push(`related_report: ${r.relatedReport}`);if(r.replyTo)lines.push(`reply_to: ${r.replyTo}`);if(payloadPath)lines.push(`payload: ${payloadPath}`);if(issueNumber)lines.push(`issue: ${issueNumber}`,`issue_url: ${issueURL}`);
  lines.push('---','','## 토론 내용','',r.outcome,'','## QA 근거와 맥락','',r.verification||'근거가 아직 기록되지 않았습니다.','','## 제안 및 대안','',r.changes||'제안이 아직 기록되지 않았습니다.','','## 결정과 후속 작업','',r.decision||'아직 결정되지 않았습니다.','',r.remaining||'후속 작업이 아직 정해지지 않았습니다.','','## 작업 시간','',`- 제안 시간: ${r.proposedTime||'작성 시 기록되지 않았습니다.'}`,`- 실제로 완료된 시간: ${r.actualTime||'논의 기록이며 제품 작업 완료를 의미하지 않습니다.'}`,`- XERJ: ${r.xerjUsage} — ${r.xerjEvidence||'코드 검색 불필요'}`,'');if(issueURL)lines.push(`[등록·상태 변경 이력](${issueURL})`,'');return lines.join('\n');
 }
 const label=r.kind==='bug'?WORKFLOW.find(([id])=>id===r.workflow)[1]:({complete:'완료',partial:'부분 완료',progress:'진행 중',verification:'검증 대기',blocked:'차단'}[r.state]);
 const lines=['---',`title: ${r.title}`,`date: ${date}`,`version: ${version}`,`status: ${label}`,`state: ${r.state}`,`project: ${r.project}`,`type: ${r.type}`,`report_kind: ${r.kind}`,`category: ${r.kind==='bug'?'버그 추적':r.type}`,`summary: ${r.summary}`,'format: tracefolio-v2',`priority: ${r.priority}`,`severity: ${r.severity}`,`reporter: ${r.reporter||'미기록'}`,`assignee: ${r.assignee||'미지정'}`];
 if(r.workflow)lines.push(`workflow: ${r.workflow}`);
 if(payloadPath)lines.push(`payload: ${payloadPath}`);
 if(issueNumber)lines.push(`issue: ${issueNumber}`,`issue_url: ${issueURL}`);
 lines.push('---','','## 작업 결과','',r.outcome,'','## 변경 내용','');
 if(r.kind==='bug')for(const [name,key]of [['재현 조건','environment'],['재현 순서','steps'],['기대 동작','expected'],['실제 동작','actual'],['원인','cause']])lines.push(`### ${name}`,'',r[key]||(key==='cause'?'미확인':missing),'');
 lines.push('### 적용 내용','',r.changes||missing,'','## 검증','',r.verification||'검증 기록이 제공되지 않았습니다.','','## 남은 사항','',r.remaining||'남은 사항이 기록되지 않았습니다.','','## 작업 시간','',`- 제안 시간: ${r.proposedTime||'작성 시 기록되지 않았습니다.'}`,`- 실제로 완료된 시간: ${r.actualTime||'제품 작업의 완료 시간은 기록되지 않았습니다.'}`,`- XERJ: ${r.xerjUsage} — ${r.xerjEvidence||'코드 검색 근거가 제공되지 않았습니다.'}`,'');
 if(issueURL)lines.push(`[등록·상태 변경 이력](${issueURL})`,'');return lines.join('\n');
}
