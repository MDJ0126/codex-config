using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overdox.OriginalData;

namespace Overdox
{
    /// <summary>원작 Skill/ConditionEffect 및 한국어 설명에 근거한 로컬 고유 스킬 실행.</summary>
    public sealed partial class Fighter
    {
        private sealed class Buff { public ConditionEffect row; public float until; }
        private readonly Dictionary<int, Buff> _buffs = new();
        private float _basicIgnoreUntil, _shadowUntil;
        public bool IsHidden => Time.time < _shadowUntil || HasBuffType(12) || HasBuffType(62);
        private float SpecialDamageMultiplier => (HasBuffType(66) ? 1 + BuffValue(66) : 1) * (HasBuffType(56) ? BuffValue(56) : 1);
        private float SpecialMovementMultiplier => (HasBuffType(50) ? BuffValue(50) : 1) * (HasBuffType(12) ? BuffValue(12) : 1) * (HasBuffType(56) ? BuffValue(56,true) : 1) * (HasBuffType(7) ? BuffValue(7) : 1);
        private bool HasBuffType(int type)
        {
            foreach (var buff in _buffs.Values) { if (buff.until > Time.time && buff.row.ConditionType == type) { return true; } }
            return false;
        }
        private float BuffValue(int type, bool second = false)
        {
            foreach (var buff in _buffs.Values)
            {
                if (buff.until > Time.time && buff.row.ConditionType == type) { return second ? buff.row.Variable2 : buff.row.Variable1; }
            }
            return 0;
        }
        private void AddBuff(int id)
        {
            var database = ArenaSession.Instance.originalData;
            if (database == null || !database.Conditions.TryGetValue(id, out var row)) { return; }
            _buffs[id] = new Buff { row = row, until = Time.time + row.DurationTime };
            ArenaSession.Instance.Record("buff");
        }
        private bool IsInvulnerable(Fighter attacker, bool special)
        {
            if (HasBuffType(55)) { return true; }
            return attacker != null && !special && (Time.time < _basicIgnoreUntil || HasBuffType(28));
        }
        private void ResetSpecialEffects()
        {
            StopAllCoroutines();_buffs.Clear();_conditions.Clear();_basicIgnoreUntil = _shadowUntil = 0;
        }
        private void TickSpecialEffects() { }

        private static void ApplyReferencedConditions(Skill skill, Fighter target, bool self)
        {
            if (skill == null) { return; }
            var database = ArenaSession.Instance.originalData;
            foreach (float value in new[] { skill.VariableValue1, skill.VariableValue2, skill.VariableValue3, skill.VariableValue4, skill.VariableValue5 })
            {
                int id = Mathf.RoundToInt(value);
                if (Mathf.Abs(value-id) > .001f || !database.Conditions.TryGetValue(id, out var row)) { continue; }
                if (self && row.Side == 1) { target.AddBuff(id); }
                else if (!self && row.Side == 2) { target.ApplyCondition(row); target.AddBuff(id); }
            }
        }

        private bool ExecuteUtilitySpecial()
        {
            bool handled=ExecuteUtilitySpecialCore();
            if(handled) { _lastAttack=Time.time;_combo=0; }
            return handled;
        }
        private bool ExecuteUtilitySpecialCore()
        {
            var database = ArenaSession.Instance.originalData;
            if (database == null) { return false; }
            var skill = database.GetSkill(weapon, _combo, true);
            _skillAttack = true;
            ApplyReferencedConditions(skill, this, true);
            switch (skill.Id)
            {
                case 10010:
                    _basicIgnoreUntil = Time.time + .8f;
                    StartCoroutine(Regenerate(skill.VariableValue3, skill.VariableValue2, skill.VariableValue1));
                    SetState(CombatState.Recovery,.8f);return true;
                case 10011:
                    StartCoroutine(EnergyWave(1.25f,12,9));SetState(CombatState.Recovery,.5f);return true;
                case 10012:
                    StartCoroutine(ShadowDimension(skill));SetState(CombatState.Recovery,.25f);return true;
                case 10013: AddBuff(20201);SetState(CombatState.Recovery,.4f);return true;
                case 10014:
                    StartCoroutine(ThrowAndTeleport(skill));return true;
                case 10021: AddBuff(20701);SetState(CombatState.Recovery,.5f);return true;
                case 10031: AddBuff(20001);SetState(CombatState.Recovery,.35f);return true;
                case 10032:
                    StartCoroutine(DamageAura(3.6f,5,.5f,20,0));SetState(CombatState.Recovery,.4f);return true;
                case 10033:
                    foreach(var target in TargetsInRange(8.5f)) { target.AddBuff(20301); }
                    SetState(CombatState.Recovery,.5f);return true;
                case 10041:
                    StartCoroutine(RandomConditionAura(4.2f,8,.2f));SetState(CombatState.Recovery,.4f);return true;
                case 10052: AddBuff(20101);AddBuff(20102);SetState(CombatState.Recovery,.35f);return true;
                case 10054:
                    StartCoroutine(ChargeExplosion(3,1.6f));return true;
                case 10064: AddBuff(21701);AddBuff(21601);SetState(CombatState.Recovery,.45f);return true;
                case 10073: AddBuff(20801);AddBuff(20802);SetState(CombatState.Recovery,.35f);return true;
                case 10082:
                case 10087:
                    StartCoroutine(DashSlash(skill.Id==10087));return true;
                default: return false;
            }
        }

