using UnityEngine;

public class Lane : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private Transform tilesRoot;

    private TileCell[] tiles;
    public TileCell[] Tiles => tiles;

    public void Initialize()
    {
        tiles = tilesRoot.GetComponentsInChildren<TileCell>(includeInactive: true);

        if (tiles == null || tiles.Length == 0)
        {
            Debug.LogError($"{name}: No TileCell found under TileRoot.");
            return;
        }

        if (spawner == null)
        {
            Debug.LogError($"{name}: Spawner reference missing.");
            return;
        }
    }
}
