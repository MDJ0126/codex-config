# 다중 조건 집계 추가 및 검증

추가한 함수는 `COUNTIFS`, `SUMIFS`, `AVERAGEIFS`입니다. 등록 수식은 총 30개이며, 수식 도움말의 예제와 9개 언어 설명, 9개 HTML 가이드 및 영어 PDF를 갱신했습니다.

```text
=COUNTIFS(C1:C5,">10",D1:D5,"<100")
=SUMIFS(D1:D5,C1:C5,">10")
=AVERAGEIFS(D1:D5,C1:C5,">10")
```

여러 조건은 같은 상대 위치에서 모두 만족해야 합니다. 합계·평균 범위는 첫 번째 인수입니다. 각 범위는 같은 행·열 크기를 가져야 하며, 최대 127개 조건 쌍과 범위당 100,000셀 제한을 적용합니다. 조건 텍스트, 비교, 와일드카드, 조건 셀 참조는 기존 단일 조건 함수와 같은 처리기를 사용합니다. 완전한 Excel 형 변환 호환은 지원하지 않습니다. 조건 안의 `&` 연결은 [후속 작업](CriteriaConcat.md)에서 추가했습니다.

## 검증 근거

- 전체 프로젝트 C# 소스를 Unity 참조 어셈블리에 대해 컴파일하고 **9,216개 회귀 검증**을 통과했습니다.
- 새 테스트는 독립적인 C# 행 필터 결과와 조건부 개수·합계·평균을 여러 경계값으로 비교합니다.
- 2차원 범위 정렬, 모든 조건의 교집합, 와일드카드, 일치 결과 없음, 범위 크기 불일치, 인수 누락, 과도한 조건 수, 순환 참조 및 비활성 IF 분기의 지연 평가를 검증했습니다.
- 영어 PDF 7페이지를 렌더링하여 페이지 배치를 확인했습니다.
- 최신 코드의 Unity 실제 실행은 **85개 항목 통과, 오류 기록 0개, 샘플 씬 3개 완료**입니다. 새 함수 3개를 Mono에서 실제 계산했습니다. [별도 JSONL](multi-criteria/unity-session.jsonl)과 [요약](multi-criteria/summary.json)을 남겼습니다.
- 검증 도구의 UTC 재해석 오류도 재현하여 수정했습니다. PowerShell이 JSON 시각을 이미 UTC DateTime으로 변환한 뒤 다시 문자열로 파싱하면 9시간 차이가 발생했습니다. UTC DateTime과 ISO 문자열 두 입력의 변환 및 오래된 결과 구분을 확인했고, 로그 생성 시각과 새 실행 기록 시각을 대조했습니다. Unity 실행 자체는 정상 완료했으며 수정 전 래퍼가 새 보고서를 오래된 것으로 잘못 거부했습니다.
- 첫 실행 요청은 자동 승인 검토 서비스의 모델 용량 오류로 실행되지 않았습니다. 동일 승인 절차로 재시도한 실행이 승인되어 위 결과를 확보했습니다.

## 남은 점검

마우스 드래그, 새 팝업 배치, IME와 다중 모니터 동작은 여전히 수동 UI 검증이 필요합니다. 이번 작업은 해당 항목을 통과 처리하지 않습니다. 기존 배포 패키지는 이전 스냅샷이며 다시 내보내지 않았습니다.

기능 비교에는 Microsoft 공식 [COUNTIFS](https://support.microsoft.com/en-gb/excel/functions/countifs-function), [SUMIFS](https://support.microsoft.com/en-au/excel/functions/sumifs-function), [AVERAGEIFS](https://support.microsoft.com/en-au/excel/functions/averageifs-function) 문서를 참고했습니다.

XERJ: 미사용 — 연결 도구가 없어 rg로 현재 소스를 확인했습니다.
