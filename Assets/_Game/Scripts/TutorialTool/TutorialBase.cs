using System.Collections;

public abstract class TutorialBase
{
    protected TutorialPopup popup;

    protected TutorialBase(TutorialPopup popup)
    {
        this.popup = popup;
    }

    public abstract IEnumerator OnStep(int stepIndex);
}