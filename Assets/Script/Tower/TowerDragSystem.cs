using UnityEngine;

public class TowerDragSystem : MonoBehaviour
{
    public MergeSystem mergeSystem;
    public TowerFactory factory;

    public bool TryDropTower(Tower draggedTower, Tile fromTile, Tile toTile)
    {
        if (draggedTower == null || fromTile == null || toTile == null) return false;

        // If target tile is empty -> MOVE
        if (!toTile.HasTower)
        {
            // Move tower instance
            draggedTower.transform.position = toTile.transform.position;

            // Update tile references
            fromTile.ClearTower();
            toTile.SetTower(draggedTower);

            return true;
        }

        // If target tile has tower -> try MERGE
        Tower targetTower = toTile.PlacedTower;

        // Use rules to compute merge result
        if (mergeSystem.rules.TryGetResult(draggedTower, targetTower, out TowerType resultType, out int resultStars))
        {
            // Destroy both old towers
            Object.Destroy(draggedTower.gameObject);
            Object.Destroy(targetTower.gameObject);

            // Spawn merged tower on target tile
            Tower merged = factory.Spawn(resultType, toTile.transform.position, resultStars);
            toTile.SetTower(merged);

            // Clear source tile
            fromTile.ClearTower();

            return true;
        }

        // Can't merge -> fail
        return false;
    }
}
