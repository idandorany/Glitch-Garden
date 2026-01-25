using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask tileLayer;
    [Header("Merge")]
    [SerializeField] private MergeDatabase mergeDatabase;

    private readonly Dictionary<TileCell, Transform> tileToUnit = new();
    private readonly Dictionary<TileCell, UnitData> tileToData = new();

    private void Reset()
    {
        cam = Camera.main;
    }

    public bool TryPlace(GameObject unitPrefab, Vector2 screenPosition)
    {
        if (cam == null) cam = Camera.main;

        if (!mergeDatabase.TryResolveByPrefab(unitPrefab, out UnitData incomingData))
            return false;

        Vector3 world = cam.ScreenToWorldPoint(screenPosition);
        world.z = 0f;

        Collider2D hit = Physics2D.OverlapPoint(world, tileLayer);
        if (hit == null) return false;

        TileCell tile = hit.GetComponent<TileCell>();
        if (tile == null) return false;

        if (!tile.IsOccupied)
        {
            GameObject unit = Instantiate(unitPrefab, tile.CenterWorld, Quaternion.identity);

            Lane lane = tile.GetComponentInParent<Lane>();
            UnitCombat combat = unit.GetComponent<UnitCombat>();
            if (lane != null && combat != null)
                combat.SetRow(lane.RowIndex);

            tile.TryOccupy(unit.transform);
            tileToUnit[tile] = unit.transform;
            tileToData[tile] = incomingData;
            return true;
        }

        return false;
    }
}
