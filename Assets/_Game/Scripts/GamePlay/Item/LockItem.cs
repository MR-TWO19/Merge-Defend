using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LockItem : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenKey;
    [SerializeField] private MeshRenderer meshRenLock;
    [SerializeField] private DOTweenAnimation animkey;

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    public void SetUp(Material materialLock)
    {
        meshRenLock.material = materialLock;
    }

    public void UnLock(Action onComplete = null)
    {
        animkey.DOKill();
        meshRenKey.transform.SetLocalPositionAndRotation(new Vector3(0, 0, -0.5f), Quaternion.Euler(0, 0, 90));
        meshRenKey.gameObject.SetActive(true);


        Sequence seq = DOTween.Sequence();

        seq.Append(meshRenKey.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f));
        seq.OnComplete(() =>
        {
            meshRenKey.gameObject.SetActive(false);
            Hide();
            onComplete?.Invoke();
        });
    }
}
