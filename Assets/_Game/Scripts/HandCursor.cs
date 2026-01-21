#if UNITY_EDITOR
using DG.Tweening;
using UnityEngine;

public class HandCursor : MonoBehaviour
{
    [Header("References")]
    public Canvas canvas;
    public RectTransform fingerRect;
    public Vector2 offset = Vector2.zero;

    [Header("Animation")]
    public float clickScale = 0.85f;
    public float animDuration = 0.1f;

    private Tween scaleTween;

    void Update()
    {
        Vector2 screenPos = Input.mousePosition + (Vector3)offset;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPos
        );

        fingerRect.anchoredPosition = localPos;

        if (Input.GetMouseButtonDown(0))
        {
            scaleTween?.Kill();
            scaleTween = fingerRect.DOScale(clickScale, animDuration).SetEase(Ease.OutQuad);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            scaleTween?.Kill();
            scaleTween = fingerRect.DOScale(1f, animDuration).SetEase(Ease.OutBack);
        }
    }


}
#endif