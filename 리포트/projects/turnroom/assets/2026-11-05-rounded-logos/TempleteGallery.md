# Templete · TurnRoom UI 모체

## 턴룸 전역 설정 · 1.7.1

`Tools > TurnRoom > Settings`와 제품의 전역 설정 버튼은 같은 공통 팝업을 엽니다. 표시 언어는 `TemplateLanguage`를 통해 계정의 EditorPrefs에 저장하며 제품과 Unity 프로젝트 사이에 공유합니다. 기존 제품의 언어는 공유 값이 없는 첫 접근에 한 번만 이전합니다. 서로 다른 기존 설정이 있으면 먼저 접근한 설정이 초기값이 되며 이후 전역 선택이 우선합니다. 다른 에디터의 설정 변경은 0.5초 간격으로 관찰합니다. 소스 주석·게임 데이터·런타임 예제 언어는 별도입니다. 번역이 없는 제품의 기존 영어 문구를 자동 번역하지는 않습니다.

설치 버전은 각 제품의 `Version.json`에서 읽습니다. `assetStoreId`가 확인된 제품만 Unity Asset Store 공개 페이지에서 직접 확인합니다. 별도 배포 목록 주소 입력·저장·서버 요청은 제거했습니다. 미등록 제품은 등록 정보 없음으로 표시하고 Templete는 제품과 함께 제공되는 항목으로 구분합니다. 공개 페이지의 canonical 제품 ID와 latestVersion을 함께 검증하며 로그인·차단·형식 변경·모호한 버전은 확인 불가로 처리합니다. 제품별 요청을 순차 처리하고 실패 시 과거 성공을 남기지 않습니다.

Unity 6000.0.83f1의 실제 JsonUtility로 Unity 공식 제품 페이지를 파싱하고, 실제 등록 제품의 네트워크 요청 완료·오류 상태·씬/선택/포커스 보존 및 분리된 UXML에서 주소 입력란 제거를 검증했습니다. 실제 새 화면 캡처는 포커스를 변경하지 않는 PrintWindow가 실패하여 미확인입니다. 기존 캡처 해시는 보존합니다. 상세 근거는 `Docs/Testing/2026-11-02/store-update/`에 있습니다. 원래 4개 제품·영상 작업은 사용자 요청으로 일시정지 상태를 유지합니다.

제품 기능을 복사하지 않고 창의 골격을 공유합니다. 폴더 이름 `Templete`는 개발 프로젝트와 배포 패키지에서 같습니다.

## 구조

아이콘이 없는 창은 `CreateDocumentChrome`의 `brand`에 `null`을 전달합니다. 공통 프레임은 빈 아이콘 슬롯을 만들지 않고 제목을 왼쪽12px에 배치합니다. 아이콘이 있는 창은 기존 배치를 유지합니다. IMGUI의 `DrawTemplateHeader`도 같은 `null` 계약을 사용하며, 아이콘 없는 빠른 명령은12px부터 배치합니다.

- `TemplateWindow`: UI Toolkit과 기존 IMGUI 창의 제목줄을 조립하는 모체입니다.
- `ToolWindow`: 모체를 상속하고 제품 ID·창 ID·목록 등록 계약을 추가합니다.
- `TemplateAssetInspector` / `TemplateImporterInspector`: 생성 에셋과 임포터의 제목·카드·속성·작업 버튼을 공유합니다. [확장 방법](SharedAssetInspectors.md)
- `UITemplate`, `UITemplateStyles`, `DefaultUITemplate.asset`: DataTable 기준 배치와 크기·글자·색상을 관리합니다.
- `WindowFrame`, `WindowServices`, `WindowList`: 창 제어와 최소화·복원을 공유합니다. DataTable이 있으면 기존 엔진의 어댑터를 사용합니다.
- `ScreenTips` → `Runtime/TipCard`: 기본 툴팁을 억제하고 DataTable에서 추출한 카드를 소유 창 안에 그립니다.
- `Development`: 갤러리·Playground·내보내기 도구입니다. 판매 패키지에는 포함하지 않습니다.

제품은 서로를 참조하지 않습니다. 기존 `TurnRoom.Editor` 어셈블리 이름과 네임스페이스, 기존 에셋 GUID를 유지합니다. `Runtime`은 별도 `TurnRoom.Templete.Runtime` 어셈블리입니다.

