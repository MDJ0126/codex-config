using UnityEngine.UI;

namespace Overdox
{
    /// <summary>
    /// 원작의 낮은 depth 그림자를 본 이미지보다 먼저 그리는 동일 Graphic의 메시 효과로 표시한다.
    /// </summary>
    public sealed class RestoredNativeShadow : Shadow
    {
        // 별도 Canvas나 매 프레임 위치 동기화 없이 기본 Shadow의 앞쪽 메시 삽입을 사용한다.
        // 원본 자식 Transform·Image·바인딩은 보존하고, 중복으로 덮어 그리는 Image만 비활성화한다.
    }
}
