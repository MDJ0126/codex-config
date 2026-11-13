# 저장 Excel에서 Windows 표시까지

2026-11-13 21:04 KST 기준, 무기 10750(흑조) 하나의 배율 변경을 저장 XLSX→실제 제작용 DDT→Windows 장비 표시까지 확인했습니다. 원작 밸런스를 바꾸기 위한 패치가 아니라 데이터 편집 경로 검증이며 원본은 복구했습니다.

## 편집과 계산 경계

원본 `Assets/GameData/Workbooks/OverdoxGameData.xlsx`는 31개 시트입니다. `Equipment`는 장비 공통 정보, `Gear`는 무기 전투 수치, `Weapons`는 실행·표시용 카탈로그입니다. 외부 원본과 기존 DDT 스키마는 변경하지 않습니다.

흑조는 Excel 행 39에 있습니다. `Gear!H39 = AW39*AX39`의 입력 120과 배율 1을 기준으로 검증 사본의 `AX39`만 1.1로 바꿨습니다. `Weapons!G39`는 Gear를 ID로 조회하고, `Balance!H39`는 기본값에 레벨 11 누적 성장량 83을 더합니다.

| 경계 | 기본 배율 | 검증 배율 |
| --- | ---: | ---: |
| Gear 기본 공격력 | 120 | 132 |
| Balance 레벨 11 | 203 | 215 |
| 실제 Windows 카드 Damage | 203 | 215 |
| 실제 Windows 장착 후 하단 공격력 | 213 | 226 |

카드와 하단 값은 같은 의미가 아닙니다. 하단은 `Fighter.Configure`의 `(AttackDamage + Effects.Sum(33)) * (1 + Effects.Sum(59))` 계산을 거쳐 `ToString("0")`으로 표시합니다. 이번 관찰은 고정된 방어구 구성에서 이루어졌으며 모든 보정 종류의 원작 동일성을 증명하지 않습니다.

```mermaid
flowchart LR
    A[저장 XLSX 사본 배율 1.1] --> B[WorkbookReader와 WorkbookCalculation]
    B --> C[ExcelGameDataCompiler.ReadTable]
    C --> D[Gear 및 Weapons DDT]
    D --> E[Windows Build24]
    E --> F[카드 215와 장착 공격력 226]
```

## 실제 전후 화면

기본 배율: 기존 고정 Build23에서 같은 흑조를 장착한 화면입니다. 사본 검증 후 재현 촬영했습니다.

![흑조 기본 배율 카드 203와 장착 공격력 213](../Verification/ExcelSavedRoundTrip/equipment-scale100-build23.png)

검증 배율: 저장 사본을 실제 제작 경로로 컴파일하고 빌드한 Build24입니다.

![흑조 검증 배율 카드 215와 장착 공격력 226](../Verification/ExcelSavedRoundTrip/equipment-scale110-build24.png)

빌드와 배율이 다른 비교이며 UI 레이아웃 수정 전후가 아닙니다. 캡처는 F12의 실제 카메라 렌더로 저장했고 원본 PNG는 보존했습니다.

## 반복 실행 방법과 주의점

1. `Tools/Spreadsheet/inspect_roundtrip_source.mjs`로 원본 주소·수식·값과 기준 화면을 확인합니다.
2. `create_saved_roundtrip_case.mjs`는 Artifact Tool로 검증 사본 한 개를 생성하고 저장 후 재열어 값을 확인합니다. 원본을 덮어쓰지 않습니다.
3. `compare_saved_roundtrip.py`는 ZIP XML을 읽기만 하여 31개 시트의 값·수식·스타일 ID·행열 및 비셀 메타데이터와 패키지 구성을 대조합니다. 실제 결과는 입력 1개와 종속 계산 4개만 변경, 스타일 바이트 동일, 시트 메타데이터 변경 없음입니다.
4. Unity 재생을 끄고 `OVERDOX > Data > Verify saved Excel through DDT`를 실행합니다. `SavedExcelDdtVerification.Verify`는 사본 파일을 실제 수식 엔진과 제작 컴파일러의 행 매핑으로 읽고 암호화 DDT를 저장한 뒤 파일을 다시 로딩합니다. 메모리 값 주입은 없습니다.
5. Windows 검증은 별도로 원본·DDT·카탈로그 백업 후 검증 사본을 기존 입력 경로에 연결하고 `Compile DDT catalog`, `Build Windows`를 실행했습니다. 기본 Build23을 보존하고 변경 빌드는 Build24로 고정했습니다.
6. 검증 후 백업을 복구하고 원본 Excel을 다시 컴파일했습니다. 백업 대상 30파일의 해시가 전부 일치했습니다. 작업용 `Builds/Windows`도 원래 데이터로 21:05:58 KST에 다시 빌드했고 Build25에 보존했습니다. Build23과 150파일을 비교하면 149파일이 같고 `boot.config`의 빌드 GUID만 다릅니다. 실제 대조 실행 대상은 Build23이며 Build25를 실행한 것으로 기록하지 않습니다.

검증용 스크립트의 고정 ID·예상 값은 의도한 회귀 조건입니다. 다른 장비·행 순서로 원본을 편집했을 때 강제로 통과하도록 바꾸지 말고 검증 조건과 실제 변경 범위를 함께 갱신해야 합니다. 검증 사본과 백업은 배포할 밸런스 데이터가 아닙니다.

## 증거와 범위

- `Docs/Verification/ExcelSavedRoundTrip/saved-file-verification.json`: 실제 저장·재열기 계산값.
- `saved-cell-comparison.json`: 31개 시트와 저장 패키지 대조.
- `unity-saved-ddt.json`: 실제 Unity 수식 엔진·행 매핑·저장 DDT 재로딩, 38행·공격력 132.
- `production-compiled-differences.json`: 실제 제작 컴파일 후 변경은 XLSX·Gear.bytes·Weapons.bytes.
- `production-recompiled-restored.json`: 원본 복구·재컴파일 후 30파일 해시 일치.
- `windows-verification.json`: Build23·24 실제 표시와 검사한 예외 0건. 모든 종류의 로그 오류가 없다는 보장은 아닙니다.
- `build24-manifest.json`, `build25-manifest.json`: 고정 실행 산출물 각 150파일의 해시.

검증 범위는 한 무기의 수식 변경입니다. 전체 119장비의 수식 변경 전수·임의 Excel 함수 호환·새 빌드 10시간 실행은 미확인입니다. 전체 복원 목표는 계속 진행합니다.
