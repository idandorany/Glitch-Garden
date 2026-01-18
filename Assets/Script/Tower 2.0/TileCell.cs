using UnityEngine;

public class TileCell : MonoBehaviour
{
    [field: SerializeField] public Vector2Int GridPos { get; private set; }

    public bool IsOccupied => occupant != null;
    private Transform occupant;

    public Vector3 CenterWorld => transform.position;

    public void Init(Vector2Int pos) => GridPos = pos;

    public bool TryOccupy(Transform unit)
    {
        if (occupant != null) return false;
        occupant = unit;
        return true;
    }

    public void Clear() => occupant = null;
}
