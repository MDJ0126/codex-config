using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Fantasya
{
    /// <summary>
    /// 로컬 동료 서비스와 실제 게임 상태를 동기화하고 명령을 Unity 메인 스레드에서 적용한다.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class AgentCompanionLink : MonoBehaviour
    {
        [SerializeField] private AgentParty _party;
        [SerializeField, Min(0.2f)] private float _syncInterval = 0.5f;
        private string _sessionId;
        private Connection _connection;
        private Coroutine _sync;
        private UnityWebRequest _syncRequest;
        private UnityWebRequest _talkRequest;
        private UnityWebRequest _speechRequest;
        private double _lastSuccess;
        public string ConnectionStatus { get; private set; } = "동료 서비스 연결 대기";
        public string SessionId => _sessionId;

        [Serializable] private sealed class Connection
        {
            public string url;
            public string token;
        }

        [Serializable] private sealed class SyncRequest
        {
            public string sessionId;
            public AgentBridgeModel model;
            public CompanionSnapshot[] companions;
        }

        [Serializable] private sealed class AgentBridgeModel
        {
            public string endpoint;
            public string model;
            public string apiKeyEnvironmentVariable;
        }

        [Serializable] private sealed class SyncResponse
        {
            public CompanionState[] companions;
        }

        [Serializable] private sealed class RpcRequest
        {
            public string tool;
            public RpcArguments arguments;
        }

        [Serializable] private sealed class RpcArguments
        {
            public string companionId;
            public string text;
        }

        [Serializable] private sealed class ErrorResponse
        {
            public string error;
        }
        [Serializable] private sealed class SpeechResponse
        {
            public string text;
            public double requestMs;
        }

        private void Awake()
        {
            _sessionId = Guid.NewGuid().ToString("N");
            if (_party == null)
            {
                _party = GetComponent<AgentParty>();
            }
        }

        private void OnEnable()
        {
            _sync = StartCoroutine(Synchronize());
        }

        private void OnDisable()
        {
            if (_sync != null)
            {
                StopCoroutine(_sync);
                _sync = null;
            }
            _syncRequest?.Abort();
            _syncRequest?.Dispose();
            _syncRequest = null;
            CancelTalk();
            CancelSpeech();
        }

        /// <summary>
        /// 게임 스냅샷 전송 후 받은 약속과 명령을 현재 세션의 동료에게만 적용한다.
        /// </summary>
        private IEnumerator Synchronize()
        {
            yield return null;
            while (enabled)
            {
                bool started = false;
                try
                {
                    LoadConnection();
                    AgentModelSettings settings = _party.ModelSettings;
                    var snapshot = new SyncRequest
                    {
                        sessionId = _sessionId,
                        model = settings == null ? null : new AgentBridgeModel
                        {
                            endpoint = settings.endpoint,
                            model = settings.model,
                            apiKeyEnvironmentVariable = settings.apiKeyEnvironmentVariable
                        },
                        companions = _party.Members.Where(agent => agent != null).Select(agent => agent.GetSnapshot()).ToArray()
                    };
                    _syncRequest = CreateRequest("/sync", JsonUtility.ToJson(snapshot), 3);
                    _syncRequest.SendWebRequest();
                    started = true;
                }
                catch (Exception)
                {
                    ConnectionStatus = "연결 도구를 먼저 실행해 주세요";
                }
                if (started)
                {
                    while (!_syncRequest.isDone)
                    {
                        yield return null;
                    }
                    if (_syncRequest.result == UnityWebRequest.Result.Success)
                    {
                        try
                        {
                            var result = JsonUtility.FromJson<SyncResponse>(_syncRequest.downloadHandler.text);
                            if (result?.companions == null)
                            {
                                throw new FormatException();
                            }
                            foreach (CompanionState state in result.companions)
                            {
                                AIAgentController agent = _party.Members.FirstOrDefault(item => item != null && item.CompanionId == state.id);
                                if (agent != null)
                                {
                                    agent.ApplyCompanionState(state, _sessionId);
                                }
                            }
                            _lastSuccess = Time.realtimeSinceStartupAsDouble;
                            ConnectionStatus = "공유 기억 연결됨 · 게임 안팎의 대화 동기화 중";
                        }
                        catch (Exception)
                        {
                            ConnectionStatus = "동료 상태를 읽지 못했습니다";
                        }
                    }
                    else
                    {
                        ConnectionStatus = "서비스 연결 끊김 · 마지막 약속 유지";
                    }
                    _syncRequest.Dispose();
                    _syncRequest = null;
                }
                if (Time.realtimeSinceStartupAsDouble - _lastSuccess > 3d)
                {
                    foreach (AIAgentController agent in _party.Members)
                    {
                        if (agent != null)
                        {
                            agent.ClearExternalControl();
                        }
                    }
                }
                yield return new WaitForSecondsRealtime(_syncInterval);
            }
        }

        /// <summary>
        /// 현재 PC에서 생성한 연결 파일만 읽고 루프백 주소와 토큰 형식을 확인한다.
        /// </summary>
        private void LoadConnection()
        {
            string directory = Environment.GetEnvironmentVariable("FANTASYA_AGENT_DATA_DIR");
            if (string.IsNullOrEmpty(directory))
            {
                directory = Path.GetFullPath(Path.Combine(Application.dataPath, "..", ".fantasya"));
            }
            var connection = JsonUtility.FromJson<Connection>(File.ReadAllText(Path.Combine(directory, "connection.json")));
            if (!Uri.TryCreate(connection?.url, UriKind.Absolute, out Uri uri) || uri.Scheme != "http" || uri.Host != "127.0.0.1" ||
                uri.AbsolutePath != "/" || connection.token == null || connection.token.Length != 64 || !connection.token.All(Uri.IsHexDigit))
            {
                throw new FormatException("Invalid local connection.");
            }
            _connection = connection;
        }

        /// <summary>
        /// 게임 대화와 외부 대화가 같은 기억 저장소를 사용하도록 플레이어 발언을 전송한다.
        /// </summary>
        public IEnumerator Talk(string companionId, string text, Action<bool, string> completed)
        {
            UnityWebRequest request = null;
            try
            {
                LoadConnection();
                request = CreateRequest("/rpc", JsonUtility.ToJson(new RpcRequest
                {
                    tool = "talk_to_companion",
                    arguments = new RpcArguments { companionId = companionId, text = text }
                }), 70);
                _talkRequest = request;
            }
            catch (Exception)
            {
                completed(false, "연결 도구를 먼저 실행해 주세요.");
            }
            if (request == null)
            {
                yield break;
            }
            using (request)
            {
                yield return request.SendWebRequest();
                if (_talkRequest != request)
                {
                    completed(false, "요청이 취소됐습니다.");
                    yield break;
                }
                _talkRequest = null;
                bool success = request.result == UnityWebRequest.Result.Success;
                string message = success ? "전달했습니다. 동료 기록에 응답이 표시됩니다." : "응답을 받지 못했습니다.";
                if (!success)
                {
                    try
                    {
                        message = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text)?.error ?? message;
                    }
                    catch (Exception)
                    {
                        message = "연결 또는 모델 요청에 실패했습니다.";
                    }
                }
                completed(success, message);
            }
        }

        /// <summary>
        /// 대화창 또는 연결이 비활성화되면 진행 중인 UI 요청을 명시적으로 정리한다.
        /// </summary>
        public void CancelTalk()
        {
            _talkRequest?.Abort();
            _talkRequest?.Dispose();
            _talkRequest = null;
        }

        /// <summary>
        /// 녹음한 음성을 인증된 로컬 음성 인식기로 전송하고 플레이어 확인용 문장만 반환한다.
        /// </summary>
        public IEnumerator Transcribe(byte[] wave, Action<bool, string, double> completed)
        {
            UnityWebRequest request = null;
            try
            {
                LoadConnection();
                request = CreateRequest("/speech", string.Empty, 95);
                request.uploadHandler.Dispose();
                request.uploadHandler = new UploadHandlerRaw(wave);
                request.SetRequestHeader("Content-Type", "audio/wav");
                _speechRequest = request;
            }
            catch (Exception)
            {
                completed(false, "동료 서비스와 로컬 음성 인식을 먼저 준비해 주세요.", 0d);
            }
            if (request == null)
            {
                yield break;
            }
            using (request)
            {
                yield return request.SendWebRequest();
                if (_speechRequest != request)
                {
                    yield break;
                }
                _speechRequest = null;
                try
                {
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        completed(false, JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text)?.error ?? "음성을 인식하지 못했습니다.", 0d);
                    }
                    else
                    {
                        SpeechResponse result = JsonUtility.FromJson<SpeechResponse>(request.downloadHandler.text);
                        completed(true, result.text, result.requestMs);
                    }
                }
                catch (Exception)
                {
                    completed(false, "음성 인식 연결에 실패했습니다. 설치 상태를 확인해 주세요.", 0d);
                }
            }
        }

        /// <summary>
        /// 모드나 동료가 바뀌면 음성 결과를 다른 대화로 전달하지 않도록 요청을 취소한다.
        /// </summary>
        public void CancelSpeech()
        {
            _speechRequest?.Abort();
            _speechRequest?.Dispose();
            _speechRequest = null;
        }

        /// <summary>
        /// 인증 정보는 로컬 요청 헤더에만 넣고 로그나 씬 에셋에 저장하지 않는다.
        /// </summary>
        private UnityWebRequest CreateRequest(string route, string json, int timeout)
        {
            var request = new UnityWebRequest(_connection.url + route, "POST")
            {
                uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json)),
                downloadHandler = new DownloadHandlerBuffer(),
                timeout = timeout,
                redirectLimit = 0
            };
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + _connection.token);
            request.SetRequestHeader("X-Fantasya-Client", "game-player");
            return request;
        }
    }
}
