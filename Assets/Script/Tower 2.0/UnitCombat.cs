using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int damage = 1;
    [SerializeField] private int hp = 1;


    private int rowIndex;
    private float timer;

    public void SetRow(int row)
    {
        rowIndex = row;
        CombatRegistry.Instance.RegisterDefender(this, rowIndex);
        Debug.Log(rowIndex);
        
    }

    private void OnDestroy()
    {
        if (CombatRegistry.Instance != null)
            CombatRegistry.Instance.UnregisterDefender(this, rowIndex);
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        Enemy target = CombatRegistry.Instance.GetClosestEnemyInRow(
            rowIndex, transform.position.x
        );


        if (target == null)
            return;

        if (Mathf.Abs(target.transform.position.x - transform.position.x) <= attackRange)
        {
            target.TakeDamage(damage);
            Debug.Log("Defender Attacks!");
            timer = attackCooldown;
        }
    }



    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        Debug.Log($"{name} took {dmg} dmg (HP={hp})");

        if (hp <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log($"{name} died");
        Destroy(gameObject, 0.5f);
    }
}
