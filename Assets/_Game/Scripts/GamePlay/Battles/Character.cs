using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum CharacterType
{
    Hero,
    Enemy
}

public abstract class Character : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected SkinnedMeshRenderer meshRenderer;

    [Header("Stats")]
    [SerializeField] protected CharacterType characterType;
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float stopDistance = 0.5f;
    [SerializeField] protected float attackInterval = 0.5f;
    [SerializeField] protected float attackRange = 1.5f;

    [Header("Avoidance")]
    [SerializeField] protected float avoidanceRadius = 1f;
    [SerializeField] protected float avoidanceStrength = 1f;

    public float HP = 0;
    public CharacterType CharacterType => characterType;

    protected CharacterInfo characterData;
    protected Character currentTarget;
    protected float atkTimer = 0f;
    protected bool isMoving = false;
    protected bool isDead = false;

    public void SetUp(CharacterInfo characterData)
    {
        this.characterData = characterData;

        HP = characterData.Health;
    }

    protected virtual void Update()
    {
        if (isDead) return;
        MoveToTarget();
        AttackLoop();
    }

    private void MoveToTarget()
    {
        // Tìm target nếu chưa có hoặc target đã chết
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
            // Nếu không có target đối phương thì di chuyển tới HomeEnemy/HomeHero
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
        if (targetTransform == null) return;

        Vector3 dir = targetTransform.position - transform.position;
        float dist = dir.magnitude;

        if (dist > stopDistance)
        {
            // Di chuyển tới target with avoidance
            Vector3 desiredDir = dir.normalized;

            // accumulate avoidance from nearby characters
            Vector3 avoid = Vector3.zero;
            int count = 0;

            // gather potential neighbors from managers
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
                    float factor = (avoidanceRadius - d) / avoidanceRadius; // 0..1
                    avoid += away * factor;
                    count++;
                }
            }

            if (count > 0)
            {
                avoid /= count;
                // combine desired direction and avoidance
                Vector3 finalDir = (desiredDir + avoid * avoidanceStrength).normalized;
                transform.position += finalDir * (characterData != null ? characterData.Speed : moveSpeed) * Time.deltaTime;

                if (animator != null) animator.SetBool("Move", true);
                if (finalDir.sqrMagnitude > 0.001f)
                {
                    Quaternion targetRot = Quaternion.LookRotation(finalDir, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
                }
            }
            else
            {
                // no neighbors to avoid
                transform.position += desiredDir * (characterData != null ? characterData.Speed : moveSpeed) * Time.deltaTime;
                if (animator != null) animator.SetBool("Move", true);
                if (dir.sqrMagnitude > 0.001f)
                {
                    Quaternion targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
                }
            }

            isMoving = true;
            StopATK();
        }
        else
        {
            isMoving = false;
            if (animator != null) animator.SetBool("Move", false);

            // If we've reached the home transform (no target) then die
            if (isMovingToHome && !isDead)
            {
                Die();
            }
        }
    }

    protected virtual void AttackLoop()
    {
        if (isMoving || currentTarget == null) { atkTimer = 0f; return; }
        float dist = Vector3.Distance(transform.position, currentTarget.transform.position);
        if (dist <= attackRange)
        {
            atkTimer -= Time.deltaTime;
            if (atkTimer <= 0f)
            {
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
        if (list == null || list.Count == 0) return null;

        // Tìm target gần nhất
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

    // Cho phép override ở class con
    public abstract void ATK();
    public abstract void StopATK();

    // Return true if died
    public virtual bool TakeDamage(float damage)
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
    }
}
