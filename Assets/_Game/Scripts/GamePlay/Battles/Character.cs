using System.Collections;
using System.Collections.Generic;
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

    [Header("Stats")]
    [SerializeField] protected CharacterType characterType;
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float stopDistance = 0.5f;
    [SerializeField] protected float attackInterval = 0.5f;
    [SerializeField] protected float attackRange = 1.5f;

    protected CharacterInfo characterData;
    protected Character currentTarget;
    protected float atkTimer = 0f;
    protected bool isMoving = false;

    public void SetUp(CharacterInfo characterData)
    {
        this.characterData = characterData;
    }

    protected virtual void Update()
    {
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
        if (currentTarget != null)
        {
            targetTransform = currentTarget.transform;
        }
        else
        {
            // Nếu không có target đối phương thì di chuyển tới HomeEnemy/HomeHero
            if (characterType == CharacterType.Hero)
                targetTransform = BattleManager.Ins != null ? BattleManager.Ins.HomeEnemy?.transform : null;
            else
                targetTransform = BattleManager.Ins != null ? BattleManager.Ins.HomeHero?.transform : null;
        }
        if (targetTransform == null) return;

        Vector3 dir = targetTransform.position - transform.position;
        float dist = dir.magnitude;

        if (dist > stopDistance)
        {
            // Di chuyển tới target
            transform.position += dir.normalized * characterData.Speed * Time.deltaTime;
            isMoving = true;

            StopATK();
            if (animator != null) animator.SetBool("Move", true);
            // Xoay hướng
            if (dir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
            }
        }
        else
        {
            isMoving = false;
            if (animator != null) animator.SetBool("Move", false);
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
}
