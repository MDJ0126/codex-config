using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using DesignDataTable;
using Overdox.OriginalData;
using UnityEngine;
using UnityEngine.UI;

namespace Overdox
{
    /// <summary>
    /// 원작 장착 슬롯의 레벨·프레임·세트 표식과 브랜드 효과 문구를 현재 DDT 값으로 표시한다.
    /// </summary>
    public sealed class RestoredEquipmentPresentation : MonoBehaviour
    {
        public RestoredEquippedSlot[] slots;
        public Color32[] rarityColors;
        public Text setTitle, setDescription;
        public TextAsset localizedTextBinary, armorBrandBinary, setAbilityBinary;
        private Dictionary<string, string> _texts;
        private Dictionary<int, ArmorBrand> _brands;
        private Dictionary<int, EquipmentSetAbility> _abilities;

        /// <summary>
        /// 실제 장착 장비만 읽어 네 슬롯과 브랜드 세트 정보를 갱신한다. 열람 카드로 장착 표시를 바꾸지 않는다.
        /// </summary>
        public void Refresh(Fighter player, int trainingOffset)
        {
            EnsureData();
            bool isMatched = player.armor[0].brand == player.armor[1].brand && player.armor[1].brand == player.armor[2].brand;
            for (int index = 0; index < slots.Length; index++)
            {
                var row = index == 3 ? player.weapon : player.armor[index];
                int level = Mathf.Clamp(row.startLevel + trainingOffset, 1, row.maxLevel);
                slots[index].Bind(row, level, rarityColors[row.rarity], index < 3 && isMatched);
            }
            UpdateSetText(player.armor);
        }

        /// <summary>
        /// 저장 XLSX에서 컴파일된 원본 스키마의 DDT 세 표를 한 번 읽어 표시용 조회를 준비한다.
        /// </summary>
        private void EnsureData()
        {
            if (_texts != null)
            {
                return;
            }
            _brands = DataLoader.ReadBinaryDictionary<ArmorBrand>(armorBrandBinary.bytes, GearCatalog.BINARY_KEY);
            _abilities = DataLoader.ReadBinaryDictionary<EquipmentSetAbility>(setAbilityBinary.bytes, GearCatalog.BINARY_KEY);
            // 제작용 DDT 리더를 그대로 사용하되 표시용14개 키만 유지한다. 전체4302행·영어 문자열은 캐시하지 않는다.
            var requiredKeys = new HashSet<string>(_brands.Values.Select(row => row.NameKey).Concat(_abilities.Values.Select(row => row.ExplanationKey)))
            {
                "armor_set_1", "armor_set_2"
            };
            _texts = DataLoader.ReadBinaryDictionary<LocalizedText>(localizedTextBinary.bytes, GearCatalog.BINARY_KEY).Values
                .Where(row => requiredKeys.Contains(row.Key)).ToDictionary(row => row.Key, row => row.Korean);
        }

        /// <summary>
        /// 브랜드 조건과 요구 개수를 만족하는 원본 효과를 표시하고 불일치하면 원작의 세트효과 없음 문구를 쓴다.
        /// </summary>
        private void UpdateSetText(GearRow[] armor)
        {
            // 카탈로그 brand는0부터, 원본 EquipmentCategory와 ArmorBrand ID는1부터다.
            int brandId = armor[0].brand + 1;
            int count = armor.Count(row => row.brand + 1 == brandId);
            var effects = _abilities.Values.Where(row => row.SetRequireStatus == 1 && row.ConditionType == 1 &&
                row.ConditionValue1 == brandId && count >= row.SetRequireCount).OrderBy(row => row.Id).ToArray();
            bool isActive = effects.Length > 0;
            if (!isActive)
            {
                setTitle.text = TextFor("armor_set_2");
                setDescription.text = string.Empty;
                setDescription.gameObject.SetActive(false);
                return;
            }
            string brandName = TextFor(_brands[brandId].NameKey);
            setTitle.text = RichText(string.Format(CultureInfo.InvariantCulture, TextFor("armor_set_1"), brandName));
            setDescription.text = string.Join("\n", effects.Select(FormatEffect));
            setDescription.gameObject.SetActive(true);
        }

        /// <summary>
        /// 원작 표의 비율 효과만 백분율로 환산하고 정수 크레딧·면역 문구는 원래 단위로 표시한다.
        /// </summary>
        private string FormatEffect(EquipmentSetAbility effect)
        {
            // 현 원본 세트 표에서9·35·59는 비율이며 원작 GetSetEffect의 배율 표도100을 사용한다.
            bool isPercent = effect.AbilityType == 9 || effect.AbilityType == 35 || effect.AbilityType == 59;
            string value = (effect.AbilityValue1 * (isPercent ? 100 : 1)).ToString("0.##", CultureInfo.InvariantCulture);
            return RichText(string.Format(CultureInfo.InvariantCulture, TextFor(effect.ExplanationKey), value));
        }

        /// <summary>
        /// 원본 한국어 문자열을 조회한다. 누락은 잘못된 브랜드 문구로 대체하지 않고 키로 드러낸다.
        /// </summary>
        private string TextFor(string key)
        {
            return _texts.TryGetValue(key, out string text) ? text : key;
        }

        /// <summary>
        /// 원본 NGUI 색상 태그를 기존 uGUI Text의 rich text 문법으로 옮긴다.
        /// </summary>
        private static string RichText(string value)
        {
            return Regex.Replace(value, @"\[([0-9A-Fa-f]{6}(?:[0-9A-Fa-f]{2})?)\]", "<color=#$1>").Replace("[-]", "</color>");
        }
    }

    /// <summary>
    /// 원본 계층을 유지한 한 장착 슬롯의 동적 표시 참조를 보관한다.
    /// </summary>
    [Serializable]
    public sealed class RestoredEquippedSlot
    {
        public Text level;
        public GameObject setMark;
        public Image normalFrame;
        public GameObject legendaryFrame;

        /// <summary>
        /// 원작의 일반 프레임 색상·전설 전용 프레임·브랜드 세트 조건을 적용한다. S는 희귀도 등급이 아니다.
        /// </summary>
        public void Bind(GearRow row, int gearLevel, Color32 color, bool matchingSet)
        {
            level.text = gearLevel.ToString(CultureInfo.InvariantCulture);
            setMark.SetActive(matchingSet);
            bool isLegendary = row.rarity == 4;
            normalFrame.gameObject.SetActive(!isLegendary);
            normalFrame.color = color;
            legendaryFrame.SetActive(isLegendary);
        }
    }
}
