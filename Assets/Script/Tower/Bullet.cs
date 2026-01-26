using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 1f;
    public float lifeTime = 3f;

    private Enemy target;

    public void SetTarget(Enemy e) => target = e;

    private void Start() => Destroy(gameObject, lifeTime);

    private void Update()
    {
        if (target == null) { Destroy(gameObject); return; }

        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy e = other.GetComponentInParent<Enemy>();
        if (e == null) return;

        // Only damage the specific target we were assigned
        if (e != target) return;

        target.TakeDamage(Mathf.RoundToInt(damage)); // your Enemy.TakeDamage takes int
        Destroy(gameObject);
    }
}
