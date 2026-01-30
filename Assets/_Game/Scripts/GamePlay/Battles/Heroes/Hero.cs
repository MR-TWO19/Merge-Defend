using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
public class Hero : MonoBehaviour
{
    //public HeroData Data { get; private set; }

    //private float hp;
    //private float damage;
    //private float attackSpeed;
    //private float range;

    //private Transform currentTarget;
    //private float attackCooldown;
    //private Animator animator;

    //public event Action<Hero> OnDie;

    //public void Initialize(HeroData data)
    //{
    //    Data = data;
    //    hp = data.HP;
    //    damage = data.Damage;
    //    attackSpeed = data.AttackSpeed;
    //    range = data.Range;
    //    animator = GetComponent<Animator>();
    //    if (animator != null) animator.SetBool("Move", false);
    //}

    //private void Update()
    //{
    //    if (Data == null) return;

    //    attackCooldown -= Time.deltaTime;

    //    // find target (closest enemy)
    //    if (currentTarget == null)
    //    {
    //        var enemy = FindClosestEnemy();
    //        if (enemy != null)
    //            currentTarget = enemy.transform;
    //    }

    //    if (currentTarget != null)
    //    {
    //        float dist = Vector3.Distance(transform.position, currentTarget.position);

    //        if (Data.Type == HeroType.Assassin)
    //        {
    //            // assassin dash if out of range
    //            if (dist > range && attackCooldown <= 0f)
    //            {
    //                // dash towards target
    //                if (animator != null) animator.SetTrigger("Dash");
    //                Vector3 dashTarget = transform.position + (currentTarget.position - transform.position).normalized * Mathf.Min(3f, dist - range);
    //                transform.DOMove(dashTarget, 0.2f).SetEase(Ease.OutQuad);
    //                attackCooldown = 1f / attackSpeed; // set cooldown after dash
    //            }
    //        }

    //        if (dist <= range)
    //        {
    //            // attack when cooldown ready
    //            if (attackCooldown <= 0f)
    //            {
    //                Attack();
    //                attackCooldown = 1f / attackSpeed;
    //            }

    //            // face target
    //            Vector3 dir = currentTarget.position - transform.position;
    //            if (dir.sqrMagnitude > 0.001f)
    //            {
    //                Quaternion targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
    //                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
    //            }
    //            if (animator != null) animator.SetBool("Move", false);
    //        }
    //        else
    //        {
    //            // move toward target
    //            if (animator != null) animator.SetBool("Move", true);
    //            Vector3 move = (currentTarget.position - transform.position).normalized * 2f * Time.deltaTime;
    //            transform.position += move;
    //        }
    //    }
    //    else
    //    {
    //        if (animator != null) animator.SetBool("Move", false);
    //    }
    //}

    //private Enemy FindClosestEnemy()
    //{
    //    var enemies = BattleManager.Ins.WaveSpawner.Enemys;
    //    Enemy closest = null;
    //    float minDist = float.MaxValue;
    //    foreach (var e in enemies)
    //    {
    //        float d = Vector3.Distance(transform.position, e.transform.position);
    //        if (d < minDist)
    //        {
    //            minDist = d;
    //            closest = e;
    //        }
    //    }
    //    return closest;
    //}

    //private void Attack()
    //{
    //    if (animator != null) animator.SetTrigger("ATK");

    //    switch (Data.Type)
    //    {
    //        case HeroType.Warrior:
    //            // single target
    //            if (currentTarget != null)
    //            {
    //                var e = currentTarget.GetComponent<Enemy>();
    //                e?.TakeDamage(damage);
    //            }
    //            break;
    //        case HeroType.Archer:
    //            // ranged single target
    //            if (currentTarget != null)
    //            {
    //                var e = currentTarget.GetComponent<Enemy>();
    //                e?.TakeDamage(damage);
    //            }
    //            break;
    //        case HeroType.Mage:
    //            // AOE damage around target
    //            if (currentTarget != null)
    //            {
    //                Collider[] hits = Physics.OverlapSphere(currentTarget.position, 2f);
    //                foreach (var hit in hits)
    //                {
    //                    var en = hit.GetComponent<Enemy>();
    //                    en?.TakeDamage(damage);
    //                }
    //            }
    //            break;
    //        case HeroType.Assassin:
    //            // high single damage
    //            if (currentTarget != null)
    //            {
    //                var e = currentTarget.GetComponent<Enemy>();
    //                e?.TakeDamage(damage);
    //            }
    //            break;
    //    }
    //}

    //public void TakeDamage(float d)
    //{
    //    hp -= d;
    //    if (hp <= 0)
    //        Die();
    //}

    //private void Die()
    //{
    //    OnDie?.Invoke(this);
    //    Destroy(gameObject);
    //}
}
