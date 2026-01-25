using System.Collections.Generic;
using UnityEngine;

public class CombatRegistry : MonoBehaviour
{
    public static CombatRegistry Instance { get; private set; }

    private List<UnitCombat>[] defendersByRow;

    private void Awake()
    {
        Instance = this;

        // Lanes are manually placed, so discover them
        Lane[] lanes = Object.FindObjectsByType<Lane>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        int maxRow = -1;
        foreach (var lane in lanes)
            maxRow = Mathf.Max(maxRow, lane.RowIndex);

        int rowCount = maxRow + 1;
        Initialize(rowCount);
    }

    private void Initialize(int rows)
    {
        defendersByRow = new List<UnitCombat>[rows];
        for (int i = 0; i < rows; i++)
            defendersByRow[i] = new List<UnitCombat>();
    }

    public void RegisterDefender(UnitCombat defender, int row)
    {
        if (!IsValidRow(row)) return;
        if (!defendersByRow[row].Contains(defender))
            defendersByRow[row].Add(defender);
        Debug.Log($"[Registry] Registered {defender.name} in row {row}");
    }

    public void UnregisterDefender(UnitCombat defender, int row)
    {
        if (!IsValidRow(row)) return;
        defendersByRow[row].Remove(defender);
    }

    public Transform GetClosestDefenderAhead(int row, float enemyX)
    {
        if (!IsValidRow(row)) return null;

        Transform best = null;
        float bestDx = float.MaxValue;

        var list = defendersByRow[row];
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var d = list[i];
            if (d == null) { list.RemoveAt(i); continue; }

            float dx = d.transform.position.x - enemyX; // ahead only
            if (dx >= 0f && dx < bestDx)
            {
                bestDx = dx;
                best = d.transform;
            }
        }
        return best;
    }

    private bool IsValidRow(int row) => defendersByRow != null && row >= 0 && row < defendersByRow.Length;
}
