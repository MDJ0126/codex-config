# PC 웹 공개 준비

대상은 PC 웹·키보드/마우스·1920×1080·하드웨어 가속 WebGL2입니다. 실제 검증 브라우저 버전과 PC 성능은 QA/WebOptimization 결과에 기록합니다. 다른 기기·최저 사양은 실기 검증 전 보장하지 않습니다.

## 호스팅

`Tools/hosting-nginx.conf`에 gzip MIME/Content-Encoding 설정을 제공합니다. `.wasm.gz`는 application/wasm, `.js.gz`는 application/javascript와 gzip 헤더가 필요합니다. 파일을 직접 열지 않고 HTTP(S)로 제공합니다. 현재 검사 서버는 루프백 전용이며 인터넷 공개 서비스가 아닙니다. 도메인 구매·유료 호스팅 결제는 진행하지 않았습니다.

## 라이선스

- [Low Poly Animated People](https://assetstore.unity.com/packages/3d/characters/humanoids/low-poly-animated-people-156748): 공식 페이지에서 Standard Unity Asset Store EULA를 확인했습니다. 세부 License type 표기에 Extension Asset이 있으므로 실제 구매 라이선스·사용 인원 조건은 계정의 구매 증빙으로 확인해야 합니다.
- [Low Poly Epic City](https://assetstore.unity.com/packages/3d/environments/urban/low-poly-epic-city-202624): 공식 페이지에서 Standard Unity Asset Store EULA를 확인했습니다.
- [Unity 공식 EULA FAQ](https://assetstore.unity.com/browse/eula-faq)는 적절한 라이선스를 취득한 에셋의 게임 내 통합 사용과 원본 재배포를 구분합니다. 프로젝트에 임포트되어 있다는 사실만으로 구매·사용 인원 충족을 확정하지 않습니다.
- Nanum Gothic은 프로젝트의 Assets/Fonts/OFL.txt를 보존합니다. 사용자 입력 때문에 한글 완성형11,172자를 포함한 폰트를 유지합니다.

에셋·패키지·설치 폴더 원본은 수정하지 않고, 자체 코드·씬·복제한 Web 렌더 설정·공식 WebGL 플러그인으로 확장합니다. 구매 증빙 미확인 상태이므로 게임 빌드의 인터넷 배포는 별도 확인이 필요합니다. 공개 QA 보고서에는 실제 실행 화면과 계측 자료만 게시합니다.

## 이름과 도메인

Afternoon Pause/오후의 틈은 현재 시제품 명칭입니다. 상표 권리·사업자 이름 중복·최종 도메인 확보는 아직 확정하지 않았습니다. [KIPRIS](https://www.kipris.or.kr/)의 상품/서비스업별 검색과 등록기관의 최종 도메인 확인이 필요하며, 단순 검색·DNS 응답을 상표 사용 가능 판정으로 취급하지 않습니다.

## 실측 합격 조건과 제한

현재 QA의 국소 성능 기준은 1920×1080에서 FPS 중앙값28 이상·프레임 시간95백분위50ms 이하입니다. 기본 목표30FPS, 배경 목표5FPS와 AI 동작 정지·복귀30FPS를 별도 판정합니다. 고정 산출물 해시는 `QA/WebOptimization/frozen-manifest.json`에 보존합니다. 계측 CPU는 브라우저 renderer의 단일 코어 점유 시간 비율입니다.

0/6/12명 모두 같은 캐릭터·시점·빌드·뷰포트로 비교하고 실제 AI 경로 요청·이동 상태·프레임·Managed/JS/WASM 메모리 값을 원자료에 남깁니다. 짧은 CPU 차이만으로 원인을 확정하지 않습니다. 최적화12명120초 검사는 짧은 유지 검사이며, 수시간 안정성·장기 메모리 수렴·백그라운드 장기 체류는 별도 미실행입니다. 구매 확인과 상표/도메인 최종 확인도 남겨 둡니다.
