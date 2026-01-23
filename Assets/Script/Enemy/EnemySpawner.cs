using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TileCell[] tileCells;

    [Header("Enemy Prefabs")]
    [SerializeField] private Enemy enemyPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private List<Enemy> spawnedEnemies;
    [SerializeField] private float spawnXOffset = 1.5f;

    [ContextMenu("Spawn Enemy")]
    public Enemy SpawnEnemy()
    {
        Enemy enemy = Instantiate(enemyPrefab, tileCells[0].transform.position, Quaternion.identity);
        spawnedEnemies.Add(enemy);
        return enemy;
    }

}
