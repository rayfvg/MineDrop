using UnityEngine;

public class GameModifiers : MonoBehaviour
{
    public static GameModifiers Instance;

    public float bookChanceBonus;
    public float eyeChanceBonus;
    public float dynamiteChanceBonus;
    public float diamondPickaxeChanceBonus;
    public float reduceEmptyChance;

    void Awake()
    {
        Instance = this;
        Load(); // 🔥 ВАЖНО
    }

    public void AddModifier(string symbolId, float value)
    {
        switch (symbolId)
        {
            case "Book": bookChanceBonus += value; break;
            case "Eye": eyeChanceBonus += value; break;
            case "Dynamite": dynamiteChanceBonus += value; break;
            case "PickaxeDiamond": diamondPickaxeChanceBonus += value; break;
            case "Empty": reduceEmptyChance += value; break;
        }

        Save(); // 🔥 сохраняем сразу
    }

    public void ResetAll()
    {
        bookChanceBonus = 0;
        eyeChanceBonus = 0;
        dynamiteChanceBonus = 0;
        diamondPickaxeChanceBonus = 0;
        reduceEmptyChance = 0;

        Save();
    }

    void Save()
    {
        PlayerPrefs.SetFloat("buff_book", bookChanceBonus);
        PlayerPrefs.SetFloat("buff_eye", eyeChanceBonus);
        PlayerPrefs.SetFloat("buff_dynamite", dynamiteChanceBonus);
        PlayerPrefs.SetFloat("buff_pickaxe", diamondPickaxeChanceBonus);
        PlayerPrefs.SetFloat("buff_empty", reduceEmptyChance);
    }

    void Load()
    {
        bookChanceBonus = PlayerPrefs.GetFloat("buff_book", 0);
        eyeChanceBonus = PlayerPrefs.GetFloat("buff_eye", 0);
        dynamiteChanceBonus = PlayerPrefs.GetFloat("buff_dynamite", 0);
        diamondPickaxeChanceBonus = PlayerPrefs.GetFloat("buff_pickaxe", 0);
        reduceEmptyChance = PlayerPrefs.GetFloat("buff_empty", 0);
    }
}
