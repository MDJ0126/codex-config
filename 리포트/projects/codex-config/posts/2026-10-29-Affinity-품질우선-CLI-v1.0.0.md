---
title: Affinity 품질 우선 지침과 전용 CLI 개발 가능성
date: 2026-10-29T18:50:18+09:00
version: v1.0.0
status: 지침 적용 완료
project: codex-config
type: 설정
category: 설정
summary: 품질과 Affinity 우선 지침 푸시 및 MCP 기반 CLI 개발 경로 확인
format: legacy-v1
---

- 작성 시각: 2026-10-29 18:50:18 KST (Asia/Seoul)
- 버전: v1.0.0
- 상태: 지침 적용·푸시 완료, CLI 가능성 조사 완료

## 작업 결과

- 품질을 최우선으로 하고 가능한 한 Affinity를 우선 사용하도록 전역 지침을 강화했습니다.
- 자동화 편의만으로 품질을 낮추거나 다른 도구로 대체하지 않도록 명시했습니다.
- 비대화형 작업이 어려울 때는 Affinity SDK·스크립트·MCP 및 CLI 구현 가능성을 먼저 확인하도록 추가했습니다.
- git diff --check와 변경 검토 후 Sync.ps1 -Direction Push를 실행했습니다. 커밋 95043d7이 원격 main에 푸시되었습니다. Sync.ps1의 설치본·저장소 해시 동일성 검사도 통과했습니다.

## CLI 개발 가능성

- 직접 개발할 수 있는 구조는 사용자 CLI → Affinity MCP → 앱 내부 JavaScript 실행 → 결과 렌더링·확인입니다. 이는 공개된 스크립트 실행·렌더링 기능을 바탕으로 제안한 구조입니다.
- 공식 커넥터에는 execute_script, render_spread, render_selection 및 SDK 문서 조회 도구가 안내되어 있습니다. [Canva 제작 공식 커넥터 안내](https://claude.com/marketplace/connectors/affinity-106b90)
- 명령줄에서 작업 인자와 파일을 받아 검증된 스크립트로 바꾸고, Affinity에 전달한 뒤 결과와 오류를 반환하도록 CLI를 만들 수 있습니다. 구체적인 작업 지원 범위는 설치 버전의 SDK 확인이 필요합니다.
- 예를 들어 affinity-cli run job.json 같은 인터페이스를 설계할 수 있습니다. 이는 제안 명령이며 설치되거나 구현된 실제 명령이 아닙니다.
- 외부 CLI에서 클릭·키 입력 자동화를 쓰지 않고 실제 Affinity 기능을 호출하는 방식이 목적에 맞습니다. 앱이 열린 상태의 자동화와 앱이 전혀 필요 없는 독립 엔진은 구분해야 합니다.

## 검증 근거와 미확인 사항

- [Affinity 스크립팅 공식 안내](https://www.affinity.studio/features/scripting)
- [Affinity SDK](https://sdk.affinity.studio/)
- 공식 커넥터 설명은 Claude Desktop과 Affinity 실행을 전제로 합니다. 사용자 제작 CLI와의 연결·인증은 실제 검증하지 않았습니다.
- 설치 버전, MCP 연결 상태, 지원 명령, 완전 헤드리스 실행, 화면 포커스 비간섭은 미확인입니다.
- 이번 요청은 가능성 질문이므로 CLI 구현·설치·실행은 수행하지 않았습니다.
- 보고서는 로컬 생성하며 인터넷 배포하지 않습니다.
- XERJ: 미사용 — 코드 검색 불필요, 지침 파일 직접 대조와 공식 문서 확인 수행

## 시간

- 제안 시간: 1~3분
- 시작 시각: 2026-10-29 18:49:23 KST
- 실제 소요 시간: 지침 적용·푸시와 조사·보고서 원본 작성까지 -32345 초
- 완료 시각: 2026-10-29 18:50:18 KST. 이후 보고서 빌드와 링크·검색 검증을 수행합니다.
