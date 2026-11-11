using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Fantasya
{
    /// <summary>
    /// 동료 한 명의 모델 판단과 즉시 대응을 분리하고 플레이어와 같은 이동·공격을 실행한다.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Monster))]
    public sealed class AIAgentController : MonoBehaviour
    {
        [SerializeField] private AgentModelSettings _modelSettings;
        [SerializeField] private Monster _leader;
        [SerializeField] private string _agentName = "Companion";
        [SerializeField] private Vector3 _formationOffset = new(-2f, 0f, -2f);
        [SerializeField, Min(1f)] private float _awarenessRadius = 8f;
        [SerializeField, Min(1f)] private float _leashRadius = 12f;
        [SerializeField, Range(0.05f, 0.9f)] private float _retreatHealthRatio = 0.25f;
        private readonly Dictionary<int, Monster> _offeredTargets = new();
        private Monster _monster;
        private Monster _target;
        private string _action = "follow";
        private float _leaseEnds;
        private double _nextDecision;
        private double _queuedAt;
        private double _requestedAt;
        private double _threatObservedAt;
        private bool _isEmergency;
        private bool _hasSlot;
        private AgentModelSettings _slotSettings;
        private UnityWebRequest _request;
        private Coroutine _thinking;
        private int _generation;
        private Vector3 _progressPosition;
        private float _progressAt;
        private bool _isBlocked;

        public string AgentName => _agentName;
        public string Action => _isEmergency ? "retreat (reflex)" : _action;
        public string ModelName => _modelSettings != null && _modelSettings.useModel ? _modelSettings.model : "Rules only";
        public string Status { get; private set; } = "Ready";
        public double LastModelMilliseconds { get; private set; } = -1;
        public double MeanModelMilliseconds { get; private set; }
        public double LastQueueMilliseconds { get; private set; } = -1;
        public double LastApplyMilliseconds { get; private set; } = -1;
        public double LastReflexMilliseconds { get; private set; } = -1;
        public double WaitingMilliseconds => _thinking != null ? (Time.realtimeSinceStartupAsDouble - _queuedAt) * 1000d : 0d;
        public int SuccessfulRequests { get; private set; }
        public int FailedRequests { get; private set; }
        public int RejectedDecisions { get; private set; }
        public string LastDecisionJson { get; private set; } = string.Empty;
        public Monster Character => _monster;

        /// <summary>
        /// 생성한 동료에 리더·모델·대형 위치를 연결하고 초기 판단을 분산한다.
        /// </summary>
        public void Configure(Monster leader, AgentModelSettings settings, string agentName, Vector3 offset)
        {
            CancelThinking();
            _leader = leader;
            _modelSettings = settings;
            _agentName = agentName;
            _formationOffset = offset;
            _nextDecision = Time.realtimeSinceStartupAsDouble + UnityEngine.Random.Range(0f, 1f);
        }

        /// <summary>
        /// 조작 대상을 찾고 경쟁하는 플레이어·몬스터 AI가 있으면 연결을 거부한다.
        /// </summary>
        private void Awake()
        {
            _monster = GetComponent<Monster>();
            _progressPosition = transform.position;
            if (GetComponent<MonsterAI>() != null)
            {
                Debug.LogError("Remove MonsterAI and BehaviorGraphAgent before attaching AIAgentController.", this);
                enabled = false;
                return;
            }
            foreach (PlayerController player in FindObjectsByType<PlayerController>(FindObjectsSortMode.None))
            {
                if (player.ControlledCharacter == _monster)
                {
                    Debug.LogError("A character cannot have both player and agent control.", this);
                    enabled = false;
                }
            }
        }

        /// <summary>
        /// 긴급 대응을 먼저 실행하며 모델 응답과 무관하게 현재 행동을 매 프레임 수행한다.
        /// </summary>
        private void Update()
        {
            if (!_monster.IsAlive)
            {
                CancelThinking();
                Status = _monster.MaxHealth == 0 ? "Loading character" : "Dead";
                return;
            }
            Monster nearest = FindNearestEnemy();
            bool emergency = nearest != null && _monster.Health <= _monster.MaxHealth * _retreatHealthRatio;
            if (emergency && !_isEmergency)
            {
                _threatObservedAt = _monster.LastDamageRealtime >= 0d ? _monster.LastDamageRealtime : Time.realtimeSinceStartupAsDouble;
            }
            _isEmergency = emergency;

            if (_isEmergency)
            {
                MoveAway(nearest);
                if (_threatObservedAt > 0d)
                {
                    LastReflexMilliseconds = (Time.realtimeSinceStartupAsDouble - _threatObservedAt) * 1000d;
                    _threatObservedAt = 0d;
                }
            }
            else
            {
                ExecuteAction(nearest);
            }

            if (_modelSettings != null && _modelSettings.useModel)
            {
                if (_thinking == null && Time.realtimeSinceStartupAsDouble >= _nextDecision)
                {
                    _queuedAt = Time.realtimeSinceStartupAsDouble;
                    _thinking = StartCoroutine(Think(_generation));
                }
            }
            else
            {
                Status = "Rules only";
            }
        }

        /// <summary>
        /// 유효 기간과 전투 범위를 검사해 선택된 행동 또는 기본 엄호를 실행한다.
        /// </summary>
        private void ExecuteAction(Monster nearest)
        {
            if (Time.time >= _leaseEnds)
            {
                _action = "follow";
                _target = null;
            }
            if (_action == "hold")
            {
                Drive(Vector3.zero);
                return;
            }
            if (_action == "retreat" && nearest != null)
            {
                MoveAway(nearest);
                return;
            }
            Monster target = _action == "attack" ? _target : nearest;
            if (_action != "retreat" && IsAllowedEnemy(target) &&
                (_action == "attack" || Offset(transform.position, target.transform.position).magnitude < 3f))
            {
                Vector3 offset = Offset(transform.position, target.transform.position);
                Drive(offset.magnitude > _monster.Reach * 0.85f ? offset.normalized : Vector3.zero, offset);
                if (_monster.CanHit(target))
                {
                    _monster.Attack(target);
                }
                return;
            }
            if (_leader != null && _leader.IsAlive)
            {
                Vector3 goal = _leader.transform.position + _leader.transform.TransformDirection(_formationOffset);
                Vector3 offset = Offset(transform.position, goal);
                Drive(offset.magnitude > 1f ? offset.normalized : Vector3.zero);
                return;
            }
            Drive(Vector3.zero);
        }

        /// <summary>
        /// 리더 활동 범위 안에서 가장 가까운 유효한 적을 찾는다.
        /// </summary>
        private Monster FindNearestEnemy()
        {
            Monster nearest = null;
            float distance = _awarenessRadius * _awarenessRadius;
            foreach (Monster candidate in Monster.ActiveMonsters)
            {
                if (IsAllowedEnemy(candidate))
                {
                    float candidateDistance = Offset(transform.position, candidate.transform.position).sqrMagnitude;
                    if (candidateDistance < distance)
                    {
                        nearest = candidate;
                        distance = candidateDistance;
                    }
                }
            }
            return nearest;
        }

        /// <summary>
        /// 진영·생존·관측 거리·리더 활동 범위를 현재 상태로 다시 검사한다.
        /// </summary>
        private bool IsAllowedEnemy(Monster target)
        {
            return _monster.CanDamage(target)
                && Offset(transform.position, target.transform.position).sqrMagnitude <= _awarenessRadius * _awarenessRadius
                && (_leader == null || Offset(_leader.transform.position, target.transform.position).sqrMagnitude <= _leashRadius * _leashRadius);
        }

        /// <summary>
        /// 가까운 적의 반대 방향으로 이동하며 후퇴 판단은 모델 호출을 기다리지 않는다.
        /// </summary>
        private void MoveAway(Monster threat)
        {
            Vector3 away = Offset(threat.transform.position, transform.position).normalized;
            if (away.sqrMagnitude < 0.01f)
            {
                away = -transform.forward;
            }
            Drive(away);
        }

        /// <summary>
        /// 지면과 장애물을 확인한 방향을 플레이어와 같은 모터에 전달하고 막힘을 기록한다.
        /// </summary>
        private void Drive(Vector3 direction, Vector3 facing = default)
        {
            _isBlocked = false;
            if (direction.sqrMagnitude > 0.01f && !CanStep(direction))
            {
                Vector3 left = Quaternion.AngleAxis(-60f, Vector3.up) * direction;
                Vector3 right = Quaternion.AngleAxis(60f, Vector3.up) * direction;
                direction = CanStep(left) ? left : CanStep(right) ? right : Vector3.zero;
                _isBlocked = direction.sqrMagnitude < 0.01f;
            }
            if (Time.time - _progressAt > 2f)
            {
                if (direction.sqrMagnitude > 0.01f && Offset(_progressPosition, transform.position).sqrMagnitude < 0.04f)
                {
                    _isBlocked = true;
                    direction = Vector3.zero;
                    _leaseEnds = 0f;
                }
                _progressPosition = transform.position;
                _progressAt = Time.time;
            }
            Vector3 forward = facing.sqrMagnitude > 0.01f ? facing.normalized : direction.sqrMagnitude > 0.01f ? direction.normalized : transform.forward;
            Vector3 rightAxis = Vector3.Cross(Vector3.up, forward);
            _monster.DrivePlayer(new Vector2(Vector3.Dot(direction, rightAxis), Vector3.Dot(direction, forward)), forward, false, false);
        }

        /// <summary>
        /// 원본 캐릭터 충돌을 유지하며 발 앞의 지면·경사와 장애물을 검사한다.
        /// </summary>
        private bool CanStep(Vector3 direction)
        {
            Vector3 point = transform.position + direction.normalized * 0.8f;
            if (!Physics.Raycast(point + Vector3.up * 1.5f, Vector3.down, out RaycastHit ground, 2.5f,
                Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore) || ground.normal.y < 0.707f || ground.collider.GetComponentInParent<Monster>() != null)
            {
                return false;
            }
            foreach (RaycastHit hit in Physics.SphereCastAll(transform.position + Vector3.up, 0.25f, direction.normalized, 0.65f,
                Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.GetComponentInParent<Monster>() != _monster && hit.normal.y < 0.707f)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 최신 관측과 허용된 적 ID만 직렬화하고 게임에 없는 능력은 제시하지 않는다.
        /// </summary>
        private string BuildObservation()
        {
            _offeredTargets.Clear();
            var text = new StringBuilder();
            text.Append($"HP {_monster.Health}/{_monster.MaxHealth}; blocked={_isBlocked}; current={Action}. ");
            text.Append("Copy one of these options exactly: ");
            text.Append("{\"action\":\"hold\",\"targetId\":0,\"seconds\":3}, ");
            text.Append("{\"action\":\"guard\",\"targetId\":0,\"seconds\":3}, ");
            text.Append("{\"action\":\"retreat\",\"targetId\":0,\"seconds\":3}");
            if (_leader != null && _leader.IsAlive)
            {
                text.Append(", {\"action\":\"follow\",\"targetId\":0,\"seconds\":3}");
            }
            text.Append(". Enemies (attack permitted only on these IDs): ");
            foreach (Monster candidate in Monster.ActiveMonsters)
            {
                if (IsAllowedEnemy(candidate) && _offeredTargets.Count < 8)
                {
                    int id = _offeredTargets.Count + 1;
                    _offeredTargets.Add(id, candidate);
                    text.Append($"id={id},hp={candidate.Health},distance={Offset(transform.position, candidate.transform.position).magnitude:F1}; ");
                    text.Append("option={\"action\":\"attack\",\"targetId\":" + id + ",\"seconds\":3}; ");
                }
            }
            if (_offeredTargets.Count > 0)
            {
                text.Append("Option: attack with one of the listed IDs.");
            }
            return text.ToString();
        }

        /// <summary>
        /// 슬롯 확보 후 최신 상황을 요청하고 오래되거나 허용 범위를 벗어난 응답은 폐기한다.
        /// </summary>
        private IEnumerator Think(int generation)
        {
            yield return null;
            AgentModelSettings settings = _modelSettings;
            Status = "Queued";
            while (!AgentModelClient.TryAcquire(settings))
            {
                yield return null;
                if (generation != _generation || !settings.useModel)
                {
                    _thinking = null;
                    yield break;
                }
            }
            _hasSlot = true;
            _slotSettings = settings;
            _requestedAt = Time.realtimeSinceStartupAsDouble;
            LastQueueMilliseconds = (_requestedAt - _queuedAt) * 1000d;
            Status = "Thinking";
            UnityWebRequestAsyncOperation operation = null;
            try
            {
                string systemPrompt = "You control one cooperative game character. Select ONLY an offered option. " +
                    "Return exactly one JSON object: {\"action\":\"follow\",\"targetId\":0,\"seconds\":3}. " +
                    "No prose or markdown. seconds must be 1..6. Use listed targetId only for attack, otherwise 0. " + settings.personality;
                _request = AgentModelClient.CreateRequest(settings, systemPrompt, BuildObservation());
                operation = _request.SendWebRequest();
            }
            catch (Exception)
            {
                Fail("Invalid connection settings");
            }
            if (operation != null)
            {
                yield return operation;
                LastModelMilliseconds = (Time.realtimeSinceStartupAsDouble - _requestedAt) * 1000d;
                if (_request.result != UnityWebRequest.Result.Success)
                {
                    Fail("Connection / HTTP error " + _request.responseCode);
                }
                else
                {
                    try
                    {
                        AgentDecision decision = JsonUtility.FromJson<AgentDecision>(AgentModelClient.ParseContent(_request.downloadHandler.text));
                        if (decision == null || string.IsNullOrEmpty(decision.action) || float.IsNaN(decision.seconds) ||
                            float.IsInfinity(decision.seconds) || decision.seconds < 1f || decision.seconds > 6f)
                        {
                            throw new FormatException("Invalid action duration.");
                        }
                        LastDecisionJson = JsonUtility.ToJson(decision);
                        if (generation != _generation || !settings.useModel || !_monster.IsAlive ||
                            Time.realtimeSinceStartupAsDouble - _requestedAt > settings.maxDecisionAge || !TryApply(decision))
                        {
                            RejectedDecisions++;
                            Status = "Discarded stale / invalid action";
                        }
                        else
                        {
                            SuccessfulRequests++;
                            MeanModelMilliseconds += (LastModelMilliseconds - MeanModelMilliseconds) / SuccessfulRequests;
                            LastApplyMilliseconds = (Time.realtimeSinceStartupAsDouble - _queuedAt) * 1000d;
                            Status = "Applied";
                        }
                    }
                    catch (Exception)
                    {
                        Fail("Invalid model JSON");
                    }
                }
            }
            ReleaseRequest();
            _nextDecision = Time.realtimeSinceStartupAsDouble + (Status.StartsWith("Error") ? settings.failureBackoff : settings.decisionInterval);
            _thinking = null;
        }

        /// <summary>
        /// 허용된 행동과 대상만 현재 게임 상태에 반영하고 유효 기간을 설정한다.
        /// </summary>
        private bool TryApply(AgentDecision decision)
        {
            Monster target = null;
            if (decision.action == "attack")
            {
                if (!_offeredTargets.TryGetValue(decision.targetId, out target) || !IsAllowedEnemy(target))
                {
                    return false;
                }
            }
            else if (decision.targetId != 0 || !(decision.action == "hold" || decision.action == "guard" || decision.action == "retreat" ||
                (decision.action == "follow" && _leader != null && _leader.IsAlive)))
            {
                return false;
            }
            _action = decision.action;
            _target = target;
            _leaseEnds = Time.time + decision.seconds;
            return true;
        }

        /// <summary>
        /// 오류를 측정 실패로 기록하고 안전한 기존 행동을 유지한다.
        /// </summary>
        private void Fail(string message)
        {
            FailedRequests++;
            Status = "Error: " + message;
        }

        /// <summary>
        /// 통신 객체를 정리하고 요청 슬롯을 정확히 한 번 반환한다.
        /// </summary>
        private void ReleaseRequest()
        {
            _request?.Dispose();
            _request = null;
            if (_hasSlot)
            {
                AgentModelClient.Release(_slotSettings);
                _hasSlot = false;
                _slotSettings = null;
            }
        }

        /// <summary>
        /// 재연결·비활성화·사망 시 진행 중인 요청과 대기를 취소한다.
        /// </summary>
        private void CancelThinking()
        {
            _generation++;
            if (_thinking != null)
            {
                StopCoroutine(_thinking);
                _thinking = null;
            }
            _request?.Abort();
            ReleaseRequest();
        }

        private void OnDisable() => CancelThinking();

        /// <summary>
        /// 높이를 제외한 거리와 이동 방향을 계산한다.
        /// </summary>
        private static Vector3 Offset(Vector3 from, Vector3 to)
        {
            Vector3 offset = to - from;
            offset.y = 0f;
            return offset;
        }
    }

    /// <summary>
    /// 모델이 선택할 수 있는 Fantasya 행동과 대상 및 유효 기간을 전달한다.
    /// </summary>
    [Serializable]
    public sealed class AgentDecision
    {
        public string action;
        public int targetId;
        public float seconds;
    }
}
