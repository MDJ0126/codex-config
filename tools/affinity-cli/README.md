# Affinity CLI

이 저장소에서 관리하는 Affinity 디자인 앱 전용 CLI입니다. **Node.js 22 이상**만 필요하며 외부 npm 의존성은 없습니다. 실제 Affinity의 MCP·JavaScript SDK를 사용합니다. 마우스·키보드 제어, 창 활성화, 앱 자동 실행·재시작은 하지 않습니다.

## 실행

저장소 루트에서 실행합니다. PowerShell 래퍼는 PATH의 Node 또는 Codex 번들 Node를 찾습니다.

```powershell
./tools/affinity-cli/affinity.ps1 help
./tools/affinity-cli/affinity.ps1 doctor --timeout 3000
./tools/affinity-cli/affinity.ps1 status
```

어떤 셸에서도 다음처럼 직접 실행할 수 있습니다.

```text
node tools/affinity-cli/bin/affinity.mjs help
```

이식 가능한 저장소 내 설치입니다. 전역 PATH나 npm 전역 설치를 변경하지 않습니다. `Sync.ps1 -Direction Pull`로 저장소를 받으면 함께 전달되며 새 PC에도 Node와 Affinity가 필요합니다.

## Affinity 연결

- 개발 시 설치 확인: Affinity 3.3.1.4940 / Windows. 공식 JSLib 3.3.1을 API 참조로 사용했습니다.
- Affinity가 실행 중이고 **로컬 MCP 서버**가 켜져 있어야 합니다. 사용 중인 버전의 설정에서 Model Context Protocol/MCP 서버 항목을 확인하세요. 설정 변경에 재시작이 필요한지는 해당 버전에서 확인합니다. 이 CLI가 설정을 몰래 변경하거나 재시작하지 않습니다.
- 기본 주소: `http://localhost:6767/sse`. `--endpoint` 또는 `AFFINITY_MCP_URL`로 변경할 수 있습니다. loopback HTTP 주소만 허용합니다. 실제 서버 주소는 설치 환경에서 검증해야 합니다.
- 기본 전송은 HTTP+SSE입니다. 호환 서버가 Streamable HTTP를 제공한다면 `--transport http --endpoint http://localhost:PORT/mcp`로 지정합니다. 자동 프로토콜 전환이나 수정 명령 재시도는 하지 않습니다.
- `doctor`는 핸드셰이크·도구 목록을 검증합니다. **문서 편집과 포커스 비간섭 검증을 대신하지 않습니다.** 실제 연결·SDK 작업 검증 상태는 `VALIDATION.md`를 보세요.
- `ECONNREFUSED`는 해당 주소에서 연결이 거부되었다는 결과입니다. 이것만으로 설정이 꺼졌거나 앱이 미설치라고 단정하지 않습니다.
- 파일 열기·저장·내보내기는 Affinity의 파일 접근 권한도 충족해야 합니다. SDK 실행 실패 시 프로그램이 반환한 오류를 확인하고 필요한 작업 폴더만 허용합니다.

## 일상 작업 순서

```powershell
# 1. 실제 버전·문서·SDK 도구 확인
./tools/affinity-cli/affinity.ps1 doctor
./tools/affinity-cli/affinity.ps1 status
./tools/affinity-cli/affinity.ps1 tools
./tools/affinity-cli/affinity.ps1 docs preamble
./tools/affinity-cli/affinity.ps1 fonts
./tools/affinity-cli/affinity.ps1 presets

# 2. 예제는 새 문서만 만들며 저장하지 않습니다. 글꼴 이름은 fonts 결과와 대조하세요.
./tools/affinity-cli/affinity.ps1 job tools/affinity-cli/examples/poster.json --dry-run
./tools/affinity-cli/affinity.ps1 job tools/affinity-cli/examples/poster.json

# 3. status 결과의 문서 세션 ID를 명시적으로 사용
./tools/affinity-cli/affinity.ps1 inspect DOCUMENT_SESSION_ID
```

**품질 우선:** Affinity로 만들었다는 이유만으로 디자인 품질이 보장되지는 않습니다. 적절한 구성·타이포그래피·원본 해상도·색상·내보내기 프리셋을 선택하고 렌더링 결과를 실제로 검토합니다. JSON 작업은 반복 실행 인터페이스이며, 복잡한 효과·정교한 조판은 SDK 문서를 확인한 사용자 스크립트로 확장합니다.

## JSON 작업

전체 스키마: `affinity.ps1 schema`. 파일 경로는 **작업 JSON 파일의 폴더 기준**입니다. `document`는 `create`, `open`, `id` 중 정확히 하나를 받습니다. 기존 문서 편집은 활성 탭 대신 세션 ID로 대상을 고정합니다.

```json
{
  "version": 1,
  "document": { "id": "STATUS에서 확인한 세션 ID" },
  "steps": [
    { "op": "rename", "layer": "Headline", "name": "Title" },
    { "op": "set-text", "layer": "Title", "text": "Updated title" },
    { "op": "fill", "layer": "Title", "fill": "#F2F5EC" },
    { "op": "transform", "layer": "Title", "x": 10, "y": 0 },
    { "op": "save", "path": "poster.af" },
    { "op": "export", "path": "poster.png", "preset": "PRESETS에서 확인한 정확한 이름" },
    { "op": "inspect" }
  ]
}
```

