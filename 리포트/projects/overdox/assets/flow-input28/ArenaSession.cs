using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Overdox
{
    /// <summary>
    /// 장비 선택, 로컬 봇 경기, 축소 구역과 검증 기록을 관리한다.
    /// </summary>
    public sealed class ArenaSession : MonoBehaviour
    {
        public static ArenaSession Instance { get; private set; }
        public GearCatalog catalog;
        public RestoredVisualLibrary restoredLibrary;
        public OriginalCombatDatabase originalData;
        public Fighter fighterPrefab;
        public GameObject[] bodies, weapons;
        public string[] bodyKeys, weaponKeys;
        public Material slashMaterial;
        public Transform zoneRing;
        public GameObject dronePrefab;
        public ArenaHud hud;
        public ArenaCamera arenaCamera;
        public List<Fighter> Fighters { get; } = new();
        public bool IsFighting { get; private set; }
        public bool IsLobby { get; private set; } = true;
        public bool IsAutoplay { get; private set; }
        public Vector2 TouchMove { get; set; }
        public Fighter Player { get; private set; }
        public int SelectedWeapon { get; private set; } = 3;
        public int SelectedSet { get; private set; } = 16;
        public bool PlayerIsFemale { get; private set; }
        public int TrainingOffset { get; private set; }
        private readonly int[] _playerArmorSets = { 16, 16, 16 };
        public int Round { get; private set; }
        public float Remaining => Mathf.Max(0, 180 - (Time.time - _roundStart));
        public float ZoneRadius => Mathf.Lerp(22, 3.2f, Mathf.Clamp01((Time.time - _roundStart - 30) / 150));
        public int AliveCount { get { int count = 0; foreach (var f in Fighters) { if (f.IsAlive) { count++; } } return count; } }
        public string Message { get; private set; } = "장비를 선택하고 전장에 진입하세요";
        private readonly Dictionary<string, int> _counters = new();
        private readonly HashSet<int> _weaponsSeen = new(), _armorSeen = new(), _weaponsAttacked = new(), _weaponsSkilled = new();
        private float _roundStart, _nextZoneTick, _nextLog, _nextCapture, _startedReal;
        private bool _soak;
        private float _soakSeconds = 36000;
        private int _equipmentCycle;
        private string _evidenceDirectory;
        private Transform _drone;

        private void Awake()
        {
            Instance = this;
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            _startedReal = Time.realtimeSinceStartup;
            _nextCapture = Time.realtimeSinceStartup + 3;
            string[] args = Environment.GetCommandLineArgs();
            _soak = Array.IndexOf(args, "-soak") >= 0;
            IsAutoplay = _soak || Array.IndexOf(args, "-autoplay") >= 0;
            int duration = Array.IndexOf(args, "-duration");
            if (duration >= 0 && duration + 1 < args.Length) { float.TryParse(args[duration + 1], out _soakSeconds); }
            _evidenceDirectory = Path.Combine(Application.dataPath, "../Evidence");
            int output = Array.IndexOf(args, "-evidence");
            if (output >= 0 && output + 1 < args.Length) { _evidenceDirectory = args[output + 1]; }
            Directory.CreateDirectory(_evidenceDirectory);
            Debug.Log($"OVERDOX_SESSION_START utc={DateTime.UtcNow:O} autoplay={IsAutoplay} soak={_soak} duration={_soakSeconds} weapons={catalog.Weapons.Count} armor={catalog.Armor.Count}");
        }

        private void Start()
        {
            RestoreLoadout();
            SpawnRoster();
            hud.Bind(this);
            arenaCamera.target = Player.transform;
            Player.transform.rotation = arenaCamera.nativeLobbyPose
                ? new Quaternion(0, -.576214f, 0, .8172988f) * Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 180, 0);
            if (Array.IndexOf(Environment.GetCommandLineArgs(), "-catalog-review") >= 0) { StartCoroutine(ReviewCatalog()); }
            else if (IsAutoplay || (GameFlowController.Instance != null && GameFlowController.Instance.IsEnteringArena))
            {
                StartCoroutine(StartMatchAfterLoading());
            }
        }

        /// <summary>
        /// 초기 씬 활성화가 끝난 뒤 자동 경기를 요청하여 로딩 중 전환 거부로 시작 명령이 사라지지 않게 한다.
        /// </summary>
        private IEnumerator StartMatchAfterLoading()
        {
            // ArenaSession.Start는 LoadSceneAsync 완료보다 먼저 실행될 수 있다.
            // 준비가 끝날 때까지 이 세션에서 기다리고, 씬이 사라지면 코루틴도 함께 종료한다.
            var flow = GameFlowController.Instance;
            while (flow != null && (flow.Phase == GameFlowPhase.Splash || flow.Phase == GameFlowPhase.Loading))
            {
                yield return null;
            }
            StartMatch();
        }

        private void RestoreLoadout()
        {
            var flow = GameFlowController.Instance;
            if (flow == null) { return; }
            var selection = flow.selection;
            for (int index = 0; index < catalog.Weapons.Count; index++)
            {
                if (catalog.Weapons[index].id == selection.weaponId)
                {
                    SelectedWeapon = index;
                    break;
                }
            }
            for (int slot = 0; slot < _playerArmorSets.Length; slot++)
            {
                if (selection.armorIds == null || slot >= selection.armorIds.Length) { continue; }
                foreach (var armor in catalog.Armor)
                {
                    if (armor.id == selection.armorIds[slot] && armor.slot == slot)
                    {
                        _playerArmorSets[slot] = armor.setId;
                        break;
                    }
                }
            }
            SelectedSet = _playerArmorSets[0];
            PlayerIsFemale = selection.female;
            TrainingOffset = Mathf.Clamp(selection.trainingOffset, 0, 24);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F8)) { IsAutoplay = !IsAutoplay; }
            if (Input.GetKeyDown(KeyCode.F12)) { Capture("manual"); }
            if (Input.GetKeyDown(KeyCode.Escape)) { ToggleLobby(); }
            if (IsLobby)
            {
                if (_drone != null) { _drone.gameObject.SetActive(false); }
                if (zoneRing != null) { zoneRing.gameObject.SetActive(false); }
                if (Input.GetKeyDown(KeyCode.Return)) { StartMatch(); }
                return;
            }
            if (IsFighting)
            {
                if (Time.time >= _nextZoneTick)
                {
                    _nextZoneTick = Time.time + 1;
                    foreach (var fighter in Fighters)
                    {
                        if (fighter.IsAlive && new Vector2(fighter.transform.position.x, fighter.transform.position.z).magnitude > ZoneRadius) { fighter.Receive(null, 35, 0); }
                    }
                }
                if (zoneRing != null) { zoneRing.localScale = new Vector3(ZoneRadius, .15f, ZoneRadius); }
                if (AliveCount <= 1 || Remaining <= 0)
                {
                    IsFighting = false;
                    Message = Player.IsAlive ? "CHAMPION" : "ELIMINATED";
                    Record("round");
                    Debug.Log($"OVERDOX_ROUND_END round={Round} winner={FindWinner()} playerAlive={Player.IsAlive} remaining={Remaining:F1}");
                    Capture("result-" + Round);
                    if (IsAutoplay) { StartCoroutine(NextAutoplayRound()); }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return)) { StartMatch(); }
            if (_drone != null && Player != null)
            {
                _drone.position = Vector3.Lerp(_drone.position, Player.transform.position + new Vector3(-1.1f, 2.1f + Mathf.Sin(Time.time * 3) * .12f, -.5f), Time.deltaTime * 5);
                _drone.Rotate(0, Time.deltaTime * 20, 0);
            }
            if (Time.realtimeSinceStartup >= _nextLog)
            {
                _nextLog = Time.realtimeSinceStartup + 60;
                WriteTelemetry();
            }
            if (IsAutoplay && Time.realtimeSinceStartup >= _nextCapture)
            {
                _nextCapture = Time.realtimeSinceStartup + (_soak ? 900 : 15);
                Capture("combat-" + Mathf.FloorToInt(Time.realtimeSinceStartup));
            }
            if (_soak && Time.realtimeSinceStartup - _startedReal >= _soakSeconds)
            {
                WriteTelemetry();
                File.WriteAllText(Path.Combine(_evidenceDirectory, "soak-complete.json"), JsonUtility.ToJson(new SoakComplete { utc = DateTime.UtcNow.ToString("O"), elapsedSeconds = Time.realtimeSinceStartup - _startedReal, rounds = Round, weapons = catalog.Weapons.Count, armor = catalog.Armor.Count }, true));
                Debug.Log("OVERDOX_SOAK_COMPLETE");
                Application.Quit();
            }
        }

        public void ChangeWeapon(int delta)
        {
            SelectedWeapon = (SelectedWeapon + delta + catalog.Weapons.Count) % catalog.Weapons.Count;
            EquipPlayer();
        }

        public void ChangeSet(int delta)
        {
            SelectedSet = (SelectedSet + delta + 27) % 27;
            for (int slot = 0; slot < 3; slot++) { _playerArmorSets[slot] = SelectedSet; }
            EquipPlayer();
        }

        public void ChangeArmorSlot(int slot, int delta)
        {
            _playerArmorSets[slot] = (_playerArmorSets[slot] + delta + 27) % 27;
            EquipPlayer();
        }

        public void ChangeTraining(int delta)
        {
            TrainingOffset = Mathf.Clamp(TrainingOffset + delta, 0, 24);
            EquipPlayer();
        }

        public void ToggleGender()
        {
            PlayerIsFemale = !PlayerIsFemale;
            EquipPlayer();
        }

        public void StartMatch()
        {
            var flow = GameFlowController.Instance;
            if (flow != null && SceneManager.GetActiveScene().name == GameFlowController.LobbyScene)
            {
                flow.StoreSelection(Player);
                flow.EnterArena();
                return;
            }
            IsLobby = false;
            if (_drone != null) { _drone.gameObject.SetActive(true); }
            if (zoneRing != null) { zoneRing.gameObject.SetActive(true); }
            IsFighting = true;
            Round++;
            _roundStart = Time.time;
            _nextZoneTick = Time.time + 1;
            Message = "생존하고 챔피언이 되세요";
            for (int i = 0; i < Fighters.Count; i++)
            {
                Fighters[i].gameObject.SetActive(true);
                float angle = i * Mathf.PI * 2 / Fighters.Count;
                Fighters[i].Respawn(i == 0 ? new Vector3(0, .1f, -4) : new Vector3(Mathf.Sin(angle) * 10, .1f, Mathf.Cos(angle) * 10));
            }
            Debug.Log($"OVERDOX_ROUND_START round={Round} weapon={Player.weapon.name} set={Player.armor[0].setName}");
        }

        public void ToggleLobby()
        {
            var flow = GameFlowController.Instance;
            if (flow != null)
            {
                if (SceneManager.GetActiveScene().name == GameFlowController.ArenaScene)
                {
                    flow.StoreSelection(Player);
                    flow.ReturnToLobby();
                }
                return;
            }
            IsLobby = !IsLobby;
            IsFighting = !IsLobby;
            if (IsLobby) { Message = "장비 선택 / EQUIPMENT"; }
            if (IsLobby && Player != null) { Player.transform.rotation = Quaternion.Euler(0, 180, 0); }
            foreach (var fighter in Fighters)
            {
                if (fighter != Player) { fighter.gameObject.SetActive(!IsLobby); }
            }
        }

        public void Record(string key)
        {
            _counters.TryGetValue(key, out int count);
            _counters[key] = count + 1;
        }
        public void RecordAction(string action,Fighter fighter)
        {
            Record(action);
            if(action=="attack") { _weaponsAttacked.Add(fighter.weapon.id); }
            if(action=="skill") { _weaponsSkilled.Add(fighter.weapon.id); }
        }

        public void SpawnSlash(Vector3 position, Quaternion rotation, bool skill)
        {
            var obj = new GameObject("Slash effect");
            obj.transform.SetPositionAndRotation(position + Vector3.up * .7f, rotation);
            var line = obj.AddComponent<LineRenderer>();
            line.sharedMaterial = slashMaterial;
            line.useWorldSpace = false;
            line.positionCount = 22;
            line.widthMultiplier = skill ? .18f : .09f;
            line.startColor = skill ? new Color(1, .65f, .12f) : new Color(.25f, .85f, 1);
            line.endColor = new Color(.8f, .95f, 1, 0);
            for (int i = 0; i < 22; i++)
            {
                float a = Mathf.Lerp(-1.25f, 1.25f, i / 21f);
                line.SetPosition(i, new Vector3(Mathf.Sin(a) * 1.5f, .15f * Mathf.Sin(a), Mathf.Cos(a) * 1.5f));
            }
            Destroy(obj, .14f);
        }

        public void Capture(string name)
        {
            var camera = Camera.main;
            var target = RenderTexture.GetTemporary(1600, 900, 24, RenderTextureFormat.ARGB32);
            var previousTarget = camera.targetTexture;
            var previousActive = RenderTexture.active;
            try
            {
                foreach (var background in camera.GetComponentsInChildren<Camera>().Where(item => item != camera && item.enabled).OrderBy(item => item.depth))
                {
                    var originalTarget = background.targetTexture;
                    try { background.targetTexture = target; background.Render(); }
                    finally { background.targetTexture = originalTarget; }
                }
                camera.targetTexture = target;
                camera.Render();
                RenderTexture.active = target;
                var pixels = new Texture2D(1600, 900, TextureFormat.RGB24, false);
                pixels.ReadPixels(new Rect(0, 0, 1600, 900), 0, 0);
                pixels.Apply();
                File.WriteAllBytes(Path.Combine(_evidenceDirectory, name + ".png"), pixels.EncodeToPNG());
                Destroy(pixels);
            }
            finally
            {
                camera.targetTexture = previousTarget;
                RenderTexture.active = previousActive;
                RenderTexture.ReleaseTemporary(target);
            }
        }

        private void SpawnRoster()
        {
            string[] names = { "OVERDOX", "UNDERDOG", "IMHERO", "REAPER", "NOVA", "BLACK SWAN", "THUNDER", "WYVERN", "CROW", "PHANTOM", "GOLIATH", "LOTUS" };
            bool previewOnly = GameFlowController.Instance != null && SceneManager.GetActiveScene().name == GameFlowController.LobbyScene;
            int rosterSize = previewOnly ? 1 : 12;
            for (int i = 0; i < rosterSize; i++)
            {
                var f = Instantiate(fighterPrefab, new Vector3(i * .5f, .1f, 0), Quaternion.identity);
                f.catalog = catalog;
                f.isPlayer = i == 0;
                f.displayName = names[i];
                f.name = names[i];
                Fighters.Add(f);
                Equip(f, i == 0 ? SelectedWeapon : (i * 3) % 38, i == 0 ? SelectedSet : i * 2 % 27);
                f.Impact += hud.OnImpact;
                if (i != 0) { f.gameObject.SetActive(false); }
            }
            Player = Fighters[0];
            if (dronePrefab != null) { _drone = Instantiate(dronePrefab).transform; }
        }

        private void EquipPlayer()
        {
            if (Player != null)
            {
                Equip(Player, SelectedWeapon, SelectedSet);
                GameFlowController.Instance?.StoreSelection(Player);
            }
        }

        private void Equip(Fighter fighter, int weaponIndex, int setIndex)
        {
            fighter.isFemale = fighter.isPlayer ? PlayerIsFemale : Fighters.IndexOf(fighter) % 2 != 0;
            var w = catalog.GetWeapon(weaponIndex);
            var pieces = new[] { catalog.GetArmor(setIndex, 0), catalog.GetArmor(setIndex, 1), catalog.GetArmor(setIndex, 2) };
            if (fighter.isPlayer)
            {
                for (int slot = 0; slot < 3; slot++) { pieces[slot] = catalog.GetArmor(_playerArmorSets[slot], slot); }
                fighter.trainingOffset = TrainingOffset;
            }
            string bodyKey = $"Fighter_{pieces[0].brand}_{pieces[0].rarity}";
            int body = Array.IndexOf(bodyKeys, bodyKey);
            int blade = Array.IndexOf(weaponKeys, w.model);
            if (body < 0 || blade < 0) { throw new InvalidOperationException($"Missing model {bodyKey}/{w.model}"); }
            fighter.Configure(w, pieces, bodies[body], weapons[blade]);
            _weaponsSeen.Add(w.id);foreach(var piece in pieces) { _armorSeen.Add(piece.id); }
        }

        private IEnumerator NextAutoplayRound()
        {
            yield return new WaitForSeconds(3);
            _equipmentCycle++;
            SelectedWeapon = _equipmentCycle % 38;
            SelectedSet = _equipmentCycle % 27;
            for (int slot = 0; slot < 3; slot++) { _playerArmorSets[slot] = SelectedSet; }
            EquipPlayer();
            foreach (var fighter in Fighters)
            {
                if (fighter != Player) { Equip(fighter, (_equipmentCycle + Fighters.IndexOf(fighter) * 5) % 38, (_equipmentCycle + Fighters.IndexOf(fighter)) % 27); }
            }
            StartMatch();
        }

        private IEnumerator ReviewCatalog()
        {
            yield return new WaitForSeconds(3);
            Capture("lobby-initial");
            int failures = 0;
            for (int i = 0; i < catalog.Weapons.Count; i++)
            {
                bool success = true;
                try { ChangeWeapon(i - SelectedWeapon); }
                catch (Exception error) { failures++; success = false; Debug.LogException(error); }
                yield return new WaitForSeconds(.4f);
                if (success) { Capture("weapon-" + catalog.GetWeapon(i).id); }
                Debug.Log($"CATALOG_REVIEW weapon={catalog.GetWeapon(i).id} success={success}");
            }
            for (int gender = 0; gender < 2; gender++)
            {
            PlayerIsFemale = gender != 0;
            for (int i = 0; i < 27; i++)
            {
                bool success = true;
                try { ChangeSet(i - SelectedSet); }
                catch (Exception error) { failures++; success = false; Debug.LogException(error); }
                yield return new WaitForSeconds(.4f);
                if (success) { Capture("armor-set-" + i + (PlayerIsFemale ? "-female" : "-male")); }
                Debug.Log($"CATALOG_REVIEW armorSet={i} female={PlayerIsFemale} success={success}");
            }
            }
            File.WriteAllText(Path.Combine(_evidenceDirectory, "catalog-review.json"), $"{{\"utc\":\"{DateTime.UtcNow:O}\",\"weaponReviews\":38,\"armorSetReviews\":54,\"armorPieces\":81,\"failures\":{failures}}}");
            Debug.Log("CATALOG_REVIEW_COMPLETE failures=" + failures);
            Application.Quit();
        }

        private string FindWinner()
        {
            foreach (var fighter in Fighters) { if (fighter.IsAlive) { return fighter.displayName; } }
            return "NONE";
        }

        private void WriteTelemetry()
        {
            string counters = string.Join(",", _counters);
            string line = $"utc={DateTime.UtcNow:O} elapsed={Time.realtimeSinceStartup - _startedReal:F1} rounds={Round} alive={AliveCount} playerPosition={Player.transform.position} memory={UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong()} objects={FindObjectsByType<GameObject>(FindObjectsSortMode.None).Length} {counters}";
            File.AppendAllText(Path.Combine(_evidenceDirectory, "telemetry.log"), line + Environment.NewLine);
            Debug.Log("OVERDOX_TELEMETRY " + line);
            File.WriteAllText(Path.Combine(_evidenceDirectory,"coverage.json"),JsonUtility.ToJson(new CoverageSnapshot { weaponsSeen=_weaponsSeen.OrderBy(i=>i).ToArray(),armorSeen=_armorSeen.OrderBy(i=>i).ToArray(),weaponsAttacked=_weaponsAttacked.OrderBy(i=>i).ToArray(),weaponsSkilled=_weaponsSkilled.OrderBy(i=>i).ToArray() },true));
        }

        [Serializable] private sealed class CoverageSnapshot { public int[] weaponsSeen,armorSeen,weaponsAttacked,weaponsSkilled; }

        [Serializable]
        private sealed class SoakComplete
        {
            public string utc;
            public float elapsedSeconds;
            public int rounds, weapons, armor;
        }
    }
}
