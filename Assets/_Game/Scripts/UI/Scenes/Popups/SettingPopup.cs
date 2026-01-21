using System.Collections;
using System.Collections.Generic;
using Game;
using Hawky.Scene;
using Hawky.Sound;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : PopupController
{
    [SerializeField] Button btnHide;
    [SerializeField] UIToggle toggleSound;
    [SerializeField] UIToggle toggleMusic;
    [SerializeField] UIToggle toggleHaptics;

    public override string SceneName()
    {
        return SceneId.SETTINGS_POPUP;
    }

    private void Start()
    {
        btnHide.onClick.AddListener(Hide);
        toggleSound.AddListener(ToggleSoundChange);
        toggleMusic.AddListener(ToggleSoundMusic);
        toggleHaptics.AddListener(ToggleSoundHaptic);

        Init();
    }

    private void Init()
    {
        toggleSound.SetValue(UserSaveData.Ins.OnSound);
        toggleMusic.SetValue(UserSaveData.Ins.OnMusic);
        toggleHaptics.SetValue(UserSaveData.Ins.OnHaptic);
    }

    private void ToggleSoundChange(bool isOn)
    {
        UserSaveData.Ins.SetOnSound(isOn);
        SoundManager.Instance.sound = isOn ? SoundManager.Instance.soundVolumeDefault : 0;
    }

    private void ToggleSoundMusic(bool isOn)
    {
        UserSaveData.Ins.SetOnMusic(isOn);
        SoundManager.Instance.music = isOn ? SoundManager.Instance.musicVolumeDefault : 0;

        if (isOn)
        {
            SoundManager.Instance.PlayBackground(SoundId.BG, SoundManager.Instance.musicVolumeDefault);
        }
    }

    private void ToggleSoundHaptic(bool isOn)
    {
        UserSaveData.Ins.SetOnHaptic(isOn);
    }

    protected override void OnHidden()
    {
        base.OnHidden();
        GameManager.Instance.InputHandler.UnlockInput();
    }
}
