using Unity.Behavior;
using UnityEngine;

namespace Fantasya
{
    /// <summary>
    /// 공용 Behavior 그래프가 실행하는 순찰, 이동, 공격 동작을 제공한다.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BehaviorGraphAgent))]
    public sealed class MonsterAI : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float _noticeDistance = 7f;
        [SerializeField, Min(0.5f)] private float _patrolRadius = 4f;
        [SerializeField, Min(0.5f)] private float _leashRadius = 12f;

        private Monster _monster;
        private Vector3 _spawnPosition;
        private Vector3 _patrolPoint;
        private float _nextPatrolTime;
        private float _patrolDeadline;
        private bool _hasPatrolPoint;

        internal bool IsAlive => _monster.IsAlive;

        /// <summary>
        /// 같은 오브젝트에서 이동과 전투를 담당할 몬스터를 찾는다.
        /// </summary>
        private void Awake()
        {
            _monster = GetComponent<Monster>();
        }

        /// <summary>
        /// 생성 또는 재활성화된 위치를 순찰과 추격의 기준점으로 기록한다.
        /// </summary>
        private void OnEnable()
        {
            _spawnPosition = transform.position;
            _hasPatrolPoint = false;
            _nextPatrolTime = Time.time;
        }

        /// <summary>
        /// 현재 위치와 생성 지점에서 감지 범위에 들어오는 상대를 찾는다.
        /// </summary>
        internal Monster FindTarget()
        {
            Monster nearest = null;
            float nearestDistance = _noticeDistance * _noticeDistance;
            foreach (Monster candidate in Monster.ActiveMonsters)
            {
                if (!CanPursue(candidate))
                {
                    continue;
                }

                float distance = PlanarOffset(transform.position, candidate.transform.position).sqrMagnitude;
                if (distance <= nearestDistance)
                {
                    nearest = candidate;
                    nearestDistance = distance;
                }
            }

            return nearest;
        }

        /// <summary>
        /// 상대가 살아 있고 생성 지점 기준 추격 범위 안에 있는지 판단한다.
        /// </summary>
        internal bool CanPursue(Monster target)
        {
            return _monster.CanDamage(target)
                && PlanarOffset(_spawnPosition, target.transform.position).sqrMagnitude <= _leashRadius * _leashRadius;
        }

        /// <summary>
        /// 생성 지점 주변의 지면을 골라 이동한다. 이동이 막히면 다른 지점을 고른다.
        /// </summary>
        internal void Patrol()
        {
            if (_hasPatrolPoint)
            {
                Vector3 direction = PlanarOffset(transform.position, _patrolPoint);
                if (direction.sqrMagnitude > 0.16f && Time.time < _patrolDeadline &&
                    HasGround(transform.position + direction.normalized * 0.5f))
                {
                    _monster.Drive(direction.normalized);
                    return;
                }

                _hasPatrolPoint = false;
                _nextPatrolTime = Time.time + Random.Range(2f, 5f);
            }

            _monster.Drive(Vector3.zero);
            if (Time.time < _nextPatrolTime)
            {
                return;
            }

            Vector2 offset = Random.insideUnitCircle * _patrolRadius;
            _patrolPoint = PlanarOffset(_spawnPosition, transform.position).sqrMagnitude > _patrolRadius * _patrolRadius
                ? _spawnPosition
                : _spawnPosition + new Vector3(offset.x, 0f, offset.y);
            _hasPatrolPoint = HasGround(_patrolPoint);
            _patrolDeadline = Time.time + 6f;
            if (!_hasPatrolPoint)
            {
                _nextPatrolTime = Time.time + Random.Range(2f, 5f);
            }
        }

        /// <summary>
        /// 추격을 시작할 때 이전 순찰 목적지를 비운다.
        /// </summary>
        internal void InterruptPatrol()
        {
            _hasPatrolPoint = false;
        }

        /// <summary>
        /// 상대가 근접 공격 거리에 들어올 때까지 이동한다.
        /// </summary>
        internal bool Approach(Monster target)
        {
            Vector3 offset = PlanarOffset(transform.position, target.transform.position);
            float stopDistance = _monster.Reach * 0.9f;
            if (offset.sqrMagnitude <= stopDistance * stopDistance)
            {
                return true;
            }

            _monster.Drive(offset.normalized);
            return false;
        }

        /// <summary>
        /// 사거리에 있는 상대를 바라보고 공격한다. 벗어나면 그래프가 다시 접근한다.
        /// </summary>
        internal bool Attack(Monster target)
        {
            Vector3 offset = PlanarOffset(transform.position, target.transform.position);
            if (offset.sqrMagnitude > _monster.Reach * _monster.Reach)
            {
                return false;
            }

            _monster.Drive(Vector3.zero);
            if (offset.sqrMagnitude > 0.000001f)
            {
                _monster.TurnTowards(offset);
            }

            if (_monster.CanHit(target))
            {
                _monster.Attack(target);
            }
            return true;
        }

        /// <summary>
        /// 순찰 후보 아래에 걸을 수 있는 지면이 있는지 확인한다.
        /// </summary>
        private static bool HasGround(Vector3 position)
        {
            return Physics.Raycast(position + Vector3.up, Vector3.down, out RaycastHit hit, 2.5f,
                Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)
                && hit.normal.y >= 0.707f && hit.collider.GetComponentInParent<Monster>() == null;
        }

        /// <summary>
        /// 높이 차를 제외한 이동 방향과 거리를 구한다.
        /// </summary>
        private static Vector3 PlanarOffset(Vector3 from, Vector3 to)
        {
            Vector3 offset = to - from;
            offset.y = 0f;
            return offset;
        }
    }
}