## 직접 실행

1. `Tools > TurnRoom > Development > Shared UI > Gallery`를 엽니다.
2. Chrome·Controls·Tooltips·Settings·Versions·Icons 탭을 확인합니다.
3. `Open Playground Scene`을 열고 Play하면 공통 카드·입력·버전 안내를 확인할 수 있습니다.
4. 공통 배치는 `Editor/DefaultUITemplate.asset`을 선택해 바꿉니다. 열린 제품 창은 변경 알림으로 다시 그립니다.

Pixora의 사용자 테마 전환은 제품 `Settings > Theme`에만 있습니다. 개발 갤러리는 밝은 기준 화면을 사용합니다.

## 창 추가

`ToolWindow`를 상속하고 `ProductId`, `ToolDisplayName`, `IncludeInWindowList`를 지정합니다. 제품은 명령·검색 결과·콘텐츠만 전달합니다. UI Toolkit은 상속받은 `CreateDocumentChrome(...)`, 기존 IMGUI는 `DrawTemplateHeader(...)`를 사용합니다. 제품별 픽셀 숫자로 공통 슬롯을 다시 만들지 않습니다.

툴팁은 요소의 `tooltip`에 `제목\n설명`을 지정한 뒤 `UITemplate.Apply(...)`를 호출합니다. 모체가 내용을 보관하고 Unity 기본 툴팁을 비웁니다. 카드 자체는 별도 OS 창을 만들지 않습니다.

## 버전과 배포

현재 `Version.json`은 1.3.1 / API 1입니다. 각 제품의 `TempleteRequirement.json`은 필요한 API와 최소 버전, 배타적 상한 주 버전을 선언합니다.

- 같은 API 1의 1.x: 요구 최소 버전 이상인 최신 설치본을 유지합니다.
- 오래된 제품을 나중에 설치: `Tools > TurnRoom > Install Product Package`가 공유 폴더의 구버전 파일을 제외합니다.
- 서로 다른 API 주 버전: 위 설치 메뉴가 가져오기 전에 차단합니다. 호환 제품 업데이트를 사용해야 합니다.
- Unity 기본 Import: 설치 시작 때 공유 파일과 사용자 공통 설정을 스냅샷으로 보관하고, 구버전 덮어쓰기를 복구합니다. 다른 주 버전 제품은 공식 설치 메뉴를 사용하십시오.
- 사용자가 수정한 `DefaultUITemplate.asset`은 호환 업그레이드와 다운그레이드 가져오기에서 보존합니다.

`Tools > TurnRoom > Development > Packages > Export Product Packages`는 명시적 에셋 목록으로 제품 + Templete를 내보냅니다. Development 폴더와 갤러리·Playground·내보내기 메뉴가 소비자에게 들어가지 않습니다. 개발 폴더까지 수동으로 선택해 배포하지 마십시오.

Windows / Unity 6000.0.83f1에서 단독 설치, 양방향 설치 순서, 구버전 직접 가져오기, 설정 보존, 다른 API 차단, 샘플 제거 후 본체 컴파일을 검증했습니다. 다른 OS와 Unity 버전은 별도 검증 대상입니다.

## 원본 아이콘

제품 로고 배경의 기본 외곽은 Entity Forge처럼 둥근 모서리입니다. Design Data Table과 Pixora 하위 제품(Image Maker·Pixel Camera·Pixel Foliage)에 적용하며, Pixora Studio 제품군 대표 로고만 예외로 기존 외곽 형태를 유지합니다. 실제 Affinity 원본에 둥근 모서리를 작성하고 PNG의 바깥 모서리 투명도를 보존합니다. 창에서만 잘라 보이게 처리한 결과는 이미지 자체 수정으로 보지 않습니다. 작은 아이콘과 원본 크기에서 윤곽·여백을 확인합니다. 템플릿 전용 예제 로고는 `Assets/TurnRoom/Templete/Development/Art/ExampleLogo/`에 두며 개별 제품의 공식 로고로 재사용하지 않습니다.

