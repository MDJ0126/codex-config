---
title: docs / bounded-099.html
date: 2026-10-18T13:03:15.192Z
version: v1.0.0
status: 기존 기록 이전
project: codeassist
type: 작업 결과
category: 이전 기록
summary: docs / bounded-099.html — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/docs/bounded-099.html
source_sha256: 79ec1fc15f00b868b06ada3d3ec89ceaac653345efcfcab2260b6ff3f1e8abcc
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

항목당 5초 자동화 실행기

## 항목당 5초 자동화 실행기

기능 하나의 VA·CA 쌍에 공통 5초 예산을 부여합니다. 검사 작업은 4.3초까지 허용하고 나머지는 검사 자식 프로세스 종료·결과 저장에 남깁니다. VS 자체는 강제 종료하지 않습니다. 항목별 실제 시간을 기록하며 5초를 넘기면 budget-violation으로 실패 처리합니다.

COM/UIA/화면 촬영은 항목별 자식 프로세스로 격리했습니다. 창 관찰기는 버튼을 누르지 않고 상위 실행기만 토큰·PID가 일치하는 창에 닫기 요청을 한 번 보냅니다. 시간 초과로 상태가 불확실한 호스트는 격리해 후속 항목을 통과로 처리하지 않습니다.

## 검증

검사기 테스트 12/12 통과. 아래 가상 시험은 실행기 검증이며 제품 기능 동기화 증거가 아닙니다.

| 시험 | 결과 | 소요 |
| --- | --- | --- |
| quick | representative-match | 154.74 ms |
| hang | timeout | 4315.64 ms |
| shared | timeout | 4314.4 ms |

## 실제 VS 호출

VA 호출·창 닫기는 수행했습니다. CA의 DTE 연결이 준비되지 않아 paired 기능 일치로 판정하지 않았습니다.

```
[
  {
    "id": "live-options-cancel",
    "elapsedMs": 2063.35,
    "status": "unverified",
    "withinBudget": true
  },
  {
    "id": "live-scope-next",
    "elapsedMs": 1030.43,
    "status": "unverified",
    "withinBudget": true
  }
]
```

실제 원본 결과 · 닫기 1회 근거 · 시간 시험 · 요약

## 실행

```
pwsh -NoProfile -File tools/ParityRunner/RunBounded.ps1 -VaProcessId <VA 전용 PID> -CaProcessId <CA 전용 PID> -Plan <계획 JSON>
```

빌드는 배치 시작 시 한 번 수행합니다. 기능별 기대 결과가 없는 창 열기/취소는 observed-only이며 통과가 아닙니다. 기존 98개 기능 검증 완료나 제품 유사도 향상으로 보고하지 않습니다.

```
{
  "task": "Bounded automation runner configuration",
  "toolConfigurationComplete": true,
  "proposedSeconds": 600,
  "startedAt": "2026-10-18T21:54:58+09:00",
  "finishedAt": "2026-10-18T22:03:15.190305+09:00",
  "elapsedSeconds": 497,
  "pairedItemBudgetMs": 5000,
  "diagnosticWorkBudgetMs": 4300,
  "supervisorTestsPassed": 12,
  "supervisorTestsTotal": 12,
  "syntheticWorstMs": 4315.64,
  "liveItemTimings": [
    {
      "id": "live-options-cancel",
      "elapsedMs": 2063.35,
      "status": "unverified",
      "withinBudget": true
    },
    {
      "id": "live-scope-next",
      "elapsedMs": 1030.43,
      "status": "unverified",
      "withinBudget": true
    }
  ],
  "singleCancelRequests": 1,
  "legacyUnboundedRunner": "disabled",
  "legacyGlobalCloser": "disabled",
  "productParityUnchanged": {
    "representativeMatches": 4,
    "differences": 7,
    "unverified": 97,
    "denominator": 108
  },
  "XERJ": "Used; retrieved candidates then verified current source. No exact bounded-runner hits."
}
```

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/docs/bounded-099.html)

- 원문 SHA-256: `79ec1fc15f00b868b06ada3d3ec89ceaac653345efcfcab2260b6ff3f1e8abcc`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
