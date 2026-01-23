using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask tileLayer;

    [Header("Merge")]
    [SerializeField] private MergeDatabase mergeDatabase;

    // Track what is on each tile (so we can merge/replace)
    private readonly Dictionary<TileCell, Transform> tileToUnit = new();
    private readonly Dictionary<TileCell, UnitData> tileToData = new();

    private void Reset()
    {
        cam = Camera.main;
    }

    public bool TryPlace(GameObject unitPrefab, Vector2 screenPosition)
    {
        if (cam == null) cam = Camera.main;

        if (mergeDatabase == null)
        {
            Debug.LogError("PlacementManager: MergeDatabase is missing!");
            return false;
        }

        if (!mergeDatabase.TryResolveByPrefab(unitPrefab, out UnitData incomingData))
        {
            Debug.LogError("PlacementManager: Could not resolve UnitData for prefab. Make sure this unit exists in MergeDatabase recipes.");
            return false;
        }

        Vector3 world = cam.ScreenToWorldPoint(screenPosition);
        world.z = 0f;

        Collider2D hit = Physics2D.OverlapPoint(world, tileLayer);
        if (hit == null) return false;

        TileCell tile = hit.GetComponent<TileCell>();
        if (tile == null) return false;

        // If empty: normal place
        if (!tile.IsOccupied)
        {
            GameObject unit = Instantiate(unitPrefab, tile.CenterWorld, Quaternion.identity);
            tile.TryOccupy(unit.transform);

            tileToUnit[tile] = unit.transform;
            tileToData[tile] = incomingData;
            return true;
        }

        // Occupied: attempt merge
        if (!tileToData.TryGetValue(tile, out UnitData existingData) || existingData == null)
        {
            Debug.LogError("PlacementManager: Tile is occupied but has no UnitData tracked (dictionary missing).");
            return false;
        }

        if (!mergeDatabase.TryGetMergeResult(existingData, incomingData, out UnitData outputData))
            return false; // no recipe

        // Merge success: replace unit
        if (tileToUnit.TryGetValue(tile, out Transform existingUnitTf) && existingUnitTf != null)
            Destroy(existingUnitTf.gameObject);

        tile.Clear();

        GameObject merged = Instantiate(outputData.prefab, tile.CenterWorld, Quaternion.identity);
        tile.TryOccupy(merged.transform);

        tileToUnit[tile] = merged.transform;
        tileToData[tile] = outputData;

        return true;
    }
}
