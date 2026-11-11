using System.Collections.Generic;
using UnityEngine;

namespace Fantasya
{
    /// <summary>
    /// 플레이어 프리팹으로 같은 팀의 동료들을 생성하고 각 컨트롤러에 독립된 대형 위치를 배정한다.
    /// </summary>
    public sealed class AgentParty : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private Monster _companionPrefab;
        [SerializeField] private AgentModelSettings _modelSettings;
        [SerializeField, Range(0, 32)] private int _companionCount = 3;
        private readonly List<AIAgentController> _members = new();
        public IReadOnlyList<AIAgentController> Members => _members;

        /// <summary>
        /// 저장된 설정을 연결하며 씬 구성 도구 외에는 기존 Inspector 값을 덮어쓰지 않는다.
        /// </summary>
        public void Configure(PlayerController player, Monster prefab, AgentModelSettings settings)
        {
            _player = player;
            _companionPrefab = prefab;
            _modelSettings = settings;
        }

        /// <summary>
        /// 유효한 플레이어와 프리팹이 있을 때 요청한 수의 동료를 생성한다.
        /// </summary>
        private void Start()
        {
            if (_player == null || _player.ControlledCharacter == null || _companionPrefab == null)
            {
                Debug.LogError("Assign player and companion prefab to AgentParty.", this);
                return;
            }
            for (int index = 0; index < Mathf.Clamp(_companionCount, 0, 32); index++)
            {
                SpawnCompanion();
            }
        }

        /// <summary>
        /// 지면이 있는 대형 위치에 동료 하나를 추가하며 모델 요청 상태는 각자 소유하게 한다.
        /// </summary>
        public AIAgentController SpawnCompanion()
        {
            if (_player == null || _companionPrefab == null || _members.Count >= 32)
            {
                return null;
            }
            Monster leader = _player.ControlledCharacter;
            int index = _members.Count;
            Vector3 offset = new((index % 4 - 1.5f) * 1.8f, 0f, -2f - index / 4 * 1.8f);
            Vector3 position = leader.transform.position + leader.transform.TransformDirection(offset);
            if (!Physics.Raycast(position + Vector3.up * 3f, Vector3.down, out RaycastHit ground, 7f,
                Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore) || ground.collider.GetComponentInParent<Monster>() != null)
            {
                Debug.LogWarning("No ground at companion spawn point.", this);
                return null;
            }
            position.y = ground.point.y + 0.1f;
            Monster character = Instantiate(_companionPrefab, position, leader.transform.rotation);
            character.name = "AI Companion " + (index + 1);
            character.transform.SetParent(transform, true);
            var controller = character.GetComponent<AIAgentController>();
            if (controller == null)
            {
                controller = character.gameObject.AddComponent<AIAgentController>();
            }
            controller.Configure(leader, _modelSettings, character.name, offset);
            _members.Add(controller);
            return controller;
        }

        /// <summary>
        /// 씬 종료와 소유자 파괴 시 동료 목록을 비운다.
        /// </summary>
        private void OnDestroy()
        {
            _members.Clear();
        }
    }
}
