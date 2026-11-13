using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Overdox.Editor
{
    public static partial class RestoredProjectBuilder
    {
        /// <summary>
        /// 영웅 리그에 맞는 무기군별 사망 클립만 선택하고 필수 클립 누락을 알린다.
        /// </summary>
        /// <param name="isLegendary">별도 전설 클립을 조회하며 없으면 null을 반환한다.</param>
        private static AnimationClip SelectHeroDeathClip(IEnumerable<AnimationClip> clips, string family, bool isLegendary)
        {
            var clip = clips.Where(candidate => candidate.name.IndexOf("_die", StringComparison.OrdinalIgnoreCase) >= 0)
                .Where(candidate => (candidate.name.IndexOf("_LE", StringComparison.OrdinalIgnoreCase) >= 0) == isLegendary)
                .OrderBy(candidate => candidate.name.Length).FirstOrDefault();
            if (clip == null)
            {
                if (isLegendary)
                {
                    return null;
                }
                throw new InvalidOperationException($"영웅 사망 클립 누락: {family}");
            }

            // 같은 이름의 몬스터 클립이 섞여도 실제 영웅 뼈 경로를 확인해 잘못된 연결을 막는다.
            bool hasHeroBinding = AnimationUtility.GetCurveBindings(clip).Any(binding => binding.path.StartsWith("motion/Bip001", StringComparison.Ordinal));
            if (!hasHeroBinding)
            {
                throw new InvalidOperationException($"영웅 리그와 다른 사망 클립: {clip.name}");
            }
            return clip;
        }

        /// <summary>
        /// 기존 시각 라이브러리의 사망 참조만 교체하고 다른 클립과 씬을 보존한다.
        /// </summary>
        [MenuItem("OVERDOX/Restore/Bind hero death animations")]
        public static void BindHeroDeathAnimations()
        {
            if (EditorApplication.isPlaying)
            {
                throw new InvalidOperationException("재생을 종료한 뒤 사망 클립을 연결하세요.");
            }
            var library = AssetDatabase.LoadAssetAtPath<RestoredVisualLibrary>("Assets/GameData/RestoredVisualLibrary.asset");
            string[] prefixes = { "SwordAndShield", "HammerAndShield", "DualBlade", "Gsword", "Ghammer", "PaxeAndShield", "Quarterstaff", "LongSword" };
            if (library == null || library.animations.Length != prefixes.Length)
            {
                throw new InvalidOperationException("8개 무기군의 시각 라이브러리를 먼저 준비하세요.");
            }

            var clips = AssetDatabase.FindAssets("t:AnimationClip", new[] { "Assets/Art/Restored/Animations" })
                .Select(AssetDatabase.GUIDToAssetPath)
                // 전체 전투 클립을 메모리에 올리지 않고 사망 후보의 경로를 먼저 거른다.
                .Where(path => Path.GetFileNameWithoutExtension(path).IndexOf("_die", StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(AssetDatabase.LoadAssetAtPath<AnimationClip>).ToArray();
            for (int family = 0; family < prefixes.Length; family++)
            {
                string prefix = prefixes[family];
                var group = clips.Where(clip => !clip.name.EndsWith("_0", StringComparison.Ordinal)
                    && (clip.name.StartsWith(prefix + "_", StringComparison.OrdinalIgnoreCase)
                        || family == 0 && clip.name.StartsWith("Sword&Shield_", StringComparison.OrdinalIgnoreCase))).ToArray();
                library.animations[family].death = SelectHeroDeathClip(group, prefix, false);
                library.animations[family].legendaryDeath = SelectHeroDeathClip(group, prefix, true);
            }
            EditorUtility.SetDirty(library);
            AssetDatabase.SaveAssets();
            Debug.Log("OVERDOX_HERO_DEATH_BOUND families=8");
        }
    }
}
