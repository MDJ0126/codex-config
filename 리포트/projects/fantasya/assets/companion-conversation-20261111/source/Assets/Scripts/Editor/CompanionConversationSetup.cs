using System;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Fantasya.Editor
{
    /// <summary>
    /// 기존 동료 파티에 성격 에셋과 편집 가능한 대화창을 한 번 추가하고 씬에 저장한다.
    /// </summary>
    public static class CompanionConversationSetup
    {
        /// <summary>
        /// 이미 만든 대화창은 보존하고 새 참조와 UI만 구성한다.
        /// </summary>
        [MenuItem("Fantasya/AI/동료 대화창 구성")]
        public static void Configure()
        {
            var scene = EditorSceneManager.OpenScene("Assets/Scenes/Playground.unity");
            AgentParty party = UnityEngine.Object.FindFirstObjectByType<AgentParty>();
            if (party == null)
            {
                throw new InvalidOperationException("먼저 Playground 동료 구성을 실행해 주세요.");
            }
            if (UnityEngine.Object.FindFirstObjectByType<AgentConversationPanel>() != null)
            {
                Debug.Log("기존 대화창과 Inspector 배치를 보존했습니다.");
                return;
            }
            if (!AssetDatabase.IsValidFolder("Assets/Data/Companions"))
            {
                AssetDatabase.CreateFolder("Assets/Data", "Companions");
            }
            string[] ids = { "luna", "bram", "sera" };
            string[] names = { "루나", "브람", "세라" };
            string[] personalities =
            {
                "신중하고 관찰력이 좋은 정찰 동료. 짧고 차분하게 말하며 위험과 약속을 먼저 확인한다.",
                "듬직하고 솔직한 수호 동료. 동료 보호를 좋아하고 실천할 수 있는 약속만 한다.",
                "호기심 많고 다정한 탐험 동료. 함께 겪은 일을 기억하며 다음 모험을 제안한다."
            };
            var serializedParty = new SerializedObject(party);
            SerializedProperty personas = serializedParty.FindProperty("_personas");
            personas.arraySize = 3;
            for (int index = 0; index < ids.Length; index++)
            {
                string assetPath = "Assets/Data/Companions/" + ids[index] + ".asset";
                CompanionPersona persona = AssetDatabase.LoadAssetAtPath<CompanionPersona>(assetPath);
                if (persona == null)
                {
                    persona = ScriptableObject.CreateInstance<CompanionPersona>();
                    persona.companionId = ids[index];
                    persona.displayName = names[index];
                    persona.personality = personalities[index];
                    AssetDatabase.CreateAsset(persona, assetPath);
                }
                personas.GetArrayElementAtIndex(index).objectReferenceValue = persona;
            }
            serializedParty.ApplyModifiedPropertiesWithoutUndo();
            AgentCompanionLink link = party.GetComponent<AgentCompanionLink>();
            if (link == null)
            {
                link = party.gameObject.AddComponent<AgentCompanionLink>();
            }
            var serializedLink = new SerializedObject(link);
            serializedLink.FindProperty("_party").objectReferenceValue = party;
            serializedLink.ApplyModifiedPropertiesWithoutUndo();

            Canvas canvas = party.GetComponentInChildren<Canvas>();
            Font font = AssetDatabase.LoadAssetAtPath<Font>("Assets/TurnRoom/Templete/Runtime/Resources/TurnRoom/Fonts/TurnRoomSans-Regular.ttf");
            RectTransform panel = Rect("Companion Conversation", canvas.transform);
            panel.anchorMin = panel.anchorMax = panel.pivot = Vector2.zero;
            panel.anchoredPosition = new Vector2(16f, 16f);
            panel.sizeDelta = new Vector2(820f, 330f);
            panel.gameObject.AddComponent<Image>().color = new Color(0.035f, 0.065f, 0.12f, 0.96f);
            AgentConversationPanel controller = panel.gameObject.AddComponent<AgentConversationPanel>();

            Text title = TextAt("Selected Companion", panel, font, 17, new Vector2(14f, -10f), new Vector2(610f, 25f), true);
            title.text = "동료를 선택하고 대화해 보세요";
            Text details = TextAt("Actual Action And Personality", panel, font, 12, new Vector2(14f, -40f), new Vector2(790f, 43f), true);
            details.text = "현재 행동과 이유, 동료별 성격이 여기에 표시됩니다.";
            Button previous = ButtonAt("Previous Companion", "이전", panel, font, new Vector2(672f, -10f), new Vector2(60f, 25f), true);
            Button next = ButtonAt("Next Companion", "다음", panel, font, new Vector2(742f, -10f), new Vector2(60f, 25f), true);
            UnityEventTools.AddPersistentListener(previous.onClick, controller.PreviousCompanion);
            UnityEventTools.AddPersistentListener(next.onClick, controller.NextCompanion);

            RectTransform scrollRoot = Rect("Conversation History", panel);
            Place(scrollRoot, new Vector2(14f, -88f), new Vector2(790f, 148f), true);
            scrollRoot.gameObject.AddComponent<Image>().color = new Color(0.06f, 0.1f, 0.17f, 0.8f);
            ScrollRect scroll = scrollRoot.gameObject.AddComponent<ScrollRect>();
            scroll.horizontal = false;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            RectTransform viewport = Rect("Viewport", scrollRoot);
            Stretch(viewport, 7f, 5f);
            viewport.gameObject.AddComponent<RectMask2D>();
            RectTransform content = Rect("Dialogue And Memories", viewport);
            content.anchorMin = new Vector2(0f, 1f);
            content.anchorMax = Vector2.one;
            content.pivot = new Vector2(0.5f, 1f);
            content.sizeDelta = Vector2.zero;
            Text transcript = content.gameObject.AddComponent<Text>();
            SetText(transcript, font, 14);
            transcript.verticalOverflow = VerticalWrapMode.Overflow;
            transcript.text = "외부 에이전트의 발언과 게임 사건도 같은 기록에 표시됩니다.";
            ContentSizeFitter fitter = content.gameObject.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            scroll.viewport = viewport;
            scroll.content = content;

            RectTransform inputRoot = Rect("Player Message", panel);
            Place(inputRoot, new Vector2(14f, 48f), new Vector2(690f, 32f), false);
            Image inputBackground = inputRoot.gameObject.AddComponent<Image>();
            inputBackground.color = new Color(0.1f, 0.16f, 0.25f, 1f);
            InputField input = inputRoot.gameObject.AddComponent<InputField>();
            input.targetGraphic = inputBackground;
            input.characterLimit = 1000;
            Text inputText = TextAt("Text", inputRoot, font, 15, Vector2.zero, Vector2.zero, true);
            Stretch(inputText.rectTransform, 8f, 4f);
            Text placeholder = TextAt("Placeholder", inputRoot, font, 13, Vector2.zero, Vector2.zero, true);
            Stretch(placeholder.rectTransform, 8f, 4f);
            placeholder.color = new Color(0.57f, 0.68f, 0.8f);
            placeholder.text = "말 걸기 · 기억해줘: 방패 역할을 좋아해 · 내가 신호할 때까지 기다려";
            input.textComponent = inputText;
            input.placeholder = placeholder;
            input.customCaretColor = true;
            input.caretColor = Color.white;
            Button send = ButtonAt("Send Message", "보내기", panel, font, new Vector2(714f, 48f), new Vector2(90f, 32f), false);
            UnityEventTools.AddPersistentListener(send.onClick, controller.Send);

            Button wait = ButtonAt("Promise Wait", "기다려", panel, font, new Vector2(14f, 10f), new Vector2(85f, 27f), false);
            Button follow = ButtonAt("Promise Follow", "따라와", panel, font, new Vector2(108f, 10f), new Vector2(85f, 27f), false);
            Button clear = ButtonAt("Clear Promise", "약속 해제", panel, font, new Vector2(202f, 10f), new Vector2(100f, 27f), false);
            UnityEventTools.AddPersistentListener(wait.onClick, controller.AskWait);
            UnityEventTools.AddPersistentListener(follow.onClick, controller.AskFollow);
            UnityEventTools.AddPersistentListener(clear.onClick, controller.ClearPromise);
            Text status = TextAt("Connection And Reply Status", panel, font, 12, new Vector2(316f, 10f), new Vector2(490f, 27f), false);
            status.text = "동료 서비스 연결 대기";
            controller.Configure(party, link, title, details, transcript, status, input, scroll, send);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            EditorSceneManager.OpenScene(scene.path);
            Debug.Log("COMPANION_CONVERSATION_SAVED_AND_REOPENED");
        }

        private static RectTransform Rect(string name, Transform parent)
        {
            var result = new GameObject(name, typeof(RectTransform)).GetComponent<RectTransform>();
            result.SetParent(parent, false);
            return result;
        }

        /// <summary>
        /// 기존 대화와 폰트 참조를 보존하면서 채팅·마이크 입력 그룹과 영속 버튼을 한 번 추가한다.
        /// </summary>
        [MenuItem("Fantasya/AI/채팅·마이크 모드 구성")]
        public static void ConfigureInputModes()
        {
            var scene = EditorSceneManager.OpenScene("Assets/Scenes/Playground.unity");
            AgentConversationPanel panel = UnityEngine.Object.FindFirstObjectByType<AgentConversationPanel>();
            if (panel == null)
            {
                Debug.Log("대화창이 없다면 먼저 구성해 주세요.");
                return;
            }
            AgentVoiceInput existing = panel.GetComponent<AgentVoiceInput>();
            if (existing != null)
            {
                var fields = new SerializedObject(existing);
                if (fields.FindProperty("_confirm").objectReferenceValue == null)
                {
                    fields.FindProperty("_confirm").objectReferenceValue = panel.transform.Find("Microphone Input Mode/Confirm And Send Speech").GetComponent<Button>();
                    fields.ApplyModifiedPropertiesWithoutUndo();
                    EditorSceneManager.MarkSceneDirty(scene);
                    EditorSceneManager.SaveScene(scene);
                    EditorSceneManager.OpenScene(scene.path);
                }
                Debug.Log("COMPANION_INPUT_MODES_SAVED_AND_REOPENED: 기존 배치 보존");
                return;
            }
            RectTransform root = (RectTransform)panel.transform;
            root.sizeDelta = new Vector2(root.sizeDelta.x, root.sizeDelta.y + 40f);
            var serialized = new SerializedObject(panel);
            InputField input = (InputField)serialized.FindProperty("_input").objectReferenceValue;
            Button send = (Button)serialized.FindProperty("_send").objectReferenceValue;
            AgentCompanionLink link = (AgentCompanionLink)serialized.FindProperty("_link").objectReferenceValue;
            Font font = input.textComponent.font;
            RectTransform textGroup = Rect("Keyboard Input Mode", root);
            Stretch(textGroup, 0f, 0f);
            input.transform.SetParent(textGroup, false);
            send.transform.SetParent(textGroup, false);
            RectTransform voiceGroup = Rect("Microphone Input Mode", root);
            Stretch(voiceGroup, 0f, 0f);
            InputField review = UnityEngine.Object.Instantiate(input, voiceGroup);
            review.gameObject.name = "Recognized Text Review";
            ((Text)review.placeholder).text = "인식한 문장을 확인·수정한 뒤 보내세요. 자동으로 보내지 않습니다.";
            Button voiceSend = UnityEngine.Object.Instantiate(send, voiceGroup);
            voiceSend.gameObject.name = "Confirm And Send Speech";
            Button textMode = ButtonAt("Select Keyboard Mode", "채팅 입력", root, font, new Vector2(14f, 89f), new Vector2(90f, 27f), false);
            Button voiceMode = ButtonAt("Select Microphone Mode", "마이크", root, font, new Vector2(112f, 89f), new Vector2(90f, 27f), false);
            Text hint = TextAt("Keyboard Mode Hint", root, font, 12, new Vector2(218f, 89f), new Vector2(580f, 27f), false);
            hint.text = "키보드로 문장을 입력하고 보내기를 눌러 주세요. 마이크는 꺼져 있습니다.";
            Button record = ButtonAt("Start Recording", "녹음 시작", voiceGroup, font, new Vector2(215f, 89f), new Vector2(90f, 27f), false);
            Button stop = ButtonAt("Stop And Recognize", "인식하기", voiceGroup, font, new Vector2(312f, 89f), new Vector2(90f, 27f), false);
            Button cancel = ButtonAt("Cancel Recording", "취소", voiceGroup, font, new Vector2(409f, 89f), new Vector2(55f, 27f), false);
            Text status = TextAt("Speech Recording Status", voiceGroup, font, 11, new Vector2(475f, 89f), new Vector2(330f, 27f), false);
            status.text = "녹음 시작 → 인식 → 문장 확인 → 보내기";
            AgentVoiceInput voice = panel.gameObject.AddComponent<AgentVoiceInput>();
            var voiceFields = new SerializedObject(voice);
            voiceFields.FindProperty("_panel").objectReferenceValue = panel;
            voiceFields.FindProperty("_link").objectReferenceValue = link;
            voiceFields.FindProperty("_textGroup").objectReferenceValue = textGroup.gameObject;
            voiceFields.FindProperty("_voiceGroup").objectReferenceValue = voiceGroup.gameObject;
            voiceFields.FindProperty("_keyboardHint").objectReferenceValue = hint;
            voiceFields.FindProperty("_status").objectReferenceValue = status;
            voiceFields.FindProperty("_review").objectReferenceValue = review;
            voiceFields.FindProperty("_record").objectReferenceValue = record;
            voiceFields.FindProperty("_stop").objectReferenceValue = stop;
            voiceFields.FindProperty("_textMode").objectReferenceValue = textMode;
            voiceFields.FindProperty("_voiceMode").objectReferenceValue = voiceMode;
            voiceFields.FindProperty("_confirm").objectReferenceValue = voiceSend;
            voiceFields.ApplyModifiedPropertiesWithoutUndo();
            serialized.FindProperty("_voice").objectReferenceValue = voice;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            UnityEventTools.AddPersistentListener(textMode.onClick, voice.SelectTextMode);
            UnityEventTools.AddPersistentListener(voiceMode.onClick, voice.SelectVoiceMode);
            UnityEventTools.AddPersistentListener(record.onClick, voice.BeginRecording);
            UnityEventTools.AddPersistentListener(stop.onClick, voice.StopAndRecognize);
            UnityEventTools.AddPersistentListener(cancel.onClick, voice.Cancel);
            textMode.interactable = false;
            stop.interactable = false;
            voiceGroup.gameObject.SetActive(false);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            EditorSceneManager.OpenScene(scene.path);
            Debug.Log("COMPANION_INPUT_MODES_SAVED_AND_REOPENED");
        }

        public static void RunInputModes()
        {
            ConfigureInputModes();
            EditorApplication.Exit(0);
        }

        private static void Place(RectTransform rect, Vector2 position, Vector2 size, bool top)
        {
            rect.anchorMin = rect.anchorMax = rect.pivot = top ? new Vector2(0f, 1f) : Vector2.zero;
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
        }

        private static void Stretch(RectTransform rect, float horizontal, float vertical)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(horizontal, vertical);
            rect.offsetMax = new Vector2(-horizontal, -vertical);
        }

        private static Text TextAt(string name, Transform parent, Font font, int size, Vector2 position, Vector2 dimensions, bool top)
        {
            RectTransform rect = Rect(name, parent);
            Place(rect, position, dimensions, top);
            Text text = rect.gameObject.AddComponent<Text>();
            SetText(text, font, size);
            return text;
        }

        private static void SetText(Text text, Font font, int size)
        {
            text.font = font;
            text.fontSize = size;
            text.color = new Color(0.9f, 0.95f, 1f);
            text.supportRichText = false;
            text.raycastTarget = false;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Truncate;
        }

        private static Button ButtonAt(string name, string label, Transform parent, Font font, Vector2 position, Vector2 size, bool top)
        {
            RectTransform rect = Rect(name, parent);
            Place(rect, position, size, top);
            Image background = rect.gameObject.AddComponent<Image>();
            background.color = new Color(0.15f, 0.26f, 0.42f);
            Button button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = background;
            Text text = TextAt("Label", rect, font, 13, Vector2.zero, Vector2.zero, true);
            Stretch(text.rectTransform, 0f, 0f);
            text.alignment = TextAnchor.MiddleCenter;
            text.text = label;
            return button;
        }

        /// <summary>
        /// 배치 구성 후 이번 에디터 세션만 정상 종료한다.
        /// </summary>
        public static void Run()
        {
            Configure();
            EditorApplication.Exit(0);
        }
    }
}
