using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Lane")]
    [SerializeField] private int rowIndex = 0; // set per lane in Inspector

    [Header("Enemy Prefab")]
    [SerializeField] private Enemy enemyPrefab;

    [Header("Runtime")]
    [SerializeField] private List<Enemy> spawnedEnemies = new();

    public void SetRow(int row) => rowIndex = row;

    [ContextMenu("Spawn Enemy")]
    public Enemy SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError($"{name}: enemyPrefab missing");
            return null;
        }

        Enemy enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        enemy.SetRow(rowIndex);

        spawnedEnemies.Add(enemy);
        return enemy;
    }
}
