using System.Collections;
using Hawky.Scene;
using ColorFight;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TwoCore;

public class IngameView : BaseView
{
    [SerializeField] private Button btnSetting;
    [SerializeField] private Button btnClearSlot;

    public TextMeshProUGUI txtLevel;

    protected override void Awake()
    {
        base.Awake();
        btnSetting.onClick.AddListener(OnPauseSettingClick);
        btnClearSlot.onClick.AddListener(OnCLickBtnClearSlot);
    }

    public void UpdateTextLevel()
    {
        txtLevel.text = $"Lv.{UserSaveData.Ins.Level}";
    }


    private void OnPauseSettingClick()
    {

    }

    private void OnCLickBtnClearSlot()
    {
        GameManager.Ins.ClearAllSlot();
    }
}
