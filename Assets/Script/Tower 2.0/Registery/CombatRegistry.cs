using System.Collections.Generic;
using UnityEngine;

public class CombatRegistry : MonoBehaviour
{
    public static CombatRegistry Instance { get; private set; }

    private List<UnitCombat>[] defendersByRow;
    private List<Enemy>[] enemiesByRow;

    private void Awake()
    {
        Instance = this;

        Lane[] lanes = Object.FindObjectsByType<Lane>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        int maxRow = -1;
        foreach (var lane in lanes)
            maxRow = Mathf.Max(maxRow, lane.RowIndex);

        int rowCount = maxRow + 1;

        defendersByRow = new List<UnitCombat>[rowCount];
        enemiesByRow = new List<Enemy>[rowCount];

        for (int i = 0; i < rowCount; i++)
        {
            defendersByRow[i] = new List<UnitCombat>();
            enemiesByRow[i] = new List<Enemy>();
        }
    }

    /* ---------------- DEFENDERS ---------------- */

    public void RegisterDefender(UnitCombat defender, int row)
    {
        if (!IsValidRow(row)) return;
        if (!defendersByRow[row].Contains(defender))
            defendersByRow[row].Add(defender);
    }

    public void UnregisterDefender(UnitCombat defender, int row)
    {
        if (!IsValidRow(row)) return;
        defendersByRow[row].Remove(defender);
    }

    /* ---------------- ENEMIES ---------------- */

    public void RegisterEnemy(Enemy enemy, int row)
    {
        if (!IsValidRow(row)) return;
        if (!enemiesByRow[row].Contains(enemy))
            enemiesByRow[row].Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy, int row)
    {
        if (!IsValidRow(row)) return;
        enemiesByRow[row].Remove(enemy);
    }

    public Enemy GetClosestEnemyInRow(int row, float fromX)
    {
        if (!IsValidRow(row)) return null;

        Enemy best = null;
        float bestDx = float.MaxValue;

        var list = enemiesByRow[row];
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var e = list[i];
            if (e == null) { list.RemoveAt(i); continue; }

            float dx = e.transform.position.x - fromX;
            if (dx >= 0f && dx < bestDx)
            {
                bestDx = dx;
                best = e;
            }
        }
        return best;
    }

    public Transform GetClosestDefenderAhead(int row, float enemyX)
    {
        if (row < 0 || row >= defendersByRow.Length) return null;

        Transform best = null;
        float bestDx = float.MaxValue;

        foreach (var d in defendersByRow[row])
        {
            if (d == null) continue;
            float dx = d.transform.position.x - enemyX;
            if (dx >= 0 && dx < bestDx)
            {
                bestDx = dx;
                best = d.transform;
            }
        }

        return best;
    }

    private bool IsValidRow(int row)
    {
        return row >= 0 && row < enemiesByRow.Length;
    }
}
