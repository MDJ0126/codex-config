using UnityEngine;
using UnityEngine.UI;

namespace Overdox
{
    /// <summary>저장된 원본 전투 HUD의 수치와 체력 구간을 갱신한다.</summary>
    public sealed class RestoredCombatHud : MonoBehaviour
    {
        public Text health, stamina, timer, alive, total, kills;
        public Image[] healthSegments;
        public Text skillTime, dashTime;
        public Image skillCharge, dashCooldown;
        public GameObject[] skillCountRings;
        private void LateUpdate()
        {
            var session = ArenaSession.Instance;
            if (session == null || session.Player == null) { return; }
            var player = session.Player;
            health.text = $"{player.Health:0}/{player.MaxHealth:0}";
            stamina.text = $"{player.Stamina:0}/{player.MaxStamina:0}";
            timer.text = $"{Mathf.FloorToInt(session.Remaining / 60):00}:{Mathf.FloorToInt(session.Remaining % 60):00}";
            alive.text = session.AliveCount.ToString();
            total.text = session.Fighters.Count.ToString();
            kills.text = player.Kills.ToString();
            // HeroInfoUI native constructor: _hpUnitValue = 0x1e (30 HP).
            int count = Mathf.Min(healthSegments.Length, Mathf.CeilToInt(player.MaxHealth / 30));
            float segments = player.Health / 30;
            for (int index = 0; index < healthSegments.Length; index++)
            {
                healthSegments[index].transform.parent.gameObject.SetActive(index < count);
                healthSegments[index].fillAmount = Mathf.Clamp01(segments - index);
            }
            if (count > 0)
            {
                var unit = (RectTransform)healthSegments[count - 1].transform.parent;
                var position = health.rectTransform.anchoredPosition;
                position.x = unit.anchoredPosition.x + healthSegments[count - 1].rectTransform.rect.width + 8;
                health.rectTransform.anchoredPosition = position;
            }
            float skillRemaining = Mathf.Max(0, player.SkillReadyAt - Time.time);
            skillTime.text = skillRemaining > 0 ? Mathf.CeilToInt(skillRemaining).ToString() : "";
            dashTime.text = player.DashRemaining > 0 ? Mathf.CeilToInt(player.DashRemaining).ToString() : "";
            skillCharge.fillAmount = player.SkillChargeFraction;
            dashCooldown.fillAmount = player.DashCooldownFraction;
            for (int index = 0; index < skillCountRings.Length; index++)
            {
                skillCountRings[index].SetActive(index == player.SkillCharges - 1);
            }
        }
    }
}
