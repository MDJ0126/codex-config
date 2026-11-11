using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Fantasya
{
    /// <summary>
    /// 씬에 저장된 텍스트에 동료별 모델·대기·적용·긴급 명령 시간을 구분해 표시한다.
    /// </summary>
    public sealed class AgentLatencyHUD : MonoBehaviour
    {
        [SerializeField] private AgentParty _party;
        [SerializeField] private Text _output;
        private float _nextRefresh;
        private int _page;
        private const int ROWS_PER_PAGE = 3;

        /// <summary>
        /// 씬 구성 시 동적 표시 대상만 연결한다.
        /// </summary>
        public void Configure(AgentParty party, Text output)
        {
            _party = party;
            _output = output;
        }

        /// <summary>
        /// 저장된 버튼 이벤트에서 다음 동료 묶음으로 전환한다.
        /// </summary>
        public void NextPage()
        {
            _page++;
            _nextRefresh = 0f;
        }

        /// <summary>
        /// 표시를 초당 네 번 갱신하며 미측정 값은 숫자 0과 구별한다.
        /// </summary>
        private void Update()
        {
            if (_party == null || _output == null || Time.unscaledTime < _nextRefresh)
            {
                return;
            }
            _nextRefresh = Time.unscaledTime + 0.25f;
            int pages = Mathf.Max(1, Mathf.CeilToInt(_party.Members.Count / (float)ROWS_PER_PAGE));
            _page %= pages;
            var text = new StringBuilder($"Companions {_party.Members.Count} | page {_page + 1}/{pages}\n");
            int end = Mathf.Min((_page + 1) * ROWS_PER_PAGE, _party.Members.Count);
            for (int index = _page * ROWS_PER_PAGE; index < end; index++)
            {
                AIAgentController agent = _party.Members[index];
                if (agent == null)
                {
                    continue;
                }
                text.Append($"\n{agent.AgentName} | {agent.Action}\n{agent.ModelName} | {agent.Status}\n");
                string reason = agent.ActionReason;
                text.Append((reason.Length > 42 ? reason.Substring(0, 42) + "…" : reason) + "\n");
                text.Append($"Request {Ms(agent.LastModelMilliseconds)} | OK avg {Ms(agent.SuccessfulRequests > 0 ? agent.MeanModelMilliseconds : -1)}\n");
                text.Append($"Queue {Ms(agent.LastQueueMilliseconds)} | applied {Ms(agent.LastApplyMilliseconds)}\n");
                text.Append($"Reflex command {Ms(agent.LastReflexMilliseconds)} | wait {agent.WaitingMilliseconds:F0} ms\n");
                text.Append($"OK {agent.SuccessfulRequests} / errors {agent.FailedRequests} / rejected {agent.RejectedDecisions}\n");
            }
            _output.text = text.ToString();
        }

        private static string Ms(double value) => value < 0d ? "--" : value.ToString("F1") + " ms";
    }
}
