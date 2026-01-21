using System.Collections;
using UnityEngine;

public class GamePlayIntroductionTutorial : TutorialBase
{
    public GamePlayIntroductionTutorial(TutorialPopup popup) : base(popup) { }

    public override IEnumerator OnStep(int stepIndex)
    {
        switch (stepIndex)
        {
            case 1: yield return Step1(); break;
            case 2: yield return Step2(); break;
            default: popup.EndFTUE(); yield break;
        }
    }

    private IEnumerator Step1()
    {
        ItemControl itemControll = GameManager.Instance.ItemManager.GetItemControl(0);
        if (itemControll == null) yield break;

        TutorialManager.Instance.ItemControlRequiresClick = itemControll;

        popup.DarkScreen.Show();
        popup.RenderTool.ShowGO(itemControll.gameObject);

        Canvas canvas = popup.Canvas;
        popup.ArrowUI.SetPositionFromWorld(itemControll.transform.position, canvas);
        popup.ArrowUI.PointAtWorldObject(itemControll.transform, canvas);
        popup.ArrowUI.PlayPulse();

        yield return new WaitUntil(() => popup.IsPaused());
        popup.ArrowUI.Hide();
        popup.Resume();
    }

    private IEnumerator Step2()
    {
        ItemControl itemControll = GameManager.Instance.ItemManager.GetItemControl(1);
        if (itemControll == null) yield break;

        TutorialManager.Instance.ItemControlRequiresClick = itemControll;

        popup.DarkScreen.Show();
        popup.RenderTool.ShowGO(itemControll.gameObject);

        Canvas canvas = popup.Canvas;
        popup.ArrowUI.SetPositionFromWorld(itemControll.transform.position, canvas);
        popup.ArrowUI.PointAtWorldObject(itemControll.transform, canvas);
        popup.ArrowUI.PlayPulse();

        yield return new WaitUntil(() => popup.IsPaused());
        popup.ArrowUI.Hide();
        popup.Resume();
    }
}
