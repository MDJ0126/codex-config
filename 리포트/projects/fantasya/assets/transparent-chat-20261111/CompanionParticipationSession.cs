using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

namespace Fantasya.Verification
{
    /// <summary>
    /// 격리된 실제 Play Mode에서 외부 참여와 대화 UI를 관찰하고 앱 자체 카메라 출력을 저장한다.
    /// </summary>
    [InitializeOnLoad]
    public static class CompanionParticipationSession
    {
        private static double _started;
        private static double _walkUntil;
        private static Keyboard _keyboard;
        private static Key[] _keys = Array.Empty<Key>();
        private static string _root;

        [Serializable] private sealed class Command
        {
            public string id;
            public string action;
            public string text;
            public string companionId;
            public string keys;
            public float seconds;
            public string filename;
        }

        [Serializable] private sealed class Result
        {
            public string id;
            public bool success;
            public string message;
            public string at;
            public string selected;
            public string input;
            public bool typing;
            public bool virtualForwardPressed;
            public bool microphoneMode;
            public bool recording;
            public string recognizedText;
            public string speechStatus;
            public Vector3 playerPosition;
            public CompanionSnapshot[] companions;
        }

        static CompanionParticipationSession()
        {
            if (SessionState.GetBool("Fantasya.Participation", false))
            {
                Initialize();
            }
        }

        /// <summary>
        /// 사용자 세션을 건드리지 않고 배치에서 Playground를 실제 플레이한다.
        /// </summary>
        public static void Run()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Playground.unity");
            SessionState.SetBool("Fantasya.Participation", true);
            Initialize();
            EditorApplication.EnterPlaymode();
        }

        private static void Initialize()
        {
            _started = EditorApplication.timeSinceStartup;
            _root = Environment.GetEnvironmentVariable("FANTASYA_PARTICIPATION_ROOT") ?? Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            Directory.CreateDirectory(Path.Combine(_root, ".utmp", "Participation", "requests"));
            Directory.CreateDirectory(Path.Combine(_root, ".utmp", "Participation", "results"));
            EditorApplication.update -= Tick;
            EditorApplication.update += Tick;
        }

        private static void Tick()
        {
            if (EditorApplication.timeSinceStartup - _started > 1200d)
            {
                Stop();
                return;
            }
            if (!EditorApplication.isPlaying)
            {
                return;
            }
            if (_keyboard == null)
            {
                _keyboard = InputSystem.AddDevice<Keyboard>();
                _keyboard.MakeCurrent();
                InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
                InputSystem.settings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
                Application.runInBackground = true;
                Application.targetFrameRate = 30;
                QualitySettings.vSyncCount = 0;
                if (Environment.GetEnvironmentVariable("FANTASYA_PARTICIPATION_SAFE_COMBAT") == "1")
                {
                    foreach (MonsterAI monsterAI in UnityEngine.Object.FindObjectsByType<MonsterAI>(FindObjectsSortMode.None))
                    {
                        monsterAI.enabled = false;
                        foreach (MonoBehaviour behaviour in monsterAI.GetComponents<MonoBehaviour>())
                        {
                            if (behaviour.GetType().Name == "BehaviorGraphAgent")
                            {
                                behaviour.enabled = false;
                            }
                        }
                    }
                    Debug.Log("Participation verification only: enemy AI paused for repeatable dialogue and input checks.");
                }
                File.WriteAllText(Path.Combine(_root, ".utmp", "Participation", "ready.json"), "{\"ready\":true}");
            }
            _keyboard.MakeCurrent();
            InputSystem.QueueStateEvent(_keyboard, new KeyboardState(EditorApplication.timeSinceStartup < _walkUntil ? _keys : Array.Empty<Key>()));
            InputSystem.Update();
            foreach (string file in Directory.GetFiles(Path.Combine(_root, ".utmp", "Participation", "requests"), "*.json"))
            {
                Command command = null;
                try
                {
                    command = JsonUtility.FromJson<Command>(File.ReadAllText(file));
                    File.Delete(file);
                    Execute(command);
                }
                catch (Exception exception)
                {
                    if (command != null)
                    {
                        Record(command.id, false, exception.Message);
                    }
                    Debug.LogException(exception);
                }
            }
        }

