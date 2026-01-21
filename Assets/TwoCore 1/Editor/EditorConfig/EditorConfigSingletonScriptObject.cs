using System.IO;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EditorConfigSingletonScriptObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance = null;
    public static T Ins
    {
        get
        {
            if (!_instance)
            {
                _instance = Resources.Load($"{typeof(T).Name}") as T;
#if UNITY_EDITOR
                if (!_instance) CreateAsset();
#endif
            }
            return _instance;
        }
    }

    public static void Reload()
    {
        _instance = Resources.Load($"{typeof(T).Name}") as T;
    }

#if UNITY_EDITOR
    private const string RESOURCE_DIR = "Assets/_Game/Editor/Resources/";
    public static void CreateAsset()
    {
        if (!Directory.Exists(RESOURCE_DIR))
            Directory.CreateDirectory(RESOURCE_DIR);

        var filepath = RESOURCE_DIR + typeof(T).Name + ".asset";
        if (File.Exists(filepath))
        {
            T _instance = Resources.Load($"{typeof(T).Name}") as T;
            Selection.activeObject = _instance;
            return;
        }

        if (!_instance)
        {
            ScriptableObject asset = CreateInstance(typeof(T));
            AssetDatabase.CreateAsset(asset, filepath);
            AssetDatabase.SaveAssets();
            Selection.activeObject = asset;
            Reload();
        }
    }
#endif
}
