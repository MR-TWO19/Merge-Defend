using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Hawky.Scene;
using Hawky.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinPopup : PopupController
{
    [SerializeField] Button btnNextLevel;
    [SerializeField] Button btnRetry;
    [SerializeField] TextMeshProUGUI txtLevel;
    [SerializeField] ProgressObstacles progressObstacles;
    [SerializeField] UIIntroductionObstacles uIIntroductionObstacles;
    [SerializeField] GameObject frameUIWin;
    public override string SceneName()
    {
        return SceneId.WIN_POPUP;
    }


    private void Start()
    {
        btnNextLevel.onClick.AddListener(OnClickNextLevel);
        btnRetry.onClick.AddListener(OnClickRetartLevel);
    }

    protected override void OnShown()
    {
        base.OnShown();

        frameUIWin.SetActive(true);
        uIIntroductionObstacles.gameObject.SetActive(false);

        txtLevel.text = $"{UserSaveData.Ins.Level}";
        SoundManager.Instance.music = 0;
        SoundManager.Instance.PlaySound(SoundId.WIN);

        btnNextLevel.transform.localScale = Vector3.zero;
        btnRetry.transform.localScale = Vector3.zero;

        progressObstacles.SetLevel(UserSaveData.Ins.Level, true, SetLevelComplete);
    }

    private void SetLevelComplete(bool isFull, UIObstacleData uIObstacleData)
    {
        if (!isFull)
        {
            btnNextLevel.transform.localScale = Vector3.one;
            btnRetry.transform.localScale = Vector3.one;
        }
        else
        {
            DOVirtual.DelayedCall(0.5f, () =>
            {
                frameUIWin.SetActive(false);
                uIIntroductionObstacles.Show(uIObstacleData, this);
            });
        }
    }

    private void OnClickNextLevel()
    {
        GameManager.Ins.NextLevel();
        Hide();
    }

    private void OnClickRetartLevel()
    {
        GameManager.Ins.RestartLevel();
        Hide();
    }
}
