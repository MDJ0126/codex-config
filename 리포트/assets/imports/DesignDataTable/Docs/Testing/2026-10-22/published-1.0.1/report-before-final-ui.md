# 1.0.1 판매본 대비 2.0.0 변경 내역

비교 기준은 Publisher Portal의 기존 업로드(Unity 2021.3.19f1, 2024-09-16 15:53 UTC, 783KB)를 2026-10-22에 직접 다운로드한 실제 판매 패키지입니다. 페이지의 버전은 1.0.1입니다. 파일명이 새 초안 이름을 사용하더라도 내부 업로드 정보와 157개 경로는 기존 판매본입니다.

판매본 SHA-256: `9afb44c6817cadeb36e7f75947c635666b529de6c5d0d72d82cff2b8445cfcae`

현재 작업 폴더와 asset 내용 단위로 대조했습니다. `.meta`와 폴더 항목을 제외한 판매본 156개, 현재 383개 중 **추가 240개 / 삭제 13개 / 내용 변경 29개 / 동일 114개**입니다. 이 수치는 기능 개수가 아니라 파일 내용 비교이며, 이름 변경은 추가+삭제로 나타날 수 있습니다. UI 캡처를 반영한 패키지 재내보내기와 최종 업로드는 별도 단계입니다.

## 새로 추가되거나 확장된 기능

- **문서 편집 기반:** 기존 JSON 기반 테이블 편집에 더해 XLSX 원본 직접 읽기/쓰기, 여러 워크시트, 일반 문서와 등록 게임 테이블의 통합 편집 경로가 추가되었습니다.
- **작업 공간:** 리본, 파일별 창, 문서 탐색과 검색, 하단 문서 작업 표시줄, 최소화·복원, 창 위치/크기 저장이 추가되었습니다.
- **선택·입력:** 범위와 비연속 선택, 복사/잘라내기/붙여넣기 및 선택하여 붙여넣기, 채우기, 셀·행·열 편집 작업이 확장되었습니다.
- **검색:** 기존 소문자 부분 일치 검색에서 찾기/바꾸기, 와일드카드, 대소문자/셀 전체 조건을 지원하는 흐름으로 확장되었습니다.
- **시트 구성:** 행·열 삽입/삭제, 크기와 숨김, 정렬·필터, 틀 고정, 병합 셀 편집 작업이 추가되었습니다. 지원 제한은 안내서를 따릅니다.
- **계산:** 지원 수식 계산, 시트 간 및 상대/절대 참조, 정의된 이름, 의존성 계산과 동적 배열의 문서화된 하위 집합이 추가되었습니다.
- **서식:** 글꼴·정렬·채우기·테두리·표시 형식, 조건부 서식과 데이터 유효성 검사 흐름이 추가되었습니다.
- **시각 개체:** 지원 범위의 차트·이미지·도형 미리보기 및 개체 편집 도구가 추가되었습니다.
- **복구·진단:** 기존 Unity Undo/Redo에서 문서 편집 세션, 초안·백업, 제한된 복구, 작업 로그·진단·버그 보고서와 선택형 AI 컨텍스트 내보내기로 확장되었습니다.
- **게임 데이터 연계:** XLSX 시트 등록, 스키마·타입·ID 검사, 원본/생성 경로 구성, ScriptableObject 데이터 내보내기가 추가되었습니다. C#·바이너리 생성 자체는 기존 기능입니다.
- **언어·안내:** 9개 언어 UI 및 오프라인 HTML 안내서, 지원/미지원 범위 문서, 브랜드 아이콘 및 샘플 원본 구성과 Third-Party Notices가 추가되었습니다.

## 기존에도 있던 기능 — 신규로 표시하지 않음

CSV 가져오기/내보내기(TableInfo.cs와 DataTableEditor.cs), 암호화 및 바이너리 저장(DataLoader.cs), 타입이 있는 C# 클래스 생성(TableCreator.cs), partial 데이터 확장(기존 DataSource.txt), ID 기반 조회와 테마는 판매본에 존재합니다. Undo/Redo와 단순 검색도 기존 DataTableEditor.cs/FindPopup.cs에서 확인됩니다. 기존 게임/상점 예제 씬도 포함되어 있어 예제의 존재 자체를 신규로 설명하지 않습니다.

## 검증 한계 및 업그레이드 주의

파일 비교와 소스 확인은 수행했지만 기존 Unity 2021.3 프로젝트에서 2.0.0으로 올리는 실제 마이그레이션 실행은 미검증입니다. 업데이트 전에 프로젝트와 원본 파일을 백업해야 합니다. 완전한 Excel 호환, macOS, IL2CPP 또는 모든 대화형 경로 검증을 의미하지 않습니다. 현재 문서 스크린샷 중 일부는 재촬영이 남아 있습니다.

## 현재 기능 117개와 판매본 경로 근거

