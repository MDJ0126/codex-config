using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TurnRoom.PixelCamera;
using TurnRoom.PixelCamera.URP;

namespace Fantasya.Editor
{
    /// <summary>
    /// 게임 전용 프리셋과 렌더러를 저장하고 캐릭터 루트에만 Pixora 픽셀 표식을 구성한다.
        /// Playground와 캐릭터 프리팹의 최초 구성을 담당한다.
        /// 제품 원본과 기존 수동 편집 설정은 보존한다.
    /// </summary>
    public static class PixelCameraPlaygroundSetup
    {
        private const string PresetPath = "Assets/Data/CharacterPixels.asset";
        private const string RendererPath = "Assets/Settings/CharacterPixelRenderer.asset";

        /// <summary>
        /// 기존 조명·재질·카메라 조작을 보존하며 비어 있는 픽셀 구성만 처음 생성한다.
        /// Playground를 열고 캐릭터 표식과 카메라 연결을 저장한다.
        /// 이미 열린 다른 씬의 미저장 변경은 먼저 저장해야 한다.
        /// </summary>
        [MenuItem("Fantasya/그래픽/플레이어·몬스터 픽셀 카메라 구성")]
        public static void Configure()
        {
            var scene = EditorSceneManager.OpenScene("Assets/Scenes/Playground.unity");
            PixelCameraPreset preset = AssetDatabase.LoadAssetAtPath<PixelCameraPreset>(PresetPath);
            if (preset == null)
            {
                preset = ScriptableObject.CreateInstance<PixelCameraPreset>();
                preset.pixelization = PixelCameraPixelization.SelectedObjects;
                preset.colorLevels = 256;
                preset.outline = false;
                preset.orderedDithering = false;
                preset.stabilizeGrid = false;
                AssetDatabase.CreateAsset(preset, PresetPath);
            }
            UniversalRendererData renderer = CreateRenderer();
            int desktop = RegisterRenderer("Assets/Settings/PC_RPAsset.asset", renderer);
            int mobile = RegisterRenderer("Assets/Settings/Mobile_RPAsset.asset", renderer);
            if (desktop != mobile)
            {
                throw new InvalidOperationException("품질별 렌더러 인덱스가 다릅니다. 카메라 구성을 확인해 주세요.");
            }
            Camera camera = Camera.main;
            PixelCameraEffect effect = camera.GetComponent<PixelCameraEffect>();
            if (effect == null)
            {
                effect = camera.gameObject.AddComponent<PixelCameraEffect>();
                effect.preset = preset;
            }
            camera.GetUniversalAdditionalCameraData().SetRenderer(desktop);
            int count = 0;
            foreach (string guid in AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/ArtResources/Playground/Characters" }))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = PrefabUtility.LoadPrefabContents(path);
                try
                {
                    if (prefab.GetComponent<Monster>() == null)
                    {
                        continue;
                    }
                    if (prefab.GetComponent<PixelCameraObject>() == null)
                    {
                        AddMarker(prefab);
                        PrefabUtility.SaveAsPrefabAsset(prefab, path);
                    }
                    count++;
                }
                finally
                {
                    PrefabUtility.UnloadPrefabContents(prefab);
                }
            }
            foreach (Monster monster in UnityEngine.Object.FindObjectsByType<Monster>(FindObjectsSortMode.None))
            {
                if (monster.GetComponent<PixelCameraObject>() == null)
                {
                    AddMarker(monster.gameObject);
                }
            }
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            EditorSceneManager.OpenScene(scene.path);
            Debug.Log("FANTASYA_CHARACTER_PIXELS_SAVED_REOPENED: prefabs=" + count + ", renderer=" + desktop);
        }

        /// <summary>
        /// 선택한 캐릭터의 외형만 4픽셀 셀로 표시하고 물리 상태는 변경하지 않는다.
        /// Monster를 가진 캐릭터 루트에만 호출한다.
        /// 새 컴포넌트를 추가하며 기존 표식은 호출 전에 제외한다.
        /// </summary>
        private static void AddMarker(GameObject root)
        {
            PixelCameraObject marker = root.AddComponent<PixelCameraObject>();
            marker.pixelSize = 4;
            marker.sizeMode = PixelCameraObjectSize.ScreenPixels;
            marker.stabilize = false;
            marker.perspectiveSnapping = false;
            marker.smoothSubpixelMotion = false;
            marker.rotationStep = 0f;
            marker.outline = false;
        }

        /// <summary>
        /// 기존 렌더러를 바꾸지 않고 기능 인스턴스를 분리한 캐릭터 전용 Forward 렌더러를 만든다.
        /// 기존 에셋이 있으면 수동 설정을 그대로 반환한다.
        /// 새 에셋 생성 시 SSAO와 픽셀 기능을 독립 서브에셋으로 저장한다.
        /// </summary>
        private static UniversalRendererData CreateRenderer()
        {
            UniversalRendererData result = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(RendererPath);
            if (result != null)
            {
                return result;
            }
            UniversalRendererData original = AssetDatabase.LoadAssetAtPath<UniversalRendererData>("Assets/Settings/PC_Renderer.asset");
            result = UnityEngine.Object.Instantiate(original);
            result.name = "Character Pixel Renderer";
            result.rendererFeatures.Clear();
            AssetDatabase.CreateAsset(result, RendererPath);
            foreach (ScriptableRendererFeature feature in original.rendererFeatures)
            {
                if (feature == null)
                {
                    continue;
                }
                ScriptableRendererFeature copy = UnityEngine.Object.Instantiate(feature);
                copy.name = feature.name;
                AssetDatabase.AddObjectToAsset(copy, result);
                result.rendererFeatures.Add(copy);
            }
            var serialized = new SerializedObject(result);
            serialized.FindProperty("m_RenderingMode").intValue = 0;
            serialized.FindProperty("m_IntermediateTextureMode").intValue = 1;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            PixelCameraRendererFeature pixels = ScriptableObject.CreateInstance<PixelCameraRendererFeature>();
            pixels.name = "Character Pixel Camera";
            AssetDatabase.AddObjectToAsset(pixels, result);
            result.rendererFeatures.Add(pixels);
            result.SetDirty();
            EditorUtility.SetDirty(result);
            return result;
        }

        /// <summary>
        /// 기본 렌더러 인덱스와 기존 목록은 보존하고 캐릭터 렌더러를 목록 끝에 등록한다.
        /// PC와 Mobile의 등록 위치가 같아야 카메라에서 공용으로 선택할 수 있다.
        /// 목록에 이미 존재하면 중복 등록하지 않는다.
        /// </summary>
        private static int RegisterRenderer(string path, UniversalRendererData renderer)
        {
            UniversalRenderPipelineAsset pipeline = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(path);
            var serialized = new SerializedObject(pipeline);
            SerializedProperty list = serialized.FindProperty("m_RendererDataList");
            for (int index = 0; index < list.arraySize; index++)
            {
                if (list.GetArrayElementAtIndex(index).objectReferenceValue == renderer)
                {
                    return index;
                }
            }
            int result = list.arraySize;
            list.arraySize++;
            list.GetArrayElementAtIndex(result).objectReferenceValue = renderer;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(pipeline);
            return result;
        }
    }
}
