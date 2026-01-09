using UnityEngine;

public class GameModifiers : MonoBehaviour
{
    public static GameModifiers Instance;

    [Header("Pickaxes")]
    public int bonusPickaxeDamage = 0;

    [Header("Chances (WEIGHT ADD)")]
    public float eyeChanceBonus = 0f;
    public float bookChanceBonus = 0f;
    public float dynamiteChanceBonus = 0f;
    public float diamondPickaxeChanceBonus = 0f;
    public float reduceEmptyChance = 0f;

    [Header("Score")]
    public int bonusScorePerBlock = 0;

    void Awake()
    {
        Instance = this;
    }

    public void ResetAll()
    {
        bonusPickaxeDamage = 0;

        eyeChanceBonus = 0f;
        bookChanceBonus = 0f;
        dynamiteChanceBonus = 0f;
        diamondPickaxeChanceBonus = 0f;
        reduceEmptyChance = 0f;

        bonusScorePerBlock = 0;
    }
}