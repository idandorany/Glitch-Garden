using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<EnemySpawner> spawners = new();

    [Header("Waves")]
    [SerializeField] private List<WaveConfig> waves = new();
    [SerializeField] private bool autoStart = true;

    private int currentWaveIndex = -1;
    private int roundRobinIndex = 0;
    private Coroutine running;

    private void Awake()
    {
        // If you don't want to drag spawners manually, auto-find:
        if (spawners.Count == 0)
            spawners.AddRange(FindObjectsByType<EnemySpawner>(FindObjectsInactive.Include, FindObjectsSortMode.None));
    }

    private void Start()
    {
        if (autoStart)
            StartNextWave();
    }

    [ContextMenu("Start Next Wave")]
    public void StartNextWave()
    {
        if (running != null) StopCoroutine(running);

        currentWaveIndex++;
        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("[Wave] No more waves.");
            return;
        }

        running = StartCoroutine(RunWave(waves[currentWaveIndex]));
    }

    private IEnumerator RunWave(WaveConfig wave)
    {
        if (spawners.Count == 0)
        {
            Debug.LogError("[Wave] No spawners assigned/found.");
            yield break;
        }

        Debug.Log($"[Wave] Start wave {currentWaveIndex} | count={wave.enemyCount}");

        for (int i = 0; i < wave.enemyCount; i++)
        {
            EnemySpawner spawner = PickSpawner(wave.roundRobin);

            // Optional: choose enemy type per spawn (if you add support in spawner)
            // For now, uses spawner's default prefab.
            spawner.SpawnEnemy();

            yield return new WaitForSeconds(wave.spawnInterval);
        }

        Debug.Log($"[Wave] End wave {currentWaveIndex}");
        running = null;
    }

    private EnemySpawner PickSpawner(bool roundRobin)
    {
        if (!roundRobin)
            return spawners[Random.Range(0, spawners.Count)];

        var s = spawners[roundRobinIndex % spawners.Count];
        roundRobinIndex++;
        return s;
    }
}
