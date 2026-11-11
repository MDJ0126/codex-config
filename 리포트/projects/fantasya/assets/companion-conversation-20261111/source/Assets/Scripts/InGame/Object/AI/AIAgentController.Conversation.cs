using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Fantasya
{
    /// <summary>
    /// 지속되는 정체성·기억·약속과 외부 에이전트의 일시 제어를 기존 모터에 연결한다.
    /// </summary>
    public sealed partial class AIAgentController
    {
        private string _companionId = "companion";
        private string _personality = "플레이어를 돕는 동료.";
        private CompanionPromise _promise = new();
        private CompanionState _companionState;
        private bool _externalActive;
        private string _externalControllerName;
        private string _actionReason = "플레이어를 따라가며 주변을 살피고 있어요.";
        private string _lastReceipt = string.Empty;
        private bool _requiresSharedMemory;
        private bool _isMemoryReady;
        public string CompanionId => _companionId;
        public string Personality => _personality;
        public CompanionState Conversation => _companionState;
        public string PromiseText => _promise.text;
        public string ControllerName => _externalActive ? _externalControllerName : "로컬 모델";
        public string ActionReason => _requiresSharedMemory && !_isMemoryReady ? "동료의 기억과 약속을 불러오는 중이에요." :
            _isEmergency ? "체력이 낮아 가까운 적에게서 긴급 후퇴 중" :
            _leader != null && !_leader.IsAlive && EffectiveAction == "hold" ? "플레이어가 쓰러져 이동을 멈추고 있어요." :
            _promise.mode != "auto" ? "약속 우선: " + _promise.text : _actionReason;
        private string EffectiveAction
        {
            get
            {
                if (_requiresSharedMemory && !_isMemoryReady)
                {
                    return "hold";
                }
                string action = _promise.mode != "auto" ? _promise.mode : Time.time >= _leaseEnds ? "follow" : _action;
                return action == "follow" && (_leader == null || !_leader.IsAlive) ? "hold" : action;
            }
        }

        /// <summary>
        /// 파티의 안정적인 동료 ID를 연결하며 모델이 바뀌어도 같은 기억을 사용하게 한다.
        /// </summary>
        public void AssignIdentity(string companionId, string displayName, string personality)
        {
            _companionId = companionId;
            _agentName = displayName;
            _personality = personality;
            _requiresSharedMemory = GetComponentInParent<AgentCompanionLink>() != null;
            gameObject.name = displayName;
        }

        /// <summary>
        /// 실제로 허용되는 적과 현재 행동을 게임 밖에서도 관측할 수 있게 제공한다.
        /// </summary>
        public CompanionSnapshot GetSnapshot()
        {
            var targets = new List<CompanionTarget>();
            foreach (Monster candidate in Monster.ActiveMonsters)
            {
                if (targets.Count >= 8)
                {
                    break;
                }
                if (IsAllowedEnemy(candidate))
                {
                    targets.Add(new CompanionTarget
                    {
                        id = candidate.GetInstanceID(),
                        name = candidate.CharacterName,
                        health = candidate.Health,
                        distance = Offset(transform.position, candidate.transform.position).magnitude
                    });
                }
            }
            return new CompanionSnapshot
            {
                id = _companionId,
                name = _agentName,
                personality = _personality,
                alive = _monster.IsAlive,
                health = _monster.Health,
                maxHealth = _monster.MaxHealth,
                action = Action,
                reason = ActionReason,
                receipt = _lastReceipt,
                position = transform.position,
                leaderPosition = _leader != null ? _leader.transform.position : transform.position,
                leaderAlive = _leader != null && _leader.IsAlive,
                leaderHealth = _leader != null ? _leader.Health : 0,
                targets = targets.ToArray()
            };
        }

        /// <summary>
        /// 공유 기억을 갱신하고 약속 또는 외부 제어가 바뀌면 이전 모델 판단을 취소한다.
        /// </summary>
        public void ApplyCompanionState(CompanionState state, string sessionId)
        {
            if (state == null || state.id != _companionId)
            {
                return;
            }
            bool promiseChanged = state.promise != null &&
                (state.promise.updatedAt != _promise.updatedAt || state.promise.mode != _promise.mode || state.promise.allowAttack != _promise.allowAttack);
            bool controllerChanged = state.controllerActive != _externalActive || state.controllerName != _externalControllerName;
            bool personalityChanged = state.personality != _personality || state.name != _agentName;
            if (promiseChanged || controllerChanged || personalityChanged)
            {
                CancelThinking();
                _leaseEnds = 0f;
                _nextDecision = Time.realtimeSinceStartupAsDouble;
            }
            if (state.promise != null && IsPromiseMode(state.promise.mode))
            {
                _promise = state.promise;
                if (promiseChanged)
                {
                    _lastReceipt = "약속 적용: " + _promise.text + " [" + _promise.mode + "; " + _promise.updatedAt + "]";
                }
            }
            _companionState = state;
            _isMemoryReady = true;
            _externalActive = state.controllerActive;
            _externalControllerName = state.controllerName;
            if (!string.IsNullOrWhiteSpace(state.name))
            {
                _agentName = state.name;
            }
            if (!string.IsNullOrWhiteSpace(state.personality))
            {
                _personality = state.personality;
            }
            if (state.commands == null)
            {
                return;
            }
            foreach (CompanionCommand command in state.commands)
            {
                bool accepted = ApplyExternalCommand(command, sessionId);
                _lastReceipt = (accepted ? "명령 적용: " : "명령 거부: ") + command.id + " · " + command.action + " · " + command.reason;
            }
        }

        /// <summary>
        /// 외부 연결이 끊기면 일시 제어만 해제하고 저장된 플레이어 약속은 유지한다.
        /// </summary>
        public void ClearExternalControl()
        {
            if (!_externalActive)
            {
                return;
            }
            _externalActive = false;
            _externalControllerName = string.Empty;
            _leaseEnds = 0f;
            _nextDecision = Time.realtimeSinceStartupAsDouble;
            _actionReason = "외부 연결이 끊겨 로컬 판단으로 복귀했어요.";
        }

        /// <summary>
        /// 제어권·세션·시간·플레이어 약속과 현재 적 상태를 확인한 명령만 기존 실행기에 전달한다.
        /// </summary>
        private bool ApplyExternalCommand(CompanionCommand command, string sessionId)
        {
            long age = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - command.issuedAt;
            if (!_externalActive || command.owner != _externalControllerName || !_monster.IsAlive || command.sessionId != sessionId || age < -1000 || age > 8000 ||
                !IsActionPermitted(command.action) || float.IsNaN(command.seconds) || float.IsInfinity(command.seconds) || command.seconds < 1f || command.seconds > 6f)
            {
                return false;
            }
            Monster target = null;
            if (command.action == "attack")
            {
                target = Monster.ActiveMonsters.FirstOrDefault(candidate => candidate.GetInstanceID() == command.targetId && IsAllowedEnemy(candidate));
                if (target == null)
                {
                    return false;
                }
            }
            else if (command.targetId != 0 || (command.action == "follow" && (_leader == null || !_leader.IsAlive)))
            {
                return false;
            }
            _action = command.action;
            _target = target;
            _leaseEnds = Time.time + command.seconds;
            _actionReason = command.owner + ": " + command.reason;
            return true;
        }

        /// <summary>
        /// 임의 행동이나 플레이어 약속을 위반하는 선택을 모델과 외부 에이전트 양쪽에서 거부한다.
        /// </summary>
        private bool IsActionPermitted(string action)
        {
            bool known = IsPromiseMode(action) && action != "auto" || action == "attack";
            return known && (_promise.mode == "auto" || _promise.mode == action) && (action != "attack" || _promise.allowAttack);
        }

        private static bool IsPromiseMode(string mode) => mode == "auto" || mode == "follow" || mode == "guard" || mode == "hold" || mode == "retreat";

        /// <summary>
        /// 성격·지속 약속·기억·최근 대화를 다음 모델 판단의 관측으로 전달한다.
        /// </summary>
        private string BuildCompanionContext()
        {
            var text = new StringBuilder();
            text.Append("Identity: " + _agentName + ". Personality: " + _personality + ". ");
            text.Append("Player promise: " + JsonUtility.ToJson(_promise) + ". ");
            if (_companionState?.memories != null)
            {
                foreach (CompanionMemory memory in _companionState.memories.TakeLast(8))
                {
                    text.Append("Remembered: " + memory.text + ". ");
                }
            }
            if (_companionState?.messages != null)
            {
                foreach (CompanionMessage message in _companionState.messages.TakeLast(6))
                {
                    text.Append("Recent " + message.role + ": " + message.text + ". ");
                }
            }
            return text.ToString();
        }
    }
}
