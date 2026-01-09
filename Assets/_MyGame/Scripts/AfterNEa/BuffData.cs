public enum BuffPower
{
    Weak,
    Medium,
    Strong
}

[System.Serializable]
public class BuffData
{
    public string title;
    public string description;
    public BuffType type;
    public float value;
    public BuffPower power;
}
