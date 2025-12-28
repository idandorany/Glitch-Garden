using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float hp = 3f;

    public void TakeDamage(float dmg)
    {
        hp -= dmg;
        if (hp <= 0f)
            Destroy(gameObject);
    }
}
