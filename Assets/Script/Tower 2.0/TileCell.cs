using UnityEngine;

public class TileCell : MonoBehaviour
{
    public Vector2Int GridPos { get; private set; }
    public bool IsOccupied { get; private set; }

    public Vector3 CenterWorld => transform.position;



    //-----------------------

        public Lane ParentLane { get; private set; }

        public void SetLane(Lane lane)
        {
            ParentLane = lane;
        }


    //-----------------------




    public void Init(Vector2Int gridPos)
    {
        GridPos = gridPos;
        IsOccupied = false;
    }

    public bool TryOccupy(Transform unit)
    {
        if (IsOccupied) return false;

        IsOccupied = true;
        unit.position = transform.position;
        return true;
    }
    
    public void Clear()
    {
        IsOccupied = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.05f);
    }
}
