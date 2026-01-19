using System.Collections.Generic;
using UnityEngine;

public class BuyManager : MonoBehaviour
{
    [System.Serializable]
    public class BuyEntry
    {
        public UnitData unit;
        public float baseWeight = 1f;
    }

    [Header("Buy Pool")]
    [SerializeField] private List<BuyEntry> buyableUnits = new();

    [Header("Repeat Decreaser")]
    [SerializeField] private bool repeatDecreaserActive = false;

    [SerializeField] private int rollsBeforePenalty = 1;
    [SerializeField] private float penaltyMultiplier = 0.5f;
    [SerializeField] private float minimumWeight = 0.1f;

    private UnitData lastRolledUnit;
    private int rollsSinceLastSame;

    // ---- PUBLIC API ----
    public bool TryRoll(out UnitData result)
    {
        result = null;

        if (buyableUnits.Count == 0)
            return false;

        float totalWeight = 0f;

        // calculate total effective weight
        foreach (var entry in buyableUnits)
        {
            float weight = GetEffectiveWeight(entry);
            totalWeight += weight;
        }

        float roll = Random.Range(0f, totalWeight);
        float cursor = 0f;

        foreach (var entry in buyableUnits)
        {
            float weight = GetEffectiveWeight(entry);
            cursor += weight;

            if (roll <= cursor)
            {
                result = entry.unit;
                UpdateRepeatState(result);
                return true;
            }
        }

        return false;
    }

    // ---- INTERNAL LOGIC ----
    private float GetEffectiveWeight(BuyEntry entry)
    {
        float weight = entry.baseWeight;

        if (!repeatDecreaserActive)
            return weight;

        if (entry.unit == lastRolledUnit && rollsSinceLastSame < rollsBeforePenalty)
        {
            weight *= penaltyMultiplier;
            weight = Mathf.Max(weight, minimumWeight);
        }

        return weight;
    }

    private void UpdateRepeatState(UnitData rolled)
    {
        if (rolled == lastRolledUnit)
            rollsSinceLastSame++;
        else
        {
            lastRolledUnit = rolled;
            rollsSinceLastSame = 0;
        }
    }
}
