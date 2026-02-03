using Doozy.Engine;
using Doozy.Engine.UI;
using TMPro;
using TwoCore;
using UnityEngine;

public class ResultPopup : BasePopup
{
    [SerializeField] private GameObject frameWin;
    [SerializeField] private GameObject frameLose;
    [SerializeField] private UIButton btnHomeWin;
    [SerializeField] private UIButton btnHomeLose;
    [SerializeField] private UIButton btnNextLevel;
    [SerializeField] private TextMeshProUGUI txtCoin;

    private bool _isWin;

    public static ResultPopup Show(bool isWin)
    {
        var popup = ShowWithParamsAndMethod<ResultPopup>("ResultPopup", PopupShowMethod.NO_QUEUE, isWin);
        return popup;
    }

    protected override void SetParams(params object[] @params)
    {
        if (@params != null && @params.Length > 0 && @params[0] is bool w)
            _isWin = w;
    }

    protected override void OnShow()
    {
        SoundManager.Ins.PlayBGMusic();

        if (frameWin) frameWin.SetActive(_isWin);
        if (frameLose) frameLose.SetActive(!_isWin);

        btnNextLevel.OnClick.OnTrigger.Event.AddListener(NextLevel);
        btnHomeWin.OnClick.OnTrigger.Event.AddListener(GoHome);
        btnHomeLose.OnClick.OnTrigger.Event.AddListener(GoHome);

        if(_isWin)
        {

            txtCoin.text = $"+ {GameManager.Ins.LevelData.CoinWin}";
            UserSaveData.Ins.AddCoin(GameManager.Ins.LevelData.CoinWin);
        }
        else
        {

            txtCoin.text = $"+ {GameManager.Ins.LevelData.CoinLose}";
            UserSaveData.Ins.AddCoin(GameManager.Ins.LevelData.CoinLose);
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        Debug.Log("ResultPopup hidden!");
        btnNextLevel.OnClick.OnTrigger.Event.RemoveAllListeners();
        btnHomeWin.OnClick.OnTrigger.Event.RemoveAllListeners();
        btnHomeLose.OnClick.OnTrigger.Event.RemoveAllListeners();
    }


    private void NextLevel()
    {
        GameManager.Ins.NextLevel();
        Hide();
    }

    private void GoHome()
    {
        GameEventMessage.SendEvent("GoToMain", null);
        GameManager.Ins.ReletAll();
        Hide();
    }
}
