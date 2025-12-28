using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Prefabs")]
    public Tile tilePrefab;
    public GameObject towerPrefab;

    [Header("Grid")]
    public int width = 10;
    public int height = 6;
    public float cellSize = 1f;
    public Vector2 origin = Vector2.zero;

    [Header("Path (blocked tiles)")]
    public int pathRowY = 2; // tiles on this row become the road (blocked)

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = origin + new Vector2(x * cellSize, y * cellSize);

                Tile t = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                t.name = $"Tile_{x}_{y}";

                bool blocked = (y == pathRowY); // simple straight road
                t.Init(blocked);
            }
        }
    }
}
