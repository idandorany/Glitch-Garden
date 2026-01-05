using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class TowerDragAndDrop : MonoBehaviour
{
    Camera cam;
    Vector3 startPos;

    Collider2D myCol;
    Rigidbody2D rb;
    Tower tower;

    void Awake()
    {
        cam = Camera.main;
        myCol = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        tower = GetComponent<Tower>();

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
    }

    void OnMouseDown()
    {
        startPos = transform.position;
    }

    void OnMouseDrag()
    {
        if (cam == null) return;
        Vector3 world = cam.ScreenToWorldPoint(Input.mousePosition);
        world.z = 0f;
        transform.position = world;
    }

    void OnMouseUp()
    {
        // Find the tower we were dropped onto
        Tower target = FindTowerUnderMe();

        if (target != null && target != tower)
        {
            MergeSystem mergeSystem = FindFirstObjectByType<MergeSystem>();
            if (mergeSystem == null)
            {
                Debug.LogError("No TowerMergeSystem in scene (add it to a Managers object).");
                transform.position = startPos;
                return;
            }

            bool merged = mergeSystem.TryMerge(tower, target);
            if (!merged) transform.position = startPos;
            return;
        }

        // Not dropped on another tower
        transform.position = startPos;
    }

    Tower FindTowerUnderMe()
    {
        // Temporarily disable our own collider so we don't detect ourselves
        myCol.enabled = false;

        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
        myCol.enabled = true;

        foreach (var h in hits)
        {
            Tower t = h.GetComponent<Tower>();
            if (t != null && t != tower) return t;
        }

        return null;
    }
}
