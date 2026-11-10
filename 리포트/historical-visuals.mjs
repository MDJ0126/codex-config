import {escapeHTML} from './render.mjs';

// Each diagram is anchored to the exact wording of its archived report.
const sequences = [
 [
  "codex-config/2026-10-29-Affinity-품질우선-CLI-v1.0.0",
  "제안 구조",
  "사용자 CLI → Affinity MCP → 앱 내부 JavaScript 실행 → 결과 렌더링·확인"
 ],
 [
  "enginemigrator/2026-10-29-EngineMigrator-외부의존성-동의와-사전진단-v1.0.0",
  "외부 의존성 확인 절차",
  "프로젝트 사전 검사 → 포팅 메뉴 → 의존성 경고 목록 확인 → 직접 네 또는 아니오 선택"
 ],
 [
  "pixora/2026-10-29-result-20261029173856236-v1.0.0",
  "어셈블리 의존 방향",
  "Pixora.Samples.Editor → Pixora.Editor → TurnRoom.Editor"
 ],
 [
  "entityforge/2026-11-02-bug-20261102103155494-v1.0.0",
  "사용자 로그에 기록된 오류 경로",
  "ListView.set_bindItem → RefreshItems → BindRows 콜백의 rows[index]에서 ArgumentOutOfRangeException"
 ],
 [
  "afternoon-pause/2026-11-03-bug-20261103151737455-v1.0.0",
  "Unity 실행 검증 순서",
  "AddComponent → Awake → 네이티브 ClearCorners → 비활성/재활성 → 제거"
 ],
 [
  "afternoon-pause/2026-11-03-bug-20261103185829401-v1.0.0",
  "대화 응답 예약 재현 순서",
  "입장→안녕하세요 전송→응답 대상 예약 확인→광장의 반대편으로 이동→4초 뒤 예약 대상의 정확한 응답 확인"
 ],
 [
  "blog-manager/2026-11-07-result-20261107123918944-v1.0.0",
  "발행 확인 이후 구현한 처리 순서",
  "원고·사진 전송 → 설정 적용 → 실제 제출 → 공개 결과 검증"
 ],
 [
  "blog-manager/2026-11-08-bug-20261108121624858-v1.0.0",
  "즉시 발행 구현의 기본 경로",
  "확인 → 카테고리·태그·공개범위 준비 → 최종 제출 → 네이버 공개 URL 인식 → 공개본문 대조 → 게시완료 기록"
 ],
 [
  "blog-manager/2026-11-08-bug-20261108115828816-v1.0.0",
  "편집기 오류 확인 및 복구 순서",
  "초안 게시 버튼 하단의 실제 오류를 읽음 → 네이버 편집기 열기 → 기존 시험 원고 복구 → 원고 제목 일치 확인 → 네이버 본문 전체 글꼴·크기 복구 → 앱의 편집기 준비 재실행 → 네이버 전체공개·현재 설정 확인"
 ],
 [
  "blog-manager/2026-11-08-bug-20261108125914743-v1.0.0",
  "복구 원고를 비교하기까지의 처리 순서",
  "복구창 감지 → 전용 확인 버튼1회 → 실제 제목 입력 영역 준비 대기 → 복구 원고와 선택한 로컬 원고 대조"
 ],
 [
  "blog-manager/2026-11-07-result-20261107120012443-v1.0.0",
  "원고 보존 확인 순서",
  "테스트 원고 작성 → 수정 → 저장 → 새로고침 → 재열기"
 ],
 [
  "last37n/2026-10-30-bug-20261030200231591-v1.0.0",
  "월드 선택 재현 절차",
  "F2→월드 ID IMGUI 필드에 city-other 입력→접속 버튼→서버 월드와 화면 확인"
 ]
,

 [
  "codex-config/2026-10-29-result-20261029125313371-v1.0.0",
  "버그 처리 단계",
  "접수 → 검토 → 진행 → 해결 → 종료"
 ],
 [
  "pixora/2026-10-29-bug-20261029132249879-v1.0.0",
  "수정 전 로그에 기록된 재진입 경로",
  "ChangeEvent<string> → BuildSettings → CreateGUI"
 ],
 [
  "pixora/2026-10-29-Pixora-usage-examples-v1.1.0",
  "단계별 예제의 실행 순서",
  "입력 → 프로필 → 옵션 → 결과 생성 → 결과 확인"
 ],
 [
  "pixelfoliage/2026-11-04-result-20261104045257294-v1.0.0",
  "팔레트 작업 순서",
  "팔레트 시작하기 → 파일명·프로젝트 폴더 → 팔레트 저장 → 식생 카드 선택 → 바닥 지정/선택한 바닥 사용 → 씬 브러시 또는 영역 채우기"
 ],
 [
  "pixelfoliage/2026-11-04-result-20261104051337902-v1.0.1",
  "팔레트 작업 순서",
  "팔레트 시작하기 → 파일명·프로젝트 폴더 → 팔레트 저장 → 식생 카드 선택 → 바닥 지정/선택한 바닥 사용 → 씬 브러시 또는 영역 채우기"
 ],
 [
  "pixelcamera/2026-10-31-result-20261031192350406-v1.0.0",
  "맵별 Play 검증 순서",
  "Play→상태읽기→게임캡처→Stop"
 ],
 [
  "pixelcamera/2026-10-31-result-20261031192910012-v1.0.1",
  "맵별 Play 검증 순서",
  "Play→상태읽기→게임캡처→Stop"
 ],
 [
  "codeassist/2026-10-29-import-codeassist-905e3675242d-v1.0.0",
  "Welcome 문서 탭 실제 검증 순서",
  "Welcome 표시 → 문서 탭 닫기 → 두 번째 바나나(Welcome) 클릭 → 문서 탭 재열기"
 ],
 [
  "codeassist/2026-10-29-import-codeassist-4d3d12c9de6d-v1.0.0",
  "Welcome 문서 탭 실제 검증 순서",
  "Welcome 표시 → 문서 탭 닫기 → 두 번째 바나나(Welcome) 클릭 → 문서 탭 재열기"
 ],
 [
  "codex-config/2026-10-30-result-20261030080929241-v1.0.0",
  "공통 문서 생성 계약",
  "JSON→Markdown→HTML"
 ]
,

 [
  "sampleprojectforunity/2026-11-01-result-20261101061915292-v1.0.0",
  "씬 전환 설계의 정상 경로",
  "전환 잠금 → 입력 차단·로딩 표시 → 다음 씬 Additive 로드(activateOnLoad:true) → SceneManager.SetActiveScene → 다음 씬 초기화 준비 확인 → 이전 Scene 수명 UI 닫기 및 이전 씬 언로드 → 새 화면 표시 → 로딩 숨김·입력 복원"
 ],
 [
  "sampleprojectforunity/2026-11-01-result-20261101184318119-v1.0.0",
  "CombatBehavior 그래프의 기본 순서",
  "Find→Approach→Attack"
 ],
 [
  "sampleprojectforunity/2026-11-01-manual-combat-framework-20261102-20261101174828301-v1.0.3",
  "저장된 Behavior 그래프의 실제 검증 순서",
  "적 탐색→7m 접근→공격→사망"
 ],
 [
  "sampleprojectforunity/2026-11-01-manual-combat-framework-20261102-20261101180841801-v1.0.4",
  "저장된 Behavior 그래프의 실제 검증 순서",
  "적 탐색→7m 접근→공격→사망"
 ],
 [
  "sampleprojectforunity/2026-11-01-manual-combat-framework-20261102-20261101184405969-v1.0.5",
  "저장된 Behavior 그래프의 실제 검증 순서",
  "적 탐색→7m 접근→공격→사망"
 ],
 [
  "sampleprojectforunity/2026-11-01-manual-combat-framework-20261102-20261101185126054-v1.0.6",
  "저장된 Behavior 그래프의 실제 검증 순서",
  "적 탐색→7m 접근→공격→사망"
 ]
];
const byPost = new Map(sequences.map(([key, label, sequence])=>{
 const [project, name]=key.split('/');
 return [`projects/${project}/posts/${name}.md`,{label,sequence}];
}));

