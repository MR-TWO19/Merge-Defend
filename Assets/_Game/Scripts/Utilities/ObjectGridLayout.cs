using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class ObjectGridLayout : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField, Min(1)] int columns = 3;
    [SerializeField] float spacingX = 1f;
    [SerializeField] float spacingY = 1f;
    [SerializeField] bool autoUpdate = true;

    [Header("Gizmo Settings")]
    [SerializeField] bool showGizmos = true;
    [SerializeField] Color gizmoColor = Color.cyan;

    private void OnValidate()
    {
        UpdateLayout();
    }

    private void Update()
    {
        if (autoUpdate)
            UpdateLayout();
    }

    public void UpdateLayout()
    {
        int activeCount = 0;
        for (int i = 0; i < transform.childCount; i++)
            if (transform.GetChild(i).gameObject.activeSelf)
                activeCount++;

        if (activeCount == 0) return;

        int rows = Mathf.CeilToInt((float)activeCount / columns);

        float totalWidth = spacingX * (columns - 1);
        float totalHeight = spacingY * (rows - 1);

        int index = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (!child.gameObject.activeSelf)
                continue;

            int col = index % columns;
            int row = index / columns;

            float x = -totalWidth / 2f + col * spacingX;
            float y = totalHeight / 2f - row * spacingY;

            child.localPosition = new Vector3(x, y, 0);
            index++;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = gizmoColor;

        int activeCount = 0;
        for (int i = 0; i < transform.childCount; i++)
            if (transform.GetChild(i).gameObject.activeSelf)
                activeCount++;

        if (activeCount == 0) return;

        int rows = Mathf.CeilToInt((float)activeCount / columns);
        float totalWidth = spacingX * (columns - 1);
        float totalHeight = spacingY * (rows - 1);

        Vector3 center = transform.position;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Vector3 pos = center + transform.right * (-totalWidth / 2f + c * spacingX)
                                      + transform.up * (totalHeight / 2f - r * spacingY);
                Gizmos.DrawWireCube(pos, Vector3.one * 0.1f);
            }
        }

        Gizmos.DrawWireCube(center, new Vector3(totalWidth, totalHeight, 0));
    }
#endif
}
