using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Editor")) LevelConfigEditorWindow.OpenWindow();
    }
}
