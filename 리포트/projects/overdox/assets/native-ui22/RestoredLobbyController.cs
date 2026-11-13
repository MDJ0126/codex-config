using System;
using UnityEngine;
using UnityEngine.UI;

namespace Overdox
{
    /// <summary>
    /// 공통 장비 ID에 남녀 표시용 아이콘을 연결한다.
    /// </summary>
    [Serializable]
    public sealed class RestoredGearIcon
    {
        public int gearId;
        public Sprite male, female;
    }

    /// <summary>
    /// 씬에 저장한 원작 UI에 현재 장비 데이터와 화면 전환 명령을 연결한다.
    /// </summary>
    public sealed class RestoredLobbyController : MonoBehaviour
    {
        public GameObject home, equipment, profile, back;
        public Text windowTitle, nickname, health, damage, weight;
        public RestoredGearCard[] cards;
        public Image[] equippedIcons;
        public RestoredGearIcon[] icons;
        public RestoredLobbyPresentation presentation;
        public RestoredEquipmentCarousel carousel;
        private ArenaSession _session;
        // -1은 무기, 0~2는 머리·몸통·망토 슬롯이다. 원작 EquipParts 값과 직접 혼용하지 않는다.
        private int _slot = -1, _first;
        private int _revision = int.MinValue;

        // 방어구는 머리·몸통·망토가 같은 세트 수로 구성된 현재 카탈로그를 전제로 한다.
        private int Count => _slot < 0 ? _session.catalog.Weapons.Count : _session.catalog.Armor.Count / 3;
        /// <summary>
        /// 실제 장착 상태와 별개로 목록 중앙에서 열람 중인 카탈로그 인덱스를 조회한다.
        /// </summary>
        public int FocusedIndex => _first;

        /// <summary>
        /// 세션과 플레이어가 준비되면 선택 장비·성별·강화 단계 변경 시 표시를 갱신한다.
        /// </summary>
        private void LateUpdate()
        {
            _session ??= ArenaSession.Instance;
            if (_session == null || _session.Player == null)
            {
                return;
            }
            var player = _session.Player;
            int revision = HashCode.Combine(player.weapon.id, player.armor[0].id, player.armor[1].id, player.armor[2].id, player.isFemale, _session.TrainingOffset);
            if (_revision == revision)
            {
                return;
            }
            bool isInitial = _revision == int.MinValue;
            _revision = revision;
            if (isInitial)
            {
                ChooseSlot(_slot);
            }
            else
            {
                Refresh();
            }
        }

        /// <summary>
        /// 장비 화면을 표시하고 장비용 카메라·배경 전환을 시작한다.
        /// </summary>
        public void OpenEquipment()
        {
            home.SetActive(false);
            equipment.SetActive(true);
            profile.SetActive(false);
            back.SetActive(true);
            windowTitle.text = "캐릭터";
            presentation?.Transition(true);
            ChooseSlot(_slot);
        }

        /// <summary>
        /// 홈 화면을 표시하고 로비용 카메라·배경 전환을 시작한다.
        /// </summary>
        public void OpenHome()
        {
            home.SetActive(true);
            equipment.SetActive(false);
            profile.SetActive(true);
            back.SetActive(false);
            presentation?.Transition(false);
        }

        /// <summary>
        /// 준비된 세션에 현재 선택 장비로 경기 시작을 요청한다.
        /// </summary>
        public void Battle()
        {
            _session?.StartMatch();
        }

        /// <summary>
        /// 캐릭터 성별을 전환하고 장비 아이콘을 다시 연결한다.
        /// </summary>
        public void Gender()
        {
            _session?.ToggleGender();
            Refresh();
        }

        /// <summary>
        /// 무기 목록의 첫 항목부터 표시한다.
        /// </summary>
        public void Weapons()
        {
            ChooseSlot(-1);
        }

        /// <summary>
        /// 머리 방어구 목록의 첫 항목부터 표시한다.
        /// </summary>
        public void Head()
        {
            ChooseSlot(0);
        }

        /// <summary>
        /// 몸통 방어구 목록의 첫 항목부터 표시한다.
        /// </summary>
        public void Body()
        {
            ChooseSlot(1);
        }

        /// <summary>
        /// 망토 목록의 첫 항목부터 표시한다.
        /// </summary>
        public void Cape()
        {
            ChooseSlot(2);
        }

        /// <summary>
        /// 목록 시작 위치를 한 항목 뒤로 이동한다.
        /// </summary>
        public void Previous()
        {
            Page(-1);
        }

