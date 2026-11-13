using UnityEngine;
using UnityEngine.UI;

namespace Overdox
{
    /// <summary>
    /// 씬에 저장한 원작 전투 HUD에 현재 플레이어와 경기의 표시값을 연결한다.
    /// </summary>
    public sealed class RestoredCombatHud : MonoBehaviour
    {
        public Text health, stamina, timer, alive, total, kills;
        public Image[] healthSegments;
        public Text skillTime, dashTime;
        public Image skillCharge, dashCooldown;
        public GameObject[] skillCountRings;

        // 원작 HeroInfoUI 생성자의 _hpUnitValue = 0x1e를 근거로 한 구간을 30HP로 표시한다.
        private const float HEALTH_PER_SEGMENT = 30;

        /// <summary>
        /// 세션과 플레이어가 준비되면 현재 프레임의 능력치와 경기 상태를 표시한다.
        /// </summary>
        private void LateUpdate()
        {
            var session = ArenaSession.Instance;
            if (session == null || session.Player == null)
            {
                return;
            }
            var player = session.Player;
            health.text = $"{player.Health:0}/{player.MaxHealth:0}";
            stamina.text = $"{player.Stamina:0}/{player.MaxStamina:0}";
            timer.text = $"{Mathf.FloorToInt(session.Remaining / 60):00}:{Mathf.FloorToInt(session.Remaining % 60):00}";
            alive.text = session.AliveCount.ToString();
            total.text = session.Fighters.Count.ToString();
            kills.text = player.Kills.ToString();
            // 최대 체력으로 필요한 구간 수를 정하고 마지막 구간에는 남은 체력 비율을 표시한다.
            int count = Mathf.Min(healthSegments.Length, Mathf.CeilToInt(player.MaxHealth / HEALTH_PER_SEGMENT));
            float segments = player.Health / HEALTH_PER_SEGMENT;
            for (int index = 0; index < healthSegments.Length; index++)
            {
                healthSegments[index].transform.parent.gameObject.SetActive(index < count);
                healthSegments[index].fillAmount = Mathf.Clamp01(segments - index);
            }
            if (count > 0)
            {
                // 원작의 가변 길이 구간 뒤에 수치가 이어지도록 마지막 활성 구간을 기준으로 둔다.
                var unit = (RectTransform)healthSegments[count - 1].transform.parent;
                var position = health.rectTransform.anchoredPosition;
                position.x = unit.anchoredPosition.x + healthSegments[count - 1].rectTransform.rect.width + 8;
                health.rectTransform.anchoredPosition = position;
            }
            float skillRemaining = Mathf.Max(0, player.SkillReadyAt - Time.time);
            // 사용 가능 상태에서는 숫자를 비워 준비된 버튼 그림을 가리지 않는다.
            skillTime.text = skillRemaining > 0 ? Mathf.CeilToInt(skillRemaining).ToString() : "";
            dashTime.text = player.DashRemaining > 0 ? Mathf.CeilToInt(player.DashRemaining).ToString() : "";
            skillCharge.fillAmount = player.SkillChargeFraction;
            dashCooldown.fillAmount = player.DashCooldownFraction;
            for (int index = 0; index < skillCountRings.Length; index++)
            {
                // 링 배열은 충전 횟수별 그림이므로 현재 횟수에 해당하는 하나만 표시한다.
                skillCountRings[index].SetActive(index == player.SkillCharges - 1);
            }
        }
    }
}
