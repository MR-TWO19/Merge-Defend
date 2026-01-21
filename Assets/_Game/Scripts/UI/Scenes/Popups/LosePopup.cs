using System.Collections;
using System.Collections.Generic;
using Hawky.Scene;
using Hawky.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LosePopup : PopupController
{
    [SerializeField] Button btnTryAgain;
    [SerializeField] TextMeshProUGUI txtLevel;
    [SerializeField] ProgressObstacles progressObstacles;
    public override string SceneName()
    {
        return SceneId.LOSE_POPUP;
    }

    protected override void OnShown()
    {
        base.OnShown();
        txtLevel.text = $"{UserSaveData.Ins.Level}";
        SoundManager.Instance.music = 0;
        SoundManager.Instance.PlaySound(SoundId.LOSE);

        btnTryAgain.transform.localScale = Vector3.zero;

        progressObstacles.SetLevel(UserSaveData.Ins.Level, false, (bool isFull, UIObstacleData uIObstacleData) =>
        {
            btnTryAgain.transform.localScale = Vector3.one;
        });
    }

    private void Start()
    {
        btnTryAgain.onClick.AddListener(OnClickTryAgain);
    }

    private void OnClickTryAgain()
    {
        GameManager.Ins.RestartLevel();
        Hide();
    }
}
