using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform tileCell;

    [Header("Enemy Prefabs")]
    [SerializeField] private Enemy enemyPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private List<Enemy> spawnedEnemies;
    [SerializeField] private float spawnXOffset = 1.5f;

    [ContextMenu("Spawn Enemy")]
    public Enemy SpawnEnemy()
    {
        Vector3 targetPostion = new Vector3(spawnXOffset, 0, 0);

        Enemy enemy = Instantiate(enemyPrefab, tileCell.transform.position - targetPostion, Quaternion.identity);
        spawnedEnemies.Add(enemy);
        return enemy;
    }

}
