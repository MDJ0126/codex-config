using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Overdox.Editor
{
    public static partial class RestoredUiBuilder
    {
        /// <summary>
        /// 기존 플레이용 로비에 원작 장착 레벨·세트·프레임·무기군 제목과 브랜드 DDT 문구를 연결한다.
        /// </summary>
        [MenuItem("OVERDOX/Restore/Bind native equipment display")]
        public static void BindNativeEquipmentDisplay()
        {
            if (EditorApplication.isPlaying || SceneManager.GetActiveScene().isDirty)
            {
                throw new InvalidOperationException("현재 씬을 저장하고 재생을 종료하세요.");
            }
            var setup = EditorSceneManager.GetSceneManagerSetup();
            try
            {
                var scene = EditorSceneManager.OpenScene("Assets/Scenes/OverdoxLobby.unity", OpenSceneMode.Single);
                var controller = UnityEngine.Object.FindFirstObjectByType<RestoredLobbyController>();
                var document = controller.GetComponent<RestoredUiDocument>();
                var layout = JsonUtility.FromJson<Layout>(File.ReadAllText("Assets/GameData/RestoredUI/Lobby.json"));
                BindEquipmentDisplay(controller, document, layout);
                EditorSceneManager.SaveScene(scene);
                AssetDatabase.SaveAssets();
            }
            finally
            {
                EditorSceneManager.RestoreSceneManagerSetup(setup);
            }
            Debug.Log("OVERDOX_EQUIPMENT_DISPLAY_BOUND slots=4 weaponTitleIcons=8 brandIcons=6 source=DDT");
        }

        /// <summary>
        /// 원본 ID로 표시 참조를 찾아 연결하고, 실제 역전이 확인된 정적 배경을 글자보다 먼저 그린다.
        /// </summary>
        private static void BindEquipmentDisplay(RestoredLobbyController controller, RestoredUiDocument document, Layout layout)
        {
            var presentation = controller.GetComponent<RestoredEquipmentPresentation>() ?? controller.gameObject.AddComponent<RestoredEquipmentPresentation>();
            var roots = new[] { 32873, 39623, 35176, 34357 }.Select(document.Find).OrderByDescending(rect => rect.localPosition.y).ToArray();
            presentation.slots = roots.Select(root => new RestoredEquippedSlot
            {
                level = root.GetComponentsInChildren<Text>(true).Single(text => text.name == "100. Label - LevelCount"),
                setMark = root.GetComponentsInChildren<Text>(true).Single(text => text.name == "100. Label - SetMark").gameObject,
                normalFrame = root.GetComponentsInChildren<Image>(true).Single(image => image.name == "14. Sprite - Frame"),
                legendaryFrame = root.GetComponentsInChildren<Image>(true).Single(image => image.name == "14. Sprite - Frame_Legend").gameObject
            }).ToArray();
            // 원작 GameConfig.cctor의 STRONG Color32 상수와 GetEquipmentRarityColor(rarity,true).
            presentation.rarityColors = new[]
            {
                new Color32(194, 194, 194, 255), new Color32(0, 174, 255, 255),
                new Color32(255, 174, 0, 255), new Color32(168, 0, 255, 255), new Color32(255, 0, 0, 255)
            };
            presentation.setTitle = document.Find(35057).GetComponent<Text>();
            presentation.setDescription = document.Find(33865).GetComponent<Text>();
            presentation.setTitle.resizeTextForBestFit = true;
            presentation.setTitle.resizeTextMinSize = 12;
            presentation.setTitle.resizeTextMaxSize = 18;
            presentation.localizedTextBinary = AssetDatabase.LoadAssetAtPath<TextAsset>(ExcelGameDataCompiler.BinaryFolder + "/LocalizedText.bytes");
            presentation.armorBrandBinary = AssetDatabase.LoadAssetAtPath<TextAsset>(ExcelGameDataCompiler.BinaryFolder + "/ArmorBrand.bytes");
            presentation.setAbilityBinary = AssetDatabase.LoadAssetAtPath<TextAsset>(ExcelGameDataCompiler.BinaryFolder + "/EquipmentSetAbility.bytes");
            if (presentation.localizedTextBinary == null || presentation.armorBrandBinary == null || presentation.setAbilityBinary == null)
            {
                throw new InvalidDataException("Excel에서 컴파일된 장비 표시 DDT 세 표가 필요합니다.");
            }
            var atlases = layout.atlases.ToDictionary(atlas => atlas.name);
            var cache = new System.Collections.Generic.Dictionary<string, Sprite>();
            Sprite Icon(string name) => ResolveSprite(new Widget { atlas = "LobbyUI", sprite = name }, atlases, cache)
                ?? throw new InvalidDataException("원작 UI 아이콘 누락: " + name);
            string[] typeNames = { "Head", "Body", "Cape", "Gear" };
            for (int index = 0; index < roots.Length; index++)
            {
                roots[index].GetComponentsInChildren<Image>(true).Single(image => image.name == "4. Sprite - Type").sprite = Icon("CardIcon_" + typeNames[index]);
            }
            string[] weaponNames = { "Sword", "Hammer", "DualSword", "GiantSword", "GiantHammer", "Halberd", "QuarterStaff", "LongSword" };
            var weaponIcons = weaponNames.Select(name => Icon("Icon_" + name)).ToArray();
            foreach (var card in controller.cards)
            {
                card.weaponTitleIcons = weaponIcons;
                EditorUtility.SetDirty(card);
            }
            RestoreSetTitleBackdrop(document, layout);
            controller.equipmentPresentation = presentation;
            EditorUtility.SetDirty(presentation);
            EditorUtility.SetDirty(controller);
        }

        /// <summary>
        /// 실제 depth273 글자를 덮던 depth274 정적 배경을 같은 컨테이너의 앞 형제로 옮기고 화면 좌표를 확인한다.
        /// </summary>
        private static void RestoreSetTitleBackdrop(RestoredUiDocument document, Layout layout)
        {
            // NGUI 원본: 글자35057 depth100, 검정 Gradation38064 depth5.
            // 실제 uGUI 진단은 두 Graphic만 제목 중심과 겹쳤고 검정 배경이 나중에 그려졌음을 확인했다.
            var title = document.Find(35057);
            var backdrop = document.Find(38064);
            var before = new Vector3[4];
            backdrop.GetWorldCorners(before);
            if (backdrop.parent == title)
            {
                // 이 배경에는 입력·동적 바인딩·애니메이션이 없다. 객체 ID·Image·원본 문서 참조는 유지한다.
                // 같은 Canvas의 정적 형제로 옮겨 추가 Canvas·복제 Graphic·매 프레임 동기화를 피한다.
                backdrop.SetParent(title.parent, true);
            }
            if (backdrop.parent != title.parent)
            {
                throw new InvalidOperationException("세트 배경의 예상 계층과 다릅니다. 사용자 변경을 보존합니다.");
            }
            backdrop.SetAsFirstSibling();
            var after = new Vector3[4];
            backdrop.GetWorldCorners(after);
            float cornerDelta = Enumerable.Range(0, 4).Max(index => Vector3.Distance(before[index], after[index]));
            if (cornerDelta > .001f)
            {
                throw new InvalidOperationException("배경의 화면 배치가 변경됐습니다: " + cornerDelta);
            }
            // 앞선 진단용 Anchor-Bottom 가지 이동은 원인이 아니었으므로 원본 순서로 복구한다.
            document.Find(38386).SetSiblingIndex(layout.transforms.Single(node => node.id == 38386).order);
            Debug.Log($"OVERDOX_EQUIPMENT_BACKDROP_RESTORED sourceId=38064 sourceDepth=5 parent=38500 worldCornerDelta={cornerDelta}");
        }
    }
}
