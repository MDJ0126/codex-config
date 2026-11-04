# Pixel Foliage 가이드

Pixora Studio 제품군의 독립 식생 도구입니다. 잔디·꽃·관목·나무가 바람·구름 명암·접촉 반응을 공유합니다.

## 시작하기

1. Tools → TurnRoom → Pixora Studio → Foliage를 엽니다. 예제 찾기로 09-MixedGarden을 열고 Play를 누르면 방문자가 식물 사이를 이동합니다. 씬 변경은 사용자가 직접 수행합니다.

2. 식생 구역에 월드와 프로필을 지정합니다. 프로필에는 하나의 서브메시와 재질이 필요합니다. 메시 원점은 밑동이며 정점 알파는 휘어짐 가중치입니다. 잔디는 크게 흔들리고 관목은 작게 움직입니다. 나무는 뿌리·줄기를 고정하고 수관을 움직일 수 있습니다.

3. 애니메이션 FPS를0으로 두면 연속,5/8/12이면 단계별로 움직입니다. 갱신 시점 분산은 식물별 갱신을 어긋나게 하며 렌더 FPS를 제한하지 않습니다. 시간 직접 지정으로 같은 순간을 비교합니다.

4. 지형 배치는 표면 레이어·탐색 높이·최대 경사를 정하고 콜라이더 표면에 굽기를 누릅니다. 배치 목록이 교체되며 실행 취소로 복원됩니다. 탐색 실패와 트리거 콜라이더는 제외됩니다.

## 선택 연동

Pixel Camera를 별도 설치하고 씬 카메라와 Full Camera 프리셋을 선택한 뒤 연결합니다. URP Renderer Feature는 별도 설정해야 합니다. 선택 예제11-PixelCameraGarden은 묶음 구성에 포함됩니다.

Pixora에서 투명 텍스처를 내보내 선택합니다. 텍스처로 재질 만들기는 새 재질을 저장하며 원본 텍스처·작업 문서를 수정하지 않습니다. 스프라이트 식물에는 UV가 있는 quad와 카메라 향하기를 사용합니다.

## 예제

- 01-MeadowBreeze.unity · MeadowGrass · 0 FPS
- 02-BlossomWalk.unity · IvoryDaisy · 0 FPS
- 03-ShrubGarden.unity · RoundedShrub · 0 FPS
- 04-YoungWoodland.unity · BranchingTree · 0 FPS
- 05-ContactTrail.unity · MeadowGrass · 0 FPS
- 06-SteppedRhythm.unity · MeadowGrass · 5 FPS
- 07-SharedGusts.unity · RoseBlossom · 0 FPS
- 08-CloudLight.unity · MeadowGrass · 0 FPS
- 09-MixedGarden.unity · MeadowGrass · 0 FPS
- 10-ExtensionGrove.unity · BranchingTree · 0 FPS
- 12-PrismaticWoodland.unity · Layered colorful woodland · 0 FPS

## 검증과 제한

개발 미리보기0.1.0입니다. C#·HLSL 정적 검사와 Unity 가져오기·Play 검증은 별개입니다. 화면 조작 보류로 실제 외형·성능·선택기·실행 취소·모든 언어 배치는 미검증입니다. 필수 Affinity 로고도 남아 있습니다.

Built-in·URP17 셰이더 경로를 구현했습니다. HDRP·XR·모바일 성능·모션 벡터 지원은 확인하지 않았습니다. 명암은 자체 방향광 스타일과 구름 변조이며 전체 PBR·추가 광원 처리가 아닙니다. 접촉은 구역별 가까운16개를 사용합니다.

독립 예제10개는 Camera·Pixora 없이 사용합니다. 샘플·선택 연동을 제거해도 본체는 이들을 참조하지 않습니다. 가져오기는 씬을 열거나 빌드 설정을 변경하지 않습니다.

