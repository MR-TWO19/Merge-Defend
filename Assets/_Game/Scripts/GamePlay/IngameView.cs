using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TwoCore;

public class IngameView : BaseView
{
    public static IngameView Ins;

    [SerializeField] private Button btnSetting;
    [SerializeField] private Button btnClearSlot;
    [SerializeField] private TextMeshProUGUI txtClear;
    [SerializeField] private TextMeshProUGUI txtCoundown;
    [SerializeField] private GameObject bossCountdownPanel;
    [SerializeField] private TextMeshProUGUI txtCoundownBoss;

    public TextMeshProUGUI txtLevel;
    private Coroutine clearCooldownRoutine;
    private Coroutine bossCountdownRoutine;

    protected override void Awake()
    {
        Ins = this; 
        base.Awake();
        btnSetting.onClick.AddListener(OnPauseSettingClick);
        btnClearSlot.onClick.AddListener(OnCLickBtnClearSlot);
    }

    public void UpdateTextLevel(int level)
    {
        txtLevel.text = $"Lv.{level}";
    }


    private void OnPauseSettingClick()
    {
        SettingPopup.Show(false);
    }

    protected override void OnShow()
    {
        base.OnShow();
        bossCountdownPanel.SetActive(GameManager.Ins.isBossLevel);

    }

    protected override void OnHide()
    {
        base.OnHide();
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

    public void StartBossCountdown()
    {
        if (bossCountdownRoutine != null)
        {
            StopCoroutine(bossCountdownRoutine);
            bossCountdownRoutine = null;
        }

        bossCountdownRoutine = StartCoroutine(BossCountdown());
    }

    private IEnumerator BossCountdown()
    {
        if (txtCoundownBoss != null) txtCoundownBoss.gameObject.SetActive(true);

        int totalSeconds = 90;
        for (int i = totalSeconds; i > 0; i--)
        {
            if (txtCoundownBoss != null)
            {
                int minutes = i / 60;
                int seconds = i % 60;
                txtCoundownBoss.text = $"{minutes:0}:{seconds:00}";
            }
            yield return new WaitForSeconds(1f);
        }

        if (txtCoundownBoss != null) txtCoundownBoss.gameObject.SetActive(false);
        Debug.Log("Boss countdown finished");

        GameManager.Ins.LoseGame();

        bossCountdownRoutine = null;
    }
}
