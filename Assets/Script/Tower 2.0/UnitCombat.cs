using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int damage = 1;

    private int rowIndex;
    private float timer;

    public void SetRow(int row)
    {
        rowIndex = row;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        Enemy target = Enemy.FindClosestInRow(rowIndex, transform.position.x);
        if (target == null) return;

        if (Mathf.Abs(target.transform.position.x - transform.position.x) <= attackRange)
        {
            target.TakeDamage(damage);
            timer = attackCooldown;
        }
    }
}
