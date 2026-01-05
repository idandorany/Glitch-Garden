using UnityEngine;

public class MergeSystem : MonoBehaviour
{
    public MergeRules rules;
    public TowerFactory factory;

    public bool TryMerge(Tile tile, Tower incomingPrefabTower)
    {
        if (tile == null || incomingPrefabTower == null) return false;
        if (!tile.HasTower) return false;

        Tower existing = tile.PlacedTower;

        if (rules.TryGetResult(existing, incomingPrefabTower, out TowerType resultType, out int resultStars))
        {
            Destroy(existing.gameObject);

            Tower merged = factory.Spawn(resultType, tile.transform.position, resultStars);
            tile.SetTower(merged);
            return true;
        }

        return false;
    }
}
    