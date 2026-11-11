using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Fantasya
{
    /// <summary>
    /// 플레이어 입력을 몬스터에 전달하고 카메라가 조작 대상을 따라가게 한다.
    /// </summary>
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private Monster _monster;
        [SerializeField] private Transform _followCamera;
        [SerializeField, Min(0f)] private float _cameraSensitivity = 0.2f;
        private Vector3 _cameraOffset;
        private Quaternion _cameraRotation;

        /// <summary>
        /// 입력을 받는 대상을 제공해 동료 생성과 중복 컨트롤 검사를 연결한다.
        /// </summary>
        public Monster ControlledCharacter => _monster;

        /// <summary>
        /// 씬에서 배치한 몬스터와 카메라 사이의 간격을 기록한다.
        /// </summary>
        private void Awake()
        {
            _cameraOffset = _followCamera.position - _monster.transform.position;
            _cameraRotation = _followCamera.rotation;
        }

        /// <summary>
        /// 이동·공격 입력을 전달하고 재시작 입력 시 현재 씬을 다시 불러온다.
        /// </summary>
        private void Update()
        {
            if (AgentConversationPanel.IsTyping)
            {
                Vector3 idleForward = Vector3.ProjectOnPlane(_cameraRotation * Vector3.forward, Vector3.up).normalized;
                _monster.DrivePlayer(Vector2.zero, idleForward, false, false);
                return;
            }
            UpdateCameraOrbit();
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return;
            }

            if (keyboard.rKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene(gameObject.scene.buildIndex);
                return;
            }

            float horizontal = Axis(keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed, keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed);
            float vertical = Axis(keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed, keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed);
            Vector3 forward = Vector3.ProjectOnPlane(_cameraRotation * Vector3.forward, Vector3.up).normalized;
            bool sprint = keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed;
            _monster.DrivePlayer(new Vector2(horizontal, vertical), forward, sprint, keyboard.spaceKey.wasPressedThisFrame);

            bool mouseAttack = Mouse.current?.leftButton.wasPressedThisFrame == true
                && (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject());
            if (keyboard.fKey.wasPressedThisFrame || mouseAttack)
            {
                _monster.Attack(SelectOpponent());
            }
        }

        /// <summary>
        /// 공격할 수 있는 상대 중 가까운 쪽을 선택한다.
        /// </summary>
        private Monster SelectOpponent()
        {
            Monster nearest = null;
            float nearestDistance = float.PositiveInfinity;
            foreach (Monster candidate in Monster.ActiveMonsters)
            {
                if (!_monster.CanHit(candidate))
                {
                    continue;
                }

                Vector3 offset = candidate.transform.position - _monster.transform.position;
                offset.y = 0f;
                if (offset.sqrMagnitude < nearestDistance)
                {
                    nearest = candidate;
                    nearestDistance = offset.sqrMagnitude;
                }
            }

            return nearest;
        }

        /// <summary>
        /// 우클릭으로 회전한 카메라 방향을 이번 프레임의 이동 입력에 반영한다.
        /// </summary>
        private void UpdateCameraOrbit()
        {
            Mouse mouse = Mouse.current;
            if (mouse != null && mouse.rightButton.isPressed)
            {
                Vector2 delta = mouse.delta.ReadValue() * _cameraSensitivity;
                Vector3 horizontalOffset = Vector3.ProjectOnPlane(_cameraOffset, Vector3.up);
                float elevation = Mathf.Atan2(_cameraOffset.y, horizontalOffset.magnitude) * Mathf.Rad2Deg;
                float pitch = Mathf.Clamp(elevation - delta.y, 10f, 75f) - elevation;
                Quaternion rotation = Quaternion.AngleAxis(delta.x, Vector3.up)
                    * Quaternion.AngleAxis(pitch, Vector3.Cross(horizontalOffset, Vector3.up));
                _cameraOffset = rotation * _cameraOffset;
                _cameraRotation = rotation * _cameraRotation;
            }
        }

        /// <summary>
        /// 이동이 끝난 플레이어 위치에 카메라를 맞춘다.
        /// </summary>
        private void LateUpdate()
        {
            _followCamera.SetPositionAndRotation(_monster.transform.position + _cameraOffset, _cameraRotation);
        }

        /// <summary>
        /// 반대 방향 키가 함께 눌리면 서로 상쇄되는 축 입력을 만든다.
        /// </summary>
        private static int Axis(bool positive, bool negative)
        {
            return (positive ? 1 : 0) - (negative ? 1 : 0);
        }
    }
}
