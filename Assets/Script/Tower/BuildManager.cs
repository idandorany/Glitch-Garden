using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    public GameObject towerPrefab;
    public bool isBuildMode = false;

    void Awake()
    {
        Instance = this;
    }

    // Called by UI Button
    public void SelectTower()
    {
        isBuildMode = true;
        Debug.Log("Build mode ON");
    }

    public void BuildOnTile(Tile tile)
    {
        if (!isBuildMode) return;
        if (tile.hasTower) return;

        Instantiate(towerPrefab, tile.transform.position, Quaternion.identity);
        tile.hasTower = true;

        isBuildMode = false; // turn off after building once
        Debug.Log("Tower built, build mode OFF");
    }
}
