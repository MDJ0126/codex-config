using UnityEngine;
using UnityEngine.UI;

namespace Overdox
{
    public sealed class RestoredGearCard : MonoBehaviour
    {
        public Text title, level, stat, statName;
        public Image thumbnail, frame, brand;
        public Sprite[] rarityFrames, brandIcons;
        public GameObject selectedMark, legendaryEffect;
        private RestoredLobbyController _owner;
        private int _index;
        public void Select() { _owner?.Select(_index); }
        public void Bind(RestoredLobbyController owner, int index, GearRow row, Sprite icon, bool selected, bool weapon, int gearLevel, float value)
        {
            _owner = owner;
            _index = index;
            title.text = row.name;
            level.text = gearLevel.ToString();
            stat.text = value.ToString("0");
            if (statName != null) { statName.text = weapon ? "Damage" : "HP"; }
            if (legendaryEffect != null) { legendaryEffect.SetActive(row.rarity == 4); }
            if (frame != null && rarityFrames != null && row.rarity < rarityFrames.Length) { frame.sprite = rarityFrames[row.rarity]; }
            if (brand != null && brandIcons != null && row.brand < brandIcons.Length) { brand.sprite = brandIcons[row.brand]; }
            thumbnail.sprite = icon;
            thumbnail.enabled = icon != null;
            selectedMark.SetActive(selected);
        }
    }
}
