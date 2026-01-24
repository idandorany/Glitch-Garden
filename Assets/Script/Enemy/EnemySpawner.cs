using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    [SerializeField] private Enemy enemyPrefab;

    [Header("Runtime")]
    [SerializeField] private List<Enemy> spawnedEnemies = new();

    [ContextMenu("Spawn Enemy")]
    public Enemy SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError($"{name}: enemyPrefab missing");
            return null;
        }

        Enemy enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        spawnedEnemies.Add(enemy);
        return enemy;
    }
}
