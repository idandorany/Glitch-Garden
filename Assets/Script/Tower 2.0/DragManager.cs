using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Canvas canvas; // Screen Space - Overlay canvas
    [SerializeField] private InventoryManager inventory;
    [SerializeField] private PlacementManager placement;
    [SerializeField] private Image dragGhost; // Canvas/DragGhost

    private bool isDragging;
    private int draggingSlotIndex = -1;

    private void Start()
    {
        if (dragGhost != null)
            dragGhost.gameObject.SetActive(false);
    }

    private void Update()
    {
        HandleStartDrag();
        HandleDragMove();
        HandleEndDrag();
    }

    public void HandleStartDrag()
    {
        if (isDragging) return;
        if (!Input.GetMouseButtonDown(0)) return;

        int slotIndex = GetSlotIndexUnderMouse();
        if (slotIndex < 0) return;

        if (!inventory.HasItem(slotIndex))
            return;

        // Begin drag
        isDragging = true;
        draggingSlotIndex = slotIndex;

        if (dragGhost != null)
        {
            dragGhost.sprite = inventory.GetIcon(slotIndex);
            dragGhost.gameObject.SetActive(true);
            dragGhost.raycastTarget = false;
            UpdateGhostPosition();
        }
    }

    private void HandleDragMove()
    {
        if (!isDragging) return;
        UpdateGhostPosition();
    }

    private void HandleEndDrag()
    {
        if (!isDragging) return;
        if (!Input.GetMouseButtonUp(0)) return;

        bool placed = false;

        GameObject prefab = inventory.GetPrefab(draggingSlotIndex);
        if (prefab != null)
        {
            placed = placement.TryPlace(prefab, Input.mousePosition);
        }

        if (placed)
        {
            inventory.Consume(draggingSlotIndex);
        }

        // End drag
        isDragging = false;
        draggingSlotIndex = -1;

        if (dragGhost != null)
            dragGhost.gameObject.SetActive(false);
    }

    private void UpdateGhostPosition()
    {
        if (dragGhost == null) return;

        RectTransform ghostRect = dragGhost.rectTransform;
        Vector2 anchoredPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            Input.mousePosition,
            null,
            out anchoredPos
        );

        ghostRect.anchoredPosition = anchoredPos;
    }

    private int GetSlotIndexUnderMouse()
    {
        // Raycast UI to find Icon under the mouse
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = Input.mousePosition;

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);

        foreach (var r in results)
        {
            // Expecting object names like: Slot_0/Icon
            // We'll search parents for Slot_#
            Transform t = r.gameObject.transform;

            while (t != null)
            {
                if (t.name.StartsWith("Slot_"))
                {
                    string idxStr = t.name.Replace("Slot_", "");
                    if (int.TryParse(idxStr, out int idx))
                        return idx;
                }
                t = t.parent;
            }
        }

        return -1;
    }
}