        private IEnumerable<Fighter> TargetsInRange(float radius)
        {
            foreach(var other in ArenaSession.Instance.Fighters)
            {
                if(other!=this&&other.IsAlive&&(other.transform.position-transform.position).sqrMagnitude<=radius*radius) { yield return other; }
            }
        }
        private IEnumerator Regenerate(float total,float amount,float interval)
        {
            for(float healed=0;healed<total&&IsAlive;healed+=amount)
            {
                yield return new WaitForSeconds(interval);
                if(IsAlive) { Heal(Mathf.Min(amount,total-healed)); }
            }
        }
        private IEnumerator DamageAura(float radius,float duration,float interval,float flat,float rate)
        {
            for(float elapsed=0;elapsed<duration&&IsAlive;elapsed+=interval)
            {
                foreach(var target in TargetsInRange(radius)) { target.Receive(this,flat+AttackDamage*rate,0,null,false,false,0,true); }
                ArenaSession.Instance.SpawnSlash(transform.position,transform.rotation,true);
                yield return new WaitForSeconds(interval);
            }
        }
        private IEnumerator RandomConditionAura(float radius,float duration,float interval)
        {
            for(float elapsed=0;elapsed<duration&&IsAlive;elapsed+=interval)
            {
                int id = new[] {101,201,301,401}[Random.Range(0,4)];
                if(ArenaSession.Instance.originalData.Conditions.TryGetValue(id,out var condition))
                {
                    foreach(var target in TargetsInRange(radius)) { target.ApplyCondition(condition); }
                }
                yield return new WaitForSeconds(interval);
            }
        }
        private IEnumerator ShadowDimension(Skill skill)
        {
            float duration=3;_shadowUntil=Time.time+duration;AddBuff(21101);
            for(float t=0;t<duration&&IsAlive;t+=.3f)
            {
                foreach(var target in TargetsInRange(3.2f)) { target.Receive(this,10,0,null,false,false,0,true); }
                yield return new WaitForSeconds(.3f);
            }
            if(IsAlive) { AreaStrike(3.2f,.75f,3); }
        }
        private IEnumerator ChargeExplosion(float radius,float rate)
        {
            _dashDirection=AimIntent.sqrMagnitude>.01f?AimIntent.normalized:transform.forward;
            SetState(CombatState.Dash,.4f);
            float finish=Time.time+.4f;
            while(Time.time<finish&&IsAlive)
            {
                bool contact=false;
                foreach(var target in TargetsInRange(.85f)) { contact=true;break; }
                if(contact) { break; }
                yield return null;
            }
            if(IsAlive) { AreaStrike(radius,rate,2);SetState(CombatState.Recovery,.3f); }
        }
        private IEnumerator DashSlash(bool legendary)
        {
            _dashDirection=AimIntent.sqrMagnitude>.01f?AimIntent.normalized:transform.forward;
            SetState(CombatState.Dash,.24f);
            yield return new WaitForSeconds(.24f);
            if(IsAlive)
            {
                AreaStrike(2.7f,1,2);
                if(legendary) { AddBuff(21801); }
                SetState(CombatState.Recovery,.3f);
            }
        }
        private void AreaStrike(float radius,float rate,int effect)
        {
            foreach(var target in TargetsInRange(radius)) { target.Receive(this,AttackDamage*SpecialDamageMultiplier*rate,60,null,true,false,effect,true); }
            ArenaSession.Instance.SpawnSlash(transform.position,transform.rotation,true);
        }
        private IEnumerator EnergyWave(float rate,float speed,float distance)
        {
            yield return new WaitForSeconds(.2f);
            if(!IsAlive) { yield break; }
            Vector3 direction=AimIntent.sqrMagnitude>.01f?AimIntent.normalized:transform.forward;
            var hit=new HashSet<Fighter>();Vector3 origin=transform.position;float travelled=0;
            var visual=new GameObject("Sword energy · original skill 10011");
            Destroy(visual,distance/speed+1);
            var line=visual.AddComponent<LineRenderer>();line.sharedMaterial=ArenaSession.Instance.slashMaterial;line.positionCount=2;line.widthMultiplier=.25f;
            while(travelled<distance&&IsAlive)
            {
                travelled+=speed*Time.deltaTime;Vector3 point=origin+direction*travelled;
                line.SetPosition(0,point+Vector3.up*.6f-direction*.45f);line.SetPosition(1,point+Vector3.up*.6f+direction*.45f);
                foreach(var target in ArenaSession.Instance.Fighters)
                {
                    Vector3 delta=target.transform.position-point;delta.y=0;
                    if(target!=this&&target.IsAlive&&!hit.Contains(target)&&delta.sqrMagnitude<.8f*.8f)
                    {
                        hit.Add(target);target.Receive(this,AttackDamage*rate,60,null,true,false,0,true);
                    }
                }
                yield return null;
            }
            Destroy(visual);
        }

