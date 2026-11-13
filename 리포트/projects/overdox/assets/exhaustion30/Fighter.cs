using System;
using System.Collections.Generic;
using UnityEngine;

namespace Overdox
{
    /// <summary>
    /// 스태미나, 공격 단계, 방향 가드, 패링과 장비 효과를 실행한다.
    /// </summary>
    public sealed partial class Fighter : MonoBehaviour
    {
        public bool isPlayer;
        public bool isFemale;
        public int trainingOffset;
        public EquipmentEffects Effects { get; } = new();
        public float HealReadyAt { get; private set; }
        public int Credits { get; private set; }
        public string displayName;
        public GearCatalog catalog;
        public Transform visual, weaponMount;
        public GearRow weapon;
        public GearRow[] armor = new GearRow[3];
        public CombatState State { get; private set; }
        public float Health { get; private set; }
        public float Stamina { get; private set; }
        public float MaxHealth { get; private set; }
        public float MaxStamina { get; private set; }
        public float SkillReadyAt { get; private set; }
        public float DashRemaining => Mathf.Max(0, _dashReady - Time.time);
        public float DashCooldownFraction => Mathf.Clamp01(DashRemaining / _dashCooldown);
        public int MaxSkillCharges { get; private set; } = 1;
        public int SkillCharges => Mathf.FloorToInt(_skillCharge / _skillChargeTime);
        public float SkillChargeFraction => Mathf.Clamp01(_skillCharge / _skillChargeTime);
        private float _skillCharge, _skillChargeTime=45;
        public int Kills { get; private set; }
        public int WeaponLevel { get; private set; } = 1;
        public float MoveSpeed { get; private set; }
        public float AttackDamage { get; private set; }
        public float CriticalChance { get; private set; }
        public float CriticalDamage { get; private set; }
        public float GuardRange { get; private set; }
        public float CriticalResistance { get; private set; }
        private float _postureRecovery,_damageEffectResistance,_conditionRecovery,_parryRecovery;
        public Vector3 MoveIntent { get; set; }
        public Vector3 AimIntent { get; set; }
        public bool GuardIntent { get; set; }
        public bool AttackIntent { get; set; }
        public bool DashIntent { get; set; }
        public bool SkillIntent { get; set; }
        public bool IsAlive => State != CombatState.Dead;
        /// <summary>
        /// 지속 시간이 없는 사망 상태에서도 상태 진입 이후의 실제 경과 시간을 조회한다.
        /// </summary>
        public float StateElapsed => Mathf.Max(0, Time.time - _stateStart);
        public float StateProgress => _stateDuration > 0 ? Mathf.Clamp01((Time.time - _stateStart) / _stateDuration) : 0;
        public float AttackElapsed => Time.time - _lastAttack;
        public int ComboIndex => _combo;
        public bool IsSkillAttack => _skillAttack;
        public float AnimationRate { get; private set; } = 1;
        public event Action<Fighter, string, float> Impact;
        private readonly HashSet<Fighter> _hit = new();
        private readonly Dictionary<Fighter, int> _hitSlots = new();
        private CharacterController _controller;
        private float _stateStart, _stateDuration, _guardStart, _lastAttack, _lastSpend, _lastDamage, _dashReady;
        private float _statusUntil, _nextStatusTick, _stunUntil, _defense;
        private string _status;
        private int _combo;
        private bool _skillAttack;
        private Vector3 _dashDirection;
        private Transform _torso, _armL, _armR, _legL, _legR;
        private Quaternion _armBaseL, _armBaseR;
        private int _brand = -1;
        private Renderer[] _renderers;
        private RestoredFighterVisual _restoredVisual;
        private float _windupDuration, _activeDuration, _recoveryDuration;
        private float _normalSpeed, _guardSpeed, _dashDistance = 4.08f, _dashCooldown = 1.15f;
        private float _regenNormal = 60, _regenCombat = 35, _regenGuard = 30, _nextOrbAt;
        private int _killLevelStacks, _killHealthStacks, _killCritStacks;
        private int _deathHealthStacks;
        private float _configuredHealth, _configuredDamage;
        private int _configuredLevel;
        private readonly StatusCollection _conditions=new();
        public bool HasCondition(int type) => _conditions.TryActive(type,Time.time,out _);
        public int ConditionStacks(int type) => _conditions.PendingStacks(type,Time.time);

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (!IsAlive) { return; }
            float dt = Time.deltaTime;
            if(ArenaSession.Instance!=null&&ArenaSession.Instance.IsFighting) { _skillCharge=Mathf.Min(MaxSkillCharges*_skillChargeTime,_skillCharge+dt); }
            bool inCombat = Time.time - Mathf.Max(_lastDamage, _lastAttack) < 5;
            if (Time.time - _lastSpend > .7f)
            {
                float regen = State == CombatState.Guard ? _regenGuard : inCombat ? _regenCombat : _regenNormal;
                // 탈진은 세 자세의 회복량에 공통 적용한다. 장비 회복 보너스와 별도 배율이다.
                float recoveryMultiplier = _conditions.StaminaRecoveryMultiplier(Time.time);
                Stamina = Mathf.Min(MaxStamina, Stamina + dt * regen * (1 + Effects.Sum(18)) * recoveryMultiplier);
            }
            if (!inCombat&&!HasCondition(3)) { Health = Mathf.Min(MaxHealth, Health + dt * Effects.Sum(26)); }
            if (Effects.Sum(41) > 0 && Time.time >= _nextOrbAt)
            {
                int count = 0;
                foreach (var entry in Effects.Entries) { if (entry.type == 41) { count++; } }
                _nextOrbAt = Time.time + 20;
                MaxHealth += 20 * count; Health += 20 * count;
            }
            TickStatus();
            TickSpecialEffects();
            TickState();
            if (ArenaSession.Instance == null || !ArenaSession.Instance.IsFighting) { Animate(); return; }
            if (CombatRules.CanAct(State))
            {
                if (DashIntent && !HasCondition(25) && Time.time >= _dashReady && Spend(22))
                {
                    _dashDirection = MoveIntent.sqrMagnitude > .01f ? MoveIntent.normalized : transform.forward;
                    _dashReady = Time.time + _dashCooldown * (HasBuffType(11) ? 1 + BuffValue(11) : 1);
                    SetState(CombatState.Dash, .24f);
                    ArenaSession.Instance.Record("dash");
                }
                else if (SkillIntent && SkillCharges>0 && Time.time >= SkillReadyAt && Spend(AttackCost(true)))
                {
                    _skillCharge-=_skillChargeTime;
                    SkillReadyAt = Time.time + weapon.cooldown * Mathf.Max(.1f, 1 - Effects.Sum(32));
                    if (!ExecuteUtilitySpecial()) { BeginAttack(true); }
                    ArenaSession.Instance.RecordAction("skill",this);
                }
                else if (AttackIntent && Spend(AttackCost(false))) { BeginAttack(false); }
                else if (GuardIntent && Stamina > 0)
                {
                    if (State != CombatState.Guard) { _guardStart = Time.time; SetState(CombatState.Guard, 0); }
                }
                else if (State == CombatState.Guard) { SetState(CombatState.Idle, 0); }
            }
            if (State == CombatState.Dash) { Move(_dashDirection * (_dashDistance / .24f) * dt); }
            else if (State == CombatState.Idle || State == CombatState.Guard)
            {
                float slow = _conditions.MovementMultiplier(Time.time);
                float speed = State == CombatState.Guard ? _guardSpeed : inCombat ? MoveSpeed : _normalSpeed;
                speed *= SpecialMovementMultiplier;
                Move(Vector3.ClampMagnitude(MoveIntent, 1) * speed * slow * dt);
            }
            else if (State == CombatState.Active) { Move(transform.forward * dt * (_skillAttack ? 4.5f : 1.5f)); }
            if (State != CombatState.Active && State != CombatState.Stunned && State != CombatState.Dash && AimIntent.sqrMagnitude > .01f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(AimIntent), dt * 850);
            }
            DashIntent = false;
            SkillIntent = false;
            Animate();
        }

        public void Configure(GearRow selectedWeapon, GearRow[] selectedArmor, GameObject body, GameObject blade)
        {
            weapon = selectedWeapon;
            ResetSpecialEffects();
            armor = selectedArmor;
            foreach (Transform child in visual) { Destroy(child.gameObject); }
            if (ArenaSession.Instance != null && ArenaSession.Instance.restoredLibrary != null)
            {
                var restored = new GameObject("Restored original character");
                restored.transform.SetParent(visual, false);
                _restoredVisual = restored.AddComponent<RestoredFighterVisual>();
                _restoredVisual.Configure(ArenaSession.Instance.restoredLibrary, this, selectedWeapon, selectedArmor, isFemale);
            }
            else
            {
            var avatar = Instantiate(body, visual).transform;
            avatar.localPosition = Vector3.zero;
            avatar.localRotation = Quaternion.Euler(0, 180, 0) * body.transform.localRotation;
            _torso = Find(avatar, "Torso");
            _armL = Find(avatar, "ArmL");
            _armR = Find(avatar, "ArmR");
            _legL = Find(avatar, "LegL");
            _legR = Find(avatar, "LegR");
            _armBaseL = _armL != null ? _armL.localRotation : Quaternion.identity;
            _armBaseR = _armR != null ? _armR.localRotation : Quaternion.identity;
            weaponMount = Find(avatar, "WeaponSocket") ?? avatar;
            var weaponObject = Instantiate(blade, weaponMount).transform;
            weaponObject.localPosition = Vector3.zero;
            weaponObject.localRotation = Quaternion.Euler(85, 0, 0) * blade.transform.localRotation;
            weaponObject.localScale = Vector3.one;
            }
            var database = ArenaSession.Instance != null ? ArenaSession.Instance.originalData : null;
            if(database!=null) { _conditions.stackDecaySeconds=database.Factors.WaittimeConditionStackInitialization; }
            MaxHealth = database != null ? 30 : 360;
            MaxStamina = database != null ? 5 : 100;
            Effects.Build(weapon, armor);
            WeaponLevel = Mathf.Clamp(weapon.startLevel + trainingOffset, weapon.startLevel, weapon.maxLevel);
            AttackDamage = database != null ? database.WeaponDamage(weapon, WeaponLevel) : weapon.damage;
            _defense = 0;
            CriticalResistance=Effects.Sum(1);_postureRecovery=0;_damageEffectResistance=Effects.Sum(10);_conditionRecovery=Effects.Sum(17);_parryRecovery=Effects.Sum(13);
            int weight = weapon.weight;
            _brand = armor[0].brand == armor[1].brand && armor[1].brand == armor[2].brand ? armor[0].brand : -1;
            foreach (var piece in armor)
            {
                int level = Mathf.Min(piece.maxLevel, piece.startLevel + trainingOffset);
                MaxHealth += database != null ? database.ArmorHealth(piece, level) : piece.health;
                MaxStamina += database != null ? database.ArmorStamina(piece, level) : piece.stamina;
                if(database!=null)
                {
                    var source=database.Armor[piece.id];
                    CriticalResistance+=source.CriticaltResist+database.TrainingBonus(source.CriticaltResistTrainingConstantsId,level);
                    _postureRecovery+=source.FastPostureRecovery+database.TrainingBonus(source.FastPostureRecoveryTrainingConstantsId,level);
                    _damageEffectResistance+=source.DamageEffectResist+database.TrainingBonus(source.DamageEffectResistTrainingConstantsId,level);
                    _conditionRecovery+=source.ConditionRecorvery;_parryRecovery+=source.BeParriedRecorvery;
                }
                _defense += piece.defense;
                weight += piece.weight;
            }
            MoveSpeed = CombatRules.MoveSpeed(weight);
            MaxHealth = (MaxHealth + Effects.Sum(4)) * (1 + Effects.Sum(6));
            MaxStamina = (MaxStamina + Effects.Sum(2)) * (1 + Effects.Sum(12));
            if (Effects.Sum(20) > 0) { MaxHealth = Effects.Sum(20); }
            AttackDamage = (AttackDamage + Effects.Sum(33)) * (1 + Effects.Sum(59));
            CriticalChance = weapon.criticalChance;
            CriticalDamage = weapon.criticalDamage;
            GuardRange = weapon.guardRange + Effects.Sum(49);
            if (database != null)
            {
                var gear = database.Gear[weapon.id];
                _skillChargeTime=Mathf.Max(1,gear.SpecialSkillChargeTime);
                MaxSkillCharges=database.MaxSkillCharges(weapon,WeaponLevel);
                CriticalChance += database.TrainingBonus(gear.CriticalChanceTrainingConstantsId, WeaponLevel);
                CriticalDamage += database.TrainingBonus(gear.CriticalDamageRateTrainingConstantsId, WeaponLevel);
                GuardRange += database.TrainingBonus(gear.GuardRangeTrainingConstantsId, WeaponLevel);
                var character = database.PlayerCharacter(isFemale);
                CriticalResistance=Mathf.Min(database.Factors.CharacterCriticalresistMax,CriticalResistance);
                _postureRecovery=Mathf.Min(database.Factors.CharacterDamageeffectrecoveryMax,_postureRecovery);
                _damageEffectResistance=Mathf.Min(database.Factors.CharacterDamageeffectresistMax,_damageEffectResistance);
                var alteration = database.Weight(weight * (1 - Effects.Sum(16)));
                MoveSpeed = character.SpeedCombat * alteration.SpeedCombatAlteration * (1 + Effects.Sum(3) + Effects.Sum(7));
                _normalSpeed = character.SpeedNormal * alteration.SpeedNormalAlteration * (1 + Effects.Sum(3) + Effects.Sum(15));
                _guardSpeed = character.SpeedGuard * (1 + Effects.Sum(3));
                _regenNormal = character.StaminaIncreaseNormal * (1 + Effects.Sum(23));
                _regenCombat = character.StaminaIncreaseCombat * (1 + Effects.Sum(22));
                _regenGuard = character.StaminaIncreaseGuard;
                _dashDistance = 4.08f * alteration.DashDistanceAlteration * (1 + Effects.Sum(14));
                _dashCooldown = 1.15f * Mathf.Max(.1f,1-Effects.Sum(19)) / Mathf.Max(.1f, alteration.DashCoolTimeAlteration);
            }
            else { _normalSpeed = MoveSpeed; _guardSpeed = MoveSpeed * .38f; }
            Credits = Mathf.RoundToInt(Effects.Sum(34));
            _nextOrbAt = Time.time + 20;
            Health = MaxHealth;
            Stamina = MaxStamina;
            _configuredHealth = MaxHealth; _configuredDamage = AttackDamage; _configuredLevel = WeaponLevel;
            _skillCharge=MaxSkillCharges*_skillChargeTime;
            SetState(CombatState.Idle, 0);
        }

        public void Respawn(Vector3 position)
        {
            _controller.enabled = false;
            transform.position = position;
            _controller.enabled = true;
            MaxHealth = _configuredHealth; AttackDamage = _configuredDamage; WeaponLevel = _configuredLevel;
            Kills = 0; _killLevelStacks = _killHealthStacks = _killCritStacks = _deathHealthStacks = 0;
            Health = MaxHealth;
            Stamina = MaxStamina;
            _status = null;
            _statusUntil = 0;
            ResetSpecialEffects();
            _conditions.Clear();
            _lastAttack = float.NegativeInfinity;
            SkillReadyAt = 0;
            _skillCharge=MaxSkillCharges*_skillChargeTime;
            _combo = 0;
            _dashReady = 0;
            HealReadyAt=0;_nextOrbAt=Time.time+20;
            GuardIntent = AttackIntent = SkillIntent = DashIntent = false;
            SetState(CombatState.Idle, 0);
            visual.localRotation = Quaternion.identity;
            visual.localScale = Vector3.one;
        }

        public void Receive(Fighter attacker, float damage, float staminaDamage, string status = null, bool canGuard = true, bool canParry = true, int damageEffect = 0, bool? skillDamage = null)
        {
            if (!IsAlive || State == CombatState.Dash) { return; }
            bool special = skillDamage ?? (attacker != null && attacker.IsSkillAttack);
            if (IsInvulnerable(attacker, special)) { return; }
            bool facing = attacker != null && CombatRules.InArc(transform.position, transform.forward, attacker.transform.position, 6, GuardRange*.5f);
            bool guarding = State == CombatState.Guard && facing && canGuard;
            if (guarding && canParry && Time.time - _guardStart <= weapon.parryTime && Stamina >= 8)
            {
                Stamina = Mathf.Min(MaxStamina, Stamina + 12);
                if (attacker != null) { attacker.Stun((ArenaSession.Instance.originalData?.Factors.ParryDownDuration??1) * Mathf.Clamp01(1 - attacker._parryRecovery)); }
                Impact?.Invoke(this, "PARRY", 0);
                ArenaSession.Instance.Record("parry");
                return;
            }
            if (guarding)
            {
                if (HasBuffType(51)) { staminaDamage /= 1 + BuffValue(51); }
                Stamina = Mathf.Max(0, Stamina - staminaDamage);
                _lastSpend = Time.time;
                ArenaSession.Instance.Record("block");
                if (Stamina <= 0) { Stun(ArenaSession.Instance.originalData?.Factors.GuardbreakDownDuration??.5f); Impact?.Invoke(this, "BREAK", 0); }
            }
            float actual = CombatRules.Mitigate(damage, _defense, guarding) * Mathf.Clamp01(1 - Effects.Sum(35));
            if(_conditions.TryActive(3,Time.time,out var bleed)&&attacker!=null) { actual*=1+bleed.Variable1*(1+attacker.Effects.Sum(53)); }
            if (attacker != null && !special && HasBuffType(52)) { actual *= BuffValue(52, true); }
            if (attacker != null && !special) { actual *= 1 - Effects.ComboReduction(attacker.ComboIndex); }
            Health = Mathf.Max(0, Health - actual);
            _lastDamage = Time.time;
            if (attacker != null && !special && HasBuffType(53)) { attacker.Receive(null, actual * BuffValue(53), 0); }
            ArenaSession.Instance.Record("hit");
            Impact?.Invoke(this, guarding ? "BLOCK" : "HIT", actual);
            if (!guarding && attacker != null)
            {
                if (status == "Stun" || status == "Shock") { Stun(.65f); }
                else if (!Effects.ImmuneDamageEffect(damageEffect)&&UnityEngine.Random.value>=_damageEffectResistance) { Stun((damageEffect == 2 ? 1.2f : damageEffect == 6 ? .8f : .12f) * Mathf.Clamp01(1 - _postureRecovery)); }
                if (!string.IsNullOrEmpty(status) && !(Effects.Sum(36) > 0 && status == "Burn") && !(Effects.Sum(37) > 0 && status == "Bleed"))
                {
                    _status = status;
                    _statusUntil = Time.time + 3;
                    _nextStatusTick = Time.time + .5f;
                }
                Vector3 push = transform.position - attacker.transform.position;
                push.y = 0;
                if (!Effects.ImmuneDamageEffect(damageEffect)) { Move(push.normalized * (damageEffect == 1 || damageEffect == 6 ? .9f : .17f)); }
            }
            if (Health <= 0)
            {
                SetState(CombatState.Dead, 0);
                ArenaSession.Instance.Record("death");
                foreach (var survivor in ArenaSession.Instance.Fighters)
                {
                    if (survivor.IsAlive && survivor.Effects.Sum(47) > 0 && survivor._deathHealthStacks < 10)
                    {
                        survivor._deathHealthStacks++;
                        survivor.MaxHealth += survivor.Effects.Sum(47);
                        survivor.Health += survivor.Effects.Sum(47);
                    }
                }
                if (attacker != null)
                {
                    attacker.Kills++;
                    attacker.OnKill();
                }
                Impact?.Invoke(this, "ELIMINATED", 0);
            }
        }

        public void Heal(float amount)
        {
            if (!IsAlive||HasCondition(3)) { return; }
            amount *= 1 + Effects.Sum(50) / 100;
            Health = Mathf.Min(MaxHealth, Health + amount);
            Impact?.Invoke(this, "RECOVER", amount);
        }

        public void UseRecovery()
        {
            if (Time.time < HealReadyAt || !IsAlive) { return; }
            HealReadyAt = Time.time + 20;
            Heal(150);
        }

        private void OnKill()
        {
            Credits += 100;
            if (Effects.Sum(44) > 0 && _killLevelStacks++ < 3)
            {
                WeaponLevel = Mathf.Min(weapon.maxLevel, WeaponLevel + Mathf.RoundToInt(Effects.Sum(44)));
                var database = ArenaSession.Instance.originalData;
                AttackDamage = ((database != null ? database.WeaponDamage(weapon, WeaponLevel) : weapon.damage) + Effects.Sum(33)) * (1 + Effects.Sum(59));
            }
            if (Effects.Sum(45) > 0 && _killHealthStacks++ < 3) { MaxHealth += Effects.Sum(45); Heal(Effects.Sum(45)); }
            if (Effects.Sum(46) > 0 && _killCritStacks < 3) { _killCritStacks++; }
        }

        private int NextCombo()
        {
            var session=ArenaSession.Instance;
            var factors=session?.originalData?.Factors;
            float timeout=factors!=null?factors.ComboInitializeNonTargetTime:1.3f;
            if(factors!=null)
            {
                foreach(var fighter in session.Fighters)
                {
                    if(fighter!=this&&fighter.IsAlive&&(fighter.transform.position-transform.position).sqrMagnitude<=factors.TargetDecisionDistance*factors.TargetDecisionDistance) { timeout=factors.ComboInitializeTime;break; }
                }
            }
            return Time.time-_lastAttack>timeout?0:(_combo+1)%Mathf.Max(1,weapon.maxCombo);
        }
        private float AttackCost(bool special)
        {
            var database = ArenaSession.Instance.originalData;
            return database != null ? database.GetSkill(weapon, NextCombo(), special).UseStamina : special ? 32 : weapon.stamina;
        }

        public void Stun(float duration)
        {
            if (!IsAlive || State == CombatState.Dash || HasBuffType(31)) { return; }
            _stunUntil = Time.time + duration;
            SetState(CombatState.Stunned, duration);
        }

        private void BeginAttack(bool skill)
        {
            _combo = NextCombo();
            if(_conditions.TryActive(4,Time.time,out var shock)&&UnityEngine.Random.value<shock.Variable2)
            {
                Receive(null,AttackDamage*shock.Variable1,0);
                if(!IsAlive) { return; }
            }
            _skillAttack = skill;
            if (skill) { ApplyReferencedConditions(ArenaSession.Instance.originalData?.GetSkill(weapon, _combo, true), this, true); }
            AnimationRate = !skill && (weapon.family == 3 || weapon.family == 4) ? 1 + Effects.Sum(38 + Mathf.Min(2, _combo)) : 1;
            _lastAttack = Time.time;
            _hit.Clear();
            _hitSlots.Clear();
            _windupDuration = weapon.duration * (skill ? .55f : .30f);
            _activeDuration = weapon.duration * .28f;
            _recoveryDuration = weapon.duration * .42f;
            if (_restoredVisual != null && _restoredVisual.GetAttackTiming(_combo, skill, out float windup, out float active, out float recovery))
            {
                _windupDuration = windup;
                _activeDuration = active;
                _recoveryDuration = recovery;
            }
            _windupDuration /= AnimationRate; _activeDuration /= AnimationRate; _recoveryDuration /= AnimationRate;
            SetState(CombatState.Windup, _windupDuration);
            ArenaSession.Instance.RecordAction("attack",this);
        }

        private void TickState()
        {
            if (State == CombatState.Active)
            {
                int hitSlot = _restoredVisual != null ? _restoredVisual.ActiveHitSlot(AttackElapsed * AnimationRate) : 1;
                float range = weapon.reach * (_skillAttack ? 1.55f : 1);
                float angle = _skillAttack ? 175 : weapon.family == 5 || weapon.family == 6 ? 40 : 75;
                foreach (var other in ArenaSession.Instance.Fighters)
                {
                    if (hitSlot == 0 || other == this || !other.IsAlive) { continue; }
                    int mask = 1 << hitSlot;
                    if (_hitSlots.TryGetValue(other, out int hitMask) && (hitMask & mask) != 0) { continue; }
                    bool? touching=!_skillAttack&&_restoredVisual!=null?_restoredVisual.WeaponTouches(other,_combo,hitSlot):null;
                    if(touching.HasValue) { if(!touching.Value) { continue; } }
                    else if (!CombatRules.InArc(transform.position, transform.forward, other.transform.position, range, angle)) { continue; }
                    _hit.Add(other);
                    _hitSlots[other] = hitMask | mask;
                    float damage = AttackDamage * (1 + _combo * .12f) * (_skillAttack ? 1.9f : 1);
                    damage *= SpecialDamageMultiplier;
                    float staminaDamage = weapon.stamina * 1.5f;
                    bool canGuard = true, canParry = true;
                    int damageEffect = 0;
                    Overdox.OriginalData.ConditionEffect condition = null;
                    int conditionStacks=1;
                    if (ArenaSession.Instance.originalData != null)
                    {
                        var database = ArenaSession.Instance.originalData;
                        var skill = database.GetSkill(weapon, _combo, _skillAttack);
                        int hitId = hitSlot switch { 2 => skill.Hit2, 3 => skill.Hit3, 4 => skill.Hit4, _ => skill.Hit1 };
                        if (hitId == 0) { continue; }
                        if (database.Hits.TryGetValue(hitId, out var hit))
                        {
                            damage = AttackDamage * SpecialDamageMultiplier * hit.DamageVariable + Mathf.Min(other.MaxHealth * hit.AdditionalDamageVariable, hit.AdditionalDamageLimit);
                            staminaDamage = hit.StaminaDamage * (1 + Effects.Sum(51));
                            canGuard = hit.CanGuard != 0; canParry = hit.CanParrying != 0;
                            damageEffect = hit.DamageEffectIndex;
                            database.Conditions.TryGetValue(hit.ConditionEffectIndex, out condition);
                            conditionStacks=Mathf.Max(1,hit.ConditionEffectParameter1);
                        }
                    }
                    damage *= 1 + Effects.Sum(31) + Effects.Sum(28 + Mathf.Min(2, _combo));
                    if (Health < 500) { damage *= 1 + Effects.Sum(25); }
                    damage += Effects.Sum(52);
                    float chance = Mathf.Max(0, CriticalChance + Effects.Sum(9) + Effects.Sum(46) * _killCritStacks + BuffValue(35) - other.CriticalResistance);
                    var factors=ArenaSession.Instance.originalData.Factors;
                    chance=Mathf.Min(factors.CharacterCriticalchanceMax,chance);
                    if (UnityEngine.Random.value < chance) { damage *= 1 + Mathf.Clamp(CriticalDamage + Effects.Sum(5) - other.Effects.Sum(62),0,factors.CharacterCriticaldamageMax); }
                    float beforeHealth = other.Health;
                    if (other.HasCondition(2)) { damage *= 1 + Effects.Sum(54); }
                    other.Receive(this, damage, staminaDamage, null, canGuard, canParry, damageEffect);
                    float dealt = Mathf.Max(0, beforeHealth - other.Health);
                    if (condition != null && other.IsAlive && dealt > 0) { other.ApplyCondition(condition,conditionStacks); }
                    if (_skillAttack && dealt > 0) { ApplyReferencedConditions(ArenaSession.Instance.originalData.GetSkill(weapon, _combo, true), other, false); }
                    float drain = Effects.Lifesteal(Health);
                    if (drain > 0 && dealt > 0) { Heal(dealt * drain); }
                    ArenaSession.Instance.SpawnSlash(transform.position + transform.forward * .7f, transform.rotation, _skillAttack);
                }
            }
            if (_stateDuration <= 0 || Time.time - _stateStart < _stateDuration) { return; }
            switch (State)
            {
                case CombatState.Windup: { SetState(CombatState.Active, _activeDuration); ArenaSession.Instance.SpawnSlash(transform.position + transform.forward, transform.rotation, _skillAttack); break; }
                case CombatState.Active: { SetState(CombatState.Recovery, _recoveryDuration); break; }
                case CombatState.Recovery:
                case CombatState.Dash:
                case CombatState.Stunned: { SetState(CombatState.Idle, 0); break; }
            }
        }

        private void TickStatus()
        {
            float burn=_conditions.BurnTick(Time.time);
            if(burn>0) { Receive(null,burn,0); }
            if (Time.time >= _statusUntil) { _status = null; return; }
            if (_status == "Root") { MoveIntent = Vector3.zero; }
            if (Time.time >= _nextStatusTick && (_status == "Burn" || _status == "Bleed" || _status == "Poison"))
            {
                _nextStatusTick = Time.time + .5f;
                Receive(null, _status == "Burn" ? 12.5f : 8, 0);
            }
        }

        /// <summary>
        /// 지원하는 상태의 스택을 축적하고 발동 시 기존 피격 알림과 기록으로 전달한다.
        /// 화상·출혈 면역을 먼저 적용하며 지속 시간과 만료는 상태 컬렉션이 관리한다.
        /// </summary>
        public void ApplyCondition(Overdox.OriginalData.ConditionEffect condition, int count = 1)
        {
            string label;
            switch (condition.ConditionType)
            {
                case 1:
                {
                    if (Effects.Sum(36) > 0) { return; }
                    label = "화상";
                    break;
                }
                case 2:
                {
                    label = "동상";
                    break;
                }
                case 3:
                {
                    if (Effects.Sum(37) > 0) { return; }
                    label = "출혈";
                    break;
                }
                case 4:
                {
                    label = "감전";
                    break;
                }
                case 25:
                {
                    label = "속박";
                    break;
                }
                case 60:
                {
                    label = "탈진";
                    break;
                }
                default:
                {
                    return;
                }
            }
            if (_conditions.Add(condition, count, Time.time, _conditionRecovery))
            {
                Impact?.Invoke(this, label, 0);
                ArenaSession.Instance.Record("condition-" + condition.ConditionType);
            }
        }

        private bool Spend(float value)
        {
            if (Stamina < value) { return false; }
            Stamina -= value;
            _lastSpend = Time.time;
            return true;
        }

        private void Move(Vector3 movement)
        {
            if(State==CombatState.Dash&&HasBuffType(56)) { movement.y=0;transform.position+=movement;return; }
            if (_controller != null && _controller.enabled) { _controller.Move(movement + Vector3.down * .1f); }
        }

        private void SetState(CombatState state, float duration)
        {
            State = state;
            _stateStart = Time.time;
            _stateDuration = duration;
        }

        private void Animate()
        {
            if (_restoredVisual != null) { return; }
            if (_armR == null) { return; }
            float walk = Mathf.Sin(Time.time * 12) * Mathf.Clamp01(MoveIntent.magnitude) * (State == CombatState.Idle ? 32 : 5);
            if (_legL != null) { _legL.localRotation = Quaternion.Euler(walk, 0, 0); }
            if (_legR != null) { _legR.localRotation = Quaternion.Euler(-walk, 0, 0); }
            _armL.localRotation = _armBaseL * Quaternion.Euler(-walk * .6f, 0, 0);
            _armR.localRotation = _armBaseR * Quaternion.Euler(walk * .5f, 0, 0);
            if (State == CombatState.Windup) { _armR.localRotation = _armBaseR * Quaternion.Euler(-65, 0, -80 * StateProgress); }
            if (State == CombatState.Active) { _armR.localRotation = _armBaseR * Quaternion.Euler(-70 + 125 * StateProgress, 0, -80 + 150 * StateProgress); }
            if (State == CombatState.Guard) { _armR.localRotation = _armBaseR * Quaternion.Euler(-80, 0, -28); _armL.localRotation = _armBaseL * Quaternion.Euler(-75, 0, 30); }
            if (State == CombatState.Dead) { visual.localRotation = Quaternion.Euler(80, 0, 0); }
            if (State == CombatState.Dash) { visual.localScale = new Vector3(1, .8f, 1.1f); }
            else { visual.localScale = Vector3.one; }
        }

        private static Transform Find(Transform root, string name)
        {
            foreach (var item in root.GetComponentsInChildren<Transform>())
            {
                if (item.name == name) { return item; }
            }
            return null;
        }
    }
}
