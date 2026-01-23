using UnityEngine;

public class Lane : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private Transform tilesRoot;

    private TileCell[] tiles;
    private TileCell firstTile;

    public TileCell[] Tiles => tiles;
    public TileCell FirstTile => firstTile;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (tilesRoot == null)
        {
            Debug.LogError($"{name}: tilesRoot is not assigned.");
            return;
        }

        tiles = tilesRoot.GetComponentsInChildren<TileCell>(includeInactive: true);

        if (tiles == null || tiles.Length == 0)
        {
            Debug.LogError($"{name}: No TileCell found under tilesRoot.");
            return;
        }

        firstTile = tiles[0];

        if (spawner == null)
        {
            Debug.LogError($"{name}: spawner is not assigned.");
            return;
        }

        spawner.TileCell = firstTile.transform;
    }
}
