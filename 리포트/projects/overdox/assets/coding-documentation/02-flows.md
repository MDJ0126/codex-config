# 흐름과 책임

## 로비 화면과 카메라

```mermaid
flowchart TD
    A[사용자가 캐릭터 또는 뒤로 선택] --> B[RestoredLobbyController: UI 활성 상태 변경]
    B --> C[RestoredLobbyPresentation.Transition: 클립과 배경 트리거 선택]
    C --> D[LateUpdate: unscaledDeltaTime으로 클립 샘플링]
    D --> E[원작 영웅 원점을 빼고 거리 0.001배 환산]
    E --> F[현재 플레이어 위치를 더해 카메라 자세 적용]
```

`LobbyToEquipment`와 `EquipmentToLobby`는 원작 곡선에서 만든 별도 legacy 클립입니다. 원작 영웅 기준점은 `(-1051, 60, 3984)`입니다. 로비 전용 연출이 활성화되면 기존 `ArenaCamera`가 로비 카메라 자세를 덮어쓰지 않도록 역할을 나눕니다. 전투 카메라 동작은 별도로 유지합니다.

클립의 재생 시간은 장비 진입 약 1.233초, 홈 복귀 1초입니다. UI 활성 상태는 현재 즉시 바뀝니다. 원작과 같은 UI 페이드·전환 중 재입력 처리의 일치 여부는 미확인입니다.

## 포인터 이동

```mermaid
flowchart TD
    A[OnPointerDown: 첫 포인터 소유] --> B[OnDrag: 소유 포인터와 전투 상태 검사]
    B --> C[화면 좌표를 UI 로컬 좌표로 변환]
    C --> D[반경으로 나누고 벡터 크기를 1로 제한]
    D --> E[ArenaSession.TouchMove와 스틱 표시 갱신]
    E --> F[FighterBrain: 키보드 입력과 결합해 이동 처리]
    E --> G[OnPointerUp 또는 OnDisable]
    G --> H[Release: 소유권과 이동값 초기화]
```

다른 포인터의 해제는 현재 이동을 취소하지 않습니다. Inspector에서 `knob`, `arrow`, 양수인 `radius`가 연결되어야 합니다. 반경 0과 필수 참조 누락을 처리하는 방어 코드는 현재 없으므로 임의 구성에서는 사용하지 않습니다.

## Excel과 DDT

```mermaid
flowchart LR
    A[XLSX: Equipment 공통 장비] --> B[수식 계산 및 형식 변환]
    C[XLSX: Gear 무기 상세·Armor 방어구 상세] --> B
    B --> D[ExcelGameDataCompiler.ReadTable]
    D --> E[기존 DDT 쓰기·암호화]
    E --> F[런타임 DDT 읽기]
    F --> G[선택 장비·전투 능력치]
```

`Equipment`는 무기와 방어구를 함께 담는 119행의 공통 마스터입니다. `Gear` 38행과 `Armor` 81행은 상세 데이터이며 ID로 대응합니다. `Weapons`와 `ArmorCatalog`는 프로젝트 편의용 뷰이며 원작 테이블 이름과 구분합니다. XLSX를 편집 원본으로 유지하고 CSV를 작성 원본으로 사용하지 않습니다.
