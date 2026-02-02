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

    protected override void OnApplyATKDamage()
    {
        if (isAttackingHome)
        {
            BattleManager.Ins.ATKHome(characterType);
            Die();
            return;
        }

        if (currentTarget == null)
            return;

        bool isDie = currentTarget.TakeDamage(characterData.Damage);
        if (isDie)
        {
            currentTarget = null;
        }
    }

    public override void StopATK()
    {
        bullet.enabled = false;
    }
}
