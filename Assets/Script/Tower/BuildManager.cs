using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public GameObject towerPrefab;
    public bool buildMode;

    Camera cam;

    void Awake()
    {
        cam = Camera.main;
        if (cam == null) Debug.LogError("No Main Camera found. Tag your camera as MainCamera.");
    }

    // Hook this to your button OnClick
    public void SelectTower()
    {
        buildMode = true;
        Debug.Log("Build mode ON - click on ground to place");
    }

    void Update()
    {
        if (!buildMode) return;
        if (!Input.GetMouseButtonDown(0)) return;
        if (cam == null) return;

        // Don’t place if clicking UI
        if (UnityEngine.EventSystems.EventSystem.current != null &&
            UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;

        Vector2 world = cam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(world, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject.name == "Ground")
        {
            Instantiate(towerPrefab, hit.point, Quaternion.identity);
            buildMode = false;
            Debug.Log("Tower placed!");
        }
    }
}
