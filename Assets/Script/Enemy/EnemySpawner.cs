using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject enemyPrefab;
        public int count = 5;
        public float spawnInterval = 0.7f;
    }

    [Header("Spawn Settings")]
    public Transform spawnPoint;
    public Transform waypointsParent;
    public Wave[] waves;

    [Header("Between Waves")]
    public float timeBetweenWaves = 3f;

    private Transform[] cachedWaypoints;

    void Start()
    {
        CacheWaypoints();
        StartCoroutine(SpawnWaves());
    }

    void CacheWaypoints()
    {
        if (waypointsParent == null)
        {
            Debug.LogError("EnemySpawner: waypointsParent not assigned!");
            return;
        }

        int n = waypointsParent.childCount;
        cachedWaypoints = new Transform[n];

        for (int i = 0; i < n; i++)
            cachedWaypoints[i] = waypointsParent.GetChild(i);
    }

    IEnumerator SpawnWaves()
    {
        if (spawnPoint == null)
        {
            Debug.LogError("EnemySpawner: spawnPoint not assigned!");
            yield break;
        }
        if (cachedWaypoints == null || cachedWaypoints.Length == 0)
        {
            Debug.LogError("EnemySpawner: no waypoints found!");
            yield break;
        }

        for (int w = 0; w < waves.Length; w++)
        {
            Wave wave = waves[w];

            for (int i = 0; i < wave.count; i++)
            {
                SpawnEnemy(wave.enemyPrefab);
                yield return new WaitForSeconds(wave.spawnInterval);
            }

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("EnemySpawner: wave enemyPrefab is missing!");
            return;
        }

        GameObject enemyObj = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        EnemyPathFollower mover = enemyObj.GetComponent<EnemyPathFollower>();
        if (mover != null)
        {
            mover.waypoints = cachedWaypoints;
        }
        else
        {
            Debug.LogWarning("Spawned enemy has no EnemyPathFollower component.");
        }
    }
}
