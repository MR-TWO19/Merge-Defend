using Doozy.Engine.UI;
using TwoCore;
using UnityEngine;

public class MainView : BaseView
{
    [SerializeField] private UIButton btnSetting;
    [SerializeField] private UIButton btnHowToPlay;
    [SerializeField] private UIButton btnPlay;
    [SerializeField] private UIButton btnBoss;
    [SerializeField] private UIButton btnUpgrade;
    [SerializeField] private GameObject objHero;


    protected override void Awake()
    {
        base.Awake();
        btnSetting.OnClick.OnTrigger.Event.AddListener(() =>
        {
            objHero.gameObject.SetActive(false);
            SettingPopup.Show(true, () => { objHero.gameObject.SetActive(true); });
        });

        btnHowToPlay.OnClick.OnTrigger.Event.AddListener(() =>
        {
            objHero.gameObject.SetActive(false);
            HowToPlayPopup.Show(() => { objHero.gameObject.SetActive(true); });
        });
        btnPlay.OnClick.OnTrigger.Event.AddListener(() =>
        {
            GameManager.Ins.StartGame(false);
        });

        btnUpgrade.OnClick.OnTrigger.Event.AddListener(() =>
        {
            objHero.gameObject.SetActive(false);
            UpgradePopup.Show(() => { objHero.gameObject.SetActive(true); });
        });
        btnBoss.OnClick.OnTrigger.Event.AddListener(() =>
        {
            GameManager.Ins.StartGame(true);
        });
    }

    protected override void OnShow()
    {
        base.OnShow();

        if (UserSaveData.Ins.MusicOn) Soundy.ToUnmute(Soundy.MusicParam);
        else Soundy.ToMute(Soundy.MusicParam);

        if (UserSaveData.Ins.SfxOn) Soundy.ToUnmute(Soundy.SfxParam);
        else Soundy.ToMute(Soundy.SfxParam);

        SoundManager.Ins.PlayBGMusic();
    }

}
