using System.Collections.Generic;
using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    [System.Serializable]
    public struct TowerPrefabEntry
    {
        public TowerType type;
        public GameObject prefab;
    }

    public TowerPrefabEntry[] prefabs;

    Dictionary<TowerType, GameObject> map;

    void Awake()
    {
        map = new Dictionary<TowerType, GameObject>();
        foreach (var e in prefabs)
        {
            if (e.prefab != null)
                map[e.type] = e.prefab;
        }
    }

    public Tower Spawn(TowerType type, Vector3 pos, int stars)
    {
        if (!map.TryGetValue(type, out var prefab))
        {
            Debug.LogError($"No prefab for tower type: {type}");
            return null;
        }

        GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
        Tower tower = obj.GetComponent<Tower>();
        if (tower != null) tower.SetStars(stars);
        return tower;
    }
}
