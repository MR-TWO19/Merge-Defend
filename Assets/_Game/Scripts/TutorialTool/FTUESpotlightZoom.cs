using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class FTUESpotlightZoom : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Material spotlightMat;   // Shader CircleHole
    [SerializeField] private Image darkScreen;        // Image full màn hình
    [SerializeField] private Canvas canvas;           // Canvas chứa darkScreen

    [Header("Zoom Settings")]
    [SerializeField] private float zoomDuration;
    [SerializeField] private float minRadius;
    [SerializeField] private float maxRadius;

    private Material runtimeMat;
    private Coroutine zoomRoutine;

    private void Awake()
    {
        if (darkScreen != null && spotlightMat != null)
        {
            runtimeMat = new Material(spotlightMat);
            darkScreen.material = runtimeMat;
            runtimeMat.SetFloat("_HoleRadius", 0f); // spotlight ẩn
        }
    }

    public void FocusOnTarget(Transform target, float radius = -1f)
    {
        if (runtimeMat == null || target == null || canvas == null) return;

        gameObject.SetActive(true);

        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPoint
        );

        Vector2 uv = new ( (localPoint.x  / 2f), (localPoint.y / 2f) );
        runtimeMat.SetVector("_HoleCenter", uv);


        // radius
        Renderer rend = target.GetComponent<Renderer>();
        float worldRadius = Mathf.Max(rend.bounds.size.x, rend.bounds.size.z) / 3f;

        Vector3 screenCenter = Camera.main.WorldToScreenPoint(rend.bounds.center);
        Vector3 screenRight = Camera.main.WorldToScreenPoint(rend.bounds.center + Vector3.right * worldRadius);
        float pixelRadius = (screenRight - screenCenter).magnitude;

        // start zoom
        if (zoomRoutine != null)
            StopCoroutine(zoomRoutine);
        zoomRoutine = StartCoroutine(ZoomToHole(pixelRadius));
    }

    public void FocusOnTarget(RectTransform target, float radius = -1f)
    {
        if (runtimeMat == null || target == null || canvas == null) return;

        gameObject.SetActive(true);

        Vector3 screenPos;

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            Vector3[] corners = new Vector3[4];
            target.GetWorldCorners(corners);
            screenPos = (corners[0] + corners[2]) * 0.5f;
        }
        else
        {
            Vector3[] corners = new Vector3[4];
            target.GetWorldCorners(corners);
            Vector3 worldCenter = (corners[0] + corners[2]) * 0.5f;
            screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, worldCenter);
        }

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPoint
        );

        Vector2 uv = new (localPoint.x /2,localPoint.y / 2);
        runtimeMat.SetVector("_HoleCenter", uv);

        float pixelRadius = (radius > 0)
            ? radius
            : (target.rect.width * 0.5f);

        if (zoomRoutine != null)
            StopCoroutine(zoomRoutine);
        zoomRoutine = StartCoroutine(ZoomToHole(pixelRadius));
    }


    private IEnumerator ZoomToHole(float targetRadius)
    {
        float time = 0f;
        runtimeMat.SetFloat("_HoleRadius", targetRadius*10);
        float startRadius = runtimeMat.GetFloat("_HoleRadius");
        while (time < zoomDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, time / zoomDuration);
            float radius = Mathf.Lerp(startRadius, targetRadius, t); // dùng targetRadius
            runtimeMat.SetFloat("_HoleRadius", radius);
            yield return null;
        }
    }

    /// <summary>
    /// Ẩn spotlight
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
        if (zoomRoutine != null)
            StopCoroutine(zoomRoutine);

        if (runtimeMat != null)
            runtimeMat.SetFloat("_HoleRadius", 0f);
    }

    /// <summary>
    /// Update vị trí spotlight theo target động
    /// </summary>
    public void UpdateTargetPosition(Transform target)
    {
        if (runtimeMat == null || target == null || canvas == null) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);
        if (screenPos.z < 0) return;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out localPoint
        );

        Vector2 uv = new Vector2(
            (localPoint.x / canvasRect.sizeDelta.x) + 0.5f,
            (localPoint.y / canvasRect.sizeDelta.y) + 0.5f
        );

        uv.y = 1f - uv.y;
        runtimeMat.SetVector("_HoleCenter", uv);
    }
}
