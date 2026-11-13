using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Overdox
{
    /// <summary>
    /// 원작 스켈레톤·스킨·클립을 원본 변경 없이 재조립하고 샘플링한다.
    /// </summary>
    public sealed class RestoredFighterVisual : MonoBehaviour
    {
        private static readonly Dictionary<Material, Material> MATERIALS = new();
        private static readonly Dictionary<AnimationClip, AnimationClip> CLIPS = new();
        private static BoneMetadata _metadata;
        private readonly Dictionary<string, Transform> _paths = new();
        private readonly Dictionary<uint, Transform> _hashes = new();
        private Fighter _fighter;
        private FamilyAnimations _animations;
        private int _family;
        private Material _materialTemplate;
        private Renderer[] _allRenderers;
        private bool _wasHidden;
        private static WeaponHitboxData _hitboxes;
        private WeaponHitboxEntry _weaponHitboxes;
        private readonly List<Transform> _weaponParts = new();

        /// <summary>
        /// 현재 전투 상태의 원작 클립을 영웅 리그에 샘플링하고 은신 표시를 갱신한다.
        /// </summary>
        private void LateUpdate()
        {
            if (_fighter == null || _animations == null) { return; }
            bool hidden=_fighter.IsHidden;
            if(hidden!=_wasHidden&&_allRenderers!=null)
            {
                _wasHidden=hidden;
                var properties=new MaterialPropertyBlock();
                properties.SetColor("_OccludedColor",_fighter.isPlayer?new Color(.1f,.75f,1,.6f):new Color(1,.25f,.15f,.5f));
                properties.SetFloat("_Opacity",hidden ? (_fighter.isPlayer ? .28f : .03f) : 1);
                foreach(var renderer in _allRenderers) { renderer.SetPropertyBlock(properties); }
            }
            AnimationClip source;
            bool loop = false;
            float time = _fighter.AttackElapsed * _fighter.AnimationRate;
            switch (_fighter.State)
            {
                case CombatState.Windup:
                case CombatState.Active:
                case CombatState.Recovery:
                {
                    source = AttackClip(_fighter.ComboIndex, _fighter.IsSkillAttack);
                    break;
                }
                case CombatState.Guard:
                {
                    source = _animations.guard;
                    time = Time.time;
                    loop = true;
                    break;
                }
                case CombatState.Dash:
                {
                    source = _animations.dash;
                    time = _fighter.StateProgress * (source != null ? source.length : 1);
                    break;
                }
                case CombatState.Dead:
                {
                    // 최종 프레임으로 즉시 건너뛰지 않고 사망 상태 진입 시각부터 재생한다.
                    source = _fighter.weapon.rarity == 4 && _animations.legendaryDeath != null ? _animations.legendaryDeath : _animations.death;
                    time = _fighter.StateElapsed;
                    break;
                }
                default:
                {
                    source = ArenaSession.Instance != null && ArenaSession.Instance.IsLobby && _animations.lobbyIdle != null
                        ? _animations.lobbyIdle : _fighter.MoveIntent.sqrMagnitude > .01f ? _animations.move : _animations.idle;
                    time = Time.time;
                    loop = true;
                    break;
                }
            }
            if (source == null) { source = _animations.idle; }
            if (source == null) { return; }
            if (!CLIPS.TryGetValue(source, out var clip))
            {
                clip = Instantiate(source);
                clip.name = source.name + " (visual adapter)";
                clip.events = Array.Empty<AnimationEvent>();
                clip.legacy = true;
                CLIPS.Add(source, clip);
            }
            float sample = loop && clip.length > 0 ? time % clip.length : Mathf.Min(time, clip.length);
            // 비반복 클립은 마지막 자세에 머문다. 사망 상태는 Idle로 자동 복귀하지 않는다.
            clip.SampleAnimation(gameObject, sample);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public void Configure(RestoredVisualLibrary library, Fighter fighter, GearRow weapon, GearRow[] armor, bool female = false)
        {
            _fighter = fighter;
            _materialTemplate = library.materialTemplate;
            _family = weapon.family;
            _animations = library.animations[_family];
            _metadata ??= JsonUtility.FromJson<BoneMetadata>(library.boneMetadata.text);
            if(library.hitboxMetadata!=null)
            {
                _hitboxes??=JsonUtility.FromJson<WeaponHitboxData>(library.hitboxMetadata.text);
                _weaponHitboxes=Array.Find(_hitboxes.weapons,w=>w.id==weapon.id);
            }
            var avatar = _metadata.avatars[female ? 1 : 0];
            foreach (var bone in avatar.bones)
            {
                Transform target = EnsurePath(bone.path);
                target.localPosition = bone.position;
                target.localRotation = bone.rotation;
                target.localScale = bone.scale;
            }
            foreach (var entry in avatar.paths)
            {
                if (!string.IsNullOrEmpty(entry.path) && _paths.TryGetValue(entry.path, out var bone)) { _hashes[entry.hash] = bone; }
            }
            foreach (var piece in armor)
            {
                var found = Array.Find(library.armor, a => a.id == piece.id);
                if (found == null) { throw new InvalidOperationException("Missing original armor: " + piece.id); }
                var part = Instantiate(female ? found.female : found.male, transform);
                part.transform.localPosition = Vector3.zero;
                part.transform.localRotation = Quaternion.identity;
                foreach (var skin in part.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    var map = Array.Find(_metadata.meshes, m => m.name == skin.sharedMesh.name);
                    if (map == null) { throw new InvalidOperationException("Missing mesh bone map: " + skin.sharedMesh.name); }
                    var bones = new Transform[map.hashes.Length];
                    for (int i = 0; i < bones.Length; i++)
                    {
                        if (!_hashes.TryGetValue(map.hashes[i], out bones[i])) { throw new InvalidOperationException($"Missing bone {map.hashes[i]} for {skin.sharedMesh.name}"); }
                    }
                    skin.bones = bones;
                    skin.rootBone = Find("Bip001");
                    skin.updateWhenOffscreen = true;
                    skin.localBounds = new Bounds(Vector3.up, Vector3.one * 5);
                    skin.shadowCastingMode = ShadowCastingMode.On;
                    skin.receiveShadows = true;
                    AdaptMaterials(skin);
                }
            }
            var gear = Array.Find(library.weapons, w => w.id == weapon.id);
            if (gear == null) { throw new InvalidOperationException("Missing original weapon: " + weapon.id); }
            for (int i = 0; i < gear.parts.Length; i++)
            {
                int slot=i==0?weapon.parentPos1:weapon.parentPos2;
                Transform hand=slot>=0&&library.attachmentBones!=null&&slot<library.attachmentBones.Length?Find(library.attachmentBones[slot]):null;
                hand ??= Find(i == 0 ? "Bip001 R Hand" : "Bip001 L Hand") ?? transform;
                var part = Instantiate(gear.parts[i], hand);
                _weaponParts.Add(part.transform);
                foreach (var renderer in part.GetComponentsInChildren<Renderer>()) { AdaptMaterials(renderer); }
            }
            transform.localScale = Vector3.one * 1.1f;
            _allRenderers=GetComponentsInChildren<Renderer>();
        }

        public AnimationClip AttackClip(int combo, bool skill)
        {
            var array = skill ? _animations.skills : _fighter.weapon.rarity==4 && _animations.legendaryAttacks!=null ? _animations.legendaryAttacks : _animations.attacks;
            if (array == null || array.Length == 0) { return _animations.idle; }
            int index = skill ? Mathf.Clamp(_fighter.weapon.rarity==4?4:_fighter.weapon.rarity-1, 0, array.Length - 1) : combo % array.Length;
            return array[index] != null ? array[index] : _animations.idle;
        }

        public int ActiveHitSlot(float elapsed)
        {
            var clip = AttackClip(_fighter.ComboIndex, _fighter.IsSkillAttack);
            if (clip == null) { return 1; }
            int active = 0;
            bool hasEvents = false;
            foreach (var evt in clip.events)
            {
                if (!evt.functionName.StartsWith("OnAttackBegin") && !evt.functionName.StartsWith("OnAttackEnd")) { continue; }
                hasEvents = true;
                if (evt.time > elapsed) { continue; }
                int underscore = evt.functionName.LastIndexOf('_');
                int slot = underscore >= 0 && int.TryParse(evt.functionName.Substring(underscore + 1), out int number) ? Mathf.Clamp(number, 1, 4) : 1;
                if (evt.functionName.StartsWith("OnAttackBegin")) { active = slot; }
                else if (active == slot) { active = 0; }
            }
            return hasEvents ? active : 1;
        }

        public bool? WeaponTouches(Fighter target,int combo,int slot)
        {
            if(_weaponHitboxes==null) { return null; }
            bool found=false;
            foreach(var part in _weaponHitboxes.parts)
            {
                if(part.index>=_weaponParts.Count) { continue; }
                string marker="Attack"+(combo+1)+"_"+(slot-1);
                var boxes=Array.FindAll(part.boxes,b=>b.path.Equals(marker,StringComparison.OrdinalIgnoreCase));
                if(boxes.Length==0) { boxes=Array.FindAll(part.boxes,b=>string.IsNullOrEmpty(b.path)); }
                foreach(var box in boxes)
                {
                    var root=_weaponParts[part.index];var transform=string.IsNullOrEmpty(box.path)?root:root.Find(box.path);
                    if(transform==null) { continue; }
                    found=true;
                    if(BoxTouchesCapsule(transform,box,target)) { return true; }
                }
            }
            return found?false:null;
        }

        private static bool BoxTouchesCapsule(Transform transform,OriginalHitbox box,Fighter target)
        {
            var controller=target.GetComponent<CharacterController>();
            Vector3 center=transform.TransformPoint(box.center),scale=transform.lossyScale;
            Vector3 half=Vector3.Scale(box.size,new Vector3(Mathf.Abs(scale.x),Mathf.Abs(scale.y),Mathf.Abs(scale.z)))*.5f;
            Quaternion inverse=Quaternion.Inverse(transform.rotation);
            Vector3 bodyCenter=target.transform.TransformPoint(controller.center);
            float radius=controller.radius,segment=Mathf.Max(0,controller.height*.5f-radius);
            for(int i=0;i<5;i++)
            {
                Vector3 point=bodyCenter+Vector3.up*Mathf.Lerp(-segment,segment,i/4f);
                Vector3 local=inverse*(point-center);
                Vector3 nearest=new Vector3(Mathf.Clamp(local.x,-half.x,half.x),Mathf.Clamp(local.y,-half.y,half.y),Mathf.Clamp(local.z,-half.z,half.z));
                if((nearest-local).sqrMagnitude<=radius*radius) { return true; }
            }
            return false;
        }

        public bool GetAttackTiming(int combo, bool skill, out float windup, out float active, out float recovery)
        {
            windup = active = recovery = 0;
            var clip = AttackClip(combo, skill);
            if (clip == null) { return false; }
            float begin = -1, end = -1, finish = clip.length;
            foreach (var evt in clip.events)
            {
                if (evt.functionName.StartsWith("OnAttackBegin") && begin < 0) { begin = evt.time; }
                if (evt.functionName.StartsWith("OnAttackEnd")) { end = evt.time; }
                if (evt.functionName == "OnFinishAction") { finish = evt.time; }
            }
            if (begin < 0 || end <= begin) { return false; }
            windup = Mathf.Max(.03f, begin);
            active = Mathf.Max(.04f, end - begin);
            recovery = Mathf.Max(.05f, finish - end);
            return true;
        }

        private Transform EnsurePath(string path)
        {
            if (_paths.TryGetValue(path, out var existing)) { return existing; }
            int slash = path.LastIndexOf('/');
            Transform parent = slash >= 0 ? EnsurePath(path.Substring(0, slash)) : transform;
            string name = slash >= 0 ? path.Substring(slash + 1) : path;
            var bone = new GameObject(name).transform;
            bone.SetParent(parent, false);
            _paths[path] = bone;
            return bone;
        }

        private Transform Find(string name)
        {
            foreach (var bone in _paths.Values) { if (bone.name == name) { return bone; } }
            return null;
        }

        private void AdaptMaterials(Renderer renderer)
        {
            var sources = renderer.sharedMaterials;
            var materials = new Material[sources.Length];
            for (int i = 0; i < sources.Length; i++)
            {
                var source = sources[i];
                if (source == null) { continue; }
                if (!MATERIALS.TryGetValue(source, out var material))
                {
                    material = new Material(_materialTemplate);
                    material.name = source.name + " (project adapter)";
                    Texture texture = null;
                    if (source.HasProperty("_MainTex")) { texture = source.GetTexture("_MainTex"); }
                    foreach (string property in source.GetTexturePropertyNames())
                    {
                        if (texture != null) { break; }
                        texture = source.GetTexture(property);
                    }
                    material.mainTexture = texture;
                    material.SetFloat("_Cutoff", .2f);
                    MATERIALS.Add(source, material);
                }
                materials[i] = material;
            }
            renderer.sharedMaterials = materials;
            var properties = new MaterialPropertyBlock();
            properties.SetColor("_OccludedColor", _fighter.isPlayer ? new Color(.1f,.75f,1,.6f) : new Color(1,.25f,.15f,.5f));
            renderer.SetPropertyBlock(properties);
        }
    }
}
