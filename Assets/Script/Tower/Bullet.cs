using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 2f;
    public float lifeTime = 3f;

    private Transform target;

    public void SetTarget(Transform t) => target = t;

    void Start() => Destroy(gameObject, lifeTime);

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        var hp = other.GetComponent<EnemyHealth>();
        if (hp != null) hp.TakeDamage(damage);

        Destroy(gameObject);
    }
}