아래는 기능별 근거 파일의 판매본 포함 여부입니다. 새 파일 경로라는 사실만으로 해당 기능 전체가 신규라고 단정하지 않습니다. 세부 호환성은 안내서 및 테스트 기록을 참조합니다.

### E1.01 XLSX 파일 직접 읽기
- Assets/DesignDataTable/Scripts/Editor/WorkbookReader.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E1.02 CSV 파일 직접 읽기
- Assets/DesignDataTable/Scripts/Editor/WorkbookReader.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E1.03 일반 문서와 등록된 게임 테이블의 편집 경로 구분
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewer.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E1.04 여러 시트 탐색
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewer.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E1.05 시트 추가
- Assets/DesignDataTable/Scripts/Editor/WorkbookStructure.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E1.06 시트 이름 변경
- Assets/DesignDataTable/Scripts/Editor/WorkbookStructure.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E1.07 시트 삭제
- Assets/DesignDataTable/Scripts/Editor/WorkbookStructure.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E1.08 원본 저장과 다른 이름 저장
- Assets/DesignDataTable/Scripts/Editor/WorkbookStructure.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E1.09 새 XLSX 문서 생성
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditSession.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E1.10 메인 색상·작업 환경 설정 저장
- Assets/DesignDataTable/Scripts/Editor/WorkspaceSettings.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E1.11 표시 언어 설정·언어 테이블
- Assets/DesignDataTable/Scripts/Editor/Localization/SettingsLocalization.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E2.01 셀·범위 값 편집
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditSession.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E2.02 인라인 및 수식 입력 편집
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewerEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E2.03 사각 범위 마우스 선택
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewerGrid.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E2.04 떨어진 선택 셀의 간격 보존 복사·붙여넣기
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditingOperations.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E2.05 떨어진 셀 잘라내기 및 원본 비우기
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewerEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E2.06 외부 TSV 표 붙여넣기 경로
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewer.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E2.07 일반 참조 수식 이동
- Assets/DesignDataTable/Scripts/Editor/WorkbookStructure.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E2.08 연속 값·수식 자동 채우기
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditingOperations.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E2.09 여러 선택 셀에 일괄 입력
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditingOperations.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E2.10 선택 셀 내용 빈칸으로 지우기
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewerEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E2.11 Tab·Shift-Tab·Ctrl/Command 키 입력 경로
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewer.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E2.12 Esc로 복사 상태 해제
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewer.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E2.13 구조화 참조·상대 이름을 포함한 모든 잘라내기 참조 변환
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationTables.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditingOperations.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookSpillClipboard.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookTableReferences.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookTableReferences.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookTableReferences.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookTableReferences.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookTableReferences.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E2.14 복사·잘라내기 점선 표시 경로
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewerGrid.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E3.01 행·열 선택 및 우클릭 구조 편집
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewerGrid.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E3.02 여러 행·열 삽입·삭제
- Assets/DesignDataTable/Scripts/Editor/WorkbookStructure.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E3.03 열 너비 변경
- Assets/DesignDataTable/Scripts/Editor/WorkbookFormatting.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E3.04 행 높이 변경
- Assets/DesignDataTable/Scripts/Editor/WorkbookFormatting.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E3.05 선택 행·열 자동 맞춤
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewerEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookCellTextGUI.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E3.06 여러 열 숨기기·다시 표시
- Assets/DesignDataTable/Scripts/Editor/WorkbookFormatting.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E3.07 여러 행 숨기기·다시 표시
- Assets/DesignDataTable/Scripts/Editor/WorkbookFormatting.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E3.08 행·열 틀 고정
- Assets/DesignDataTable/Scripts/Editor/WorkbookFormatting.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E3.09 행·열 경계 드래그 처리
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewerGrid.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E4.01 오름차순·내림차순 정렬
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditingOperations.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E4.02 여러 열 값 필터
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditingOperations.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E4.03 조건 조합 필터
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditingOperations.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E4.04 상위·하위 개수/퍼센트 필터
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditingOperations.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E4.05 날짜 그룹 필터
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditingOperations.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E4.06 지원된 셀·글자 색 필터
- Assets/DesignDataTable/Scripts/Editor/WorkbookColorFilters.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E4.07 패턴·그라데이션 색 필터
- Assets/DesignDataTable/Scripts/Editor/WorkbookFillColors.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookGradientFill.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookGradientGUI.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookFillPaint.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookFillFormat.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookFillControls.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookFillReading.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookColorFilters.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookToolPopups.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewerEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookFillPaint.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookColorFilters.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookColorFilters.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookToolPopups.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewerEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookTableStyleColors.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E4.08 테이블 스타일·시스템 팔레트·아이콘 기반 필터
- Assets/DesignDataTable/Scripts/Editor/WorkbookColors.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookTableStyleColors.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookColorFilters.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookIconFilters.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditingOperations.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookToolPopups.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookColors.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/DataTablePreferences.cs — 판매본에 같은 경로 있음 — 내용 변경; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookColors.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkspaceSettings.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewer.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditingOperations.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewerEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditSession.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E4.09 대체 달력과 전체 로케일 필터
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditingOperations.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookToolPopups.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditingOperations.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditingOperations.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookToolPopups.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditingOperations.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditingOperations.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookToolPopups.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditingOperations.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookToolPopups.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditingOperations.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookToolPopups.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E5.01 산술 연산과 기존 집계 함수
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculation.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E5.02 조건·논리·오류 처리
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculation.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E5.03 기본·고급 조회 함수 경로
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculation.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E5.04 배열 상수와 브로드캐스팅
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationArrayMath.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E5.05 동적 배열 유출 영역 계산
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationSpills.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E5.06 유출 영역 저장·재배치
- Assets/DesignDataTable/Scripts/Editor/WorkbookSpillStorage.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E5.07 유출 영역 클립보드 처리
- Assets/DesignDataTable/Scripts/Editor/WorkbookSpillClipboard.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E5.08 SEQUENCE/FILTER/SORT/UNIQUE/SORTBY
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationArrayMath.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E5.09 TRANSPOSE/TAKE/DROP/CHOOSE 계열
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationArrayMath.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E5.10 VSTACK/HSTACK/TOCOL/TOROW
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationArrayMath.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E5.11 TEXTJOIN/TEXTBEFORE/TEXTAFTER
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationText.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E5.12 TEXTSPLIT/WRAPROWS/WRAPCOLS
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationText.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E5.13 LET 지역 바인딩
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationBindings.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E5.14 수식 도움말 및 9개 언어 설명
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookToolPopups.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E5.15 정의된 이름 관리
- Assets/DesignDataTable/Scripts/Editor/WorkbookNames.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E5.16 LAMBDA 및 사용자 정의 함수 값
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationLambda.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationLambda.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculation.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookNames.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E5.17 상대·동적·구조화 이름과 전체 의존성 호환
- Assets/DesignDataTable/Scripts/Editor/WorkbookTableReferences.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationTables.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationRanges.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationBindings.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookStructure.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationRanges.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculation.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationRanges.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditSession.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookDebugContext.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationRanges.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationSpills.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookNamePopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookNames.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookNamePopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E5.18 기존 수식 함수의 미지원 인수·모드 해소
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationLambda.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationRanges.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationRanges.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationArrayMath.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationLambdaArrays.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationRanges.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E5.19 현재 카탈로그 밖 수식 지원 범위 확정·구현
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationLambdaArrays.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationRanges.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationLookups.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookToolPopups.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationArrayMath.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationArrayMath.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Documentation/FormulaSupportScope.json — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Tools/Reporting/formula_scope.py — 기준 커밋에 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationArrayMath.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationRanges.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationArrayMath.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationText.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookXmlText.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationText.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationArrayMath.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditSession.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookToolPopups.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewerEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationSpills.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E6.01 글꼴·크기·굵게·기울임
- Assets/DesignDataTable/Scripts/Editor/WorkbookFormatting.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E6.02 글자색·배경색
- Assets/DesignDataTable/Scripts/Editor/WorkbookFormatting.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E6.03 기본 테두리
- Assets/DesignDataTable/Scripts/Editor/WorkbookFormatting.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E6.04 가로·세로 맞춤 및 자동 줄바꿈
- Assets/DesignDataTable/Scripts/Editor/WorkbookFormatting.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E6.05 지원 숫자·날짜·분수 표시 형식
- Assets/DesignDataTable/Scripts/Editor/WorkbookFormatting.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E6.06 셀 병합·병합 해제
- Assets/DesignDataTable/Scripts/Editor/WorkbookFormatting.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E6.07 지원 조건부 서식 작성·수정·삭제·순서 변경
- Assets/DesignDataTable/Scripts/Editor/WorkbookConditionalEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E6.08 조건부 색조·데이터 막대 표시
- Assets/DesignDataTable/Scripts/Editor/WorkbookDataBars.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookColorScales.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E6.09 조건부 차등 서식 전체 속성
- Assets/DesignDataTable/Scripts/Editor/WorkbookConditionalFont.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookConditionalEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookTextDecoration.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookBorderFormat.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookBorderGUI.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookConditionalFont.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookAlignmentFormat.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookAlignmentControls.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookAlignmentFormat.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookAlignmentControls.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookCellTextGUI.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookConditionalFont.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewerGrid.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookFormatting.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookFontMetadata.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookFontMetadata.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookFontMetadataControls.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookConditionalFormats.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookConditionalEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookConditionalPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookReader.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewerEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookReader.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookPackage.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookSpillStorage.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditingOperations.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookFormatting.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookToolPopups.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookFontMetadata.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookCellTextGUI.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookFontMetadata.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookFormatting.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookToolPopups.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E6.10 상대·혼합 참조 및 퇴화 임계값의 조건부 서식
- Assets/DesignDataTable/Scripts/Editor/WorkbookStructure.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookColorScales.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookDataBars.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookColorScaleEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookConditionalPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookScaleFormula.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookConditionalFormats.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewerGrid.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookDiagnostics.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E6.11 x14 데이터 막대와 아이콘 집합
- Assets/DesignDataTable/Scripts/Editor/WorkbookIconSets.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookIconGUI.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookExtendedIcons.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookStructure.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookConditionalEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookIconEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookIconPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookExtendedDataBars.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookDataBarGUI.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookDataBarEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookDataBarPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E6.12 전체 로케일·혼합 표시 형식
- Assets/DesignDataTable/Scripts/Editor/WorkbookFormatting.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookFormatting.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookToolPopups.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E7.01 지원 이미지·도형 읽기 및 표시 모델
- Assets/DesignDataTable/Scripts/Editor/WorkbookDrawings.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E7.02 이미지·도형 삽입과 텍스트 편집
- Assets/DesignDataTable/Scripts/Editor/WorkbookObjects.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E7.03 지원 개체 이동·크기·삭제
- Assets/DesignDataTable/Scripts/Editor/WorkbookObjects.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E7.04 개체 복사·잘라내기·붙여넣기
- Assets/DesignDataTable/Scripts/Editor/WorkbookObjects.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E7.05 그룹 내부 개체 편집
- Assets/DesignDataTable/Scripts/Editor/WorkbookObjects.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E7.06 그룹 내부 개체 복사·관계 재연결
- Assets/DesignDataTable/Scripts/Editor/WorkbookObjects.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E7.07 반전된 부모 그룹 좌표 처리
- Assets/DesignDataTable/Scripts/Editor/WorkbookDrawingGroups.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E7.08 지원 2D 차트 종류 읽기·표시
- Assets/DesignDataTable/Scripts/Editor/WorkbookCharts.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E7.09 차트 삽입과 데이터 범위 편집
- Assets/DesignDataTable/Scripts/Editor/WorkbookChartEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E7.10 차트 계열·포인트 색과 투명도 편집
- Assets/DesignDataTable/Scripts/Editor/WorkbookChartEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E7.11 산점도 범위·로그 축 편집
- Assets/DesignDataTable/Scripts/Editor/WorkbookChartEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E7.12 막대·꺾은선 혼합 차트와 누적 배치
- Assets/DesignDataTable/Scripts/Editor/WorkbookChartEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E7.13 혼합 차트 보조 축 범위·로그·방향 편집
- Assets/DesignDataTable/Scripts/Editor/WorkbookChartEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E7.14 혼합 차트 꺾은선 기본 축·보조 축 전환
- Assets/DesignDataTable/Scripts/Editor/WorkbookChartEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E7.15 회전된 부모 그룹·일반 아핀/전단 변환
- Assets/DesignDataTable/Scripts/Editor/WorkbookDrawingAffine.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookObjects.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookDrawingGUI.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookObjects.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookObjectPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookObjectGrouping.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookObjectPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E7.16 그룹 생성·해제 및 그룹 내부 차트
- Assets/DesignDataTable/Scripts/Editor/WorkbookObjectGrouping.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookObjectPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCharts.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookObjects.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookChartGUI.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookObjectGrouping.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookDrawingAffine.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookObjectGrouping.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookObjectPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E7.17 복잡한·3D·부드러운 곡선 차트
- Assets/DesignDataTable/Scripts/Editor/WorkbookCharts.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookChartGUI.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookChartEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookObjectPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookBubbleCharts.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookBubbleCharts.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookChartGUI.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookChartEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookChartPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookBubbleEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookObjectPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookChartEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookObjectPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookBubbleEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookPackage.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookNames.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookChartEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookBubbleCharts.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookChartGUI.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookBubbleEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookStockCharts.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookStockEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookStockEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookStockEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookStockEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookStockDates.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookChartEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookChartEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookObjectPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookChartMath.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCharts.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookChartGUI.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E7.18 계열별 차트 종류·축 자유 변경
- Assets/DesignDataTable/Scripts/Editor/WorkbookChartSeriesLayers.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookObjectPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookChartGUI.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCharts.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookChartEditing.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookObjectPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E7.19 전체 개체 스타일·부모 확장 메타데이터 편집
- Assets/DesignDataTable/Scripts/Editor/WorkbookObjects.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookObjectPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookObjects.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookDrawings.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookDrawingGUI.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookObjectPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookObjects.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookDrawings.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookObjectPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookObjects.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookObjectPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookObjects.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookObjectPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookObjects.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookObjectPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookObjects.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookDrawingGUI.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookObjects.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookObjects.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookObjects.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookObjectPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E7.20 외부 데이터의 표시·숨김 완전 지원
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditSession.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCharts.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookObjectPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditSession.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCharts.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditSession.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookDebugPopup.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditSession.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculation.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculation.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationTables.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True
- Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationRanges.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E8.01 원본 ZIP 항목 보관 구조
- Assets/DesignDataTable/Scripts/Editor/WorkbookPackage.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E8.02 셀 편집 시 기존 XML 병합
- Assets/DesignDataTable/Scripts/Editor/WorkbookPreservation.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E8.03 지원되지 않는 변경 차단
- Assets/DesignDataTable/Scripts/Editor/WorkbookPreservation.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E8.04 원본 백업 저장
- Assets/DesignDataTable/Scripts/Editor/WorkbookPreservation.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E8.05 CSV 인코딩·BOM·구분자 저장 유지
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditSession.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E8.06 전체 실제 문서의 개체·관계·스타일 편집 보존