`Editor/Art/RibbonAtlas*.png`는 DataTable 원본 바이트와 GUID를 그대로 옮긴 공통 자산입니다. `TemplateIcons`가 원래 UV·틴트 규칙을 유지하며 IMGUI와 UI Toolkit `TemplateIcon`이 같은 그리기를 사용합니다. 최소화·최대화·복원·닫기는 `WindowControls`의 동일 Paint를 공유합니다. Undo/Redo도 카탈로그의 기존 `search.undo`, `search.redo`를 사용합니다. 제품의 기능 자체를 추가하지는 않습니다.

Affinity 3.3.1에서 원본 컬러 아틀라스를 열고 `Development/Art/TurnRoom-Icons-Color.af`로 저장했습니다. 이는 원본 래스터 레이어를 가진 네이티브 문서이며 벡터 원본으로 재작성하지 않았습니다. 배포에서는 Development 원본이 제외됩니다.

창 선택은 실제 네이티브 순서를 읽습니다. 하단 목록의 MouseDown 전 앞에 있던 창 그룹을 유지하고 MouseUp의 선택 뒤에 재배치합니다. 사용자가 Unity 본체를 직접 선택한 경우 이미 뒤에 있는 창을 임의로 올리지 않습니다.

## 입력 행

UITemplateFields가 라벨·실제 입력·벡터 축·슬라이더 숫자칸·자산 선택기의 행 높이와 세로 중심을 적용합니다. fieldHeight와 fieldLabelWidth는 일반 행, dialogFieldHeight와 dialogLabelWidth는 설정 행의 기준입니다. 벡터와 슬라이더 컨테이너에 입력 테두리를 중복 적용하지 않습니다. 긴 라벨은 같은 열 너비 안에서 줄임표로 표시하며 공통 툴팁으로 전체 문구를 확인할 수 있습니다. 갤러리 Controls 탭에서 실제 입력 부품들을 확인합니다.

TemplateControlFaces는 DataTable의 체크박스·색상 필드 그림과 기존 아틀라스를 공유합니다. 선택기와 화살표도 같은 카탈로그를 사용합니다. ColorSelection은 공통 팝업에서 받은 선택 클릭을 Unity 화면 스포이트의 완료에 전달합니다. 내부 Unity API가 없는 버전에서는 원래 입력 엔진을 유지합니다. 다른 Unity 버전은 별도 실행 검증 대상입니다.

## Edit history · 1.2.2

`EditHistory<T>` is a serializable, window-owned100-step value history. Products provide independent snapshots and decide which changes to record. Pixora uses it for design edits. DataTable keeps its existing workbook history and the original shared Undo/Redo artwork. API major remains1; Pixora requires1.2.2, while DataTable remains compatible with1.2.1 or later.

## Common default color · 1.2.3

Edit `Editor/DefaultUITemplate.asset` → 도구 기본 강조색 → 기본 색상 (`toolAccent`). The shared fallback is `#181818`. Every new product must start with a new default theme accent unused by existing products. Compare existing product theme settings before creation and record the selected colour in the product documentation. Manage it through the product theme settings; child windows, Inspectors, selector popups and tooltip accents inherit the owning product theme. Existing document-specific colors remain explicit overrides.

## Peer selection · 1.2.4

`IWindowFocusBatchHost` optionally batches prior visible peers through `WindowFrame.RestackNativePeers`. Already visible, hidden, minimized and unrelated windows are not restored by this operation. Updated DataTable hosts use one deferred native position transaction; older hosts retain their existing contract. Click-time native flicker verification remains pending while desktop control is paused.

## Native shelf activation · 1.2.5

`WindowMouseActivationGuard` keeps a host behind its visible documents during a shelf item click. It preserves native button delivery, so click, close and drag handling remain in the shelf. The guard is limited to the supplied item rectangles and the short activation interval. It removes all native subclasses before an assembly reload or editor exit. DataTable supplies its shared shelf item rectangles; Pixora uses the same host. API major remains 1.

The 2026-10-31 native QA reproduced a main-editor cover frame despite correct final visibility. The root-only/no-activate attempts failed and remain in QA evidence. The final guard also preserves the root Z order during Unity mouse handling; final frame/event checks must be evaluated together.

## Window observation performance · 1.2.6

The registry reuses a read-only membership snapshot until windows are added, removed or reidentified. Discovery still runs on each query; closed/unlisted windows are not hidden behind a time-based cache. Focus tracking reads each window once per ordinary update and keeps stable order for equal Z values. Existing hosts remain supported.

