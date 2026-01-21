using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSpawnPrefab : MonoBehaviour
{
    public List<GameObject> prefabs;

#if UNITY_EDITOR
    public List<GameObject> EditorPrefabs;
#endif

    private void Reset()
    {
        gameObject.name = "AutoSpawnPrefab";
    }

    void Start()
    {
        foreach (GameObject prefab in prefabs)
        {
            if (prefab != null)
                Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }

#if UNITY_EDITOR
        foreach (GameObject prefab in EditorPrefabs)
        {
            if (prefab != null)
                Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }
#endif
    }
}
