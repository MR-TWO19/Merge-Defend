using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TwoCore;

public class IngameView : BaseView
{
    [SerializeField] private Button btnSetting;
    [SerializeField] private Button btnClearSlot;
    [SerializeField] private TextMeshProUGUI txtClear;
    [SerializeField] private TextMeshProUGUI txtCoundown;

    public TextMeshProUGUI txtLevel;
    private Coroutine clearCooldownRoutine;

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

        if (clearCooldownRoutine != null)
        {
            StopCoroutine(clearCooldownRoutine);
            clearCooldownRoutine = null;
        }

        clearCooldownRoutine = StartCoroutine(ClearSlotCooldown());
    }

    private IEnumerator ClearSlotCooldown()
    {
        if (btnClearSlot != null) btnClearSlot.interactable = false;
        if (txtClear != null) txtClear.gameObject.SetActive(false);
        if (txtCoundown != null) txtCoundown.gameObject.SetActive(true);

        for (int i = 10; i > 0; i--)
        {
            if (txtCoundown != null) txtCoundown.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        if (btnClearSlot != null) btnClearSlot.interactable = true;
        if (txtClear != null) txtClear.gameObject.SetActive(true);
        if (txtCoundown != null) txtCoundown.gameObject.SetActive(false);

        clearCooldownRoutine = null;
    }
}
