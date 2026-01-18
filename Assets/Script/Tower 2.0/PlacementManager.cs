using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask tileLayer;

    private void Reset()
    {
        cam = Camera.main;
    }

    public bool TryPlace(GameObject unitPrefab, Vector2 screenPosition)
    {
        if (cam == null) cam = Camera.main;

        Vector3 world = cam.ScreenToWorldPoint(screenPosition);
        world.z = 0f;

        Collider2D hit = Physics2D.OverlapPoint(world, tileLayer);
        if (hit == null) return false;

        TileCell tile = hit.GetComponent<TileCell>();
        if (tile == null) return false;
        if (tile.IsOccupied) return false;

        GameObject unit = Instantiate(unitPrefab, tile.CenterWorld, Quaternion.identity);
        tile.TryOccupy(unit.transform);
        return true;
    }
}
