using TwoCore;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameConfigEditorWindow : BaseEditorWindow
{
    #region static
    public static GameConfigEditorWindow Ins { get; private set; }

    [MenuItem("TwoCore/Game Setting &l", false, 1)]
    private static void OnMenuItemClicked()
    {
        //GameConfig.CreateAsset(typeof(GameConfig), "GameConfig.asset");
        OpenWindow();
    }

    public static void OpenWindow()
    {
        Ins = (GameConfigEditorWindow)GetWindow(typeof(GameConfigEditorWindow), false, "Game Setting");
        Ins.autoRepaintOnSceneChange = true;
    }
    #endregion

    #region template
    protected TabContainer tabContainer;
    protected GameConfig gameConfig;

    protected override Object GetTarget() => GameConfig.Ins;

    protected override void OnEnable()
    {
        base.OnEnable();
        Ins = this;
        gameConfig = GameConfig.Ins;
        tabContainer = new TabContainer();
        _Init();
    }

    protected override void OnDraw()
    {
        Undo.RecordObject(gameConfig, "Game Config");

        _OnDraw();

        if (GUI.changed)
            EditorUtility.SetDirty(GameConfig.Ins);
    }
    #endregion

    private void _Init()
    {
        // create view here
        tabContainer.AddTab("Global", new GlobalTab());
        tabContainer.AddTab("Items", new ItemTab());
        tabContainer.AddTab("Locks", new LockTab());
    }

    private void _OnDraw()
    {
        tabContainer.DoDraw();

        // customize draw here
    }
}