using UnityEngine;

public static class SlotWeightCalculator
{
    public static float GetWeight(SymbolConfig s)
    {
        float w = s.weight;

        var m = GameModifiers.Instance;

        if (s.id == "Book")
            w += m.bookChanceBonus;

        if (s.id == "Eye")
            w += m.eyeChanceBonus;

        if (s.id == "Dynamite")
            w += m.dynamiteChanceBonus;

        if (s.id == "PickaxeDiamond")
            w += m.diamondPickaxeChanceBonus;

        if (s.isEmpty)
            w = Mathf.Max(0f, w - m.reduceEmptyChance);

        return Mathf.Max(0.01f, w);
    }
}
