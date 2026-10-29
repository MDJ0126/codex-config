---
name: Tracefolio 결과·버그 보고서
about: 사람과 AI 공통 JSON 양식으로 보고서를 등록합니다.
title: '[Tracefolio] '
---

사이트의 작성 폼을 사용하면 동일한 JSON을 생성할 수 있습니다.
https://mdj0126.github.io/codex-config/write/

아래 JSON을 폼 또는 공통 CLI로 생성한 내용으로 교체하세요. 비밀정보와 개인 로그를 넣지 마세요.

<!-- tracefolio:v2 -->

```json
{
  "schemaVersion": 2,
  "project": "codex-config",
  "kind": "result",
  "title": "실제 작업 제목",
  "summary": "확인한 결과와 범위",
  "outcome": "현재 작업 결과",
  "state": "verification",
  "verification": "검증한 근거 또는 미확인 사항",
  "xerjUsage": "미사용"
}
```
