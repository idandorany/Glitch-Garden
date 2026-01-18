using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private int width = 5;
    [SerializeField] private int height = 5;

    [SerializeField] private Vector2 spacing = new Vector2(1.6f, 1.2f);
    [SerializeField] private Vector2 origin = new Vector2(-3.2f, 2.4f);

    [SerializeField] private TileCell tilePrefab;

    private TileCell[,] tiles;

    private void Awake()
    {
        Generate();
    }

    private void Generate()
    {
        tiles = new TileCell[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 pos = origin + new Vector2(x * spacing.x, -y * spacing.y);

                TileCell tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                tile.name = $"Tile ({x},{y})";
                tile.Init(new Vector2Int(x, y));

                tiles[x, y] = tile;
            }
        }
    }
}