### E9.01 실행 취소·다시 실행
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditSession.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E9.02 실패한 편집의 상태 롤백
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditSession.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E9.03 변경 이력 저장·복구
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditSession.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E9.04 입력 중 초안 복구
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewerRecovery.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E9.05 복구 저널 저장·로드
- Assets/DesignDataTable/Scripts/Editor/WorkbookRecovery.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E9.06 원본 외부 변경 충돌 검사
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditSession.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E9.07 임시 파일 후 원자적 원본 교체
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditSession.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E9.08 도메인 리로드 직렬화 경로
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewer.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E9.09 복구 정리 실패 안내 보관
- Assets/DesignDataTable/Scripts/Editor/WorkbookEditSession.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E10.01 작업·오류 진단 이벤트 기록
- Assets/DesignDataTable/Scripts/Editor/WorkbookDiagnostics.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E10.02 로그 파일 크기·보관 기간 제한
- Assets/DesignDataTable/Scripts/Editor/WorkbookDiagnostics.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E10.03 버그 리포트 작성 UI
- Assets/DesignDataTable/Scripts/Editor/Window/WorkbookBugReportWindow.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E10.04 리포트 ZIP 내보내기
- Assets/DesignDataTable/Scripts/Editor/WorkbookDiagnostics.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E10.05 로그 기록 켜기·끄기
- Assets/DesignDataTable/Scripts/Editor/WorkbookDiagnostics.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E10.06 AI용 구조화 디버그 컨텍스트
- Assets/DesignDataTable/Scripts/Editor/WorkbookDebugContext.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E10.07 게임 테이블 AI 디버그 컨텍스트
- Assets/DesignDataTable/Scripts/Editor/WorkbookDebugContext.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

