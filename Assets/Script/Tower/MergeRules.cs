using UnityEngine;

public class MergeRules : MonoBehaviour
{
    [System.Serializable]
    public struct MergeRecipe
    {
        public TowerType a;
        public TowerType b;
        public int stars;              // both inputs must have this many stars
        public TowerType result;
        public int resultStars;        // set 0 to “keep same stars”
    }

    public MergeRecipe[] recipes;

    public bool TryGetResult(Tower t1, Tower t2, out TowerType resultType, out int resultStars)
    {
        resultType = default;
        resultStars = 0;

        if (t1 == null || t2 == null) return false;
        if (t1.stars != t2.stars) return false;

        TowerType x = t1.towerType;
        TowerType y = t2.towerType;
        int stars = t1.stars;

        // Normalize order (A+B == B+A)
        if ((int)x > (int)y) { var tmp = x; x = y; y = tmp; }

        foreach (var r in recipes)
        {
            TowerType ra = r.a;
            TowerType rb = r.b;
            if ((int)ra > (int)rb) { var tmp = ra; ra = rb; rb = tmp; }

            if (ra == x && rb == y && r.stars == stars)
            {
                resultType = r.result;
                resultStars = (r.resultStars <= 0) ? stars : r.resultStars;
                return true;
            }
        }

        return false;
    }
}
