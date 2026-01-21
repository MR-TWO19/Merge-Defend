using UnityEditor;
using UnityEngine;
using Doozy.Editor.Nody.Settings;

public class CreateNodyWindowSettings
{
    [MenuItem("Doozy/Nody/Create NodyWindowSettings")]
    public static void CreateSettingsAsset()
    {
        // Gọi Instance → sẽ tự tạo asset nếu chưa tồn tại
        var settings = NodyWindowSettings.Instance;

        Debug.Log("NodyWindowSettings.asset đã được tạo tại: " + "Assets/_Game/Graphs/NodyWindowSettings.asset");

        // Chọn asset trong Project Window
        Selection.activeObject = settings;
    }
}
