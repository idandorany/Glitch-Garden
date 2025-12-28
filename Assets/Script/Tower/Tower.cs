using UnityEngine;

public class Tower : MonoBehaviour
{
    public float range = 3f;
    public float fireRate = 1f; 
    public GameObject bulletPrefab;
    public Transform firePoint;

    private float nextShotTime = 0f;

    void Update()
    {
        if (Time.time < nextShotTime) return;

        Transform target = FindClosestEnemy();
        if (target == null) return;

        Shoot(target);
        nextShotTime = Time.time + (1f / fireRate);
    }

    Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform best = null;
        float bestDist = Mathf.Infinity;

        foreach (GameObject e in enemies)
        {
            float d = Vector2.Distance(transform.position, e.transform.position);
            if (d <= range && d < bestDist)
            {
                bestDist = d;
                best = e.transform;
            }
        }
        return best;
    }

    void Shoot(Transform target)
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        b.GetComponent<Bullet>()?.SetTarget(target);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
