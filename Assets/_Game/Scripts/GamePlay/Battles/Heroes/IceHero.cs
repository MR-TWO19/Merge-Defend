using DG.Tweening;
using Doozy.Engine.UI.Animation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceHero : Character
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private GameObject posBullet;

    List<Bullet> bullets = new();

    public override void ATK()
    {
        if (animator != null)
            animator.SetTrigger("ATK");
    }

    protected override void OnApplyATKDamage()
    {
        Bullet bullet = GetBullet();

        bullet.transform.position = posBullet.transform.position;

        bullet.gameObject.SetActive(true);

        Transform tranTarget = !isAttackingHome ? currentTarget.transform :
            characterType == CharacterType.Enemy ? BattleManager.Ins.HomeHero.transform : BattleManager.Ins.HomeEnemy.transform;

        bullet.Attack(tranTarget, () =>
        {
            if (isAttackingHome)
            {
                BattleManager.Ins.ATKHome(characterType);
                return;
            }

            if (currentTarget == null)
            {
                bullet.HideBullet();
                bullet.isUse = false;
                return;
            }

            bool isDie = currentTarget.TakeDamage(characterData.Damage);
            if (isDie)
            {
                currentTarget = null;
            }
            DOVirtual.DelayedCall(0.2f, () =>
            {
                bullet.HideBullet();
                bullet.isUse = false;
            });
        }, 0, attackInterval);

        if (isAttackingHome)
        {
            Die();
        }
    }

    public override void StopATK()
    {
        //bullet.gameObject.SetActive(false);
    }

    public Bullet GetBullet()
    {
        foreach (var item in bullets)
            if (item.isUse == false)
            {
                item.isUse = true;
                return item;
            }

        Bullet bullet = Instantiate(this.bullet, posBullet.transform.position, Quaternion.identity, transform);
        bullet.HideBullet();
        bullets.Add(bullet);

        bullet.isUse = true;

        return bullet;
    }
}