        /// <summary>
        /// 목록 시작 위치를 한 항목 앞으로 이동한다.
        /// </summary>
        public void Next()
        {
            Page(1);
        }

        /// <summary>
        /// 표시 슬롯을 변경하고 목록의 시작 위치를 초기화한다.
        /// </summary>
        private void ChooseSlot(int slot)
        {
            _slot = slot;
            _first = _session == null || _session.Player == null ? 0 : slot < 0 ? _session.SelectedWeapon : _session.Player.armor[slot].setId;
            Refresh();
            carousel?.ResetTo(_first);
        }

        /// <summary>
        /// 준비된 세션의 목록을 순환하며 시작 위치를 변경한다.
        /// </summary>
        private void Page(int direction)
        {
            if (_session == null)
            {
                return;
            }
            Focus(_first + direction);
        }

        /// <summary>
        /// 열람할 카드의 중심을 변경하고 실제 장비 변경은 장착 버튼을 누를 때까지 보류한다.
        /// </summary>
        public void Focus(int index)
        {
            if (_session == null || Count <= 0)
            {
                return;
            }
            int next = Mathf.Clamp(index, 0, Count - 1);
            if (next == _first)
            {
                return;
            }
            _first = next;
            Refresh();
            Debug.Log($"OVERDOX_EQUIPMENT_FOCUS slot={_slot} index={_first}");
        }

        /// <summary>
        /// 중앙에서 열람 중인 카드를 실제 장비로 세션에 반영한다.
        /// </summary>
        public void EquipFocused()
        {
            Select(_first);
            Debug.Log($"OVERDOX_EQUIPMENT_EQUIPPED slot={_slot} index={_first}");
        }

        /// <summary>
        /// 현재 슬롯의 카탈로그 인덱스를 장착 변경 명령으로 전달한다.
        /// </summary>
        /// <param name="index">카드에 연결된 무기 인덱스 또는 방어구 세트 인덱스이다.</param>
        public void Select(int index)
        {
            if (_session == null)
            {
                return;
            }
            // 기존 세션 API는 절대 인덱스가 아닌 현재 선택에서의 차이를 받는다.
            if (_slot < 0)
            {
                _session.ChangeWeapon(index - _session.SelectedWeapon);
            }
            else
            {
                _session.ChangeArmorSlot(_slot, index - _session.Player.armor[_slot].setId);
            }
            Refresh();
        }

        /// <summary>
        /// 장비 ID와 현재 성별에 대응하는 아이콘을 조회하고 없으면 null을 반환한다.
        /// </summary>
        private Sprite Icon(int id)
        {
            var entry = Array.Find(icons, icon => icon.gearId == id);
            return entry == null ? null : _session.PlayerIsFemale ? entry.female : entry.male;
        }

        /// <summary>
        /// 현재 장비 합계와 카드 풀의 표시값을 실제 DDT 능력치로 갱신한다.
        /// </summary>
        private void Refresh()
        {
            if (_session == null || _session.Player == null)
            {
                return;
            }
            var player = _session.Player;
            nickname.text = "OVERDOX";
            health.text = player.MaxHealth.ToString("0");
            damage.text = player.AttackDamage.ToString("0");
            int totalWeight = player.weapon.weight;
            for (int slot = 0; slot < 3; slot++)
            {
                totalWeight += player.armor[slot].weight;
                equippedIcons[slot].sprite = Icon(player.armor[slot].id);
            }
            equippedIcons[3].sprite = Icon(player.weapon.id);
            weight.text = totalWeight.ToString();
            for (int cardIndex = 0; cardIndex < cards.Length; cardIndex++)
            {
                // 중앙 앞뒤의 카드만 고정 풀에 연결하며 목록 끝을 반대쪽 장비로 순환시키지 않는다.
                int index = _first + cardIndex - cards.Length / 2;
                cards[cardIndex].transform.parent.gameObject.SetActive(index >= 0 && index < Count);
                if (index < 0 || index >= Count)
                {
                    continue;
                }
                GearRow row = _slot < 0 ? _session.catalog.Weapons[index] : _session.catalog.GetArmor(index, _slot);
                bool selected = _slot < 0 ? player.weapon.id == row.id : player.armor[_slot].id == row.id;
                int level = Mathf.Clamp(row.startLevel + _session.TrainingOffset, 1, row.maxLevel);
                float value = _slot < 0 ? _session.originalData.WeaponDamage(row, level) : _session.originalData.ArmorHealth(row, level);
                cards[cardIndex].Bind(this, index, row, Icon(row.id), selected, _slot < 0, level, value);
            }
            carousel?.Bind(_first, Count);
        }
    }
}
