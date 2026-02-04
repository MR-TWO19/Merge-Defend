using Doozy.Engine.UI;
using System;
using System.Collections.Generic;
using TMPro;
using TwoCore;
using UnityEngine;

public class UpgradePopup : BasePopup
{
    public static UpgradePopup Show(Action action)
    {
        return ShowWithParamsAndMethod<UpgradePopup>("UpgradePopup", PopupShowMethod.NO_QUEUE, action);
    }

    [SerializeField] private TextMeshProUGUI txtLevel;

    [SerializeField] private TextMeshProUGUI txtHP;
    [SerializeField] private TextMeshProUGUI txtDamege;

    [SerializeField] private TextMeshProUGUI txtNextHP;
    [SerializeField] private TextMeshProUGUI txtNextDamege;
    [SerializeField] private TextMeshProUGUI txtNextLevel;

    [SerializeField] private TextMeshProUGUI txtPriceUpgrade;

    [SerializeField] private UIButton btnUpgrade;

    [SerializeField] private UIButton btnNextLeft;
    [SerializeField] private UIButton btnNextRight;

    [SerializeField] private GameObject posHero;

    private int currentIndex;
    private GameObject currentHeroObj;

    public Action OnHideComplete;

    protected override void Awake()
    {
        base.Awake();

        if (btnUpgrade != null)
            btnUpgrade.OnClick.OnTrigger.Event.AddListener(UpgradeOnClick);

        if (btnNextLeft != null)
            btnNextLeft.OnClick.OnTrigger.Event.AddListener(() => ChangeHero(-1));

        if (btnNextRight != null)
            btnNextRight.OnClick.OnTrigger.Event.AddListener(() => ChangeHero(1));
    }

    private void OnEnable()
    {
        currentIndex = 0;
        RefreshUI();
    }

    protected override void SetParams(params object[] @params)
    {
        base.SetParams(@params);
        OnHideComplete = (Action)@params[0];
    }

    protected override void OnHide()
    {
        base.OnHide();
        OnHideComplete?.Invoke();
    }

    private void ChangeHero(int delta)
    {
        var heroes = GameConfig.Ins?.HeroDatas;
        if (heroes == null || heroes.Count == 0) return;

        currentIndex = Mathf.Clamp(currentIndex + delta, 0, heroes.Count - 1);
        RefreshUI();
    }

    private void RefreshUI()
    {
        var heroes = GameConfig.Ins?.HeroDatas;
        if (heroes == null || heroes.Count == 0) return;

        currentIndex = Mathf.Clamp(currentIndex, 0, heroes.Count - 1);

        if (btnNextLeft != null) btnNextLeft.gameObject.SetActive(currentIndex > 0);
        if (btnNextRight != null) btnNextRight.gameObject.SetActive(currentIndex < heroes.Count - 1);

        var heroData = heroes[currentIndex];
        var info = UserSaveData.Ins.GetCharacter(heroData.id);
        if (info == null) return;

        SpawnHero(heroData);

        if (txtLevel != null) txtLevel.text = $"Lv {info.Level}";
        if (txtHP != null) txtHP.text = info.Health.ToString("0.##");
        if (txtDamege != null) txtDamege.text = info.Damage.ToString("0");

        if (UserSaveData.Ins.TryGetUpgradePreview(heroData.id, out var nextLevel, out var nextHp, out var nextDamage, out var cost))
        {
            if (txtNextLevel != null) txtNextLevel.text = $"Lv {nextLevel}";
            if (txtNextHP != null) txtNextHP.text = nextHp.ToString("0.##");
            if (txtNextDamege != null) txtNextDamege.text = nextDamage.ToString("0");
            if (txtPriceUpgrade != null) txtPriceUpgrade.text = cost == int.MaxValue ? "MAX" : cost.ToString();
        }
        else
        {
            if (txtNextLevel != null) txtNextLevel.text = "MAX";
            if (txtNextHP != null) txtNextHP.text = info.Health.ToString("0.##");
            if (txtNextDamege != null) txtNextDamege.text = info.Damage.ToString("0");
            if (txtPriceUpgrade != null) txtPriceUpgrade.text = "MAX";
        }
    }

    private void SpawnHero(CharacterData heroData)
    {
        if (posHero == null || heroData == null || heroData.Prefab == null)
            return;

        if (currentHeroObj != null)
        {
            Destroy(currentHeroObj);
            currentHeroObj = null;
        }

        currentHeroObj = Instantiate(heroData.Prefab, posHero.transform);
        currentHeroObj.GetComponent<Character>().enabled = false;
        currentHeroObj.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0f, 180f, 0f));
        currentHeroObj.transform.localScale = Vector3.one * 300f;
    }

    private void UpgradeOnClick()
    {
        var heroes = GameConfig.Ins?.HeroDatas;
        if (heroes == null || heroes.Count == 0) return;

        var heroData = heroes[currentIndex];
        if (UserSaveData.Ins.UpgradeCharacter(heroData.id))
        {
            RefreshUI();
        }
    }
}
