[System.Serializable]
public struct TowerId
{
    public TowerType type;
    public int stars;

    public TowerId(TowerType type, int stars)
    {
        this.type = type;
        this.stars = stars;
    }
}
