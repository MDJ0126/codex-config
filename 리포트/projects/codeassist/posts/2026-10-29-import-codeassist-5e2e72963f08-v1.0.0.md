---
title: 실제 단축키 입력 검사
date: 2026-10-18T06:40:38.912Z
version: v1.0.0
status: 기존 기록 이전
project: codeassist
type: 작업 결과
category: 이전 기록
summary: 실제 단축키 입력 검사 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/docs/physical-shortcuts-092.html
source_sha256: 7716568ec854eea443d4485c1e4ad3cbe4aff8a1653ea0815ef467f781f92014
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

SendInput 실제 키 전송: 22/22 (100.00%). 기대한 CA 명령 수신: 14/22 (63.64%). 미확인: 8개. 명령 수신은 기능 결과 동등성을 뜻하지 않습니다.

일반 VS에는 VA와 CA가 함께 존재하므로 VA 단독 원본 대조로 계산하지 않습니다. Exp 첫 실행은 COM 호출 거부로 12개 입력 후 중단됐습니다.

| 키 | 입력 | CA 명령 수신 | 실제 수신 명령 | 사진 |
| --- | --- | --- | --- | --- |
| ALT+K | True | True | [{'guid': '{F9D77864-FAC8-457E-BD4F-552715151B6C}', 'id': 282}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 30}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 40}, {'guid': '{5EFC7975-14BC-11CF-9B2B-00AA00573819}', 'id': 684}, {'guid': '{1496A755-94DE-11D0-8C3F-00C04FC2AAE2}', 'id': 1990}] | [화면](../assets/imports/docs/validation/physical-shortcuts-092/VA-1.png) |
| CTRL+SHIFT+V | True | False | [{'guid': '{160961B3-909D-4B28-9353-A1BEF587B4A6}', 'id': 33}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 30}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 40}, {'guid': '{5EFC7975-14BC-11CF-9B2B-00AA00573819}', 'id': 684}, {'guid': '{1496A755-94DE-11D0-8C3F-00C04FC2AAE2}', 'id': 1990}] | [화면](../assets/imports/docs/validation/physical-shortcuts-092/VA-2.png) |
| ALT+SHIFT+H | True | True | [{'guid': '{F9D77864-FAC8-457E-BD4F-552715151B6C}', 'id': 272}] | [화면](../assets/imports/docs/validation/physical-shortcuts-092/VA-3.png) |
| ALT+SHIFT+] | True | False | [] | [화면](../assets/imports/docs/validation/physical-shortcuts-092/VA-4.png) |
| ALT+SHIFT+[ | True | False | [{'guid': '{160961B3-909D-4B28-9353-A1BEF587B4A6}', 'id': 21}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 30}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 40}, {'guid': '{5EFC7975-14BC-11CF-9B2B-00AA00573819}', 'id': 684}, {'guid': '{1496A755-94DE-11D0-8C3F-00C04FC2AAE2}', 'id': 1990}] | [화면](../assets/imports/docs/validation/physical-shortcuts-092/VA-5.png) |
| ALT+] | True | True | [{'guid': '{F9D77864-FAC8-457E-BD4F-552715151B6C}', 'id': 276}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 30}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 40}, {'guid': '{5EFC7975-14BC-11CF-9B2B-00AA00573819}', 'id': 684}, {'guid': '{1496A755-94DE-11D0-8C3F-00C04FC2AAE2}', 'id': 1990}] | [화면](../assets/imports/docs/validation/physical-shortcuts-092/VA-6.png) |
| ALT+[ | True | True | [{'guid': '{F9D77864-FAC8-457E-BD4F-552715151B6C}', 'id': 277}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 30}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 40}, {'guid': '{5EFC7975-14BC-11CF-9B2B-00AA00573819}', 'id': 684}, {'guid': '{1496A755-94DE-11D0-8C3F-00C04FC2AAE2}', 'id': 1990}] | [화면](../assets/imports/docs/validation/physical-shortcuts-092/VA-7.png) |
| CTRL+0 | True | True | [{'guid': '{F9D77864-FAC8-457E-BD4F-552715151B6C}', 'id': 271}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 30}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 40}, {'guid': '{5EFC7975-14BC-11CF-9B2B-00AA00573819}', 'id': 684}, {'guid': '{1496A755-94DE-11D0-8C3F-00C04FC2AAE2}', 'id': 1990}] | [화면](../assets/imports/docs/validation/physical-shortcuts-092/VA-8.png) |
| ALT+G | True | True | [{'guid': '{F9D77864-FAC8-457E-BD4F-552715151B6C}', 'id': 256}, {'guid': '{5EFC7975-14BC-11CF-9B2B-00AA00573819}', 'id': 935}, {'guid': '{5EFC7975-14BC-11CF-9B2B-00AA00573819}', 'id': 935}] | [화면](../assets/imports/docs/validation/physical-shortcuts-092/VA-9.png) |
| ALT+SHIFT+F | True | True | [{'guid': '{F9D77864-FAC8-457E-BD4F-552715151B6C}', 'id': 258}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 30}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 40}, {'guid': '{5EFC7975-14BC-11CF-9B2B-00AA00573819}', 'id': 684}, {'guid': '{1496A755-94DE-11D0-8C3F-00C04FC2AAE2}', 'id': 1990}] | [화면](../assets/imports/docs/validation/physical-shortcuts-092/VA-10.png) |
| ALT+SHIFT+O | True | True | [{'guid': '{F9D77864-FAC8-457E-BD4F-552715151B6C}', 'id': 259}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 30}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 40}, {'guid': '{5EFC7975-14BC-11CF-9B2B-00AA00573819}', 'id': 684}, {'guid': '{1496A755-94DE-11D0-8C3F-00C04FC2AAE2}', 'id': 1990}] | [화면](../assets/imports/docs/validation/physical-shortcuts-092/VA-11.png) |
| ALT+SHIFT+S | True | True | [{'guid': '{F9D77864-FAC8-457E-BD4F-552715151B6C}', 'id': 260}] | [화면](../assets/imports/docs/validation/physical-shortcuts-092/VA-12.png) |
| ALT+O | True | False | [] | [화면](../assets/imports/docs/validation/physical-shortcuts-092/VA-13.png) |
| ALT+M | True | True | [{'guid': '{F9D77864-FAC8-457E-BD4F-552715151B6C}', 'id': 262}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 30}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 40}, {'guid': '{5EFC7975-14BC-11CF-9B2B-00AA00573819}', 'id': 684}, {'guid': '{1496A755-94DE-11D0-8C3F-00C04FC2AAE2}', 'id': 1990}] | [화면](../assets/imports/docs/validation/physical-shortcuts-092/VA-14.png) |
| ALT+SHIFT+G | True | True | [{'guid': '{F9D77864-FAC8-457E-BD4F-552715151B6C}', 'id': 263}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 30}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 40}, {'guid': '{5EFC7975-14BC-11CF-9B2B-00AA00573819}', 'id': 684}, {'guid': '{1496A755-94DE-11D0-8C3F-00C04FC2AAE2}', 'id': 1990}] | [화면](../assets/imports/docs/validation/physical-shortcuts-092/VA-15.png) |
| ALT+LEFT | True | True | [{'guid': '{F9D77864-FAC8-457E-BD4F-552715151B6C}', 'id': 264}, {'guid': '{5EFC7975-14BC-11CF-9B2B-00AA00573819}', 'id': 809}] | [화면](../assets/imports/docs/validation/physical-shortcuts-092/VA-16.png) |
| ALT+RIGHT | True | False | [{'guid': '{1496A755-94DE-11D0-8C3F-00C04FC2AAE2}', 'id': 107}] | [화면](../assets/imports/docs/validation/physical-shortcuts-092/VA-17.png) |
| ALT+SHIFT+R | True | False | [{'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 30}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 40}, {'guid': '{5EFC7975-14BC-11CF-9B2B-00AA00573819}', 'id': 684}, {'guid': '{1496A755-94DE-11D0-8C3F-00C04FC2AAE2}', 'id': 1990}] | [화면](../assets/imports/docs/validation/physical-shortcuts-092/VA-18.png) |
| ALT+SHIFT+Q | True | True | [{'guid': '{F9D77864-FAC8-457E-BD4F-552715151B6C}', 'id': 267}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 1}] | [화면](../assets/imports/docs/validation/physical-shortcuts-092/VA-19.png) |
| ALT+SHIFT+C | True | True | [{'guid': '{F9D77864-FAC8-457E-BD4F-552715151B6C}', 'id': 268}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 1}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 30}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 40}, {'guid': '{5EFC7975-14BC-11CF-9B2B-00AA00573819}', 'id': 684}, {'guid': '{1496A755-94DE-11D0-8C3F-00C04FC2AAE2}', 'id': 1990}] | [화면](../assets/imports/docs/validation/physical-shortcuts-092/VA-20.png) |
| ALT+DOWN | True | False | [{'guid': '{2A8866DC-7BDE-4DC8-A360-A60679534384}', 'id': 259}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 30}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 40}, {'guid': '{5EFC7975-14BC-11CF-9B2B-00AA00573819}', 'id': 684}, {'guid': '{1496A755-94DE-11D0-8C3F-00C04FC2AAE2}', 'id': 1990}] | [화면](../assets/imports/docs/validation/physical-shortcuts-092/VA-21.png) |
| ALT+UP | True | False | [{'guid': '{2A8866DC-7BDE-4DC8-A360-A60679534384}', 'id': 258}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 30}, {'guid': '{4C7763BF-5FAF-4264-A366-B7E1F27BA958}', 'id': 40}, {'guid': '{5EFC7975-14BC-11CF-9B2B-00AA00573819}', 'id': 684}, {'guid': '{1496A755-94DE-11D0-8C3F-00C04FC2AAE2}', 'id': 1990}] | [화면](../assets/imports/docs/validation/physical-shortcuts-092/VA-22.png) |

프로그램: tools/ShortcutProbe/ShortcutProbe.cs. JSON: 전체 증거

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/docs/physical-shortcuts-092.html)

- 원문 SHA-256: `7716568ec854eea443d4485c1e4ad3cbe4aff8a1653ea0815ef467f781f92014`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
