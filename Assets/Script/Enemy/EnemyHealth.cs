using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHp = 10f;
    private float hp;

    void Awake() => hp = maxHp;

    public void TakeDamage(float dmg)
    {
        hp -= dmg;
        if (hp <= 0f) Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
