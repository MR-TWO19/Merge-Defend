using TwoCore;
using UnityEditor;
using UnityEngine;

public class LevelConfigEditorWindow : BaseEditorWindow
{
    #region static
    public static LevelConfigEditorWindow Ins { get; private set; }

    [MenuItem("TwoCore/Level Config &v", false, 1)]
    private static void OnMenuItemClicked()
    {
        //LevelConfig.CreateAsset(typeof(LevelConfig), "LevelConfig.asset");
        OpenWindow();
    }

    public static void OpenWindow()
    {
        Ins = (LevelConfigEditorWindow)GetWindow(typeof(LevelConfigEditorWindow), false, "Level Setting");
        Ins.autoRepaintOnSceneChange = true;
    }
    #endregion

    #region template
    protected TabContainer tabContainer;
    protected LevelConfig levelConfig;

    protected override Object GetTarget() => LevelConfig.Ins;

    protected override void OnEnable()
    {
        base.OnEnable();
        Ins = this;
        levelConfig = LevelConfig.Ins;
        tabContainer = new TabContainer();
        _Init();
    }

    protected override void OnDraw()
    {
        Undo.RecordObject(levelConfig, "Level Config");

        _OnDraw();

        if (GUI.changed)
            EditorUtility.SetDirty(LevelConfig.Ins);
    }
    #endregion

    private void _Init()
    {
        // create view here
        tabContainer.AddTab("Level", new LevelTab());
    }

    private void _OnDraw()
    {
        tabContainer.DoDraw();

        // customize draw here
    }
}
