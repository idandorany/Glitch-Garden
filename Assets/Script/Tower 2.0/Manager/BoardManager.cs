using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("Grid Size")]
    [SerializeField] private int width = 5;
    [SerializeField] private int height = 5;

    [Header("Tile Settings")]
    [SerializeField] private TileCell tilePrefab;
    [SerializeField] private Vector2 spacing = new Vector2(1.2f, 1.2f);
    [SerializeField] private Vector2 origin = Vector2.zero;

    private TileCell[,] tiles;

    public int GetWidth() => width;
    public Vector2 GetSpacing() => spacing;
    public Vector2 GetOrigin() => origin;


    public int RowCount => height;

    private void Awake()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        tiles = new TileCell[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 pos = origin + new Vector2(
                    x * spacing.x,
                    -y * spacing.y
                );

                TileCell tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                tile.Init(new Vector2Int(x, y));

                tiles[x, y] = tile;
            }
        }
    }

    // ---------------- GIZMOS ----------------

    private void OnDrawGizmos()
    {
        Gizmos.matrix = Matrix4x4.identity;

        // Draw tiles
        Gizmos.color = Color.gray;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 pos = origin + new Vector2(
                    x * spacing.x,
                    -y * spacing.y
                );

                Gizmos.DrawWireCube(pos, spacing * 0.9f);
            }
        }

        // Draw lanes (row centers)
        Gizmos.color = Color.yellow;

        for (int y = 0; y < height; y++)
        {
            float laneY = origin.y - y * spacing.y;

            Vector3 start = new Vector3(
                origin.x - spacing.x * 0.5f,
                laneY,
                0
            );

            Vector3 end = new Vector3(
                origin.x + (width - 1) * spacing.x + spacing.x * 0.5f,
                laneY,
                0
            );

            Gizmos.DrawLine(start, end);
        }
    }
}
