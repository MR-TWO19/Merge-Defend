using Doozy.Engine;
using Doozy.Engine.UI;
using System;
using TwoCore;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : BasePopup
{
    [SerializeField] private UIButton soundBtn, musicBtn, homeBtn;
    [SerializeField] private Image imgSound, imgMusic;
    [SerializeField] private Sprite SoundLight, SoundDark;

    private static SettingPopup _instance;

    private Action onHide2 = null;

    public static SettingPopup Show(bool isHome, Action action = null)
    {
        if (_instance == null)
        {
            _instance = ShowWithParamsAndMethod<SettingPopup>("SettingPopup", PopupShowMethod.QUEUE, isHome, action);
        }
        else
        {
            UIPopupManager.ShowPopup(_instance.UIPopup, true, false);
            _instance.SetParams(isHome, action);
        }

        return _instance;
    }

    protected override void SetParams(params object[] @params)
    {
        base.SetParams(@params);

        bool isHome = (bool)@params[0];
        if(isHome)
        {
            onHide2 = (Action)@params[1];
        }

        homeBtn.gameObject.SetActive(!isHome);
    }

    protected override void OnHide()
    {
        base.OnHide();
        onHide2?.Invoke();
    }

    public static void HidePopup()
    {
        _instance.Hide();
    }

    private void Start()
    {
        soundBtn.OnClick.OnTrigger.Event.AddListener(BtnSoundOnClick);
        musicBtn.OnClick.OnTrigger.Event.AddListener(BtnMusicOnClick);
        homeBtn.OnClick.OnTrigger.Event.AddListener(OnClickHome);
    }

    private void BtnSoundOnClick()
    {
        UserSaveData.Ins.SfxOn = !UserSaveData.Ins.SfxOn;
        UserSaveData.Ins.Save();
        imgSound.color = UserSaveData.Ins.SfxOn ? Color.white : Color.gray;

        if (UserSaveData.Ins.SfxOn) Soundy.ToUnmute(Soundy.SfxParam);
        else Soundy.ToMute(Soundy.SfxParam);
    }

    private void BtnMusicOnClick()
    {
        UserSaveData.Ins.MusicOn = !UserSaveData.Ins.MusicOn;
        UserSaveData.Ins.Save();
        imgMusic.color = UserSaveData.Ins.MusicOn ? Color.white : Color.gray;

        if (UserSaveData.Ins.MusicOn) Soundy.ToUnmute(Soundy.MusicParam);
        else Soundy.ToMute(Soundy.MusicParam);
    }

    protected override void OnShow()
    {
        base.OnShow();

        imgSound.color = UserSaveData.Ins.SfxOn ? Color.white : Color.gray;
        imgMusic.color = UserSaveData.Ins.MusicOn ? Color.white : Color.gray;
    }

    private void OnClickHome()
    {
        GameEventMessage.SendEvent("GoToMain", null);
        GameManager.Ins.ReletAll();
        Hide();
    }
}
