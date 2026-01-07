using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public static ChestManager Instance;
    ChestMultiplier[] chests;

    void Awake()
    {
        Instance = this;
        chests = FindObjectsOfType<ChestMultiplier>(true);
    }

    public void ResetAll()
    {
        foreach (var c in chests)
            c.ResetChest();
    }
}