using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Overdox.Verification
{
    /// <summary>
    /// 실제 Fighter.Update에서 탈진의 자세별 회복량과 만료를 측정한다.
    /// 명시적 실행 인자로만 작동하며 내부 필드 설정은 검증용 초기 조건에 한정한다.
    /// </summary>
    public sealed class ExhaustionReview : MonoBehaviour
    {
        [Serializable] private sealed class Sample
        {
            public string stance;
            public float healthyRate, affectedRate, ratio, expectedRatio;
            public bool passed;
        }
        [Serializable] private sealed class Report
        {
            public string startedUtc, finishedUtc, scope;
            public int conditionId = 501;
            public bool threeStacksInactive, fourStacksActive, expired, respawnCleared, passed;
            public Sample[] samples;
        }
        private readonly List<Sample> _samples = new();
        private Report _report;
        private Fighter _target;
        private ArenaSession _session;
        private float _rate;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            if (!Environment.GetCommandLineArgs().Contains("-exhaustion-review")) { return; }
            var runner = new GameObject("Verification · exhaustion recovery");
            DontDestroyOnLoad(runner);
            runner.AddComponent<ExhaustionReview>();
        }

        private IEnumerator Start()
        {
            _report = new Report
            {
                startedUtc = DateTime.UtcNow.ToString("O"),
                scope = "Isolated actual Fighter.Update measurements with controlled stamina and combat timestamps. Direct DDT condition application; Hydra contact and original server parity are not asserted."
            };
            float deadline = Time.realtimeSinceStartup + 60;
            while (ArenaSession.Instance == null || ArenaSession.Instance.Player == null ||
                   GameFlowController.Instance == null || GameFlowController.Instance.Phase != GameFlowPhase.Lobby)
            {
                if (Time.realtimeSinceStartup > deadline) { Save("Lobby readiness timeout"); yield break; }
                yield return null;
            }
            ArenaSession.Instance.StartMatch();
            while (ArenaSession.Instance == null || !ArenaSession.Instance.IsFighting ||
                   GameFlowController.Instance.Phase != GameFlowPhase.Arena)
            {
                if (Time.realtimeSinceStartup > deadline) { Save("Arena readiness timeout"); yield break; }
                yield return null;
            }
            _session = ArenaSession.Instance;
            _session.enabled = false;
            foreach (var fighter in _session.Fighters)
            {
                var brain = fighter.GetComponent<FighterBrain>();
                if (brain != null) { brain.enabled = false; }
                if (fighter != _session.Player) { fighter.gameObject.SetActive(false); }
            }
            _target = _session.Player;
            var condition = _session.originalData.Conditions[501];
            _target.Respawn(new Vector3(0, .08f, 0));
            _target.ApplyCondition(condition, 3);
            _report.threeStacksInactive = !_target.HasCondition(60) && _target.ConditionStacks(60) == 3;
            _target.ApplyCondition(condition, 1);
            _report.fourStacksActive = _target.HasCondition(60);
            _session.Capture("exhaustion-four-stacks");
            foreach (string stance in new[] { "guard", "combat", "normal" })
            {
                yield return Measure(stance, false);
                var sample = new Sample { stance = stance, healthyRate = _rate, expectedRatio = condition.Variable1 };
                yield return Measure(stance, true);
                sample.affectedRate = _rate;
                sample.ratio = sample.healthyRate > 0 ? sample.affectedRate / sample.healthyRate : -1;
                sample.passed = Mathf.Abs(sample.ratio - sample.expectedRatio) < .025f;
                _samples.Add(sample);
                Debug.Log($"EXHAUSTION_RATE stance={stance} healthy={sample.healthyRate:F4} affected={sample.affectedRate:F4} ratio={sample.ratio:F4} expected={sample.expectedRatio:F4}");
            }
            // 실제 시간의 만료를 기다린다. 검증용 시계나 만료 시각 덮어쓰기는 사용하지 않는다.
            yield return new WaitForSeconds(condition.DurationTime + .15f);
            _report.expired = !_target.HasCondition(60);
            _target.ApplyCondition(condition, 4);
            _target.Respawn(new Vector3(0, .08f, 0));
            _report.respawnCleared = !_target.HasCondition(60) && _target.ConditionStacks(60) == 0;
            _report.passed = _report.threeStacksInactive && _report.fourStacksActive && _report.expired &&
                             _report.respawnCleared && _samples.All(sample => sample.passed);
            _session.Capture("exhaustion-expired");
            Save(null);
        }

        private IEnumerator Measure(string stance, bool affected)
        {
            _target.Respawn(new Vector3(0, .08f, 0));
            _target.MoveIntent = Vector3.zero;
            _target.AttackIntent = false;
            _target.SkillIntent = false;
            _target.DashIntent = false;
            _target.GuardIntent = stance == "guard";
            Set("_lastSpend", Time.time - 2);
            Set("_lastDamage", stance == "combat" ? Time.time : Time.time - 20);
            Set("_lastAttack", Time.time - 20);
            if (affected) { _target.ApplyCondition(_session.originalData.Conditions[501], 4); }
            yield return null;
            typeof(Fighter).GetProperty(nameof(Fighter.Stamina)).SetValue(_target, 20f);
            float before = _target.Stamina, started = Time.time;
            yield return new WaitForSeconds(.6f);
            _rate = (_target.Stamina - before) / (Time.time - started);
        }

        private void Set(string name, float value)
        {
            typeof(Fighter).GetField(name, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_target, value);
        }

        private void Save(string error)
        {
            _report.finishedUtc = DateTime.UtcNow.ToString("O");
            _report.samples = _samples.ToArray();
            string[] args = Environment.GetCommandLineArgs();
            int index = Array.IndexOf(args, "-evidence");
            string directory = index >= 0 && index + 1 < args.Length ? args[index + 1] : Path.Combine(Application.dataPath, "../Evidence");
            Directory.CreateDirectory(directory);
            File.WriteAllText(Path.Combine(directory, "exhaustion-review.json"), JsonUtility.ToJson(_report, true));
            if (error != null) { Debug.LogError("EXHAUSTION_REVIEW " + error); }
            Debug.Log("EXHAUSTION_REVIEW_COMPLETE passed=" + _report.passed);
            Application.Quit();
        }
    }
}
