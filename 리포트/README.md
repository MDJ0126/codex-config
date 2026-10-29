# Tracefolio — Reports & Issues

## 프로젝트별 파일 구조

```text
리포트/
  projects/
    pixora/
      posts/       Markdown 원본
      reports/     생성된 HTML
      assets/      프로젝트 로고·첨부파일·이전 자료
      originals/   변경하지 않은 원문 백업 (웹 패키지 제외)
    codeassist/
    designdatatable/
    enginemigrator/
    windowsfoldersize/
    codex-config/
    affinity-cli/
  assets/brand/    Tracefolio 공통 브랜드
  assets/style.css
  assets/app.js
  index.html
```

`project` 값과 프로젝트 폴더 이름은 같아야 합니다. `layout.mjs`가 프로젝트 폴더에서 게시물을 수집하며 누락된 필수 메타데이터나 잘못된 위치는 빌드 오류가 됩니다. 과거 주소의 HTML은 이동 안내 파일이며 실제 보고서는 프로젝트 폴더 한 곳에서 관리합니다. 외부 주소는 그대로 유지합니다. 사이트의 표기는 Tracefolio이며 주황색 문서 보관함 심벌을 사용합니다.

외부 사이트: https://mdj0126.github.io/codex-config/

main의 리포트 변경을 푸시하면 `.github/workflows/reports-pages.yml`이 빌드·링크·검색·원본 해시 검증 후 GitHub Pages로 자동 배포합니다. 설정 파일과 도구·이전 매니페스트·프로젝트별 원문 백업은 Pages 패키지에서 제외합니다. 공개 Git 저장소의 원문 백업은 별도로 읽을 수 있습니다.

`index.html`을 브라우저에서 열면 게시글 목록을 볼 수 있습니다. 별도의 설치나 서버 없이 파일로 열 수 있습니다. 공개 인터넷 배포는 별도 작업입니다.

## 보고서 추가

1. `projects/<project>/posts/YYYY-MM-DD-작업명-vN.N.N.md`를 새로 만듭니다. 기존 파일을 덮어쓰지 않습니다.
2. 아래 메타데이터와 본문을 작성합니다.
3. 저장소 루트에서 `node 리포트/build.mjs`를 실행합니다.
4. `리포트/index.html`과 해당 보고서를 열어 확인하고 원본과 생성물을 함께 커밋합니다.

```markdown
---
title: 작업 결과 제목
date: 2026-10-29T18:00:00+09:00
version: v1.0.0
status: 완료
category: 개발
project: pixora
type: 작업 결과
summary: 목록에 표시할 짧은 설명
---
# 작업 결과 제목

## 결과
- 실제 완료한 내용

## 검증 근거
- 실행과 관찰 결과

## 남은 사항
- 미확인 항목 또는 없음

## 작업 시간
- 제안 시간: 시작 시 제시한 예상 시간
- 실제로 완료된 시간: 실제 소요 시간 및 완료 시각과 시간대
- XERJ: 사용 / 미사용 / 실패 — 사유
```

지원하는 Markdown은 제목 1~3단계, 문단, 불릿 목록, 굵게, 인라인 코드, 코드 블록입니다. HTML과 실행 가능한 링크를 렌더링하지 않습니다. 표 문법은 지원하지 않습니다. HTTPS/HTTP 링크와 assets/reports/posts 아래의 상대 링크·이미지는 지원하며 실행 가능한 URL과 raw HTML은 계속 차단합니다. 빌드는 Node.js 기본 모듈만 사용합니다. 외부 글꼴은 선택 사항이며 오프라인에서는 시스템 글꼴을 사용합니다.

전체 게시물은 최신 작성일 순이며 검색과 분류 필터를 함께 사용할 수 있습니다. 원본 저장 버튼으로 Markdown을 내려받을 수 있습니다. `예제` 상태는 실제 작업 기록과 구분됩니다.

## 프로젝트와 보고서 유형

모든 결과 보고는 이 폴더에서 관리합니다. `project`와 `type`은 필수입니다. `project`는 projects.json의 ID를 사용하고 `type`은 작업 결과, 버그, 수정사항, 검토, 설정 중 하나입니다. category는 개발·QA 등 세부 분야입니다. 프로젝트, 유형, 검색을 동시에 적용할 수 있습니다.

프로젝트 로고는 원본 프로젝트에서 가져와 assets/logos에 보관합니다. 출처는 assets/logos/sources.json에 기록하며 projects.json의 logo는 리포트 폴더를 기준으로 한 상대 경로입니다. 확인된 로고가 없으면 null로 두며 화면에는 프로젝트 첫 글자를 대체 표시합니다. 대체 표시는 공식 로고가 아닙니다. 처음 만든 디자인 예제 2개는 삭제했습니다. 제품의 실제 기능인 사용 예제 구현 보고서는 유지합니다.

등록 후 `node 리포트/build.mjs`와 `node 리포트/verify.mjs`를 실행합니다. 등록하지 않은 프로젝트나 잘못된 유형은 빌드 오류로 처리합니다.
