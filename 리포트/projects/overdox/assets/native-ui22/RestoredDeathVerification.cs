using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Overdox.Editor
{
    /// <summary>
    /// 남녀 영웅의 실제 뼈 경로에서 무기군별 사망 클립이 포즈를 바꾸는지 확인한다.
    /// </summary>
    public static class RestoredDeathVerification
    {
        [Serializable]
        private sealed class PoseResult
        {
            public int family, gender, bindings, matchedBindings;
            public string clip;
            public float maxDisplacement, maxAngle;
            public float pelvisStartHeight, pelvisEndHeight;
            public string[] missingPaths;
            public bool passed;
        }

        [Serializable]
        private sealed class VerificationResult
        {
            public string utc;
            public int previousMatchedBindings, previousBindings;
            public PoseResult[] poses;
        }

        /// <summary>
        /// 기존 잘못된 클립과 교체한 클립을 같은 영웅 리그에서 비교하고 JSON 근거를 저장한다.
        /// </summary>
        [MenuItem("OVERDOX/Restore/Verify hero death rig bindings")]
        public static void Verify()
        {
            if (EditorApplication.isPlaying)
            {
                throw new InvalidOperationException("재생을 종료한 뒤 검증하세요.");
            }
            var library = AssetDatabase.LoadAssetAtPath<RestoredVisualLibrary>("Assets/GameData/RestoredVisualLibrary.asset");
            var metadata = JsonUtility.FromJson<BoneMetadata>(library.boneMetadata.text);
            var results = new List<PoseResult>();
            var report = new VerificationResult { utc = DateTime.UtcNow.ToString("o") };
            var oldClip = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/Art/Restored/Animations/Die_01.anim");
            for (int gender = 0; gender < metadata.avatars.Length; gender++)
            {
                var root = new GameObject("Death rig verification") { hideFlags = HideFlags.HideAndDontSave };
                try
                {
                    BuildRig(root.transform, metadata.avatars[gender]);
                    if (gender == 0)
                    {
                        var oldBindings = AnimationUtility.GetCurveBindings(oldClip);
                        report.previousBindings = oldBindings.Length;
                        report.previousMatchedBindings = oldBindings.Count(binding => string.IsNullOrEmpty(binding.path) || root.transform.Find(binding.path) != null);
                    }
                    for (int family = 0; family < library.animations.Length; family++)
                    {
                        var animations = library.animations[family];
                        results.Add(Sample(root, animations.death, family, gender));
                        if (animations.legendaryDeath != null)
                        {
                            results.Add(Sample(root, animations.legendaryDeath, family, gender));
                        }
                    }
                }
                finally
                {
                    UnityEngine.Object.DestroyImmediate(root);
                }
            }
            report.poses = results.ToArray();
            Directory.CreateDirectory("Docs/Verification/DeathAnimations");
            File.WriteAllText("Docs/Verification/DeathAnimations/rig-bindings.json", JsonUtility.ToJson(report, true));
            if (results.Any(result => !result.passed))
            {
                throw new InvalidOperationException("영웅 사망 포즈 검증 실패: JSON의 뼈 경로와 포즈 변화를 확인하세요.");
            }
            Debug.Log($"OVERDOX_HERO_DEATH_VERIFIED poses={results.Count} previousMatched={report.previousMatchedBindings}/{report.previousBindings}");
        }

        /// <summary>
        /// 보존된 Avatar 메타데이터의 경로·로컬 자세로 검증용 영웅 리그를 구성한다.
        /// </summary>
        private static void BuildRig(Transform root, AvatarBoneMap avatar)
        {
            foreach (var bone in avatar.bones)
            {
                Transform current = root;
                foreach (string segment in bone.path.Split('/'))
                {
                    if (string.IsNullOrEmpty(segment))
                    {
                        continue;
                    }
                    var next = current.Find(segment);
                    if (next == null)
                    {
                        next = new GameObject(segment).transform;
                        next.SetParent(current, false);
                    }
                    current = next;
                }
                current.localPosition = bone.position;
                current.localRotation = bone.rotation;
                current.localScale = bone.scale;
            }
        }

        /// <summary>
        /// 실제 클립의 시작·끝 자세와 영웅 뼈에 대응한 커브 수를 측정한다.
        /// </summary>
        private static PoseResult Sample(GameObject root, AnimationClip clip, int family, int gender)
        {
            if (clip == null)
            {
                throw new InvalidOperationException($"사망 클립 누락: family={family}");
            }
            var bindings = AnimationUtility.GetCurveBindings(clip);
            var bones = root.GetComponentsInChildren<Transform>();
            clip.SampleAnimation(root, 0);
            var pelvis = root.transform.Find("motion/Bip001/Bip001 Pelvis");
            float startHeight = pelvis != null ? pelvis.position.y : 0;
            var positions = bones.Select(bone => bone.localPosition).ToArray();
            var rotations = bones.Select(bone => bone.localRotation).ToArray();
            clip.SampleAnimation(root, clip.length);
            var result = new PoseResult
            {
                family = family, gender = gender, clip = clip.name, bindings = bindings.Length,
                matchedBindings = bindings.Count(binding => string.IsNullOrEmpty(binding.path) || root.transform.Find(binding.path) != null),
                missingPaths = bindings.Where(binding => !string.IsNullOrEmpty(binding.path) && root.transform.Find(binding.path) == null).Select(binding => binding.path).Distinct().ToArray(),
                pelvisStartHeight = startHeight,
                pelvisEndHeight = pelvis != null ? pelvis.position.y : 0
            };
            for (int index = 0; index < bones.Length; index++)
            {
                result.maxDisplacement = Mathf.Max(result.maxDisplacement, Vector3.Distance(positions[index], bones[index].localPosition));
                result.maxAngle = Mathf.Max(result.maxAngle, Quaternion.Angle(rotations[index], bones[index].localRotation));
            }
            // 리그에 연결되는 커브와 실제 회전 변화가 모두 있어야 잘못된 몬스터 클립을 통과시키지 않는다.
            result.passed = result.matchedBindings > 0 && result.maxAngle > 1;
            return result;
        }
    }
}
