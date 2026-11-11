using UnityEngine;

namespace Fantasya
{
    /// <summary>
    /// 동료의 영구 식별자와 초기 이름·성격을 저장하고 세션 간 같은 동료를 연결한다.
    /// </summary>
    [CreateAssetMenu(menuName = "Fantasya/AI/Companion Persona")]
    public sealed class CompanionPersona : ScriptableObject
    {
        public string companionId = "luna";
        public string displayName = "루나";
        [TextArea(3, 6)] public string personality = "신중하고 관찰력이 좋은 정찰 동료.";
    }
}
