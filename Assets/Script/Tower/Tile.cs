using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Tile : MonoBehaviour
{
    public Tower PlacedTower { get; private set; }
    public bool HasTower => PlacedTower != null;

    public void SetTower(Tower tower)
    {
        PlacedTower = tower;
    }

    public void ClearTower()
    {
        PlacedTower = null;
    }
}