        /// <summary>
        /// 검증용 가상 입력과 실제 UI 버튼 경로만 실행하고 임의 코드·경로는 받지 않는다.
        /// </summary>
        private static void Execute(Command command)
        {
            if (command == null || !Guid.TryParse(command.id, out _))
            {
                throw new FormatException("Invalid verification command ID.");
            }
            AgentConversationPanel panel = UnityEngine.Object.FindFirstObjectByType<AgentConversationPanel>();
            switch (command.action)
            {
                case "observe":
                {
                    Record(command.id, true, "Actual Play Mode snapshot");
                    break;
                }
                case "voice_mode":
                {
                    AgentVoiceInput voice = panel.GetComponent<AgentVoiceInput>();
                    var serialized = new SerializedObject(voice);
                    ((Button)serialized.FindProperty("_voiceMode").objectReferenceValue).onClick.Invoke();
                    Record(command.id, true, "Stored microphone mode event; physical microphone remains off");
                    break;
                }
                case "text_mode":
                {
                    AgentVoiceInput voice = panel.GetComponent<AgentVoiceInput>();
                    var serialized = new SerializedObject(voice);
                    ((Button)serialized.FindProperty("_textMode").objectReferenceValue).onClick.Invoke();
                    Record(command.id, true, "Stored keyboard mode event");
                    break;
                }
                case "speech_sample":
                {
                    byte[] wave = File.ReadAllBytes(Path.Combine(_root, ".utmp", "LocalSpeech", "korean-sample.wav"));
                    int count = (wave.Length - 44) / 2;
                    var data = new float[count];
                    for (int index = 0; index < count; index++)
                    {
                        data[index] = BitConverter.ToInt16(wave, 44 + index * 2) / 32768f;
                    }
                    AudioClip clip = AudioClip.Create("Explicit synthetic Korean verification", count, 1, 16000, false);
                    clip.SetData(data, 0);
                    panel.GetComponent<AgentVoiceInput>().RecognizeRecording(clip, count);
                    UnityEngine.Object.Destroy(clip);
                    Record(command.id, true, "Synthetic Korean audio through production WAV encoder and speech request; no microphone capture");
                    break;
                }
                case "voice_send":
                {
                    var serialized = new SerializedObject(panel.GetComponent<AgentVoiceInput>());
                    ((Button)serialized.FindProperty("_confirm").objectReferenceValue).onClick.Invoke();
                    Record(command.id, true, "Stored voice confirmation Send event invoked");
                    break;
                }
                case "capture":
                {
                    if (string.IsNullOrEmpty(command.filename) || command.filename != Path.GetFileName(command.filename) || !command.filename.EndsWith(".png"))
                    {
                        throw new FormatException("Expected a PNG filename without directories.");
                    }
                    string directory = Path.Combine(_root, "Dosc", "reference", "conversation");
                    Directory.CreateDirectory(directory);
                    Canvas.ForceUpdateCanvases();
                    Camera camera = Camera.main;
                    var target = new RenderTexture(1280, 720, 24);
                    camera.targetTexture = target;
                    Canvas[] overlays = UnityEngine.Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None).Where(canvas => canvas.renderMode == RenderMode.ScreenSpaceOverlay).ToArray();
                    foreach (Canvas canvas in overlays)
                    {
                        canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    }
                    Canvas.ForceUpdateCanvases();
                    camera.Render();
                    foreach (Canvas canvas in overlays)
                    {
                        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    }
                    RenderTexture.active = target;
                    var image = new Texture2D(1280, 720, TextureFormat.RGB24, false);
                    image.ReadPixels(new Rect(0, 0, 1280, 720), 0, 0);
                    image.Apply();
                    File.WriteAllBytes(Path.Combine(directory, command.filename), image.EncodeToPNG());
                    camera.targetTexture = null;
                    RenderTexture.active = null;
                    UnityEngine.Object.DestroyImmediate(image);
                    UnityEngine.Object.DestroyImmediate(target);
                    Record(command.id, true, command.filename);
                    break;
                }
                case "walk":
                {
                    _keys = (command.keys ?? "W").Split(',').Select(key => (Key)Enum.Parse(typeof(Key), key, true)).ToArray();
                    _walkUntil = EditorApplication.timeSinceStartup + Mathf.Clamp(command.seconds, 0.1f, 6f);
                    Record(command.id, true, "Virtual Unity keyboard only; no OS input");
                    break;
                }
                case "focus_input":
                {
                    var serialized = new SerializedObject(panel);
                    var input = (InputField)serialized.FindProperty("_input").objectReferenceValue;
                    EventSystem.current.SetSelectedGameObject(input.gameObject);
                    input.ActivateInputField();
                    Record(command.id, true, "Conversation input focused inside isolated Unity");
                    break;
                }
                case "clear_input":
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    Record(command.id, true, "Conversation focus released");
                    break;
                }
                case "send_chat":
                {
                    var serialized = new SerializedObject(panel);
                    var input = (InputField)serialized.FindProperty("_input").objectReferenceValue;
                    var send = (Button)serialized.FindProperty("_send").objectReferenceValue;
                    input.text = command.text;
                    send.onClick.Invoke();
                    Record(command.id, true, "Stored Send button event invoked");
                    break;
                }
                case "select_companion":
                {
                    AgentParty party = UnityEngine.Object.FindFirstObjectByType<AgentParty>();
                    for (int index = 0; index < party.Members.Count && panel.Selected.CompanionId != command.companionId; index++)
                    {
                        panel.NextCompanion();
                    }
                    Record(command.id, panel.Selected.CompanionId == command.companionId, "Selected companion");
                    break;
                }
                case "stop":
                {
                    Record(command.id, true, "Participation session stopped");
                    Stop();
                    break;
                }
                default:
                {
                    throw new ArgumentException("Unknown verification action.");
                }
            }
        }

        private static void Record(string id, bool success, string message)
        {
            AgentParty party = UnityEngine.Object.FindFirstObjectByType<AgentParty>();
            PlayerController player = UnityEngine.Object.FindFirstObjectByType<PlayerController>();
            AgentConversationPanel panel = UnityEngine.Object.FindFirstObjectByType<AgentConversationPanel>();
            InputField input = panel == null ? null : (InputField)new SerializedObject(panel).FindProperty("_input").objectReferenceValue;
            AgentVoiceInput voice = panel == null ? null : panel.GetComponent<AgentVoiceInput>();
            var result = new Result
            {
                id = id,
                success = success,
                message = message,
                at = DateTimeOffset.Now.ToString("o"),
                selected = panel != null && panel.Selected != null ? panel.Selected.CompanionId : string.Empty,
                input = input != null ? input.text : string.Empty,
                typing = AgentConversationPanel.IsTyping,
                virtualForwardPressed = _keyboard != null && _keyboard.wKey.isPressed,
                microphoneMode = voice != null && voice.IsVoiceMode,
                recording = voice != null && voice.IsRecording,
                recognizedText = voice != null ? voice.ReviewInput.text : string.Empty,
                speechStatus = voice != null ? ((Text)new SerializedObject(voice).FindProperty("_status").objectReferenceValue).text : string.Empty,
                playerPosition = player != null ? player.ControlledCharacter.transform.position : Vector3.zero,
                companions = party != null ? party.Members.Select(agent => agent.GetSnapshot()).ToArray() : Array.Empty<CompanionSnapshot>()
            };
            File.WriteAllText(Path.Combine(_root, ".utmp", "Participation", "results", id + ".json"), JsonUtility.ToJson(result, true));
        }

        private static void Stop()
        {
            SessionState.SetBool("Fantasya.Participation", false);
            EditorApplication.update -= Tick;
            if (_keyboard != null)
            {
                InputSystem.RemoveDevice(_keyboard);
                _keyboard = null;
            }
            EditorApplication.Exit(0);
        }
    }
}
