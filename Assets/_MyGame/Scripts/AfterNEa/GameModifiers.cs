using UnityEngine;

public class GameModifiers : MonoBehaviour
{
    public static GameModifiers Instance;

    [Header("Pickaxe")]
    public int bonusDamage = 0;
    public int extraHits = 0;

    [Header("Score")]
    public float scoreMultiplier = 1f;

    [Header("Slots")]
    public float extraPickaxeChance = 0f;
    public float extraBookChance = 0f;
    public float extraEyeChance = 0f;

    [Header("Special")]
    public int extraFreeSpins = 0;

    void Awake()
    {
        Instance = this;
    }

    public void ResetAll()
    {
        bonusDamage = 0;
        extraHits = 0;
        scoreMultiplier = 1f;

        extraPickaxeChance = 0f;
        extraBookChance = 0f;
        extraEyeChance = 0f;

        extraFreeSpins = 0;
    }
}
