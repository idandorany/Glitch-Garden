using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TowerDrag : MonoBehaviour
{
    public float snapBackSpeed = 25f;

    Camera cam;
    Vector3 startPos;
    Tile startTile;
    bool dragging;

    Tower tower;

    void Awake()
    {
        cam = Camera.main;
        tower = GetComponent<Tower>();
    }

    void OnMouseDown()
    {
        // record start
        startPos = transform.position;
        startTile = FindTileUnderMe();
        dragging = true;
    }

    void OnMouseDrag()
    {
        if (!dragging || cam == null) return;

        Vector3 world = cam.ScreenToWorldPoint(Input.mousePosition);
        world.z = 0f;
        transform.position = world;
    }

    void OnMouseUp()
    {
        if (!dragging) return;
        dragging = false;

        Tile targetTile = FindTileUnderMe();

        // If no tile under drop -> snap back
        if (targetTile == null)
        {
            transform.position = startPos;
            return;
        }

        // If dropped on same tile -> snap back
        if (targetTile == startTile)
        {
            transform.position = startPos;
            return;
        }

        // Ask Systems to handle move/merge
        var dragSystem = FindFirstObjectByType<TowerDragSystem>();
        if (dragSystem == null)
        {
            Debug.LogError("No TowerDragSystem in scene.");
            transform.position = startPos;
            return;
        }

        bool success = dragSystem.TryDropTower(tower, startTile, targetTile);

        if (!success)
            transform.position = startPos;
    }

    Tile FindTileUnderMe()
    {
        // Raycast at tower position to find Tile
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
        if (hit.collider == null) return null;
        return hit.collider.GetComponent<Tile>();
    }
}
