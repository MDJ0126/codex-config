using System.Collections.Generic;
using Overdox.OriginalData;
using UnityEngine;

namespace Overdox
{
    /// <summary>원작의 상태별 스택 축적·발동·개별 만료를 관리한다.</summary>
    public sealed class StatusCollection
    {
        private sealed class Entry
        {
            public ConditionEffect active;
            public int stacks;
            public float lastAdded, until, nextTick;
        }
        private readonly Dictionary<int,Entry> _entries=new();
        public float stackDecaySeconds=10;
        public void Clear() { _entries.Clear(); }
        public int PendingStacks(int type,float now)
        {
            return _entries.TryGetValue(type,out var e)&&now-e.lastAdded<=stackDecaySeconds?e.stacks:0;
        }
        public bool Add(ConditionEffect row,int count,float now,float recovery=0)
        {
            if(!_entries.TryGetValue(row.ConditionType,out var entry)) { entry=new Entry();_entries[row.ConditionType]=entry; }
            if(now-entry.lastAdded>stackDecaySeconds) { entry.stacks=0; }
            entry.stacks+=Mathf.Max(1,count);entry.lastAdded=now;
            if(entry.stacks<Mathf.Max(1,row.StackCount)) { return false; }
            entry.stacks=0;entry.active=row;
            entry.until=now+row.DurationTime*Mathf.Clamp01(1-recovery);entry.nextTick=now+.5f;
            return true;
        }
        public bool TryActive(int type,float now,out ConditionEffect row)
        {
            if(_entries.TryGetValue(type,out var entry)&&entry.active!=null&&entry.until>now) { row=entry.active;return true; }
            row=null;return false;
        }
        public float BurnTick(float now)
        {
            if(!_entries.TryGetValue(1,out var entry)||entry.active==null) { return 0; }
            float damage=0;
            while(entry.nextTick<=now&&entry.nextTick<=entry.until)
            {
                damage+=entry.active.Variable2*.5f;entry.nextTick+=.5f;
            }
            return damage;
        }
        /// <summary>
        /// 활성 탈진의 스태미나 회복 배율을 반환한다.
        /// APK87 BuffStat의 초기값1에 ConditionEffect.Variable1을 곱하는 계산을 따른다.
        /// 501행의0.2는 남는 회복량이며, 감소량으로 다시 빼지 않는다.
        /// </summary>
        public float StaminaRecoveryMultiplier(float now)
        {
            return TryActive(60, now, out var exhaustion) ? exhaustion.Variable1 : 1;
        }

        public float MovementMultiplier(float now)
        {
            float multiplier=1;
            if(TryActive(1,now,out var burn)) { multiplier*=burn.Variable1; }
            if(TryActive(2,now,out var frost)) { multiplier*=frost.Variable1; }
            if(TryActive(25,now,out _)) { return 0; }
            return multiplier;
        }
    }
}
