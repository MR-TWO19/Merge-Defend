using Hawky;
using Hawky.Scene;
using System.Collections;
using TwoCore;
using UnityEngine;

public class TutorialManager : SingletonMono<TutorialManager>
{
    public static TutorialManager Instance => Ins;

    [HideInInspector] public TutorialPopup TutorialPopup;
    [HideInInspector] public Tutorial CurrTutorial;

    [HideInInspector] public ItemControl ItemControlRequiresClick;

    public void StartTutorial(Tutorial tutorial)
    {
        CurrTutorial = tutorial;

        //CoroutineManager.Ins.Start(OpenPopup());

        //IEnumerator OpenPopup()
        //{
        //    RDM.Ins.TutorialPopupModel = new ColorFight.TutorialPopupModel();
        //    SceneManager.Instance.OpenPopup(SceneId.TUTORIAL_POPUP);
        //    yield return new WaitUntil(() => RDM.Ins.TutorialPopupModel.ReturnData.Returned != ReturnId.WAIT);
        //    SceneManager.Instance.ClosePopup(SceneId.TUTORIAL_POPUP);
        //}
    }

    public void NextStep()
    {
        if (TutorialPopup != null && TutorialPopup.gameObject.activeSelf)
            TutorialPopup.NextStep();
    }

    public void NextStep(ItemControl itemControl)
    {
        if (TutorialPopup != null && TutorialPopup.gameObject.activeSelf && ItemControlRequiresClick == itemControl)
            TutorialPopup.NextStep();
    }

}
