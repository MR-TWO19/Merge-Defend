using System;
using DG.Tweening;
using Hawky.Sound;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private GameObject model;
    [SerializeField] private ParticleSystem vfx;

    [HideInInspector] public bool IsUsed;
    [HideInInspector] public int Idx;
    [HideInInspector] public bool IsExploded;
    [HideInInspector] public bool IsExplodeWaiting;

    [SerializeField] private TargetLevelData currTargetData;
    private TargetLine currTargetLine;

    public TargetLevelData CurrTargetData => currTargetData;
    public TargetLine CurrTargetLine => currTargetLine;

    public void SetUp(TargetLevelData targetData, TargetLine targetLine)
    {
        currTargetData = targetData;
        currTargetLine = targetLine;

        var itemData = GameConfig.Ins.GetItemData(currTargetData.ItemId);
        if (itemData != null && itemData.MaterialTarget != null)
        {
            var main = vfx.main;
            main.startColor = itemData.ColorBalloon;
            meshRenderer.material = itemData.MaterialTarget;
        }

        IsExploded = false;
        IsUsed = true;
        transform.localScale = Vector3.one;
    }

    public void Explode(Action onComplete = null)
    {
        if (IsExploded) return;
        IsExploded = true;

        vfx.gameObject.SetActive(true);
        model.SetActive(false);

        //SoundManager.Instance.PlaySound(SoundId.BALLOON);

        DOVirtual.DelayedCall(0.8f, () =>
        {
            currTargetLine.RunCompleteTarget(this);
            onComplete?.Invoke();
        });

        //Sequence seq = DOTween.Sequence();

        //SoundManager.Instance.PlaySound(SoundId.BALLOON);

        //seq.Append(transform.DOScale(Vector3.zero, 0.4f)
        //    .SetEase(Ease.InBack));

        //seq.OnComplete(() =>
        //{
        //    currTargetLine.RunCompleteTarget(this);
        //    onComplete?.Invoke();
        //});
    }

    public void ResetAll()
    {
        IsExploded = false;
        IsExplodeWaiting = false;
        IsUsed = false;
        Idx = -1;
        transform.localScale = Vector3.one;
        gameObject.SetActive(true);
        model.SetActive(true);
        vfx.gameObject.SetActive(false);
    }
}
