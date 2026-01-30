using System;
using DG.Tweening;
using UnityEngine;

public class EnemyNormal : Character
{
    [SerializeField] private Collider bullet;

    private void OnEnable()
    {
        bullet.enabled = false;
    }

    public override void ATK()
    {
        if (animator != null)
            animator.SetTrigger("ATK");
        bullet.enabled = true;
    }

    public override void StopATK()
    {
        bullet.enabled = false;
    }
}
