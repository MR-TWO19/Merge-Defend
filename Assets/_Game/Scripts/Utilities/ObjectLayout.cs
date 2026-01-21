using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectLayout : MonoBehaviour
{
    enum Stype
    {
        Horizontal,
        Vertical
    }

    [SerializeField, Range(0f, 10f)] float spacing = 1.0f;
    [SerializeField] bool autoUpdate = false;
    [SerializeField] Stype stype = Stype.Horizontal;

    void Start()
    {
        UpdateLayout();
    }

    private void Reset()
    {
        UpdateLayout();
    }

    private void OnValidate()
    {
        UpdateLayout();
    }

    void Update()
    {
        if (autoUpdate)
        {
            UpdateLayout();
        }
    }

    public void UpdateLayout()
    {
        int activeCount = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
                activeCount++;
        }

        if (activeCount == 0) return;

        float totalLength = spacing * (activeCount - 1);
        float startOffset = -totalLength / 2f;

        int index = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            if (!child.gameObject.activeSelf)
                continue;

            float pos = startOffset + spacing * index;
            index++;

            if (stype == Stype.Horizontal)
                child.localPosition = new Vector3(pos, 0, 0);
            else
                child.localPosition = new Vector3(0, -pos, 0);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ObjectLayout))]
public class ObjectLayoutEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ObjectLayout layout = (ObjectLayout)target;

        if (GUILayout.Button("Update Layout"))
        {
            layout.UpdateLayout();
        }
    }
}
#endif
