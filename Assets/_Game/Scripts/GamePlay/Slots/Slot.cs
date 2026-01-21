using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private int index;
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public bool IsUsed = false;
    public int Index => index;

    public void SetUp(int index)
    {
        this.index = index;
    }

    public void RunAnim()
    {
        transform.DOKill();

        DOTween.Sequence()
            .Append(transform.DOScale(originalScale * 0.9f, 0.1f).SetEase(Ease.InOutQuad))
            .Append(transform.DOScale(originalScale, 0.15f).SetEase(Ease.OutBack));
    }
}
