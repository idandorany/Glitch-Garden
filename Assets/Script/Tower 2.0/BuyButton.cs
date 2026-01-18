using UnityEngine;

public class BuyButton : MonoBehaviour
{
    [SerializeField] private InventoryManager inventory;

    [Header("Item to buy")]
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private Sprite unitIcon;

    public void Buy()
    {
        if (inventory == null || unitPrefab == null || unitIcon == null)
        {
            Debug.LogError("BuyButton: Missing references!");
            return;
        }

        bool success = inventory.TryAddToFirstEmpty(unitPrefab, unitIcon);

        if (!success)
            Debug.Log("BuyButton: Inventory full.");
    }
}
