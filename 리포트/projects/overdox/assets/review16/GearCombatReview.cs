using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Overdox.Verification
{
    /// <summary>명시적 실행 인자에서만 작동하는 격리된 전체 무기 실전 명중 검증.</summary>
    public sealed class GearCombatReview : MonoBehaviour
    {
        [Serializable] private sealed class Result
        {
            public int id, family, normalHits, specialHits, chargesBefore, chargesAfter;
            public string name, specialName, specialObservation;
            public float normalDamage, specialDamage, healthBefore, healthAfter;
            public bool targetCondition, hidden, normalPassed;
        }
        [Serializable] private sealed class Report
        {
            public string startedUtc, finishedUtc, scope;
            public int total, normalPassed;
            public Result[] results;
        }
        private ArenaSession _session;
        private Fighter _actor, _target;
        private Result _current;
        private bool _special;
        private Vector3 _origin=new Vector3(1000, .08f, 1000);
        private float _distance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            if(!Environment.GetCommandLineArgs().Contains("-gear-combat-review")) { return; }
            new GameObject("Verification · all weapon contact").AddComponent<GearCombatReview>();
        }

        private IEnumerator Start()
        {
            yield return null;
            _session=ArenaSession.Instance;
            if(_session==null) { throw new InvalidOperationException("ArenaSession is required"); }
            var report=new Report { startedUtc=DateTime.UtcNow.ToString("O"), scope="Isolated stationary target, four normal-attack distances and one special activation per weapon. Verifies actual hit callbacks, not original server parity." };
            _session.StartMatch();
            _session.enabled=false; // 검증 동안만 구역 피해·승패·자동 재시작을 정지합니다.
            foreach(var fighter in _session.Fighters) { fighter.GetComponent<FighterBrain>().enabled=false; }
            _actor=_session.Player;_target=_session.Fighters[1];
            foreach(var fighter in _session.Fighters.Skip(2)) { fighter.gameObject.SetActive(false); }
            var floor=GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name="Verification fixture floor";floor.transform.position=_origin+Vector3.down*.55f;floor.transform.localScale=new Vector3(60,1,60);
            _target.Impact+=OnImpact;
            var results=new List<Result>();
            for(int i=0;i<_session.catalog.Weapons.Count;i++)
            {
                _session.ChangeWeapon(i-_session.SelectedWeapon);
                var weapon=_actor.weapon;
                _current=new Result { id=weapon.id, family=weapon.family, name=weapon.name, specialName=weapon.skill };
                _special=false;
                foreach(float distance in new[]{.65f,1f,1.4f,1.8f})
                {
                    _distance=distance;ResetActors();
                    _actor.AttackIntent=true;
                    yield return new WaitForSeconds(2.5f);
                    _actor.AttackIntent=false;
                }
                _current.normalPassed=_current.normalHits>0;
                _special=true;_distance=1.2f;ResetActors();
                _actor.Receive(null,_actor.MaxHealth*.4f,0);
                _current.healthBefore=_actor.Health;
                _current.chargesBefore=_actor.SkillCharges;
                _actor.SkillIntent=true;
                yield return new WaitForSeconds(6);
                _current.healthAfter=_actor.Health;_current.chargesAfter=_actor.SkillCharges;
                _current.hidden=_actor.IsHidden;
                _current.targetCondition=Enumerable.Range(1,4).Any(_target.HasCondition);
                _current.specialObservation=_current.specialHits>0?"damage observed":_current.healthAfter>_current.healthBefore?"recovery observed":"activation only; utility behavior requires separate assertion";
                _session.Capture("gear-contact-"+weapon.id);
                results.Add(_current);
                Debug.Log($"GEAR_CONTACT id={weapon.id} normalHits={_current.normalHits} normalDamage={_current.normalDamage} specialHits={_current.specialHits} charges={_current.chargesBefore}->{_current.chargesAfter}");
                Save(report,results,false);
            }
            Save(report,results,true);
            Debug.Log($"GEAR_CONTACT_COMPLETE passed={results.Count(r=>r.normalPassed)}/{results.Count}");
            Application.Quit();
        }

        private void ResetActors()
        {
            _actor.Respawn(_origin);_target.Respawn(_origin+Vector3.forward*_distance);
            _actor.MoveIntent=Vector3.zero;_actor.AimIntent=Vector3.forward;
            _target.MoveIntent=Vector3.zero;_target.AimIntent=Vector3.back;
        }

        private void LateUpdate()
        {
            if(_target==null||_actor==null||_current==null) { return; }
            if(!_target.IsAlive) { _target.Respawn(_origin+Vector3.forward*_distance); }
            // 피격 밀림으로 다음 콤보의 거리가 달라지지 않게 하는 QA 전용 고정 표적입니다.
            var controller=_target.GetComponent<CharacterController>();
            controller.enabled=false;
            _target.transform.position=(_special?_actor.transform.position:_origin)+Vector3.forward*_distance;
            controller.enabled=true;
        }

        private void OnImpact(Fighter fighter,string kind,float amount)
        {
            if(_current==null||kind!="HIT") { return; }
            if(_special) { _current.specialHits++;_current.specialDamage+=amount; }
            else { _current.normalHits++;_current.normalDamage+=amount; }
        }

        private static void Save(Report report,List<Result> results,bool finished)
        {
            report.results=results.ToArray();report.total=results.Count;report.normalPassed=results.Count(r=>r.normalPassed);
            if(finished) { report.finishedUtc=DateTime.UtcNow.ToString("O"); }
            var args=Environment.GetCommandLineArgs();int output=Array.IndexOf(args,"-evidence");
            string directory=output>=0&&output+1<args.Length?args[output+1]:Path.Combine(Application.dataPath,"../Evidence");
            Directory.CreateDirectory(directory);
            File.WriteAllText(Path.Combine(directory,"gear-contact-results.json"),JsonUtility.ToJson(report,true));
        }
    }
}
