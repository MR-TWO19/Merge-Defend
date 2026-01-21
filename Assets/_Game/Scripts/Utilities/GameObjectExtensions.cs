using UnityEngine;

public static class GameObjectExtensions
{
    public static void SetLayer(this GameObject obj, int layer)
    {
        if (obj == null) return;
        obj.layer = layer;

        var transform = obj.transform;
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child != null)
                child.gameObject.SetLayer(layer);
        }
    }
}
