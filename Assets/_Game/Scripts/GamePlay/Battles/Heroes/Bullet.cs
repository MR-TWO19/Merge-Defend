using DG.Tweening;
using System;
using TwoCore;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject effectBullet;

    private Vector3 startPos;

    public bool isUse;

    private void Awake()
    {
        startPos = transform.position;
    }

    public void HideBullet()
    {
        bullet.SetActive(false);
        effectBullet.SetActive(false);
    }

    public void Attack(Transform target, Action onHit = null, float delayShowEffect = 0, float speedBullet = 0.01f)
    {
        if (target == null) return;

        transform.DOKill();
        transform.position = startPos;
        transform.rotation = Quaternion.identity;
        bullet.SetActive(true);
        effectBullet.SetActive(false);

        Vector3 posEnd = target.position;
        Vector3 direction = (posEnd - startPos).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0, 0, angle);
        //SoundManager.Ins.PlayOneShot(SoundID.SKILL01);
        transform.DOMove(posEnd, speedBullet)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                onHit?.Invoke();

                DOVirtual.DelayedCall(delayShowEffect, () =>
                {
                    bullet.SetActive(false);
                    transform.rotation = Quaternion.identity;
                    effectBullet.SetActive(true);
                    //SoundManager.Ins.PlayOneShot(SoundID.SKILL02);
                });
            });
    }

    public void SetStartPosition(Vector3 newStartPos)
    {
        startPos = newStartPos;
        transform.position = newStartPos;
    }
}
