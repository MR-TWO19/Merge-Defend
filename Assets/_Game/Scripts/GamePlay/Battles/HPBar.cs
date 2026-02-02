using DG.Tweening;
using TMPro;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer hpBar;
    [SerializeField] private TextMeshPro txtHP;

    [Header("Settings")]
    [SerializeField] private float speedFill = 0.5f;

    [HideInInspector] public int totalHP;
    [HideInInspector] public int curHP;

    private Vector3 hpBarOriginalScale;
    private Tween hpTween;

    private void Awake()
    {
        if (hpBar != null)
            hpBarOriginalScale = hpBar.transform.localScale;
    }

    public void SetData(int totalHp)
    {
        totalHP = Mathf.Max(1, totalHp);
        curHP = totalHP;
        UpdateHPBar(0f);
    }

    public void AddHP(int hpChange)
    {
        curHP = Mathf.Clamp(curHP + hpChange, 0, totalHP);
        UpdateHPBar(speedFill);
    }

    public bool IsOutOfHP => curHP <= 0;

    private void UpdateHPBar(float speed)
    {
        if (!hpBar) return;

        float fill = (float)curHP / totalHP;

        hpTween?.Kill();
        hpTween = hpBar.transform.DOScaleX(fill * hpBarOriginalScale.x, speed)
            .SetEase(Ease.InOutQuad)
            .SetUpdate(true);

        if (txtHP)
            txtHP.text = curHP.ToString();
    }
}
