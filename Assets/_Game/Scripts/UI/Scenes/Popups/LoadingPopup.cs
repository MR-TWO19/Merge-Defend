using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Hawky.Scene;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPopup : PopupController
{
    [SerializeField] private Image imgFill;
    [SerializeField] private TextMeshProUGUI txtLoading;

    private Coroutine loadingTextRoutine;

    public override string SceneName()
    {
        return SceneId.LOADING_POPUP;
    }

    protected override void OnShown()
    {
        base.OnShown();
        RunLoading();
    }

    private void RunLoading()
    {
        imgFill.fillAmount = 0;
        txtLoading.text = "Loading 0%";

        if (loadingTextRoutine != null)
            StopCoroutine(loadingTextRoutine);

        imgFill.DOFillAmount(1, 1f)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                int percent = Mathf.RoundToInt(imgFill.fillAmount * 100);
                txtLoading.text = $"Loading {percent}%";
            })
            .OnComplete(() =>
            {
                if (loadingTextRoutine != null)
                    StopCoroutine(loadingTextRoutine);

                txtLoading.text = "Loading 100%";
                DOVirtual.DelayedCall(1, () =>
                {
                    TrySetLoadingReturnedOk();
                    Hide();
                });
            });
    }

    private static void TrySetLoadingReturnedOk()
    {
        var rdmType = Type.GetType("RDM") ?? Type.GetType("RDM, Assembly-CSharp");
        if (rdmType == null) return;

        var insProp = rdmType.GetProperty("Ins", BindingFlags.Public | BindingFlags.Static);
        var ins = insProp?.GetValue(null, null);
        if (ins == null) return;

        var loadingModelProp = rdmType.GetProperty("LoadingPopupModel", BindingFlags.Public | BindingFlags.Instance);
        var loadingModel = loadingModelProp?.GetValue(ins, null);
        if (loadingModel == null) return;

        var returnDataProp = loadingModel.GetType().GetProperty("ReturnData", BindingFlags.Public | BindingFlags.Instance);
        var returnData = returnDataProp?.GetValue(loadingModel, null);
        if (returnData == null) return;

        var returnedProp = returnData.GetType().GetProperty("Returned", BindingFlags.Public | BindingFlags.Instance);
        if (returnedProp == null || !returnedProp.CanWrite) return;

        var returnIdType = Type.GetType("ReturnId") ?? Type.GetType("ReturnId, Assembly-CSharp");
        if (returnIdType == null || !returnIdType.IsEnum) return;

        object okValue;
        try
        {
            okValue = Enum.Parse(returnIdType, "OK");
        }
        catch
        {
            return;
        }

        returnedProp.SetValue(returnData, okValue, null);
    }
}
