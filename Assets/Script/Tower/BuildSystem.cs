/*using UnityEngine;

 public class BuildSystem : MonoBehaviour
{
    public TowerFactory factory;
    public MergeSystem mergeSystem;

    public TowerType selectedType = TowerType.Warrior;
    public int selectedStars = 1;

    // This is only used as an "incoming description" for rules checks
    // We'll avoid dummy objects by using the prefab's Tower component.
    public GameObject selectedTowerPrefab;

    public void TryPlaceOrMerge(Tile tile)
    {
        if (tile == null) return;

        if (selectedTowerPrefab == null)
        {
            Debug.LogError("BuildSystem: selectedTowerPrefab is not set.");
            return;
        }

        Tower incomingPrefabTower = selectedTowerPrefab.GetComponent<Tower>();
        if (incomingPrefabTower == null)
        {
            Debug.LogError("BuildSystem: selectedTowerPrefab has no Tower component.");
            return;
        }

        if (!tile.HasTower)
        {
            Tower placed = factory.Spawn(selectedType, tile.transform.position, selectedStars);
            tile.SetTower(placed);
            return;
        }

        bool merged = mergeSystem.TryMerge(tile, incomingPrefabTower);
        if (!merged)
            Debug.Log("Cannot merge these towers.");
    }
} */
