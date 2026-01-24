using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    private static List<Enemy> allEnemies = new List<Enemy>();

    [SerializeField] private int health = 3;
    [SerializeField] private float speed = 1f;

    private int rowIndex;

    public void SetRow(int row)
    {
        rowIndex = row;
    }

    private void OnEnable()
    {
        allEnemies.Add(this);
    }

    private void OnDisable()
    {
        allEnemies.Remove(this);
    }

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
            Destroy(gameObject);
    }

    public static Enemy FindClosestInRow(int row, float fromX)
    {
        Enemy closest = null;
        float bestDist = float.MaxValue;

        foreach (var e in allEnemies)
        {
            if (e.rowIndex != row) continue;

            float d = Mathf.Abs(e.transform.position.x - fromX);
            if (d < bestDist)
            {
                bestDist = d;
                closest = e;
            }
        }
        return closest;
    }
}
