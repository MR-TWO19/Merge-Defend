using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KeatonCore.Tutorial
{
    public class FTUERenderTool : MonoBehaviour
    {
        private class UITempData
        {
            public Canvas tempCanvas;
            public GraphicRaycaster tempRaycaster;
        }

        [Header("References")]
        [SerializeField] private Camera ftueCamera;
        [SerializeField] private RawImage renderImage;
        [SerializeField] private CanvasGroup renderImageCanvasGroup;
        [SerializeField] private Canvas canvas;

        [Header("Render Settings")]
        [SerializeField] private string tutorialLayerName;
        [SerializeField] private int sortingOrder = 30;

        private RenderTexture renderTexture;

        private readonly Dictionary<GameObject, int> originalLayers = new Dictionary<GameObject, int>();
        private readonly Dictionary<Transform, UITempData> tempUIData = new Dictionary<Transform, UITempData>();

        private void Start()
        {
            SetUpCamera();
            SetupRenderImage();
        }

        #region GameObject highlight
        public void ShowGO(GameObject targetGO)
        {
            if (ftueCamera == null || renderImage == null || targetGO == null)
            {
                Debug.LogWarning("FTUERenderTool missing required references.");
                return;
            }

            if (canvas == null)
            {
                Debug.LogWarning("FTUERenderTool must be on a GameObject with a Canvas component.");
                return;
            }

            canvas.overrideSorting = true;
            canvas.sortingOrder = sortingOrder;
            SetLayerRecursive(targetGO, LayerMask.NameToLayer(tutorialLayerName));

            if (renderImageCanvasGroup != null)
                renderImageCanvasGroup.alpha = 1;
        }

        public void HideGO(GameObject targetGO)
        {
            if (originalLayers.TryGetValue(targetGO, out int layer))
            {
                targetGO.SetLayer(layer);
                originalLayers.Remove(targetGO);
            }
        }
        #endregion

        #region UI highlight
        public void ShowUI(Transform targetUI)
        {
            if (targetUI == null)
                return;

            if (!tempUIData.ContainsKey(targetUI))
            {
                var data = new UITempData();

                // Canvas
                var canvasComp = targetUI.GetComponent<Canvas>();
                if (canvasComp == null)
                {
                    data.tempCanvas = targetUI.gameObject.AddComponent<Canvas>();
                    data.tempCanvas.overrideSorting = true;
                    data.tempCanvas.sortingOrder = sortingOrder + 1;
                }
                else
                {
                    canvasComp.overrideSorting = true;
                    canvasComp.sortingOrder = sortingOrder + 1;
                }

                // Raycaster
                var raycaster = targetUI.GetComponent<GraphicRaycaster>();
                if (raycaster == null)
                {
                    data.tempRaycaster = targetUI.gameObject.AddComponent<GraphicRaycaster>();
                }

                tempUIData[targetUI] = data;
            }

            if (ftueCamera != null)
                ftueCamera.enabled = true;

            if (renderImageCanvasGroup != null)
                renderImageCanvasGroup.alpha = 1;
        }

        public void HideUI(Transform targetUI)
        {
            if (targetUI == null)
                return;

            if (tempUIData.TryGetValue(targetUI, out var data))
            {
                if (data.tempRaycaster != null)
                    Destroy(data.tempRaycaster);

                if (data.tempCanvas != null)
                    Destroy(data.tempCanvas);

                tempUIData.Remove(targetUI);
            }
        }
        #endregion

        #region Restore
        public void HideAll()
        {
            RestoreAllLayers();
            RestoreAllUIs();
        }

        public void RestoreAllLayers()
        {
            foreach (var kvp in originalLayers)
            {
                if (kvp.Key != null)
                    kvp.Key.SetLayer(kvp.Value);
            }

            originalLayers.Clear();
        }

        public void RestoreAllUIs()
        {
            foreach (var kvp in tempUIData)
            {
                if (kvp.Value.tempRaycaster != null)
                    Destroy(kvp.Value.tempRaycaster);

                if (kvp.Value.tempCanvas != null)
                    Destroy(kvp.Value.tempCanvas);
            }

            tempUIData.Clear();
        }
        #endregion

        #region Render Mask
        public void AddRenderMask(string mask)
        {
            if (ftueCamera == null)
            {
                Debug.LogWarning("FTUERenderTool: ftueCamera is not assigned.");
                return;
            }

            int maskLayer = LayerMask.NameToLayer(mask);
            if (maskLayer == -1)
            {
                Debug.LogWarning($"FTUERenderTool: Layer '{mask}' does not exist.");
                return;
            }

            ftueCamera.cullingMask |= (1 << maskLayer);
        }

        public void RemoveRenderMask(string mask)
        {
            if (ftueCamera == null)
            {
                Debug.LogWarning("FTUERenderTool: ftueCamera is not assigned.");
                return;
            }

            int maskLayer = LayerMask.NameToLayer(mask);
            if (maskLayer == -1)
            {
                Debug.LogWarning($"FTUERenderTool: Layer '{mask}' does not exist.");
                return;
            }

            ftueCamera.cullingMask &= ~(1 << maskLayer);
        }
        #endregion

        #region Setup Camera & RenderTexture
        private void SetUpCamera()
        {
            if (ftueCamera == null)
            {
                Debug.LogWarning("FTUERenderTool: ftueCamera is not assigned.");
                return;
            }

            var mainCam = Camera.main;
            if (mainCam == null)
            {
                Debug.LogWarning("FTUERenderTool: No main camera found.");
                return;
            }

            // Copy transform
            ftueCamera.transform.SetPositionAndRotation(mainCam.transform.position, mainCam.transform.rotation);

            // Copy camera settings
            ftueCamera.clearFlags = CameraClearFlags.Depth;
            ftueCamera.orthographic = mainCam.orthographic;

            if (mainCam.orthographic)
                ftueCamera.orthographicSize = mainCam.orthographicSize;
            else
                ftueCamera.fieldOfView = mainCam.fieldOfView;

            ftueCamera.nearClipPlane = mainCam.nearClipPlane;
            ftueCamera.farClipPlane = mainCam.farClipPlane;
            ftueCamera.cullingMask = LayerMask.GetMask(tutorialLayerName); // Just render the tutorial layer
            ftueCamera.depth = mainCam.depth + 1;
        }

        private void SetupRenderImage()
        {
            if (canvas == null || ftueCamera == null || renderImage == null)
            {
                Debug.LogWarning("FTUERenderTool: Missing canvas, camera, or renderImage.");
                return;
            }

            int width = Screen.width;
            int height = Screen.height;

            // Hủy RenderTexture cũ nếu có
            if (renderTexture != null)
            {
                RenderTexture.ReleaseTemporary(renderTexture);
                renderTexture = null;
            }

            // Tạo RenderTexture mới theo kích thước của canvas
            renderTexture = RenderTexture.GetTemporary(width, height, 24, RenderTextureFormat.ARGB32);
            renderTexture.name = "FTUE_RenderTexture";

            // Gán vào camera và RawImage
            ftueCamera.targetTexture = renderTexture;
            renderImage.texture = renderTexture;
        }
        #endregion

        private void SetLayerRecursive(GameObject obj, int newLayer)
        {
            if (!originalLayers.ContainsKey(obj))
            {
                originalLayers[obj] = obj.layer;
            }

            obj.layer = newLayer;

            for (int i = 0; i < obj.transform.childCount; i++)
            {
                SetLayerRecursive(obj.transform.GetChild(i).gameObject, newLayer);
            }
        }
    }
}
