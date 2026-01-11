using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotItem : MonoBehaviour
{
    public Image image;
    public TMP_Text amountText;

    public RectTransform Rect => transform as RectTransform;

    public SymbolConfig CurrentSymbol { get; private set; }
    public int Amount { get; private set; }

    public void SetRandom(SymbolConfig[] symbols)
    {
        float total = 0f;

        foreach (var s in symbols)
            total += SlotWeightCalculator.GetWeight(s);


        float roll = Random.Range(0f, total);
        float current = 0f;

        foreach (var s in symbols)
        {
            current += SlotWeightCalculator.GetWeight(s);
            if (roll <= current)
            {
                SetSymbol(s);
                return;
            }
        }
    }

    void SetSymbol(SymbolConfig s)
    {
        CurrentSymbol = s;

        if (s.hasAmount)
        {
            Amount = Random.Range(1, 6); // 🔥 ВОЗВРАЩАЕМ
        }
        else
        {
            Amount = 1;
        }

        if (image != null)
        {
            image.enabled = true;
            image.sprite = s.sprite;
        }

        if (amountText != null)
        {
            amountText.text = Amount > 1 ? Amount.ToString() : "";
        }
    }

    //float GetModifiedWeight(SymbolConfig s)
    //{
    //    float w = SlotWeightCalculator.GetWeight(s);


    //    var buffs = GameModifiers.Instance;

    //    if (s.id == "PickaxeDiamond")
    //        w += buffs.diamondPickaxeChanceBonus;

    //    if (s.id == "Book")
    //        w += buffs.bookChanceBonus;

    //    if (s.id == "Dynamite")
    //        w += buffs.dynamiteChanceBonus;

    //    if (s.id == "Eye")
    //        w += buffs.eyeChanceBonus;

    //    if (s.isEmpty)
    //        w = Mathf.Max(0f, w - buffs.reduceEmptyChance);

    //    return Mathf.Max(0.01f, w);
    //}

}
