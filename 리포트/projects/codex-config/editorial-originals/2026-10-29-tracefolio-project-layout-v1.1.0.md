---
title: Tracefolio — 프로젝트별 폴더 정리와 공개 반영 완료
date: 2026-10-29T21:05:21+09:00
version: v1.1.0
status: 구조 변경·공개 배포·브라우저 확인 완료
project: codex-config
type: 수정사항
category: 보고서 관리
summary: 프로젝트별 원본·HTML·첨부자료 폴더를 적용하고 Tracefolio 브랜딩을 공개했습니다. 기존 공유 링크 이동과 실제 화면·이미지 로드를 확인했습니다.
---
# Tracefolio — Reports & Issues

프로젝트별 파일 구조 변경과 Tracefolio 이름·헤더·색상 적용, GitHub Pages 배포를 완료했습니다. 기존 주황색 문서 보관함 심벌에 워드마크를 조합했습니다.

[Tracefolio 열기](https://mdj0126.github.io/codex-config/)

## 파일 구조
```
리포트/projects/<project>/posts/       Markdown
리포트/projects/<project>/reports/     HTML
리포트/projects/<project>/assets/      로고·첨부·이전 자료
리포트/projects/<project>/originals/   원문 백업
리포트/assets/brand/                  Tracefolio 공통 로고
```

- 기존 글 256개를 7개 프로젝트 아래로 옮기고 새 결과 보고서도 같은 구조로 등록했습니다.
- 실제 보고서와 프로젝트 파일은 프로젝트 폴더에서 관리합니다. 공통 스타일·스크립트·브랜드·이전 매니페스트는 공용 파일로 둡니다.
- 프로젝트 폴더 이름과 front matter project가 다르거나 등록되지 않은 폴더가 있으면 빌드가 중단됩니다.
- 이전 주소에는 이동 안내 파일만 둡니다. 보고서 원본을 여러 경로에서 중복 편집하지 않습니다.
- 원문 백업은 Pages 패키지에서 제외되며 공개 Git 저장소에는 보존됩니다.

## 검증과 배포
- 로컬에서 보고서 257쌍, 링크 3,098개와 프로젝트·유형·검색 조합 검증을 통과했습니다. 마지막 결과 기록을 추가한 뒤 다시 검사합니다.
- 원문 230개와 연결 자료 4,211개 해시가 이동 전과 동일함을 확인했습니다.
- 새 경로에도 Git 줄바꿈 변환 방지 규칙을 적용했습니다.
- 구조 변경 커밋 2c922d4를 푸시하고 Actions 실행 40870113884의 빌드·검증·배포 단계 성공을 확인했습니다.
- 실제 브라우저 제목이 모든 보고서 · Tracefolio이고 헤더에 Tracefolio REPORTS & ISSUES가 표시되는 것을 확인했습니다.
- 메인 화면을 캡처해 확인했으며 DOM에서 로드 실패한 이미지가 없었습니다.
- 이전 공유 주소 reports/2026-10-29-project-report-migration-v1.1.0.html에 접속했을 때 projects/codex-config/reports 아래 실제 문서로 이동하고 본문이 표시되는 것을 확인했습니다.
- 승인하신 CodeAssist·EngineMigrator·WindowsFolderSize의 보고서 정리 커밋도 원격 main에 푸시하고 해시 일치를 확인했습니다. 각 프로젝트에 별도 결과를 기록했습니다.

## 시간과 검색
- 제안 시간: 5~8분.
- 시작 시각: 2026-10-29 20:56:26 KST.
- 실제로 완료된 시간: 구조 변경·첫 배포·실제 브라우저·추가 승인된 원격 정리까지 8분 55초, 2026-10-29 21:05:21 KST. 제안 범위를 55초 넘었습니다. 마지막 결과 기록·배포 종료는 채팅에 별도 표기합니다.
- XERJ: 미사용 — 도구 미제공으로 현재 소스·파일·해시·실행과 실제 브라우저를 대조했습니다.
