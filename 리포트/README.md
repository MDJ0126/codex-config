# Tracefolio — Reports & Issues

프로젝트별 결과·버그·수정 기록을 같은 게시물 형식으로 관리합니다.

- 공개 사이트: https://mdj0126.github.io/codex-config/
- 사람·AI 공통 작성 가이드: [guide/authoring.md](guide/authoring.md)
- 웹 가이드: https://mdj0126.github.io/codex-config/guide/
- 결과 템플릿: [guide/template-report.md](guide/template-report.md)
- 버그 템플릿: [guide/template-bug.md](guide/template-bug.md)
- 메타데이터 스키마: [guide/schema.json](guide/schema.json)

## 프로젝트별 파일

```text
projects/<project>/posts/                Markdown 원본
projects/<project>/reports/              생성된 HTML
projects/<project>/assets/               로고·첨부·이전 자료
projects/<project>/originals/            이전 원문 백업
projects/<project>/editorial-originals/  공통 형식 편집 전 Markdown
assets/brand/                           공통 브랜드
index.html                              메인 목록
```

등록된 프로젝트 ID와 폴더·메타데이터의 project는 같아야 합니다. 원본과 편집 전 백업은 Pages 패키지에서 제외되지만 공개 Git 저장소에서는 읽을 수 있습니다. 예전 HTML 주소는 이동 안내로 유지합니다.

## 작성·검증·배포

새 글은 가이드를 먼저 읽고 템플릿에서 시작합니다. format은 tracefolio-v1이며 작업 결과 → 변경 내용 → 검증 → 남은 사항 → 작업 시간 순서를 사용합니다. 본문에 H1 제목을 중복 작성하지 않습니다. 메타데이터·버그 필수 항목과 과거 예외 무단 사용은 빌드에서 확인합니다. 과거 글은 등록된 legacy-v1 목록으로만 허용합니다.

```sh
node 리포트/build.mjs
node 리포트/verify.mjs
node 리포트/verify-migration.mjs
node 리포트/verify-editorial.mjs
node --test 리포트/tests/*.test.mjs
```

공통 렌더러는 표·코드·이미지 설명·인용·목록·목차를 지원하며 raw HTML과 실행 URL은 비활성 처리합니다. HTML·CSS를 게시물마다 별도로 만들지 않습니다. Marked 17.0.5는 vendor/marked에 라이선스와 함께 보관합니다.

검증 후 승인된 변경을 main에 푸시하면 GitHub Actions가 빌드·검사·포장·Pages 배포를 수행합니다. 푸시와 배포 성공을 구분하고 공개 URL까지 확인합니다. 모든 작업 결과는 이 사이트에 기록하고 채팅에는 핵심 결과와 HTML 링크를 안내합니다.
