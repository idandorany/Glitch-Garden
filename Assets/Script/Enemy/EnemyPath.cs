using UnityEngine;

public class EnemyPathFollower : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 1f;

    int index = 0;

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[index];
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        float dist = Vector2.Distance(transform.position, target.position);
        if (dist < 0.1f)
        {
            index++;
            if (index >= waypoints.Length)
            {
                // Reached the end
                Destroy(gameObject);
            }
        }
    }
}
