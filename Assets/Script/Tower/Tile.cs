using UnityEngine;

public class Tile : MonoBehaviour
{
    [HideInInspector]
    public bool hasTower = false;

    void OnMouseDown()
    {
        if (BuildManager.Instance == null) return;
        BuildManager.Instance.BuildOnTile(this);
    }
}