접촉 갱신 빈도는 기본10Hz이며0이면 요청마다 수집합니다. 잔류 반응 시간은 유닛이 지난 자리가 복원되는 시간이며0이면 기록을 끕니다. 애니메이션 FPS0은 연속 바람입니다. 단계별 움직임은 표현 방식이며 GPU 렌더링을 생략하지 않습니다. 선택 항목인 SoftGarden-Lightweight 카메라 프리셋은 기존 Automatic 조건에서 원본 화면을 재사용하며 원래 지오메트리 렌더 해상도를 낮추지 않습니다. 화면 조작 보류로 현재 UI와 개선 씬의 새 캡처는 미검증입니다.

## 렌더링과 모듈 옵션

전체 카메라 빌보드는 카메라 기울기와 회전을 따르고, 카메라 향하기는 식물을 세워 둡니다. UV 휘어짐·UV 원근 변형은 UV와 텍스처가 있는 카드에 사용합니다. 색상 영역 강도·배율·경계 부드러움과 세 영역 색상이 큰 색상 군집을 만듭니다. 잔디·지면 예제는 같은 영역 설정을 사용합니다. 구름 높이 투영은 두 노이즈 층을 빛 방향으로 구름 높이에 투영하며 수평 광원은 안전하게 처리합니다. 방향 광원에 씬 조명을 연결할 수 있고, 씬 조명 사용은 Built-in/URP 주광원·그림자 수신 경로를 켭니다. 추가 광원과 전체 PBR은 범위 밖입니다. 디버그 보기는 일반·바람·접촉입니다. 08씬에는 실제 방향 광원, 10씬에는 세운 빌보드와 전체 빌보드 비교가 있습니다. 06씬의 네 FPS 구역은 동일한 방향 모드를 유지합니다. 현재 외형·그림자 바이어스·실제 UI 배치는 실행 검증이 남았습니다.



<!-- prismatic-woodland -->

## Prismatic Woodland

Prismatic Woodland: Samples/Scenes/12-PrismaticWoodland.unity를 여십시오. 정면에 가까운 직교 카메라로 8가지 수목 색상, 관목·고사리·꽃, 현무암 절벽과 늘어진 덩굴을 구성한 참고 이미지 연구 예제입니다. 수목과 하층 식생은 바람을 공유하며 절벽·덩굴은 정적입니다. 대기와 부드러운 빛은 샘플 메시 재질이며 이미지 텍스처·별도 후처리 패키지를 요구하지 않습니다. 선택 통합 예제 13-PixelCameraWoodland는 같은 씬에 360p Full Camera를 적용합니다. URP는 기존 Renderer Feature 설정이 필요합니다. Built-in 실제 외형은 개발 QA 프로젝트에서 촬영했으며, 성능과 원본과의 동일성은 미검증입니다. 수정 씬은 나무48그루·관목45개·고사리260개·꽃 군락136개입니다. 앞쪽의 드러난 절벽은 강물로 덮고 실제 반사를 추가했습니다. 샘플 전용 평면 반사와 부드러운 카메라 Bloom은 Built-in 전용이며 URP 기능이 아닙니다. 반사는 기본 너비512픽셀·12Hz이고 갱신 시 추가 장면 렌더가 발생합니다. 선택 Pixel Camera는 Bloom 다음에 적용됩니다. 실제 촬영 보고서에서 외형을 검토하며 플랫폼별 성능을 보장하지 않습니다.

<!-- /prismatic-woodland -->

<!-- pastel-examples -->

따뜻한 파스텔 예제: Camera의 주요·개발 예제는 기존 카메라와 효과 시연을 유지하며 조명을 부드럽게 하고 정원 경계를 보강했습니다. 식생 예제는 접촉 경로와 동일 조건의 프레임률 비교 구획을 유지하며 나무·관목·꽃 경계를 추가했습니다. Prismatic Woodland는 나무48그루·관목45개·고사리260개·꽃 군락136개·이끼 낀 수호자·부엉이·토끼 동상6개로 구성됩니다. 동상은 정적이며 식물은 바람을 공유합니다. 기하 증가로 렌더링 비용이 늘며 최적화 기능이 아닙니다. 평면 수면 반사와 Bloom은 Built-in 전용 샘플 효과입니다.

<!-- /pastel-examples -->









<!-- palette-guide-link -->

[식생 팔레트](../PaletteGuide.ko.html)

<!-- /palette-guide-link -->
