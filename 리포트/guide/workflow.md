# 결과·버그 보고서 작성과 5단계 처리

사람과 AI는 같은 JSON 계약 v2를 사용합니다. 사이트의 작성 폼, AI용 CLI, GitHub 자동 접수가 모두 assets/report-contract.mjs의 validateReport를 호출합니다. Markdown만 따로 만들어 필수 입력을 우회하지 않습니다. v2 게시물은 원본 JSON을 함께 보존하며 빌드가 두 파일의 일치를 확인합니다.

## 분류

- 프로젝트 → 결과 보고서: 작업 결과·수정사항·검토·설정, 결과 상태와 우선순위를 기록합니다.
- 프로젝트 → 버그 보고서: 처리 단계·심각도·우선순위·담당자와 재현 자료를 기록합니다.
- 기존 단계가 없는 보고서는 과거·단계 미분류로 표시합니다. 완료 문구만 보고 해결이나 종료로 바꾸지 않습니다.

## 버그 처리 5단계

| JSON 값 | 화면 | 의미 |
| --- | --- | --- |
| new | 접수 | 현상과 재현 자료가 등록됨 |
| review | 검토 | 재현·원인·우선순위를 확인 중 |
| in_progress | 진행 | 담당자가 수정·분석을 수행 중 |
| resolved | 해결 | 수정 내용과 검증 근거를 기록한 상태 |
| closed | 종료 | 결과를 확인하고 처리를 마침 |

해결·종료에는 changes와 verification이 필요합니다. 원인이 확인되지 않았으면 cause를 비워두고 미확인으로 기록합니다. 다시 열린 GitHub 버그 이슈는 검토 단계로 돌아갑니다. GitHub에서 종료한 이슈도 수정·검증 근거가 없으면 사이트에 종료로 반영하지 않고 검사에서 차단합니다.

## 사람이 작성하기

1. 사이트 상단의 보고서 작성으로 이동합니다.
2. 프로젝트와 결과·버그 분류를 고르고 실제 내용을 입력합니다.
3. 검사·미리보기로 공통 JSON과 게시물 내용을 확인합니다.
4. 공개 가능한 정보인지 확인하고 GitHub에서 최종 등록을 누릅니다.
5. GitHub 로그인 후 최종 제출합니다. 자동 검사·배포가 끝나면 사이트에 TF-이슈번호로 표시됩니다.

브라우저 임시저장은 해당 기기의 초안일 뿐 공개 등록이 아닙니다. 등록 전 GitHub 화면으로 이동했다는 것만으로 제출 완료로 간주하지 않습니다. 내용이 등록 URL 제한보다 길면 JSON을 복사·다운로드해 GitHub의 Tracefolio 등록 양식에 붙여넣습니다.

## AI가 작성하기

같은 JSON 템플릿을 실제 값으로 채운 다음 공통 CLI를 실행합니다. CLI는 프로젝트의 assets/submissions에 원본 JSON, posts에 Markdown을 생성합니다.

```sh
node 리포트/create-report.mjs --input report.json --dry-run
node 리포트/create-report.mjs --input report.json
node 리포트/build.mjs
node 리포트/verify.mjs
node --test 리포트/tests/*.test.mjs
```

AI도 접수 저장소를 사용할 때는 GitHub 이슈 본문에 <!-- tracefolio:v2 --> 표시와 JSON 코드 블록을 넣습니다. 이슈 제목·본문은 공개되며 인증정보와 개인 로그를 포함하지 않습니다.

## 단계 변경과 이력

등록된 GitHub 이슈의 JSON workflow를 변경하면 새 게시물 버전과 단계 이력을 보존합니다. 이슈를 종료·재개하는 이벤트도 처리합니다. 사이트 목록은 같은 이슈의 최신 버전만 표시하고 이전 HTML과 JSON은 보존합니다. 담당자는 기록 필드이며 자동으로 GitHub 사용자를 배정하거나 별도 메시지를 보내지 않습니다.

## 공통 JSON 필드

| 구분 | 필드 |
| --- | --- |
| 필수 | schemaVersion: 2, project, kind: result 또는 bug, title, summary, outcome |
| 결과 | type, state; 완료 상태에는 verification 필수 |
| 버그 | workflow, environment, steps, expected, actual; 해결·종료에는 changes와 verification 필수 |
| 운영 | priority, severity, reporter, assignee |
| 근거 | cause, changes, verification, remaining |
| 시간·검색 | proposedTime, actualTime, xerjUsage, xerjEvidence |

빠진 시간·검증·원인을 추측으로 채우지 않습니다. 작성자가 제공하지 않은 항목은 미기록 또는 미확인으로 표시합니다. 세부 필드는 JSON 템플릿을 기준으로 합니다. 이미 작성된 v1·과거 기록은 읽기 호환을 유지하지만 새 보고서는 공통 계약 v2로 작성합니다.
