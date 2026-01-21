using System.Collections;
using Hawky.Scene;
using ColorFight;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : MonoBehaviour
{
    [SerializeField] private Button btnSetting;
    [SerializeField] private Button btnRetry;

    public TextMeshProUGUI txtLevel;

    private void Awake()
    {
        btnSetting.onClick.AddListener(OnPauseSettingClick);
        btnRetry.onClick.AddListener(RetryOnClick);
    }

    public void UpdateTextLevel()
    {
        txtLevel.text = $"Lv.{UserSaveData.Ins.Level}";
    }


    private void OnPauseSettingClick()
    {
        //CoroutineManager.Ins.Start(OpenPauseSettingPopup());

        //IEnumerator OpenPauseSettingPopup()
        //{
        //    RDM.Ins.SettingPopupModel = new SettingPopupModel();
        //    SceneManager.Instance.OpenPopup(SceneId.SETTINGS_POPUP);
        //    GameManager.Instance.InputHandler.LockInput();
        //    yield return new WaitUntil(() => RDM.Ins.SettingPopupModel.ReturnData.Returned != ReturnId.WAIT);
        //    SceneManager.Instance.ClosePopup(SceneId.SETTINGS_POPUP);
        //}
    }

    private void RetryOnClick()
    {
        //CoroutineManager.Ins.Start(OpenPauseSettingPopup());

        //IEnumerator OpenPauseSettingPopup()
        //{
        //    RDM.Ins.ReplayLevelPopupModel = new ReplayLevelPopupModel();
        //    GameManager.Instance.InputHandler.LockInput();
        //    SceneManager.Instance.OpenPopup(SceneId.REPLAY_LEVEL_POPUP);
        //    yield return new WaitUntil(() => RDM.Ins.ReplayLevelPopupModel.ReturnData.Returned != ReturnId.WAIT);
        //    SceneManager.Instance.ClosePopup(SceneId.REPLAY_LEVEL_POPUP);
        //    GameManager.Instance.InputHandler.UnlockInput();
        //}
    }
}
