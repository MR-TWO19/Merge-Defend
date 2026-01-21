using System.Collections;
using System.Collections.Generic;
using Hawky.Scene;
using Hawky.Sound;
using UnityEngine;

public class InitScene : MonoBehaviour
{
    void Start()
    {
        //SceneManager.Instance.SetSceneLoader(new PrefabLoader("Scenes", transform));

        //RDM.Ins.LoadingPopupModel = new ColorFight.LoadingPopupModel();

        //SceneManager.Instance.OpenPopup(SceneId.LOADING_POPUP);
        //CoroutineManager.Ins.Start(WaitPauseSettingPopup());

        //IEnumerator WaitPauseSettingPopup()
        //{
        //    yield return new WaitUntil(() => RDM.Ins.LoadingPopupModel.ReturnData.Returned != ReturnId.WAIT);
        //    SceneManager.Instance.OpenScene(SceneId.SCENE_GAME_PLAY);
        //    InitSound();
        //}
    }

    private void InitSound()
    {
        var userSaveData = UserSaveData.Ins;
        SoundManager.Instance.music = userSaveData.OnMusic ? SoundManager.Instance.musicVolumeDefault : 0;
        SoundManager.Instance.sound = userSaveData.OnSound ? SoundManager.Instance.soundVolumeDefault : 0;
        if(userSaveData.OnMusic)
        {
            SoundManager.Instance.PlayBackground(SoundId.BG, SoundManager.Instance.musicVolumeDefault);
        }
    }
}
