using UnityEngine;

namespace Overdox
{
    /// <summary>
    /// 원작 로비 카메라 클립과 배경 상태를 현재 플레이어의 좌표계에 맞춰 재생한다.
    /// </summary>
    [DefaultExecutionOrder(200)]
    public sealed class RestoredLobbyPresentation : MonoBehaviour
    {
        // 에디터 복원 도구가 생성한 클립과 참조를 씬에 저장한다. 원본 클립은 수정하지 않는다.
        public AnimationClip toEquipment, toLobby;
        public Transform sampledPose;
        public Animator backgroundAnimator;
        public Vector3 nativeHeroOrigin = new(-1051, 60, 3984);

        // 원작 씬의 1,000단위 거리를 복원 프로젝트의 1단위 거리로 환산한다.
        private const float NATIVE_POSITION_SCALE = .001f;
        private AnimationClip _clip;
        private float _time;

        /// <summary>
        /// 홈 복귀 클립의 마지막 자세를 초기 로비 카메라 자세로 준비한다.
        /// </summary>
        private void OnEnable()
        {
            _clip = toLobby;
            _time = toLobby != null ? toLobby.length : 0;
        }

        /// <summary>
        /// 로비에서만 원작 클립을 샘플링하고 캐릭터 기준 카메라 위치와 회전을 적용한다.
        /// </summary>
        private void LateUpdate()
        {
            var session = ArenaSession.Instance;
            if (session == null || !session.IsLobby || session.Player == null || _clip == null || sampledPose == null)
            {
                return;
            }

            // UI 전환은 전투 시간 배율과 독립적으로 재생하며 클립 끝에서 자세를 유지한다.
            _time = Mathf.Min(_time + Time.unscaledDeltaTime, _clip.length);
            _clip.SampleAnimation(sampledPose.gameObject, _time);
            // 클립의 절대 좌표에서 원작 영웅 원점을 제거한 뒤 현재 플레이어 위치에 더한다.
            transform.SetPositionAndRotation(session.Player.transform.position + (sampledPose.localPosition - nativeHeroOrigin) * NATIVE_POSITION_SCALE, sampledPose.localRotation);
        }

        /// <summary>
        /// 지정한 화면의 카메라 이동을 처음부터 재생하고 로비 배경 애니메이션을 전환한다.
        /// </summary>
        /// <param name="isEquipment">장비 화면이면 true, 홈 화면이면 false를 전달한다.</param>
        public void Transition(bool isEquipment)
        {
            _clip = isEquipment ? toEquipment : toLobby;
            _time = 0;
            if (backgroundAnimator != null)
            {
                // 반대 트리거가 대기한 채 다음 전환에 소비되지 않도록 먼저 해제한다.
                backgroundAnimator.ResetTrigger(isEquipment ? "Lobby_Idle" : "Lobby_Move");
                backgroundAnimator.SetTrigger(isEquipment ? "Lobby_Move" : "Lobby_Idle");
            }
        }
    }
}
