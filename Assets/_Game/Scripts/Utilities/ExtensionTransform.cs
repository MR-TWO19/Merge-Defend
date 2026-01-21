using UnityEngine;

public static class ExtensionTransform
{
    public static void AttachTo(this Transform transform, Transform parent, Vector3 scale, Vector3 position = default, Quaternion rotation = default)
    {
        transform.SetParent(parent);
        transform.SetLocalPositionAndRotation(position, rotation);
        transform.localScale = scale;
    }

    public static void DeleteAllChild<T>(this T target, int start = 0, int end = 0) where T : Transform
    {
        for (int i = target.childCount - 1 - end; i >= start; i--)
        {
            UnityEngine.Object.Destroy(target.GetChild(i).gameObject);
        }
    }

    public static void DeleteAllChildImmediate<T>(this T target, int start = 0, int end = 0) where T : Transform
    {
        for (int i = target.childCount - 1 - end; i >= start; i--)
        {
            UnityEngine.Object.DestroyImmediate(target.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Convert RectTransform position (pivot) to world position in another camera space
    /// </summary>
    public static Vector3 ToWorldPosition(this RectTransform rt, Canvas canvas, Camera worldCamera, float desiredWorldZ = 0f)
    {
        if (rt == null || canvas == null || worldCamera == null)
            return Vector3.zero;

        Camera canvasCamera = (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            ? canvas.worldCamera
            : null;

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(canvasCamera, rt.position);
        Vector3 worldPoint = worldCamera.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, worldCamera.nearClipPlane));
        worldPoint.z = desiredWorldZ;

        return worldPoint;
    }

    public static Vector3 GetWorldCenter(this RectTransform rt)
    {
        return rt.TransformPoint(rt.rect.center);
    }

    /// <summary>
    /// Get world position of rect corner.
    /// Order of corners: 
    /// 0 = bottom-left, 1 = top-left, 2 = top-right, 3 = bottom-right
    /// </summary>
    public static Vector3 GetWorldCorner(this RectTransform rt, int index)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        return corners[Mathf.Clamp(index, 0, 3)];
    }

    public static Vector3 GetWorldEdge(this RectTransform rt, RectEdge edge)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        switch (edge)
        {
            case RectEdge.Left: return (corners[0] + corners[1]) * 0.5f;   // bottom-left + top-left
            case RectEdge.Right: return (corners[2] + corners[3]) * 0.5f;  // top-right + bottom-right
            case RectEdge.Top: return (corners[1] + corners[2]) * 0.5f;    // top-left + top-right
            case RectEdge.Bottom: return (corners[0] + corners[3]) * 0.5f; // bottom-left + bottom-right
            default: return rt.position;
        }
    }

    public static Vector2 GetAnchoredPositionTo(this Transform worldSource, RectTransform target, Canvas canvas)
    {
        if (target == null || target.parent == null)
            return Vector2.zero;

        var parentRect = target.parent as RectTransform;
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, worldSource.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            screenPoint,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        return localPoint;
    }

    public static Vector2 GetAnchoredPosition(this Transform worldSource, Canvas canvas)
    {
        if (worldSource == null || canvas == null)
            return Vector2.zero;

        Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(cam, worldSource.position);

        RectTransform canvasRect = (RectTransform)canvas.transform;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPoint,
            cam,
            out Vector2 localPoint
        );

        return localPoint;
    }

    public static Vector2 GetAnchoredPositionTo(this RectTransform source, RectTransform target, Canvas canvas)
    {
        if (target == null || source == null || target.parent == null)
            return Vector2.zero;

        var parentRect = target.parent as RectTransform;
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, source.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (
                parentRect,
                screenPoint,
                canvas.worldCamera,
                out Vector2 localPoint
            );

        return localPoint;
    }
}

public enum RectEdge
{
    Left,
    Right,
    Top,
    Bottom
}
