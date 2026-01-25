using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;

    private int rowIndex;

    public void SetRow(int row)
    {
        rowIndex = row;
    }

    [ContextMenu("Spawn Enemy")]

    public Enemy SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError($"{name}: Enemy prefab missing");
            return null;
        }

        Enemy enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        enemy.SetRow(rowIndex);
        return enemy;
    }
}