        private IEnumerator ThrowAndTeleport(Skill skill)
        {
            // 투척80%·순간이동 후70%와 탈진은 원문 설명/HitReference1024 근거입니다.
            // 비행 속도·선 표현은 로컬 프로토타입 표현이며 원작 서버 값은 미확인입니다.
            Vector3 origin=transform.position;
            Vector3 direction=AimIntent.sqrMagnitude>.01f?AimIntent.normalized:transform.forward;
            float distance=skill.GuideUiPointerLength;
            if(Physics.SphereCast(origin+Vector3.up*.6f,.25f,direction,out var wall,distance,1,QueryTriggerInteraction.Ignore))
            {
                distance=Mathf.Max(0,wall.distance-.3f);
            }
            Vector3 destination=origin+direction*distance;
            SetState(CombatState.Recovery,.55f);
            var hit=new HashSet<Fighter>();
            float travelled=0;
            var visual=new GameObject("Thrown sword · original skill 10014");
            Destroy(visual,1);
            var line=visual.AddComponent<LineRenderer>();line.sharedMaterial=ArenaSession.Instance.slashMaterial;line.positionCount=2;line.widthMultiplier=.3f;
            while(travelled<distance&&IsAlive)
            {
                travelled=Mathf.Min(distance,travelled+15*Time.deltaTime);
                Vector3 point=origin+direction*travelled;
                line.SetPosition(0,point+Vector3.up*.6f-direction*.4f);line.SetPosition(1,point+Vector3.up*.6f+direction*.4f);
                foreach(var target in ArenaSession.Instance.Fighters)
                {
                    Vector3 delta=target.transform.position-point;delta.y=0;
                    if(target!=this&&target.IsAlive&&!hit.Contains(target)&&delta.sqrMagnitude<.8f*.8f)
                    {
                        hit.Add(target);target.Receive(this,AttackDamage*SpecialDamageMultiplier*.8f,0,null,true,false,0,true);
                    }
                }
                yield return null;
            }
            Destroy(visual);
            if(!IsAlive) { yield break; }
            _controller.enabled=false;transform.position=destination;_controller.enabled=true;
            var database=ArenaSession.Instance.originalData;
            if(database.Hits.TryGetValue(skill.Hit1,out var impact))
            {
                foreach(var target in TargetsInRange(skill.GuideUiRangeRadius))
                {
                    float before=target.Health;
                    target.Receive(this,AttackDamage*SpecialDamageMultiplier*impact.DamageVariable,impact.StaminaDamage,null,impact.CanGuard!=0,impact.CanParrying!=0,impact.DamageEffectIndex,true);
                    if(target.Health<before&&database.Conditions.TryGetValue(impact.ConditionEffectIndex,out var condition))
                    {
                        target.ApplyCondition(condition,Mathf.Max(1,impact.ConditionEffectParameter1));
                    }
                }
            }
            ArenaSession.Instance.SpawnSlash(destination,transform.rotation,true);
            SetState(CombatState.Recovery,.3f);
        }
    }
}
