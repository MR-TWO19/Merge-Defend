using UnityEngine;

public class ScriptableObjectBase<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance;
    public static T Ins
    {
        get
        {
            if (_instance == null)
            {
                string typeName = typeof(T).Name;
                _instance = Resources.Load<T>(typeName);
                if (_instance == null)
                    Debug.LogError($"Cannot load {typeName} from Resources.");
            }
            return _instance;
        }
    }
}