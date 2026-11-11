using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Fantasya
{
    /// <summary>
    /// 선택한 동료의 실제 행동·성격·기억·대화를 보여주고 플레이어 발언을 공유 서비스로 전달한다.
    /// </summary>
    public sealed class AgentConversationPanel : MonoBehaviour
    {
        [SerializeField] private AgentParty _party;
        [SerializeField] private AgentCompanionLink _link;
        [SerializeField] private Text _title;
        [SerializeField] private Text _details;
        [SerializeField] private Text _transcript;
        [SerializeField] private Text _status;
        [SerializeField] private InputField _input;
        [SerializeField] private ScrollRect _scroll;
        [SerializeField] private Button _send;
        [SerializeField] private AgentVoiceInput _voice;
        private int _selected;
        private float _nextRefresh;
        private Coroutine _talk;
        private string _lastTranscript;
        private string _requestStatus;
        public AIAgentController Selected => _party != null && _party.Members.Count > 0 ? _party.Members[_selected % _party.Members.Count] : null;
        public InputField ActiveInput => _voice != null && _voice.IsVoiceMode ? _voice.ReviewInput : _input;
        public bool IsSending => _talk != null;

        /// <summary>
        /// 대화 입력에 포커스가 있으면 플레이어 이동·공격·씬 재시작 입력을 차단한다.
        /// </summary>
        public static bool IsTyping => EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null &&
            EventSystem.current.currentSelectedGameObject.GetComponentInParent<InputField>() != null;

        private void Update()
        {
            if (Time.unscaledTime < _nextRefresh || _party == null)
            {
                return;
            }
            _nextRefresh = Time.unscaledTime + 0.2f;
            AIAgentController agent = Selected;
            if (agent == null)
            {
                _title.text = "생성된 동료가 없습니다";
                _send.interactable = false;
                return;
            }
            _title.text = agent.AgentName;
            _details.text = agent.ControllerName + " · " + agent.ActionReason;
            _send.interactable = _talk == null;
            _status.text = string.IsNullOrEmpty(_requestStatus) ? agent.Conversation?.awaitingExternalReply == true ? agent.ControllerName + "의 답변을 기다리고 있어요…" : _link.ConnectionStatus : _requestStatus;
            var text = new StringBuilder();
            CompanionState state = agent.Conversation;
            if (state?.memories != null && state.memories.Length > 0)
            {
                text.AppendLine("기억 · " + state.memories.Last().text);
            }
            if (state?.messages != null)
            {
                foreach (CompanionMessage message in state.messages.Where(item => item.role == "player" || item.role == "companion").TakeLast(12))
                {
                    string speaker = message.role == "player" ? message.source == "game-player" ? "나" : "나 · 게임 밖" : message.role == "companion" ? agent.AgentName : "기록";
                    if (message.role == "companion" && message.source != "promise" && message.source != "memory" && !message.source.StartsWith("model:"))
                    {
                        speaker += " · " + message.source;
                    }
                    text.AppendLine(speaker + ": " + VisibleMessage(message.text));
                }
            }
            string current = text.ToString();
            if (current != _lastTranscript)
            {
                _lastTranscript = current;
                _transcript.text = current;
                Canvas.ForceUpdateCanvases();
                _scroll.verticalNormalizedPosition = 0f;
            }
        }

        private void OnDisable()
        {
            if (_talk != null)
            {
                StopCoroutine(_talk);
                _talk = null;
            }
            if (_link != null)
            {
                _link.CancelTalk();
            }
        }

        /// <summary>
        /// 에디터 구성 시 저장할 컴포넌트 참조만 연결하고 레이아웃과 글꼴은 변경하지 않는다.
        /// </summary>
        public void Configure(AgentParty party, AgentCompanionLink link, Text title, Text details, Text transcript, Text status,
            InputField input, ScrollRect scroll, Button send)
        {
            _party = party;
            _link = link;
            _title = title;
            _details = details;
            _transcript = transcript;
            _status = status;
            _input = input;
            _scroll = scroll;
            _send = send;
        }

        /// <summary>
        /// 현재 입력을 선택한 동료에게 한 번 전송하고 모델 응답 대기 상태를 표시한다.
        /// </summary>
        public void Send()
        {
            if (_talk != null || Selected == null || string.IsNullOrWhiteSpace(ActiveInput.text) || (_voice != null && _voice.IsRecording))
            {
                return;
            }
            string text = ActiveInput.text.Trim();
            ActiveInput.text = string.Empty;
            _requestStatus = "동료가 답하는 중이에요…";
            _talk = StartCoroutine(SendMessage(Selected.CompanionId, text));
        }

        private IEnumerator SendMessage(string id, string text)
        {
            yield return null;
            yield return _link.Talk(id, text, (success, message) => _requestStatus = success ? string.Empty : message);
            _talk = null;
        }

        public void NextCompanion() => Select(1);
        public void PreviousCompanion() => Select(-1);

        private void Select(int direction)
        {
            if (_party.Members.Count == 0)
            {
                return;
            }
            _selected = (_selected + direction + _party.Members.Count) % _party.Members.Count;
            if (_voice != null)
            {
                _voice.Cancel();
            }
            _lastTranscript = null;
            _requestStatus = string.Empty;
            _nextRefresh = 0f;
        }

        /// <summary>
        /// 지원하는 명시적인 대기 약속을 선택한 동료에게 전달한다.
        /// </summary>
        public void AskWait() => SendQuick("기다려");
        public void AskFollow() => SendQuick("따라와");
        public void ClearPromise() => SendQuick("약속 해제");

        private void SendQuick(string text)
        {
            if (_talk == null)
            {
                ActiveInput.text = text;
                Send();
            }
        }

        /// <summary>
        /// 게임 안에서는 제어 식별자와 시각 대신 플레이어가 이해할 수 있는 사건만 표시한다.
        /// </summary>
        private static string VisibleMessage(string text)
        {
            if (text.StartsWith("명령 적용:") || text.StartsWith("명령 거부:"))
            {
                string[] parts = text.Split('·');
                return (text.StartsWith("명령 적용:") ? "행동에 반영: " : "행동을 실행할 수 없어요: ") + string.Join("·", parts.Skip(2)).Trim();
            }
            int details = text.LastIndexOf(" [", System.StringComparison.Ordinal);
            return text.StartsWith("약속 적용:") && details >= 0 ? text.Substring(0, details) : text;
        }
    }
}
