const statuses={pass:'통과',fail:'실패',partial:'부분 확인',not_run:'미실행'};
const fields=['id','group','title','steps','expected','actual','fix','retest','reference','referenceImage','beforeImage','afterImage','missingEvidence'];
function image(value){if(!value)return '';if(!/^\.\.\/assets\/[A-Za-z0-9_./%~-]+$/.test(value)||value.slice(10).split('/').some(p=>p==='..'||decodeURIComponent(p)==='..'))throw new Error('TC 이미지는 보고서 assets 경로여야 합니다.');return value;}
export function validateTestCases(input){
 if(!Array.isArray(input)||!input.length||input.length>60)throw new Error('TC는 1~60개로 입력하세요.');
 const seen=new Set();return input.map(raw=>{if(!raw||typeof raw!=='object')throw new Error('TC 형식을 확인하세요.');const c={};for(const field of fields){c[field]=String(raw[field]??'').trim();if(c[field].length>1200)throw new Error('TC 항목 길이를 확인하세요.');}
 if(!/^[A-Z][A-Z0-9_-]{1,30}$/.test(c.id)||seen.has(c.id))throw new Error('TC ID는 고유해야 합니다.');seen.add(c.id);
 for(const key of ['group','title','steps','expected','actual'])if(!c[key])throw new Error('TC 절차·기대값·실제 결과가 필요합니다.');
 c.status=String(raw.status??'');if(!Object.hasOwn(statuses,c.status))throw new Error('TC 상태를 확인하세요.');
 for(const key of ['referenceImage','beforeImage','afterImage'])c[key]=image(c[key]);
 if((!c.referenceImage||!c.afterImage)&&!c.missingEvidence)throw new Error('누락된 화면 근거의 사유를 기록하세요.');
 return c;});
}
const cell=s=>s.replaceAll('|','\\|').replace(/\r?\n/g,'<br>');
export function testCaseMarkdown(cases){const lines=[];for(const c of cases){lines.push(`### ${c.id} · ${c.title}`,'',`분류: ${c.group} · 결과: **${statuses[c.status]}**`,'','| 항목 | 내용 |','| --- | --- |');for(const[label,key]of[['실행 절차','steps'],['기대값','expected'],['실제 결과','actual'],['수정 내용','fix'],['재검증','retest'],['원본 출처·버전','reference']])if(c[key])lines.push(`| ${label} | ${cell(c[key])} |`);lines.push('');for(const[label,key]of[['Excel 원본','referenceImage'],['수정 전','beforeImage'],['수정 후 실제 실행','afterImage']])if(c[key])lines.push(`**${label}**`,'',`![${c.id} ${label}](${c[key]})`,'');if(c.missingEvidence)lines.push(`화면 근거: ${c.missingEvidence}`,'');}return lines;}
