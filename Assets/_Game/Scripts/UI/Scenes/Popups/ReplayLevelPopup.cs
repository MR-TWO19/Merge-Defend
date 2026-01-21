using Hawky.Scene;
using UnityEngine;
using UnityEngine.UI;

public class ReplayLevelPopup : PopupController
{
    [SerializeField] Button btnHide;
    [SerializeField] Button btnYes;
    [SerializeField] Button btnNo;
    public override string SceneName()
    {
        return SceneId.REPLAY_LEVEL_POPUP;
    }

    private void Start()
    {
        btnHide.onClick.AddListener(Hide);
        btnNo.onClick.AddListener(Hide);
        btnYes.onClick.AddListener(YesOnClick);
    }

    private void YesOnClick()
    {
        Hide();
        GameManager.Ins.RestartLevel();
    }

    protected override void OnHidden()
    {
        base.OnHidden();
        GameManager.Ins.InputHandler.UnlockInput();
    }
}