- `shape`: `rectangle`/`ellipse`, 좌표·크기·채움·이름. 색상은 `#RRGGBB` 또는 `#RRGGBBAA`입니다.
- `text`: 좌표·본문·글꼴·크기·채움. `font`는 **정확한 PostScript 이름**이며 없는 글꼴로 자동 대체하지 않습니다. `width`·`height` 둘 다 주면 프레임 텍스트, 없으면 아트 텍스트입니다. 크기·좌표는 픽셀 기준입니다.
- `image`: 파일·좌표·크기·이름. 비율은 지정 크기로 조절하므로 원본 비율에 맞게 입력하세요.
- `rename`/`set-text`/`fill`/`transform`: 레이어 이름이 정확히 하나 일치해야 합니다. 변환은 스프레드 원점 기준 배율·회전(도)·상대 이동입니다.
- `save`/`export`: 기본적으로 기존 파일을 거부합니다. 명시적 `overwrite: true`일 때만 덮어씁니다. `export`의 크기 강제 지정은 `width`와 `height`를 함께 사용합니다.
- `inspect`: 문서 메타데이터와 레이어 목록을 반환합니다.
- 파일 존재 확인은 CLI와 Affinity 양쪽에서 수행합니다. 검사 후 쓰기 사이의 경쟁을 완전히 차단하는 원자적 Affinity 저장은 보장하지 않습니다.
- 작업은 **원자적 트랜잭션이 아닙니다.** 중간 실패 시 앞선 편집과 저장 파일이 남을 수 있습니다. 오류의 `completed`, `partialChangesPossible`를 확인하고 무작정 재실행하지 마세요. 삭제·문서 닫기·자동 롤백은 하지 않습니다.

## SDK 스크립트와 네이티브 도구

```powershell
./tools/affinity-cli/affinity.ps1 run tools/affinity-cli/examples/inspect.js
./tools/affinity-cli/affinity.ps1 docs TOPIC
./tools/affinity-cli/affinity.ps1 call TOOL_NAME --args '@arguments.json'
./tools/affinity-cli/affinity.ps1 render spread --args '@render-arguments.json' --artifacts ./renders
```

도구 인자는 버전마다 달라질 수 있으므로 `tools`가 반환한 **실제 inputSchema**를 사용하세요. 임의의 인자 이름을 추정하지 않습니다. 스크립트 실행 전 같은 MCP 연결에서 `preamble` 문서를 읽습니다. `execute_script`가 추가 필수 인자를 요구하면 `run --args`로 전달합니다. JSON 작업에 필요한 네이티브 도구 형식이 호환되지 않으면 명확한 오류로 중단합니다.

`run`/`call execute_script`는 신뢰하는 스크립트를 Affinity 권한으로 실행하는 고급 기능입니다. CLI가 스크립트의 파일 접근이나 GUI API 호출을 차단하는 샌드박스는 아닙니다. 기본 제공 JSON 작업은 UI API를 호출하지 않습니다. 사용자 스크립트에서도 alert·prompt·대화상자 등을 사용하지 않아야 화면 방해를 피할 수 있습니다.

렌더링 이미지의 base64는 stdout에서 생략합니다. `--artifacts DIR`을 지정하면 고유 파일명으로 저장하고 경로·SHA-256·이미지 크기를 반환합니다. 렌더링 도구의 대상 문서는 해당 버전의 스키마를 따릅니다. 현재 문서만 지원할 때는 실제 대상이 맞는지 먼저 확인합니다.

```powershell
./tools/affinity-cli/affinity.ps1 verify ./renders/ACTUAL_FILE.png --width 1600 --height 1000
```

`verify`는 파일 바이트·SHA-256 및 PNG/JPEG 헤더 크기를 확인합니다. 파일 완전성 디코딩이나 미적 품질 판정은 아니며 이미지를 열어 시각 검증해야 합니다.

## 결과·종료 코드

성공과 오류 모두 JSON 한 개를 stdout에 출력합니다. `--out FILE`은 동일 JSON을 **새 파일에만** 기록합니다. `--script-out FILE --dry-run`은 연결 없이 생성된 SDK 스크립트를 새 파일로 저장해 검토할 수 있습니다.

| 코드 | 의미 |
|---|---|
| 0 | 요청 성공 |
| 2 | 입력·설정·스키마 오류 |
| 3 | 연결·프로토콜·도구 미지원 |
| 4 | Affinity 실행·작업·출력 검증 실패 |
| 5 | 시간 초과, 수정 결과 미확정 가능 |
| 6 | 다른 CLI 작업 실행 중 |
| 1 | 기타 오류 |

기본 시간 제한은 요청당 30초이며 `--timeout`으로 100~600000ms를 지정합니다. 시간 초과는 Affinity 내부 실행 취소를 보장하지 않습니다. 별도 실행 중인 CLI는 사용자 단위 잠금으로 직렬화하며, 외부 앱이나 사용자의 수동 편집까지 잠그지는 않습니다.

## 개발·검증

```text
node --test tools/affinity-cli/test/*.test.mjs
```

테스트는 로컬 모의 MCP 서버를 띄웁니다. 개발 코드·테스트·예제만 버전 관리합니다. `.cache/`, `output/`, `node_modules/`, `*.local.json`은 제외하며 인증정보를 저장하지 않습니다.

공식 참조:
- [Affinity SDK 3.3.1](https://sdk.affinity.studio/latest/)
- [공식 JavaScript API](https://sdk.affinity.studio/latest/js/)
- [Affinity 스크립팅](https://www.affinity.studio/features/scripting)
- [Canva 제작 Affinity 커넥터의 도구 목록](https://claude.com/marketplace/connectors/affinity-106b90)
