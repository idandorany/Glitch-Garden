using UnityEngine;

public class MergeSystem : MonoBehaviour
{
    [Header("Prefabs (drag these from Project)")]
    public GameObject warriorPrefab;
    public GameObject wizardPrefab;
    public GameObject paladinPrefab;

    public bool TryMerge(Tower a, Tower b)
    {
        if (a == null || b == null) return false;
        if (a == b) return false;

        // Optional: require same stars to merge
        if (a.stars != b.stars) return false;

        int s = a.stars;
        Vector3 pos = b.transform.position;

        // Rule 1: Warrior + Wizard => Paladin (order doesn't matter)
        bool warriorWizard =
            (a.towerType == TowerType.Warrior && b.towerType == TowerType.Wizard) ||
            (a.towerType == TowerType.Wizard && b.towerType == TowerType.Warrior);

        if (warriorWizard)
        {
            Destroy(a.gameObject);
            Destroy(b.gameObject);

            GameObject obj = Instantiate(paladinPrefab, pos, Quaternion.identity);
            Tower t = obj.GetComponent<Tower>();
            t.towerType = TowerType.Paladin;
            t.SetStars(s); // or SetStars(s + 1) if you want a bonus
            return true;
        }

        // Rule 2: Same type + same stars => +1 star
        if (a.towerType == b.towerType)
        {
            GameObject prefab = PrefabForType(a.towerType);
            if (prefab == null) return false;

            Destroy(a.gameObject);
            Destroy(b.gameObject);

            GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
            Tower t = obj.GetComponent<Tower>();
            t.towerType = a.towerType;
            t.SetStars(s + 1);
            return true;
        }

        return false;
    }

    GameObject PrefabForType(TowerType type)
    {
        switch (type)
        {
            case TowerType.Warrior: return warriorPrefab;
            case TowerType.Wizard: return wizardPrefab;
            case TowerType.Paladin: return paladinPrefab;
            default: return null;
        }
    }
}
