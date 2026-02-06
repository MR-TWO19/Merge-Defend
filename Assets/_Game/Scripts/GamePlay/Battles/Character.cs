using DG.Tweening;
using Doozy.Engine.UI.Animation;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum CharacterType
{
    Hero,
    Enemy,
    Boss
}

public abstract class Character : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected SkinnedMeshRenderer meshRenderer;

    [Header("Stats")]
    [SerializeField] protected CharacterType characterType;
    [SerializeField] protected float stopDistance = 0.5f;
    [SerializeField] protected float attackInterval = 0.5f;
    [SerializeField] protected float attackRange = 1.5f;

    [Header("Avoidance")]
    [SerializeField] protected float avoidanceRadius = 1f;
    [SerializeField] protected float avoidanceStrength = 1f;
    [SerializeField] protected float minSeparation = 0.6f;
    [SerializeField] protected float separationStrength = 1f;

    public int HP = 0;
    public CharacterType CharacterType => characterType;

    protected CharacterInfo characterData;
    protected Character currentTarget;
    protected float atkTimer = 0f;
    protected bool isMoving = false;
    protected bool isDead = false;
    protected bool isAttackingHome = false;
    protected bool isAttacking = false;
    protected float targetRefreshTimer = 0f;

    public virtual void SetUp(CharacterInfo characterInfo)
    {
        this.characterData = characterInfo;
        HP = characterData.Health;
        isDead = false;
        isAttackingHome = false;
    }

    protected virtual void Update()
    {
        if (isDead) return;

        MoveToTarget();
        AttackLoop();

        targetRefreshTimer -= Time.deltaTime;
        if (targetRefreshTimer <= 0f)
        {
            currentTarget = null;
            targetRefreshTimer = 3f;
        }
    }

    private void MoveToTarget()
    {
        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
        {
            currentTarget = FindTarget();
        }

        Transform targetTransform = null;
        bool isMovingToHome = false;
        if (currentTarget != null)
        {
            targetTransform = currentTarget.transform;
        }
        else
        {
            if (!BattleManager.Ins.IsBossBattle)
            {
                if (characterType == CharacterType.Hero)
                {
                    targetTransform = BattleManager.Ins != null ? BattleManager.Ins.HomeEnemy?.transform : null;
                    isMovingToHome = true;
                }
                else
                {
                    targetTransform = BattleManager.Ins != null ? BattleManager.Ins.HomeHero?.transform : null;
                    isMovingToHome = true;
                }
            }
        }
        if (targetTransform == null) return;

        Vector3 dir = new Vector3(targetTransform.position.x - transform.position.x, 0f, targetTransform.position.z - transform.position.z);
        float dist = dir.magnitude;

        if (dist > stopDistance)
        {
            Vector3 desiredDir = dir.normalized;
            Vector3 avoid = Vector3.zero;
            int count = 0;

            List<Character> neighbors = new List<Character>();
            if (BattleManager.Ins != null)
            {
                if (BattleManager.Ins.HeroManager != null) neighbors.AddRange(BattleManager.Ins.HeroManager.activeHeroes);
                if (BattleManager.Ins.EnemyManager != null) neighbors.AddRange(BattleManager.Ins.EnemyManager.activeEnemys);
            }

            foreach (var other in neighbors)
            {
                if (other == null || other == this) continue;
                if (!other.gameObject.activeInHierarchy) continue;

                float d = Vector3.Distance(transform.position, other.transform.position);
                if (d < Mathf.Epsilon) continue;
                if (d < avoidanceRadius)
                {
                    Vector3 away = (transform.position - other.transform.position).normalized;
                    float factor = (avoidanceRadius - d) / avoidanceRadius;
                    avoid += away * factor;
                    count++;
                }
            }

            Vector3 finalDir = desiredDir;
            if (count > 0)
            {
                avoid /= count;
                finalDir = desiredDir + avoid * avoidanceStrength;
                if (finalDir.sqrMagnitude < 0.001f)
                    finalDir = desiredDir;
                else
                    finalDir.Normalize();
            }

            float speed = characterData.Speed;
            Vector3 separation = Vector3.zero;
            int sepCount = 0;
            foreach (var other in neighbors)
            {
                if (other == null || other == this) continue;
                if (!other.gameObject.activeInHierarchy) continue;

                float d = Vector3.Distance(transform.position, other.transform.position);
                if (d < Mathf.Epsilon) continue;
                if (d < minSeparation)
                {
                    Vector3 away = (transform.position - other.transform.position).normalized;
                    float factor = (minSeparation - d) / minSeparation;
                    separation += away * factor;
                    sepCount++;
                }
            }
            if (sepCount > 0)
            {
                separation /= sepCount;
                separation *= separationStrength;
            }

            Vector3 targetPosition = transform.position + (finalDir + separation).normalized * speed * Time.deltaTime;
             targetPosition.y = 0f;
             transform.position = targetPosition;

            if (animator != null) animator.SetBool("Move", true);
            if (finalDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(finalDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
            }

            isMoving = true;
            isAttacking = false;
            StopATK();
        }
        else
        {
            isMoving = false;
            if (animator != null) animator.SetBool("Move", false);

            if (isMovingToHome && !isDead)
            {
                isAttackingHome = true;
                isAttacking = true;
                ATK();
            }
        }
    }

    protected virtual void AttackLoop()
    {
        if (isMoving || currentTarget == null) { atkTimer = 0f; return; }
        float dist = Vector3.Distance(transform.position, currentTarget.transform.position);
        if (dist <= (attackRange + 0.1))
        {
            atkTimer -= Time.deltaTime;
            if (atkTimer <= 0f)
            {
                isAttackingHome = false;
                ATK();
                atkTimer = attackInterval;
            }
        }
        else
        {
            atkTimer = 0f;
        }
    }

    protected virtual Character FindTarget()
    {
        List<Character> list = null;
        if (characterType == CharacterType.Hero)
        {
            if (BattleManager.Ins != null && BattleManager.Ins.EnemyManager != null)
                list = BattleManager.Ins.EnemyManager.activeEnemys;
        }
        else if (characterType == CharacterType.Enemy)
        {
            if (BattleManager.Ins != null && BattleManager.Ins.HeroManager != null)
                list = BattleManager.Ins.HeroManager.activeHeroes;
        }
        else if (characterType == CharacterType.Boss)
        {
            if (BattleManager.Ins != null && BattleManager.Ins.HeroManager != null)
                list = BattleManager.Ins.HeroManager.activeHeroes;
        }
        if (list == null || list.Count == 0) return null;

        Character closest = null;
        float minDist = float.MaxValue;
        foreach (var c in list)
        {
            if (c == null || !c.gameObject.activeInHierarchy) continue;
            float d = Vector3.Distance(transform.position, c.transform.position);
            if (d < minDist)
            {
                minDist = d;
                closest = c;
            }
        }
        return closest;
    }

    public abstract void ATK();
    public abstract void StopATK();

    public void ApplyATKDamage()
    {
        isAttacking = false;
        OnApplyATKDamage();
    }

    protected virtual void OnApplyATKDamage()
    {
    }

    public virtual bool TakeDamage(int damage)
    {
        bool isDie = false;

        if (meshRenderer != null)
        {
            meshRenderer.material.color = Color.red;
            DOVirtual.DelayedCall(0.1f, () =>
            {
                if (meshRenderer != null)
                    meshRenderer.material.color = Color.white;
            });
        }

        HP -= damage;
        if (HP <= 0 && !isDead)
        {
            Die();
            isDie = true;
        }

        return isDie;
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null) animator.SetTrigger("Die");

        BattleManager.Ins.RemoveCharacter(this);

        if(characterType == CharacterType.Boss)
        {
            DOVirtual.DelayedCall(2f, () =>
            {
                GameManager.Ins.WinGame();
            });
        }    
    }
}
