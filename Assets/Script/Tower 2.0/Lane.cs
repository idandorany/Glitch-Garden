using UnityEngine;

public class Lane : MonoBehaviour
{
    [Header("Lane")]
    [SerializeField] private int rowIndex = 0;
    public int RowIndex => rowIndex;

    [Header("References")]
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private Transform tilesRoot;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (spawner != null)
            spawner.SetRow(rowIndex);

        // Optional: sanity check tiles exist
        if (tilesRoot == null)
            Debug.LogWarning($"{name}: tilesRoot not assigned");
    }
}
