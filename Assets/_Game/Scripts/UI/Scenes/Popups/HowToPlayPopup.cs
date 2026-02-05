using Doozy.Engine.UI;
using TwoCore;
using UnityEngine;

public class HowToPlayPopup : BasePopup
{
    [SerializeField] private UIButton btnClose;

    private void Start()
    {
        btnClose.OnClick.OnTrigger.Event.AddListener(() =>
        {
            Hide();
        });
    }

    public static HowToPlayPopup Show()
    {
        var popup = Show<HowToPlayPopup>("HowToPlayPopup", PopupShowMethod.QUEUE);
        return popup;
    }
}