### E10.08 민감 셀 내용 기본 제외·수동 공유
- Assets/DesignDataTable/Scripts/Editor/WorkbookDiagnostics.cs — 판매본에 같은 경로 없음; 현재 파일 존재: True

## 전체 파일 변경 목록
### 추가 (240)
- `Assets/DesignDataTable/.xerjignore`
- `Assets/DesignDataTable/Docs/index.de.html`
- `Assets/DesignDataTable/Docs/index.en.html`
- `Assets/DesignDataTable/Docs/index.es.html`
- `Assets/DesignDataTable/Docs/index.fr.html`
- `Assets/DesignDataTable/Docs/index.html`
- `Assets/DesignDataTable/Docs/index.ja.html`
- `Assets/DesignDataTable/Docs/index.ko.html`
- `Assets/DesignDataTable/Docs/index.pt-BR.html`
- `Assets/DesignDataTable/Docs/index.zh-Hans.html`
- `Assets/DesignDataTable/Docs/index.zh-Hant.html`
- `Assets/DesignDataTable/Docs/style.css`
- `Assets/DesignDataTable/Documentation/AutomationCLI.html`
- `Assets/DesignDataTable/Documentation/FormulaSupportScope.json`
- `Assets/DesignDataTable/Documentation/README.md`
- `Assets/DesignDataTable/Documentation/ScreenshotManifest.json`
- `Assets/DesignDataTable/Documentation/UserGuide.de.html`
- `Assets/DesignDataTable/Documentation/UserGuide.en.html`
- `Assets/DesignDataTable/Documentation/UserGuide.en.pdf`
- `Assets/DesignDataTable/Documentation/UserGuide.es.html`
- `Assets/DesignDataTable/Documentation/UserGuide.fr.html`
- `Assets/DesignDataTable/Documentation/UserGuide.ja.html`
- `Assets/DesignDataTable/Documentation/UserGuide.ko.html`
- `Assets/DesignDataTable/Documentation/UserGuide.pt-BR.html`
- `Assets/DesignDataTable/Documentation/UserGuide.zh-Hans.html`
- `Assets/DesignDataTable/Documentation/UserGuide.zh-Hant.html`
- `Assets/DesignDataTable/Documentation/Verification.md`
- `Assets/DesignDataTable/Documentation/guide.css`
- `Assets/DesignDataTable/Documentation/images/01-shop-scene.png`
- `Assets/DesignDataTable/Documentation/images/02-game-scene.png`
- `Assets/DesignDataTable/Documentation/images/clipboard-range-paste-ui.jpg`
- `Assets/DesignDataTable/Documentation/images/clipboard-sparse-paste-ui.jpg`
- `Assets/DesignDataTable/Documentation/images/closed-shape-gradient-controls-ui.jpg`
- `Assets/DesignDataTable/Documentation/images/closed-shape-gradient-ui.jpg`
- `Assets/DesignDataTable/Documentation/images/embedded-number-literals-ui.jpg`
- `Assets/DesignDataTable/Documentation/images/empty-text-coercion-ui.jpg`
- `Assets/DesignDataTable/Documentation/images/formula-bar-game-fixed.jpg`
- `Assets/DesignDataTable/Documentation/images/formula-bar-viewer-fixed.jpg`
- `Assets/DesignDataTable/Documentation/images/general-section-format-ui.jpg`
- `Assets/DesignDataTable/Documentation/images/index-zero-axis-ui.jpg`
- `Assets/DesignDataTable/Documentation/images/locale-calendar-month-preview.jpg`
- `Assets/DesignDataTable/Documentation/images/logical-text-coercion-ui.jpg`
- `Assets/DesignDataTable/Documentation/images/ribbon-screen-tip-copy.jpg`
- `Assets/DesignDataTable/Documentation/images/selection-add-mode-ui.jpg`
- `Assets/DesignDataTable/Documentation/images/unified-game-data-tools.jpg`
- `Assets/DesignDataTable/Documentation/images/unified-game-home.jpg`
- `Assets/DesignDataTable/Documentation/images/unified-general-home.jpg`
- `Assets/DesignDataTable/Documentation/images/window-motion-minimized.jpg`
- `Assets/DesignDataTable/Documentation/images/window-motion-restored.jpg`
- `Assets/DesignDataTable/Documentation/index.html`
- `Assets/DesignDataTable/Editor/Icons/Csv.png`
- `Assets/DesignDataTable/Editor/Icons/Csv16.png`
- `Assets/DesignDataTable/Editor/Icons/DesignDataTableLogo.png`
- `Assets/DesignDataTable/Editor/Icons/RibbonAtlas.png`
- `Assets/DesignDataTable/Editor/Icons/RibbonAtlasWhite.png`
- `Assets/DesignDataTable/Editor/Icons/Workbook.png`
- `Assets/DesignDataTable/Editor/Icons/Workbook16.png`
- `Assets/DesignDataTable/Generated/Binary/SampleCharacter.bytes`
- `Assets/DesignDataTable/Generated/Code/SampleCharacter.cs`
- `Assets/DesignDataTable/Generated/Code/SampleCharacterTable.cs`
- `Assets/DesignDataTable/Generated/Code/SampleDropItem.cs`
- `Assets/DesignDataTable/Generated/Code/SampleDropItemTable.cs`
- `Assets/DesignDataTable/Generated/Code/SampleShopProduct.cs`
- `Assets/DesignDataTable/Generated/Code/SampleShopProductTable.cs`
- `Assets/DesignDataTable/Generated/Code/SampleSkill.cs`
- `Assets/DesignDataTable/Generated/Code/SampleSkillTable.cs`
- `Assets/DesignDataTable/Sample/Scripts/DataExtensions/SampleSkill.Extension.cs`
- `Assets/DesignDataTable/Scripts/Editor/CellClipboardSnapshot.cs`
- `Assets/DesignDataTable/Scripts/Editor/CellSelectionSet.cs`
- `Assets/DesignDataTable/Scripts/Editor/ClipboardBorderAnimation.cs`
- `Assets/DesignDataTable/Scripts/Editor/ClipboardOutlineState.cs`
- `Assets/DesignDataTable/Scripts/Editor/ColumnAutoFit.cs`
- `Assets/DesignDataTable/Scripts/Editor/CsvCodec.cs`
- `Assets/DesignDataTable/Scripts/Editor/DimensionResizeBatch.cs`
- `Assets/DesignDataTable/Scripts/Editor/FormulaEngine.cs`
- `Assets/DesignDataTable/Scripts/Editor/FormulaReferenceToggle.cs`
- `Assets/DesignDataTable/Scripts/Editor/FormulaText.cs`
- `Assets/DesignDataTable/Scripts/Editor/HeaderSelectionSet.cs`
- `Assets/DesignDataTable/Scripts/Editor/Localization/AdvancedLanguageTable.cs`
- `Assets/DesignDataTable/Scripts/Editor/Localization/EditorLanguageTable.cs`
- `Assets/DesignDataTable/Scripts/Editor/Localization/RibbonHelpLanguageTable.cs`
- `Assets/DesignDataTable/Scripts/Editor/Localization/SettingsLanguageTable.cs`
- `Assets/DesignDataTable/Scripts/Editor/Localization/SettingsLocalization.cs`
- `Assets/DesignDataTable/Scripts/Editor/PasteSpecial.cs`
- `Assets/DesignDataTable/Scripts/Editor/QuickFormula.cs`
- `Assets/DesignDataTable/Scripts/Editor/ScriptableObjectExport.cs`
- `Assets/DesignDataTable/Scripts/Editor/SelectionSpecial.cs`
- `Assets/DesignDataTable/Scripts/Editor/SelectionSummary.cs`
- `Assets/DesignDataTable/Scripts/Editor/SelectionTraversal.cs`
- `Assets/DesignDataTable/Scripts/Editor/SheetAddress.cs`
- `Assets/DesignDataTable/Scripts/Editor/SheetNavigation.cs`
- `Assets/DesignDataTable/Scripts/Editor/SheetViewport.cs`
- `Assets/DesignDataTable/Scripts/Editor/SheetVisibility.cs`
- `Assets/DesignDataTable/Scripts/Editor/Sources/ScriptableObjectSource.txt`
- `Assets/DesignDataTable/Scripts/Editor/TableAutoFill.cs`
- `Assets/DesignDataTable/Scripts/Editor/TableClipboard.cs`
- `Assets/DesignDataTable/Scripts/Editor/TableCut.cs`
- `Assets/DesignDataTable/Scripts/Editor/TableDiagnostics.cs`
- `Assets/DesignDataTable/Scripts/Editor/TableExport.cs`
- `Assets/DesignDataTable/Scripts/Editor/TableFilters.cs`
- `Assets/DesignDataTable/Scripts/Editor/TableImport.cs`
- `Assets/DesignDataTable/Scripts/Editor/TablePaths.cs`
- `Assets/DesignDataTable/Scripts/Editor/TableRowLayout.cs`
- `Assets/DesignDataTable/Scripts/Editor/TableSearch.cs`
- `Assets/DesignDataTable/Scripts/Editor/TableStructure.cs`
- `Assets/DesignDataTable/Scripts/Editor/TableWorkbook.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/ColumnFilterPopup.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/FindReplacePopup.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/FormulaHelpPopup.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/PasteSpecialPopup.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/RibbonHelp.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/SpreadsheetRibbon.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/SpreadsheetRibbonIcons.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/SpreadsheetScreenTip.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/TableDiagnosticsPopup.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WindowPlacement.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookAlignmentControls.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookBorderGUI.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookBugReportWindow.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookCellTextGUI.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookChartGUI.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookChartPopup.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookConditionalPopup.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookControls.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookDataBarGUI.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookDataBarPopup.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookDebugPopup.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookDrawingGUI.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookFillControls.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookFontMetadataControls.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookGradientGUI.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookIconGUI.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookIconPopup.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookInteractionMetrics.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookMinimizedShelf.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookNamePopup.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookNativeWindow.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookObjectPopup.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookPolygonGUI.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookPopup.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookRegistrationPopup.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookRenderCache.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookTaskbarLayout.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookTaskbarMenu.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookTextDecoration.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookToolPopups.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewer.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewerChrome.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewerEditing.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewerGameTools.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewerGrid.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewerObjects.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookViewerRecovery.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkbookWindowTheme.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/WorkspaceFileMenu.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookAlignmentFormat.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookAssets.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookAutomation.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookBorderFormat.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookBubbleCharts.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookBubbleEditing.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookCalculation.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationArrayMath.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationArrays.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationBindings.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationLambda.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationLambdaArrays.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationLookups.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationRanges.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationSpills.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationSubtotal.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationTables.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookCalculationText.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookChartEditing.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookChartMath.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookChartSeriesLayers.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookCharts.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookColorFilters.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookColorScaleEditing.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookColorScales.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookColors.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookConditionalEditing.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookConditionalFont.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookConditionalFormats.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookConditionalNumberFormat.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookConversion.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookDataBarEditing.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookDataBars.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookDebugContext.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookDiagnostics.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookDrawingAffine.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookDrawingGroups.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookDrawings.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookEditSession.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookEditingOperations.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookExtendedDataBars.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookExtendedIcons.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookFilePaths.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookFillColors.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookFillFormat.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookFillPaint.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookFillReading.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookFontMetadata.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookFormatting.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookFrameGeometry.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookGameMode.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookGradientFill.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookHealth.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookIconEditing.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookIconFilters.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookIconSets.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookNames.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookObjectGesture.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookObjectGrouping.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookObjects.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookPackage.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookPreservation.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookReader.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookRecovery.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookRegistration.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookScaleFormula.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookSearch.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookSharedFormulas.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookSourceImporter.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookSpillClipboard.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookSpillStorage.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookStockCharts.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookStockDates.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookStockEditing.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookStructure.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookTableReferences.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookTableStyleColors.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkbookXmlText.cs`
- `Assets/DesignDataTable/Scripts/Editor/WorkspaceSettings.cs`
- `Assets/DesignDataTable/Scripts/Editor/ddt_cli.py`
- `Assets/DesignDataTable/Tables/SampleCharacter.xlsx`
- `Assets/DesignDataTable/Tables/SampleDropItem.xlsx`
- `Assets/DesignDataTable/Tables/SampleShopProduct.xlsx`
- `Assets/DesignDataTable/Tables/SampleSkill.xlsx`
- `Assets/DesignDataTable/Third-Party Notices.txt`

