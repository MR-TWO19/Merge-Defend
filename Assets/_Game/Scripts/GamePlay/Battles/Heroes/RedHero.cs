using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHero : Character
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
