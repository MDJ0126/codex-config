# 공통 제품 다큐먼트 작성 기준

DDT의 `Documentation/guide.css`가 승인된 기준입니다. 제품마다 별도 테마를 만들지 않고 `Tools/Documentation/ProductGuide.mjs`의 `assets()`와 `render()`를 재사용합니다. `assets()`는 같은 스타일·Inter/Pretendard 글꼴과 라이선스를 제품 안에 복사해 DDT 설치 의존성을 만들지 않습니다.

## 구조와 자산

- 영어 기본과 기존 지원 9개 언어를 모두 생성합니다. 제품군은 문서 한 개 안의 제품 탭과 선택 제품의 목차를 사용합니다.
- 모든 모듈의 첫 두 항목은 개요 / 빠른 시작입니다. 개요는 짧은 목적 설명과 실제 예제 이미지 한 장·짧은 캡션, 빠른 시작은 처음 결과까지 필요한 간결한 순서로 구성합니다. 이후는 해당 제품의 기능·예제·호환/제한·문제 해결을 다룹니다.
- 헤더·제품 탭·파비콘은 확정된 해당 제품의 고유 로고를 사용합니다. DDT나 예제의 심볼은 신규 제품의 기본값이 아닙니다. 신규 로고는 실제 Affinity로 만들고 네이티브 원본을 보존해야 합니다.
- 원문과 이미지의 실제 경로를 유지합니다. iframe과 부분적인 body 색 덮어쓰기로 모듈 문서를 합치지 않습니다. 기존 가이드를 다시 생성하는 경우에도 마지막에 공통 렌더러를 실행합니다.

## 실행 가능한 참고 예제

`node Tools/Documentation/BuildGuideExamples.mjs`

산출물: `Assets/TurnRoom/Templete/Documentation/ProductGuideExample.en.html`과 나머지 8개 언어. 실제 DDT 문서와 캡처를 사용하는 **DDT 제품 예제**이며 Templete나 새 제품의 공식 로고를 의미하지 않습니다.

Pixora Studio: `node Tools/Pixora/BuildStudioGuide.mjs`

Entity Forge: `node Tools/EntityForge/ApplyDocumentationStyle.mjs` (기존 전체 문서 생성의 마지막 단계에도 연결됨)

DDT 헤더/파비콘: `Tools/Verification/DocumentFavicon.py` (기존 문서 빌더의 마지막 단계에도 연결됨)

## 새 제품 적용

생성 어댑터에서 `assets(output)` 후 `render({output, filename, language, brand, logo, favicon, modules})`를 호출합니다. 모듈마다 `id`, `label`, `logo`, `chapters: [{title, html}]`를 제공합니다. 로고가 없거나 문서가 두 항목 미만이면 생성기가 거부합니다. 예제 코드를 복사한 후 제품명만 바꾸어 다른 제품의 심볼을 남기지 않습니다.

## QA

장 이동 검사와 별도로, JavaScript가 생성한 각 장의 실제 본문에서 이미지·링크 URL을 추출해 해당 HTML의 배포 위치 기준으로 해석하고 대상 파일을 검사합니다. HTML 바깥쪽의 정규식 검사나 파일 존재 목록만으로 동적 본문 경로를 통과 처리하지 않습니다. 특히 파일로 여는 오프라인 문서의 `/Images/...` 같은 루트 절대경로와 모듈 간 상대경로를 확인합니다.

DDT와 생성된 `guide.css` 및 배포 글꼴의 해시를 대조하고, 로고/파비콘/예제 이미지 경로·개요와 빠른 시작 순서·카테고리/장 이동·언어 전환을 검사합니다. 실제 브라우저의 표시·작은 화면 배치·클릭/키보드·탭 파비콘도 따로 확인합니다. 정적/DOM 어댑터 검사를 실제 브라우저 QA로 표시하지 않습니다. 화면 조작이 중단되었거나 브라우저 정책으로 로컬 파일 접근이 막히면 우회하지 않고 해당 QA를 미실행으로 기록합니다. 캡처하지 않은 ScreenshotManifest 시각/소스 해시는 갱신하지 않습니다.