`IWindowFocusReadHost` is optional. DataTable supplies a native read scope shared by Pixora and DataTable. It captures visible native candidates once per observation, then releases them before any restacking. It never caches across editor updates. Nested, repeated, out-of-order and exception disposal are covered by QA. Empty registries do not begin a native observation.

Native title matching also accepts Unity's trailing unsaved marker when `hasUnsavedChanges` is true. Ambiguous handles still remain unresolved; the code never chooses an arbitrary overlapping window.

Development-only stress fixtures are `Tools/Verification/WindowStressQA.cs.txt` (DataTable editor assembly) and `WindowNativeStressQA.cs.txt` (isolated QA assembly referencing both products). They are not part of asset distribution. Evidence is under `Artifacts/WindowStress/2026-10-31`. Allocation/process-memory APIs returned zero in the tested Unity Mono runtime; use the recorded counter validity checks and external process samples, not a claim of zero allocations or leaks.

## 공통 헬프박스 · 1.3.1

세 제품의 메시지 박스는 `TurnRoom.Editor.TemplateHelpBox`를 사용합니다. UI Toolkit에서는 `new TemplateHelpBox(text, MessageType.Info)`를 부모에 추가하고, Inspector·IMGUI에서는 `TemplateHelpBox.Draw(text, type)`를 호출합니다. UI Toolkit은 `UITemplate.Apply(root, light)`의 테마를 따릅니다. IMGUI 창은 세 번째 인수로 창의 라이트 여부를 전달하며, 생략하면 Unity 에디터 스킨을 따릅니다. DataTable은 기존 밝은 테마를 명시적으로 전달합니다.

- `MessageType.None`은 아이콘 없는 안내이며 Info·Warning·Error는 기존 diagnostics 컬러 아이콘을 공유합니다. 새 이미지 자산은 만들지 않습니다.
- `Text`로 UI Toolkit 메시지를 바꿀 수 있습니다. 긴 문구와 명시적 줄바꿈은 고정 높이 없이 배치합니다.
- `DefaultUITemplate.asset`의 `helpBoxPadding`, `helpBoxIconSize`, `helpBoxRadius`, 기존 `bodyFont`·`fieldSpacing`·밝은/어두운 색상을 공유합니다.
- 갤러리의 `Help boxes / 안내 박스`에서 두 API의 4개 심각도와 Light / Dark를 확인할 수 있습니다. 갤러리는 Development 전용입니다.
- `EditorStyles.helpBox`를 배경으로 쓰는 편집 카드·그룹은 메시지 박스와 다르므로 기존 입력·버튼 구조를 보존합니다.

공통·세 제품·갤러리의 정적 컴파일은 통과했습니다. Unity 6000.0.83f1의 별도 검증 프로젝트에서 실제 라이트·다크, 240px·440px 폭, 긴 문구·명시적 줄바꿈, 4개 심각도를 확인했습니다. Pixel Camera는 영어 라이트·다크, PixoraWorkInspector는 한국어 다크, DataTable FindPopup은 영어 안내·오류 상태를 캡처했습니다. 각 제품의 ScreenshotManifest에 실제 범위와 해시를 등록했으며, 9개 언어의 UI 배치를 모두 검증한 것은 아닙니다.

