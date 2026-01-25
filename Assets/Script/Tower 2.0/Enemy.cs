using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    private static List<Enemy> allEnemies = new List<Enemy>();

    [Header("Stats")]
    [SerializeField] private int health = 3;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float attackRange = 0.2f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float attackDamage = 1;

    [Header("Base Settings")]
    [SerializeField] public Transform enterBasePos;

    private float lastAttackTime;
    private GameObject currentTarget;
    private bool reachedBase;

    private int rowIndex;

    public void SetRow(int row)
    {
        rowIndex = row;
    }

    private void OnEnable()
    {
        allEnemies.Add(this);
    }

    private void OnDisable()
    {
        allEnemies.Remove(this);
    }

    private void Update()
    {
        if (health <= 0) return;

        if (reachedBase)
            return;

        if (currentTarget == null)
        {
            FindNextTarget();
            MoveForward();
        }
        else
        {
            float xDistance = Mathf.Abs(currentTarget.transform.position.x - transform.position.x);

            if (xDistance <= attackRange)
            {
                AttackTarget();
            }
            else
            {
                MoveForward();
            }
        }
    }

    private void MoveForward()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void FindNextTarget()
    {
        Transform defender = CombatRegistry.Instance != null
            ? CombatRegistry.Instance.GetClosestDefenderAhead(rowIndex, transform.position.x)
            : null;

        if (defender != null)
        {
            currentTarget = defender.gameObject;
            return;
        }

        // Base (global) fallback
        GameObject playerBase = GameObject.FindGameObjectWithTag("Base");
        if (playerBase != null)
            currentTarget = playerBase;
    }


    // private GameObject FindClosestWithTag(string tag)
    // {
    //     GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
    //     GameObject closest = null;
    //     float bestDist = float.MaxValue;
    //
    //     foreach (var t in targets)
    //     {
    //         float d = Mathf.Abs(t.transform.position.x - transform.position.x);
    //         if (d < bestDist)
    //         {
    //             bestDist = d;
    //             closest = t;
    //         }
    //     }
    //     return closest;
    // }

    private void AttackTarget()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;

        if (currentTarget == null)
            return;

        Debug.Log($"{name} attacks {currentTarget.name}");

        // Defender has no required script — destroy is enough
        if (currentTarget.CompareTag("Defender"))
        {
            //Destroy(currentTarget);
            currentTarget = null;
        }
        else if (currentTarget.CompareTag("Base"))
        {
            ReachBase();
        }
    }

    private void ReachBase()
    {
        if (reachedBase) return;

        reachedBase = true;
        Debug.Log("Reached Player Base");

        if (enterBasePos != null)
            transform.position = enterBasePos.position;

        Destroy(gameObject, 2f);
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log($"{name} took {dmg} damage. HP left: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{name} died");

        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Die");
        }

        Destroy(gameObject, 0.5f);
    }

    public static Enemy FindClosestInRow(int row, float fromX)
    {
        Enemy closest = null;
        float bestDist = float.MaxValue;

        foreach (var e in allEnemies)
        {
            if (e.rowIndex != row) continue;

            float d = Mathf.Abs(e.transform.position.x - fromX);
            if (d < bestDist)
            {
                bestDist = d;
                closest = e;
            }
        }
        return closest;
    }
}
