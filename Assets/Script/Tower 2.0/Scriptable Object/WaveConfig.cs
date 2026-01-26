using UnityEngine;

[CreateAssetMenu(menuName = "Tower/Wave Config", fileName = "WaveConfig")]
public class WaveConfig : ScriptableObject
{
    [Header("Wave")]
    public int enemyCount = 10;
    public float spawnInterval = 0.75f;

    [Header("Lane Distribution")]
    public bool roundRobin = true;

    [Header("Optional: choose enemy prefab per wave (if empty, spawner uses its default)")]
    public Enemy[] possibleEnemies;
}
