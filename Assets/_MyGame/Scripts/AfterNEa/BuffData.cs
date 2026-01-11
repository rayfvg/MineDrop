using UnityEngine;

[System.Serializable]
public class BuffData
{
    public string id;                 // "Buff_Dynamite"
    public string title;              // "Шанс динамита"
    public string symbolId;           // "Dynamite"
    public float weightDelta;         // +5, +10, -3
    public string particleId; // например: "Diamond", "Book"
}