export function historicalDiagram(postPath, source){
 const record=byPost.get(postPath);
 if(!record)return '';
 const {label,sequence}=record;
 if(!source.includes(sequence))throw new Error('Missing historical diagram evidence: '+postPath);
 const nodes=sequence.split('→').map((part,index)=>`  step${index}["${part.trim().replaceAll('<','#60;').replaceAll('>','#62;')}"]`);
 const edges=nodes.slice(1).map((_,index)=>`  step${index} --> step${index+1}`);
 return `<figure class="historical-diagram"><figcaption>${escapeHTML(label)} · 원문 기준</figcaption><pre class="mermaid">${escapeHTML(['flowchart TD',...nodes,...edges].join('\n'))}</pre><p class="diagram-fallback">${escapeHTML(sequence)}</p></figure>`;
}

export function historicalCode(postPath, source){
 if(postPath!=='projects/sampleprojectforunity/posts/2026-11-01-result-20261101220456873-v1.0.0.md')return '';
 const revision='85eba5315c7a17d0346d4a774de5df3241e96537';
 const file='Assets/Scripts/InGame/Equipment/EquipmentLoadout.cs';
 if(!source.includes('/commit/'+revision))throw new Error('Missing historical code commit: '+postPath);
 const code="        public bool Select(int index)\n        {\n            if (weapons == null || index < 0 || index >= weapons.Length || weapons[index] == null)\n            {\n                return false;\n            }\n            _selections[SelectionKey] = index;\n            return true;\n        }";
 return `<figure class="historical-code"><figcaption>무기 선택을 공유 상태에 저장하는 범위 검사</figcaption><p>무기 목록이 없거나 선택이 비어 있으면 거부하고, 유효한 선택만 씬 사이에 공유하는 키에 저장합니다. <a class="report-link" href="https://github.com/MDJ0126/SampleProjectForUnity/blob/${revision}/${file}">EquipmentLoadout.cs · 공개 커밋 원본</a></p><pre><code data-language="csharp">${escapeHTML(code)}</code></pre></figure>`;
}