실제 화면 QA에서 IMGUI의 `GUILayout.MinHeight`가 긴 문구의 높이를 고정하는 실패를 확인하고 제거했습니다. [Unity 6.0 GUIWordWrapSizer](https://github.com/Unity-Technologies/UnityCsReference/blob/6000.0/Modules/IMGUI/LayoutEntry.cs)는 강제 최소·최대 높이를 별도로 저장하므로, 메시지 높이는 폭에 따른 자동 계산에 맡깁니다. 정적 컴파일만으로 줄바꿈 통과를 판정하지 않습니다. 기존 다른 기능의 캡처·미검증 기록은 보존했으며, 전체 가이드 검사에 남은 기존 소스 불일치는 이번 새 헬프박스 캡처 통과와 구분합니다.

## 공용 폰트

`Runtime/Resources/TurnRoom/Fonts`에 **TurnRoom Sans Regular·SemiBold**를 포함합니다. Noto Sans KR을 기반으로 만든 정적 400·600 굵기이며 원본 문자표를 유지합니다. SIL OFL 1.1 원문과 원본 출처·해시·변경 내용을 같은 폴더에 둡니다. OS 폰트 설치나 네트워크 연결 없이 사용합니다.

- `TemplateTypography.Regular`, `TemplateTypography.Semibold`: 런타임·에디터에서 같은 Font 자산을 가져옵니다.
- `TemplateTypography.Apply(skin, styles...)`: 제품이 만든 IMGUI 스타일에 적용하며 Unity 전역 EditorStyles는 수정하지 않습니다.
- `UITemplateStyles`: 두 제품의 UI Toolkit 창에 Regular를 상속합니다.
- `DemoCanvas`: 샘플·개발 갤러리에서 Regular·SemiBold와 같은 카드·버튼을 사용합니다. 기본 강조는 검정입니다.

DataTable의 통합문서 표·수식 입력·버튼과 Pixora 샘플이 이 자산을 사용합니다. API 1 호환은 유지하며 이 기능을 사용하는 제품은 Templete 1.3.0 이상을 요구합니다. 두 TTF 합계는 약12.45MB입니다. 문서를 작성하거나 글꼴을 단독 판매하는 패키지가 아니라 제품에 포함하는 폰트 자산입니다.

## 공통 선택 패널 자동 연결

제품 UI를 구성한 뒤 `UITemplate.Apply`를 호출하면 일반 ObjectField·ColorField·DropdownField·EnumField도 공통 선택 패널에 연결됩니다. 바인딩은 필드당 한 번만 등록됩니다. 명령·오류는 `TemplateInspectorPopover.Show`로 소유 테마를 상속합니다. 정적 패널 레이아웃은 `TemplateSelectionPanel.uxml`과 `.uss`, Entity Forge 창은 `EntityForge/Editor/ForgeWindow.uxml`과 `.uss`에서 편집합니다. 제품명은 문서 제목 영역에 두고 로고 슬롯에는 그림만 넣습니다. 긴 필드는 `template-readable-toggle` 또는 `template-readable-field`를 사용합니다.

패키징 전에 `TemplatePickerPolicy.ValidateProduct`로 제품 Editor 폴더의 기본 팝업 직접 호출을 검사합니다. 현재 검사는 명시된 API 호출 패턴 검사이며 별칭 등 모든 C# 구문을 분석하는 보장은 아닙니다. Unity의 Build Profiles 창이나 운영체제 파일 선택창은 제품 선택 패널과 구분합니다. 실제 마우스·키보드의 반환값·취소·테마와 배치를 별도 검증해야 합니다.

### 공통 명령 메뉴 · 1.6.2

제목줄 명령 검색은 검색창 폭에 맞춘 간결한 목록을 사용합니다. 중복 제목·닫기 상자·항목별 버튼 테두리는 생략하고, 32px 행과 선택 강조를 공통 `TemplateSelectionPanel.uss`에서 관리합니다. 방향키로 선택하고 Enter로 실행하며 Escape 또는 바깥 클릭으로 취소합니다. 일반 Inspector 액션 버튼 스타일과 분리하므로 에셋·색상 선택기의 기존 헤더와 입력 계약은 유지합니다. 제품별 메뉴를 다시 만들지 말고 `UITemplate.CommandSearch`를 사용합니다.

호스트 없는 독립 Windows 설치의 최소화는 `WindowServices` 상태를 갱신하고 기존 공통 `WindowList`를 엽니다. DataTable이 설치된 경우에는 기존 하단 shelf 호스트 경로를 유지합니다. 로고는 `brand-symbol` 클래스와 공통 슬롯 크기를 사용하며 고정 28px 이미지로 20px 슬롯을 넘기지 않습니다.

## 최대화·원래 크기 복원

최대화 버튼과 제목줄 더블클릭은 공통 호스트를 통해 실제 창을 직접 전환합니다. 최대화 시 캡처 표면 생성·실제 창 숨김·약 0.30초 뒤 다시 표시하던 경로는 제거했습니다. 최소화 목록에서의 복원 애니메이션과 사용자 설정은 유지합니다. standalone 템플릿은 기존 직접 전환 경로를 유지합니다. 실제 화면의 전환·갱신 지연 검증은 별도이며, 소스 변경이나 컴파일 통과만으로 체감 지연이 해소됐다고 처리하지 않습니다.


## 검색형 공통 선택 메뉴와 Windows 작업 전환 · 1.9.1

`TemplateInspectorPopover.Select`는 열거형·드롭다운·직렬화 Inspector의 단일 선택을 검색창, 현재 값 체크, 위/아래 키·Enter 선택 및 Escape 취소로 연결합니다. `SelectMask`는 레이어·플래그를 같은 행 디자인으로 표시하고 적용 전까지 임시 값으로 보존합니다. 취소·바깥 클릭·Escape는 원본 값을 바꾸지 않습니다. UXML의 `TemplateSelectionPanel`과 `TemplateSelectionRow`, USS가 정적 레이아웃을 소유합니다. 색상 편집기와 명령 검색 메뉴는 기존 목적을 유지하고 에셋 목록도 공통 행을 사용합니다.

`UITemplate.Apply`가 UI Toolkit의 DropdownField·EnumField·LayerMaskField·MaskField에 공통 선택 메뉴를 연결합니다. IMGUI에서 별도 네이티브 선택기를 호출하던 DDT 폴백은 `SelectIMGUI`로 결과를 다음 GUI 이벤트에 전달하고 기존 GUI.changed 계약을 유지합니다. DDT 차트 이동과 표 스타일 적용 메뉴도 기존 테마 메뉴로 연결했습니다.

`WindowServices.Register`는 `IncludeInWindowList`인 독립 ToolWindow만 Windows 작업 전환 대상으로 구성합니다. 정확히 일치하는 같은 프로세스의 최상위 창에서 TOOLWINDOW·NOACTIVATE를 제거하고 APPWINDOW를 설정하며 소유자를 해제합니다. 도킹 창·툴팁·임시 메뉴는 제외합니다. 프레임 갱신 뒤 ITaskbarList로 실제 셸 항목을 등록하며 종료·도메인 재로드에서 해제합니다. 등록된 독립 문서가 Windows의 실제 활성화 대상이면 해당 Unity 뷰를 맞추고 Win32 기본 활성화로 키보드 포커스를 설정합니다. 비활성화 및 나머지 메시지는 Unity 처리로 전달합니다. 주기적으로 창을 숨기거나 외부 앱의 포커스를 빼앗지 않습니다. 다른 OS의 동작은 변경하지 않습니다. 스타일·미리보기·선택 후 실제 활성 창은 각각 검증합니다.

2026-11-04 검증 근거는 `Docs/Testing/2026-11-04/shared-popup-menu/`에 보존합니다. 정적 컴파일·자산 파싱·언어/폰트 검사와 실제 UI 검증을 구분합니다. 이전 화면 제어 중단 동안의 QA 보류 기록은 보존합니다. 이후 사용자 요청으로 공통 및 제품별 전후 72쌍을 `Docs/Testing/2026-11-04/popup-visual-review/`에 촬영했고, Windows Alt+Tab의 미리보기와 Entity Forge↔Pixel Camera 실제 키 왕복을 `Docs/Testing/2026-11-04/task-switch-native/`에 별도 검증했습니다. 사용자의 최종 답변도 전환 정상입니다. 모든 제품 종류·사용자 정의 최소화 상태 및 기존 팝업 선택/취소/X 입력 전체를 통과한 것으로 확장하지 않습니다. 9개 언어 공통 안내에 이를 기록했고 기존 ScreenshotManifest의 캡처 시각·해시는 갱신하지 않았습니다. DDT의 기존 확인/오류 대화상자 때문에 전체 팝업 정책 검사는 여전히 실패하며, 이번 선택 메뉴 변경으로 제품 전체의 기본 대화상자 제거를 인증하지 않습니다.

## DDT 기준 제품 다큐먼트 예제

공통 렌더러와 9언어 실제 예제는 [제품 다큐먼트 기준](ProductDocumentation.md)을 따릅니다. 새 제품의 개요·빠른 시작·예제 이미지·고유 로고/파비콘은 최초 생성부터 필수이며 DDT 스타일을 재사용합니다. 예제 로고를 새 제품에 그대로 물려주지 않습니다. 실제 브라우저 QA 전에는 시각 검증 완료로 기록하지 않습니다.
