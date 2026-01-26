using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int health = 3;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float attackRange = 0.2f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Base")]
    [SerializeField] private Transform enterBasePos;

    private int rowIndex = -1;
    private float lastAttack;
    private Transform currentTarget;
    private bool reachedBase;

    public void SetRow(int row)
    {
        // if row changes, unregister from old row
        if (CombatRegistry.Instance != null && rowIndex != -1)
            CombatRegistry.Instance.UnregisterEnemy(this, rowIndex);

        rowIndex = row;

        // register now if registry exists
        if (CombatRegistry.Instance != null)
            CombatRegistry.Instance.RegisterEnemy(this, rowIndex);
    }

    private void OnEnable()
    {
        // handles the case where SetRow happened BEFORE registry existed
        if (CombatRegistry.Instance != null && rowIndex != -1)
            CombatRegistry.Instance.RegisterEnemy(this, rowIndex);
    }
    private void OnDisable()
    {
        if (CombatRegistry.Instance != null && rowIndex != -1)
            CombatRegistry.Instance.UnregisterEnemy(this, rowIndex);
    }

    private void Update()
    {
        if (health <= 0 || reachedBase)
            return;

        if (currentTarget == null)
        {
            currentTarget = CombatRegistry.Instance.GetClosestDefenderAhead(
                rowIndex, transform.position.x
            );

            if (currentTarget == null)
            {
                MoveForward();
                return;
            }
        }

        float dx = currentTarget.position.x - transform.position.x;

        if (dx <= attackRange)
            Attack();
        else
            MoveForward();
    }

    private void MoveForward()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void Attack()
    {
        if (Time.time - lastAttack < attackCooldown)
            return;

        lastAttack = Time.time;

        var combat = currentTarget.GetComponent<UnitCombat>();
        if (combat != null)
            combat.TakeDamage(1);
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log($"{name} took {dmg} dmg (HP={health})");

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log($"{name} died");
        Destroy(gameObject, 0.5f);
    }
}
