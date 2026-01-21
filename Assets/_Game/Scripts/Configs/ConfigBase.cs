using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigBase<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance;
    public static T Ins
    {
        get
        {
            if (_instance == null)
            {
                string typeName = typeof(T).Name;
                string fullPath = $"Configs/{typeName}";
                _instance = Resources.Load<T>(fullPath);
                if (_instance == null)
                    Debug.LogError($"Cannot load {fullPath} from Resources.");
            }
            return _instance;
        }
    }
}
