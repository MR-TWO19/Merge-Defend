using Doozy.Engine.UI;
using System;
using TwoCore;
using UnityEngine;

public class HowToPlayPopup : BasePopup
{
    [SerializeField] private UIButton btnClose;

    public Action onHide2;

    private void Start()
    {
        btnClose.OnClick.OnTrigger.Event.AddListener(() =>
        {
            Hide();
        });
    }

    public static HowToPlayPopup Show(Action action = null)
    {
        var popup = ShowWithParamsAndMethod<HowToPlayPopup>("HowToPlayPopup", PopupShowMethod.QUEUE, action);
        return popup;
    }

    protected override void SetParams(params object[] @params)
    {
        base.SetParams(@params);

        onHide2 = (Action)@params[1];
    }

    override protected void OnHide()
    {
        base.OnHide();
        onHide2?.Invoke();
    }
}
