using UnityEngine;
using UnityEngine.UI;

namespace Overdox
{
    /// <summary>
    /// 재사용하는 장비 카드에 표시 데이터를 연결하고 선택 명령을 로비 제어기에 전달한다.
    /// </summary>
    public sealed class RestoredGearCard : MonoBehaviour
    {
        public Text title, level, stat, statName;
        public Image thumbnail, frame, brand;
        public Sprite[] rarityFrames, brandIcons;
        public GameObject selectedMark, legendaryEffect;
        private RestoredLobbyController _owner;
        private int _index;
        /// <summary>
        /// 마지막으로 연결한 카탈로그 인덱스를 목록 중앙으로 이동한다.
        /// </summary>
        public void Select()
        {
            _owner?.Focus(_index);
        }

        /// <summary>
        /// 장비와 계산된 능력치로 카드를 갱신하며 선택·희귀도·브랜드 표시를 맞춘다.
        /// </summary>
        /// <param name="index">로비 선택 명령에 다시 전달할 카탈로그 인덱스이다.</param>
        /// <param name="value">로비 제어기가 DDT에서 계산한 현재 단계의 공격력 또는 체력이다.</param>
        public void Bind(RestoredLobbyController owner, int index, GearRow row, Sprite icon, bool selected, bool weapon, int gearLevel, float value)
        {
            _owner = owner;
            _index = index;
            title.text = row.name;
            level.text = gearLevel.ToString();
            stat.text = value.ToString("0");
            if (statName != null)
            {
                statName.text = weapon ? "Damage" : "HP";
            }
            if (legendaryEffect != null)
            {
                legendaryEffect.SetActive(row.rarity == 4);
            }
            // 배열은 현재 카탈로그의 0 이상 희귀도·브랜드 값으로 구성한다.
            if (frame != null && rarityFrames != null && row.rarity < rarityFrames.Length)
            {
                frame.sprite = rarityFrames[row.rarity];
            }
            if (brand != null && brandIcons != null && row.brand < brandIcons.Length)
            {
                brand.sprite = brandIcons[row.brand];
            }
            thumbnail.sprite = icon;
            thumbnail.enabled = icon != null;
            selectedMark.SetActive(selected);
        }
    }
}
