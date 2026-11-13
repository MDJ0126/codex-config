using System;
using UnityEngine;
using UnityEngine.UI;

namespace Overdox
{
    [Serializable] public sealed class RestoredGearIcon
    {
        public int gearId;
        public Sprite male, female;
    }

    /// <summary>저장된 원본 UI에 세션 장비 데이터와 화면 전환만 연결한다.</summary>
    public sealed class RestoredLobbyController : MonoBehaviour
    {
        public GameObject home, equipment, profile, back;
        public Text windowTitle, nickname, health, damage, weight;
        public RestoredGearCard[] cards;
        public Image[] equippedIcons;
        public RestoredGearIcon[] icons;
        private ArenaSession _session;
        private int _slot = -1, _first;
        private int _revision = int.MinValue;

        public void OpenEquipment() { home.SetActive(false); equipment.SetActive(true); profile.SetActive(false); back.SetActive(true); windowTitle.text = "캐릭터"; Refresh(); }
        public void OpenHome() { home.SetActive(true); equipment.SetActive(false); profile.SetActive(true); back.SetActive(false); }
        public void Battle() { _session?.StartMatch(); }
        public void Gender() { _session?.ToggleGender(); Refresh(); }
        public void Weapons() { ChooseSlot(-1); }
        public void Head() { ChooseSlot(0); }
        public void Body() { ChooseSlot(1); }
        public void Cape() { ChooseSlot(2); }
        public void Previous() { Page(-1); }
        public void Next() { Page(1); }
        private void ChooseSlot(int slot) { _slot = slot; _first = 0; Refresh(); }
        private int Count => _slot < 0 ? _session.catalog.Weapons.Count : _session.catalog.Armor.Count / 3;
        private void Page(int direction) { if (_session == null) { return; } _first = (_first + direction + Count) % Count; Refresh(); }

        private void LateUpdate()
        {
            _session ??= ArenaSession.Instance;
            if (_session == null || _session.Player == null) { return; }
            var player = _session.Player;
            int revision = HashCode.Combine(player.weapon.id, player.armor[0].id, player.armor[1].id, player.armor[2].id, player.isFemale, _session.TrainingOffset);
            if (_revision == revision) { return; }
            _revision = revision;
            Refresh();
        }

        public void Select(int index)
        {
            if (_session == null) { return; }
            if (_slot < 0) { _session.ChangeWeapon(index - _session.SelectedWeapon); }
            else { _session.ChangeArmorSlot(_slot, index - _session.Player.armor[_slot].setId); }
            Refresh();
        }

        private Sprite Icon(int id)
        {
            var entry = Array.Find(icons, icon => icon.gearId == id);
            return entry == null ? null : _session.PlayerIsFemale ? entry.female : entry.male;
        }

        private void Refresh()
        {
            if (_session == null || _session.Player == null) { return; }
            var player = _session.Player;
            nickname.text = "OVERDOX";
            health.text = player.MaxHealth.ToString("0");
            damage.text = player.AttackDamage.ToString("0");
            int totalWeight = player.weapon.weight;
            for (int slot = 0; slot < 3; slot++) { totalWeight += player.armor[slot].weight; equippedIcons[slot].sprite = Icon(player.armor[slot].id); }
            equippedIcons[3].sprite = Icon(player.weapon.id);
            weight.text = totalWeight.ToString();
            for (int cardIndex = 0; cardIndex < cards.Length; cardIndex++)
            {
                int index = (_first + cardIndex) % Count;
                GearRow row = _slot < 0 ? _session.catalog.Weapons[index] : _session.catalog.GetArmor(index, _slot);
                bool selected = _slot < 0 ? player.weapon.id == row.id : player.armor[_slot].id == row.id;
                int level = Mathf.Clamp(row.startLevel + _session.TrainingOffset, 1, row.maxLevel);
                float value = _slot < 0 ? _session.originalData.WeaponDamage(row, level) : _session.originalData.ArmorHealth(row, level);
                cards[cardIndex].Bind(this, index, row, Icon(row.id), selected, _slot < 0, level, value);
            }
        }
    }
}
