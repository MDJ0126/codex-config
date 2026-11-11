using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Fantasya
{
    /// <summary>
    /// 그래프의 순찰, 추격, 공격 단계와 공유 Target을 몬스터 동작에 연결한다.
    /// </summary>
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Monster Step", story: "[Step] with [Target]", category: "Action/Combat", id: "49c2315a78d2481684185dcc26e9b976")]
    public partial class MonsterBehaviorAction : Action
    {
        [BlackboardEnum]
        public enum eStep { Patrol, Approach, Attack }

        [SerializeReference] public BlackboardVariable<eStep> Step = new BlackboardVariable<eStep>();
        [SerializeReference] public BlackboardVariable<GameObject> Target = new BlackboardVariable<GameObject>();

        private MonsterAI _controller;

        /// <summary>
        /// 이 그래프를 실행하는 몬스터의 동작 컴포넌트를 찾는다.
        /// </summary>
        protected override void OnSetup()
        {
            _controller = GameObject.GetComponent<MonsterAI>();
        }

        protected override Status OnStart() => Execute();

        protected override Status OnUpdate() => Execute();

        /// <summary>
        /// 단계의 결과를 그래프에 반환해 다음 행동을 선택하게 한다.
        /// </summary>
        private Status Execute()
        {
            if (_controller == null || !_controller.isActiveAndEnabled || !_controller.IsAlive)
            {
                return Status.Failure;
            }

            if (Step.Value == eStep.Patrol)
            {
                Monster found = _controller.FindTarget();
                Target.Value = found != null ? found.gameObject : null;
                if (found == null)
                {
                    _controller.Patrol();
                    return Status.Running;
                }

                _controller.InterruptPatrol();
                return Status.Success;
            }

            Monster target = Target.Value != null ? Target.Value.GetComponent<Monster>() : null;
            if (!_controller.CanPursue(target))
            {
                return Status.Failure;
            }

            if (Step.Value == eStep.Approach)
            {
                return _controller.Approach(target) ? Status.Success : Status.Running;
            }

            return _controller.Attack(target) ? Status.Running : Status.Failure;
        }
    }
}