### 내용 변경 (29)
- `Assets/DesignDataTable/DataTableSetupGuide.pdf`
- `Assets/DesignDataTable/Resources/TableManagement.asset`
- `Assets/DesignDataTable/Sample/Resources/Prefabs/Characters/Enemy1.prefab`
- `Assets/DesignDataTable/Sample/Resources/Prefabs/Characters/Enemy2.prefab`
- `Assets/DesignDataTable/Sample/Resources/Prefabs/Characters/Enemy3.prefab`
- `Assets/DesignDataTable/Sample/Resources/Prefabs/Characters/Enemy4.prefab`
- `Assets/DesignDataTable/Sample/Resources/Prefabs/Characters/Player.prefab`
- `Assets/DesignDataTable/Sample/Resources/Prefabs/Projectiles/AxeProjectile.prefab`
- `Assets/DesignDataTable/Sample/Scenes/Example - Game.unity`
- `Assets/DesignDataTable/Sample/Scenes/Example - UI (Shop).unity`
- `Assets/DesignDataTable/Sample/Scripts/Objects/Character/CharacterManager.cs`
- `Assets/DesignDataTable/Sample/Scripts/Objects/HUD/HUDManager.cs`
- `Assets/DesignDataTable/Sample/Scripts/Objects/HUD/HpBar.cs`
- `Assets/DesignDataTable/Sample/Scripts/Objects/Projectile/ProjectileManager.cs`
- `Assets/DesignDataTable/Sample/StaticResources/Game/Background/Background.mat`
- `Assets/DesignDataTable/Scripts/DataLoader.cs`
- `Assets/DesignDataTable/Scripts/Editor/TableCreator.cs`
- `Assets/DesignDataTable/Scripts/Editor/TableInfo/Column.cs`
- `Assets/DesignDataTable/Scripts/Editor/TableInfo/Row.cs`
- `Assets/DesignDataTable/Scripts/Editor/TableInfo/TableInfo.cs`
- `Assets/DesignDataTable/Scripts/Editor/TableManagementEditorExtension.cs`
- `Assets/DesignDataTable/Scripts/Editor/Utils.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/AboutPopup.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/DataTableEditor.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/DataTablePreferences.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/FindPopup.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/SelectPopup.cs`
- `Assets/DesignDataTable/Scripts/Editor/Window/ValueInputPopup.cs`
- `Assets/DesignDataTable/Scripts/TableManagement.cs`

