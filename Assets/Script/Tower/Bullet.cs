using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 1f;
    public float lifeTime = 3f;

    Transform target;

    public void SetTarget(Transform t) => target = t;

    void Start() => Destroy(gameObject, lifeTime);

    void Update()
    {
        if (target == null) { Destroy(gameObject); return; }

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // IMPORTANT: if collider is on a child, this still finds the parent health
        if (!other.CompareTag("Enemy")) return;

        EnemyHealth hp = other.GetComponentInParent<EnemyHealth>();
        if (hp != null)
            hp.TakeDamage(damage);

        Destroy(gameObject);
    }
}
