export const TOPICS=[['qa','QA 토론'],['gap','부족한 점'],['proposal','개선 제안'],['feature','새 기능 요청']];
export const DISCUSSION_STATES=[['open','논의 중'],['considering','검토 중'],['accepted','채택'],['deferred','보류'],['implemented','반영 완료']];
export const discussionSections=['토론 내용','QA 근거와 맥락','제안 및 대안','결정과 후속 작업','작업 시간'];
export function validateDiscussion(input,r){
 r.type='토론';r.topic=input.topic||'qa';r.discussionState=input.discussionState||'open';
 if(!TOPICS.some(([id])=>id===r.topic)||!DISCUSSION_STATES.some(([id])=>id===r.discussionState))throw new Error('토론 종류 또는 논의 상태를 확인하세요.');
 for(const key of ['relatedReport','replyTo','decision']){r[key]=String(input[key]||'').trim();if(r[key].length>5000)throw new Error('토론 입력이 너무 깁니다.');}
 for(const key of ['relatedReport','replyTo'])if(r[key]&&!new RegExp('^projects/'+r.project+'/reports/[\\p{L}\\p{N}_. -]+\\.html$','u').test(r[key]))throw new Error('같은 프로젝트의 사이트 보고서 경로를 선택하세요.');
 if(r.topic==='qa'&&!r.relatedReport)throw new Error('QA 토론에는 원본 QA 보고서를 연결하세요.');
 if(['accepted','deferred','implemented'].includes(r.discussionState)&&!r.decision)throw new Error('채택·보류·반영 완료에는 결정 이유와 후속 작업을 기록하세요.');
 if(r.discussionState==='implemented'&&!r.verification)throw new Error('반영 완료에는 검증 근거가 필요합니다.');
 r.state=r.discussionState==='implemented'?'complete':'progress';return r;
}
export function validateDiscussionLinks(post,catalog){
 if((post.kind||post.report_kind)!=='discussion')return;
 const related=post.relatedReport||post.related_report||'',reply=post.replyTo||post.reply_to||'';
 if(related){const target=catalog.find(p=>p.reportPath===related);if(!target||target.project!==post.project||(target.kind||target.report_kind)==='discussion')throw new Error('연결할 원본 보고서가 없거나 프로젝트가 다릅니다.');}
 if(reply){const parent=catalog.find(p=>p.reportPath===reply);if(!parent||(parent.kind||parent.report_kind)!=='discussion'||parent.project!==post.project||reply===post.reportPath)throw new Error('후속 글의 원본 토론을 확인하세요.');if((parent.relatedReport||parent.related_report||'')!==related)throw new Error('후속 글은 같은 QA 보고서에 연결해야 합니다.');}
 const seen=new Set(post.reportPath?[post.reportPath]:[]);let cursor=reply;while(cursor){if(seen.has(cursor))throw new Error('후속 토론의 순환 연결은 허용되지 않습니다.');seen.add(cursor);const parent=catalog.find(p=>p.reportPath===cursor);cursor=parent?.replyTo||parent?.reply_to||'';}
}
