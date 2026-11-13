# 프로젝트 구조

| 경로 | 책임 |
| --- | --- |
| `Assets/Scripts` | `Overdox.Runtime` 어셈블리의 세션·전투·화면 연결 |
| `Assets/Scripts/UI` | 복원한 UI의 동적 값, 포인터 입력, 로비 연출 |
| `Assets/Scripts/Editor` | `Overdox.Editor` 어셈블리의 복원·데이터 컴파일·빌드 도구 |
| `Assets/TurnRoom/DesignDataTable` | 기존 DDT 직렬화·런타임 조회 기반 |
| `Assets/GameData/Workbooks` | 수식이 있는 Excel 편집 원본 |
| `Assets/GameData/OriginalTables` | 확보한 원작 테이블의 보존 자료 |
| `Assets/Art/Original` | 원작에서 회수한 시각 자산과 의존성 |
| `Assets/Art/Restored` | 새 셰이더·변환 클립·플레이용 UI 등 프로젝트 소유 파생 자산 |
| `ArtSource` | 실제 Blender·Affinity에서 저장한 편집 가능한 원본 |
| `VerificationBuilds` | 특정 검증에 사용한 실행 파일과 전체 데이터의 고정 사본 |

`ArenaSession`은 전투 상태와 선택 장비를 소유합니다. UI는 세션 명령을 호출하고 표시값을 읽습니다. `RestoredLobbyController`는 홈·장비 UI 전환을, `RestoredLobbyPresentation`은 카메라 클립과 배경 Animator를 맡습니다. `RestoredMoveStick`은 이동 입력과 스틱 표시만 갱신하며 실제 전투 이동은 기존 전투 코드가 처리합니다.

샘플 프로젝트의 게임 코드를 직접 참조하지 않습니다. 기존 씬·프리팹의 직렬화 필드와 UnityEvent 연결은 이름 변경 전에 확인합니다. 원작 코드·서버를 제품 의존성으로 추가하지 않습니다.
