using System.Collections;
using System.Collections.Generic;
using TwoCore;
using UnityEngine;

public class BossSkeleton : Character
{
    [SerializeField] private HPBar hPBar;
    [SerializeField] private Transform tran;

    protected override void Update()
    {
        base.Update();
        tran.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public override void SetUp(CharacterInfo characterInfo)
    {
        base.SetUp(characterInfo);
        hPBar.SetData(HP);
    }

    public override void ATK()
    {
        if (animator != null)
            animator.SetTrigger("ATK");

    }

    public override bool TakeDamage(int damage)
    {
        hPBar.AddHP(-damage);
        return base.TakeDamage(damage);
    }

    public override void StopATK()
    {
    }

    protected override void OnApplyATKDamage()
    {
        SoundManager.Ins.PlayOneShot(SoundId.PUNCH);

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
}

