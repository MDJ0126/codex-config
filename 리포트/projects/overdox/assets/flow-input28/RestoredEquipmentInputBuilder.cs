using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Overdox.Editor
{
    public static partial class RestoredUiBuilder
    {
        /// <summary>
        /// 플레이용 로비의 이전·다음 입력 영역을 원본 BoxCollider 크기로 저장한다. 표시와 원본 자산은 유지한다.
        /// </summary>
        [MenuItem("OVERDOX/Restore/Restore native equipment button hit areas")]
        public static void RestoreNativeEquipmentButtonHitAreas()
        {
            if (EditorApplication.isPlaying || SceneManager.GetActiveScene().isDirty)
            {
                throw new InvalidOperationException("현재 씬을 저장하고 재생을 종료하세요.");
            }
            var setup = EditorSceneManager.GetSceneManagerSetup();
            try
            {
                var scene = EditorSceneManager.OpenScene("Assets/Scenes/OverdoxLobby.unity", OpenSceneMode.Single);
                var controller = UnityEngine.Object.FindFirstObjectByType<RestoredLobbyController>();
                RestoreEquipmentNavigationHitAreas(controller.GetComponent<RestoredUiDocument>());
                EditorSceneManager.SaveScene(scene);
            }
            finally
            {
                EditorSceneManager.RestoreSceneManagerSetup(setup);
            }
        }

        /// <summary>
        /// 원본 콜라이더 중심0·크기30×350을 기존 투명 버튼 Rect에 적용해 작은 화살표 크기 추정을 교체한다.
        /// </summary>
        private static void RestoreEquipmentNavigationHitAreas(RestoredUiDocument document)
        {
            // main81 level3의 BoxCollider60978·60999를 내장 타입 트리로 읽어 확인했다.
            // 기존 Wire는 자식 그림20×46만 사용해 긴 입력 영역을 축소했다. 자식 화살표 위치는 변경하지 않는다.
            foreach (int sourceId in new[] { 34185, 36471 })
            {
                var rect = document.Find(sourceId);
                if (rect.pivot != new Vector2(.5f, .5f))
                {
                    throw new InvalidOperationException("원본 중심 입력 영역과 다른 pivot입니다: " + sourceId);
                }
                rect.sizeDelta = new Vector2(30, 350);
                EditorUtility.SetDirty(rect);
            }
            Debug.Log("OVERDOX_EQUIPMENT_HIT_AREAS_RESTORED sourceIds=34185,36471 size=30x350 center=0,0");
        }
    }
}
