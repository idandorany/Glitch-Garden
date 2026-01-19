using System;
using UnityEngine;
using UnityEngine.UI;

public class DragManager : MonoBehaviour
{
    // Events (optional subscribers)
    public event Action<int> DragStarted;                 // slotIndex
    public event Action<Vector2> DragMoved;               // screenPos
    public event Action<int, Vector2, bool> DragEnded;    // slotIndex, screenPos, placedSuccess

    [Header("References")]
    [SerializeField] private Canvas canvas;               // Screen Space - Overlay
    [SerializeField] private InventoryManager inventory;
    [SerializeField] private PlacementManager placement;

    [Header("Slot Icons (5) - drag Slot_0/Icon ... Slot_4/Icon here")]
    [SerializeField] private RectTransform[] slotIconRects = new RectTransform[5];

    [Header("Drag Ghost (Canvas/DragGhost Image)")]
    [SerializeField] private Image dragGhost;

    private bool isDragging;
    private int draggingSlotIndex = -1;

    private void Start()
    {
        if (dragGhost != null)
        {
            dragGhost.raycastTarget = false;
            dragGhost.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isDragging)
        {
            if (Input.GetMouseButtonDown(0))
                TryBeginDrag(Input.mousePosition);

            return;
        }

        // dragging
        UpdateGhostPosition(Input.mousePosition);
        DragMoved?.Invoke(Input.mousePosition);

        if (Input.GetMouseButtonUp(0))
            EndDrag(Input.mousePosition);
    }

    private void TryBeginDrag(Vector2 screenPos)
    {
        int slotIndex = FindSlotIndexAtScreenPos(screenPos);
        if (slotIndex < 0) return;

        if (!inventory.HasItem(slotIndex))
            return;

        isDragging = true;
        draggingSlotIndex = slotIndex;

        if (dragGhost != null)
        {
            dragGhost.sprite = inventory.GetIcon(slotIndex);
            dragGhost.gameObject.SetActive(true);
            UpdateGhostPosition(screenPos);
        }

        DragStarted?.Invoke(slotIndex);
    }

    private void EndDrag(Vector2 screenPos)
    {
        bool placed = false;

        GameObject prefab = inventory.GetPrefab(draggingSlotIndex);
        if (prefab != null)
            placed = placement.TryPlace(prefab, screenPos);

        if (placed)
            inventory.Consume(draggingSlotIndex);

        isDragging = false;
        int endedSlot = draggingSlotIndex;
        draggingSlotIndex = -1;

        if (dragGhost != null)
            dragGhost.gameObject.SetActive(false);

        DragEnded?.Invoke(endedSlot, screenPos, placed);
    }

    private void UpdateGhostPosition(Vector2 screenPos)
    {
        if (dragGhost == null || canvas == null) return;

        RectTransform ghostRect = dragGhost.rectTransform;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            screenPos,
            null,
            out Vector2 anchoredPos
        );

        ghostRect.anchoredPosition = anchoredPos;
    }

    private int FindSlotIndexAtScreenPos(Vector2 screenPos)
    {
        // Only checks up to 5 rects (very cheap)
        for (int i = 0; i < slotIconRects.Length; i++)
        {
            RectTransform rt = slotIconRects[i];
            if (rt == null) continue;

            if (RectTransformUtility.RectangleContainsScreenPoint(rt, screenPos, null))
                return i;
        }

        return -1;
    }
}
