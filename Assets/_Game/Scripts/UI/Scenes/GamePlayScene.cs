//using System.Collections;
//using Hawky.Scene;
//using ColorFight;
//using UnityEngine;
//using UnityEngine.UI;

//public class GamePlayScene : Controller
//{
//    [SerializeField] private Button btnSetting;
//    [SerializeField] private Button btnRetry;
//    public override string SceneName()
//    {
//        return SceneId.SCENE_GAME_PLAY;
//    }

//    protected override void OnAwake()
//    {
//        base.OnAwake(); 
//        GameManager.Instance.StartGame();
        
//    }

//    protected override void OnShown()
//    {
//        base.OnShown();
//        //btnSetting.onClick.AddListener(OnPauseSettingClick);
//        //btnRetry.onClick.AddListener(RetryOnClick);
//    }

//    protected override void OnHidden()
//    {
//        base.OnHidden();
//        //btnSetting.onClick.RemoveListener(OnPauseSettingClick);
//        //btnRetry.onClick.RemoveListener(RetryOnClick);
//    }

//    private void OnPauseSettingClick()
//    {

//    }

//    private void RetryOnClick()
//    {

//    }
//}