### 삭제 (13)
- `Assets/DesignDataTable/Sample/Tables/SampleCharacter/SampleCharacter.cs`
- `Assets/DesignDataTable/Sample/Tables/SampleCharacter/SampleCharacter.json`
- `Assets/DesignDataTable/Sample/Tables/SampleCharacter/SampleCharacterTable.cs`
- `Assets/DesignDataTable/Sample/Tables/SampleDropItem/SampleDropItem.cs`
- `Assets/DesignDataTable/Sample/Tables/SampleDropItem/SampleDropItem.json`
- `Assets/DesignDataTable/Sample/Tables/SampleDropItem/SampleDropItemTable.cs`
- `Assets/DesignDataTable/Sample/Tables/SampleShopProduct/SampleShopProduct.cs`
- `Assets/DesignDataTable/Sample/Tables/SampleShopProduct/SampleShopProduct.json`
- `Assets/DesignDataTable/Sample/Tables/SampleShopProduct/SampleShopProductTable.cs`
- `Assets/DesignDataTable/Sample/Tables/SampleSkill/SampleSkill.Extension.cs`
- `Assets/DesignDataTable/Sample/Tables/SampleSkill/SampleSkill.cs`
- `Assets/DesignDataTable/Sample/Tables/SampleSkill/SampleSkill.json`
- `Assets/DesignDataTable/Sample/Tables/SampleSkill/SampleSkillTable.cs`
