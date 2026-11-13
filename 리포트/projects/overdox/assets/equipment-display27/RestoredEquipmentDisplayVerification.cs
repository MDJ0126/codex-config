using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DesignDataTable;
using Overdox.OriginalData;
using UnityEditor;
using UnityEngine;

namespace Overdox.Editor
{
    /// <summary>
    /// 실제 DDT 공통 마스터와 플레이용 UI 참조를 대조한다. 전 장비 애니메이션 실행 검증과 구분한다.
    /// </summary>
    public static class RestoredEquipmentDisplayVerification
    {
        /// <summary>
        /// 장비119행의 희귀도·시작 레벨·카테고리와 원작에서 확인한 무기군·브랜드 아이콘 연결을 검사한다.
        /// </summary>
        [MenuItem("OVERDOX/Restore/Verify native equipment display data")]
        public static void Verify()
        {
            var controller = UnityEngine.Object.FindFirstObjectByType<RestoredLobbyController>();
            var session = UnityEngine.Object.FindFirstObjectByType<ArenaSession>();
            if (controller == null || session == null || controller.equipmentPresentation == null)
            {
                throw new InvalidOperationException("표시 연결을 마친 로비 씬이 필요합니다.");
            }
            var masterBinary = AssetDatabase.LoadAssetAtPath<TextAsset>(ExcelGameDataCompiler.BinaryFolder + "/Equipment.bytes");
            var master = DataLoader.ReadBinaryDictionary<Equipment>(masterBinary.bytes, GearCatalog.BINARY_KEY);
            var brands = DataLoader.ReadBinaryDictionary<ArmorBrand>(controller.equipmentPresentation.armorBrandBinary.bytes, GearCatalog.BINARY_KEY);
            string literalJson = File.ReadAllText("Docs/Verification/EquipmentDisplay/native-ui-literals.json");
            var literals = JsonUtility.FromJson<NativeLiteralList>("{\"rows\":" + literalJson + "}").rows.Where(row => !string.IsNullOrEmpty(row.@string)).Select(row => row.@string).ToArray();
            var errors = new List<string>();
            var rows = session.catalog.Weapons.Concat(session.catalog.Armor).ToArray();
            foreach (var row in rows)
            {
                if (!master.TryGetValue(row.id, out var source))
                {
                    errors.Add("공통 마스터 누락: " + row.id);
                    continue;
                }
                int category = row.id < 20000 ? row.family + 1 : row.brand + 1;
                if (row.rarity != source.Rarity || row.startLevel != source.TrainingStartLevel || category != source.EquipmentCategory)
                {
                    errors.Add("공통 마스터 값 불일치: " + row.id);
                }
            }
            foreach (var card in controller.cards)
            {
                if (card.weaponTitleIcons == null || card.weaponTitleIcons.Length != literals.Length ||
                    !card.weaponTitleIcons.Select(AssetDatabase.GetAssetPath).SequenceEqual(literals.Select(SpritePath)))
                {
                    errors.Add("원작 무기군 아이콘 불일치: " + card.name);
                }
                if (card.brandIcons == null || card.brandIcons.Length != brands.Count ||
                    !card.brandIcons.Select(AssetDatabase.GetAssetPath).SequenceEqual(brands.Values.OrderBy(row => row.Id).Select(row => SpritePath(row.ResourceName))))
                {
                    errors.Add("DDT 브랜드 아이콘 불일치: " + card.name);
                }
            }
            if (rows.Length != 119 || controller.equipmentPresentation.slots.Length != 4)
            {
                errors.Add("현재 원작119행 또는 장착4슬롯 범위 불일치");
            }
            var result = new DisplayVerification
            {
                recordedAt = DateTime.UtcNow.ToString("O"), equipmentRows = rows.Length,
                weaponRows = session.catalog.Weapons.Count, armorRows = session.catalog.Armor.Count,
                cardPool = controller.cards.Length, weaponFamilies = literals.Length, brands = brands.Count,
                errors = errors.ToArray(), scope = "Actual saved DDT master/catalog and serialized UI icon references; not all119 gameplay/rig runs"
            };
            Directory.CreateDirectory("Docs/Verification/EquipmentDisplay");
            File.WriteAllText("Docs/Verification/EquipmentDisplay/ddt-display-verification.json", JsonUtility.ToJson(result, true));
            if (errors.Count > 0)
            {
                throw new InvalidDataException(string.Join("; ", errors));
            }
            Debug.Log("OVERDOX_EQUIPMENT_DISPLAY_VERIFIED " + JsonUtility.ToJson(result));
        }

        /// <summary>
        /// 원작 문자열을 기존 변환기의 저장 자산 경로로 변환한다. 저장 후 붙는 atlas 접두사를 원본 이름과 혼동하지 않는다.
        /// </summary>
        private static string SpritePath(string sourceName)
        {
            return "Assets/Art/Restored/UI/Sprites/LobbyUI_" + sourceName + ".asset";
        }

        /// <summary>
        /// ELF 재배치로 확인한 실제 원작 문자열 슬롯의 값만 역직렬화한다.
        /// </summary>
        [Serializable]
        private sealed class NativeLiteral
        {
            public string @string;
        }

        /// <summary>
        /// JsonUtility가 읽을 수 있도록 원작 문자열 배열의 루트를 감싼다.
        /// </summary>
        [Serializable]
        private sealed class NativeLiteralList
        {
            public NativeLiteral[] rows;
        }

        /// <summary>
        /// 실제 표·직렬화 참조 대조의 시각과 적용 범위·오류를 남긴다.
        /// </summary>
        [Serializable]
        private sealed class DisplayVerification
        {
            public string recordedAt, scope;
            public int equipmentRows, weaponRows, armorRows, cardPool, weaponFamilies, brands;
            public string[] errors;
        }
    }
}
