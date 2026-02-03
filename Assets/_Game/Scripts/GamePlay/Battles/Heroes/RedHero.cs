using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHero : Character
{

    public override void ATK()
    {
        if (animator != null)
            animator.SetTrigger("ATK");
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
    }
}
