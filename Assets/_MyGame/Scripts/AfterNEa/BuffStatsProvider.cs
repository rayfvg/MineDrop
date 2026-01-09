public static class BuffStatsProvider
{
    public static float GetValue(string id)
    {
        var m = GameModifiers.Instance;

        return id switch
        {
            "PickaxeDamage" => m.bonusPickaxeDamage,
            "EyeChance" => m.eyeChanceBonus,
            "BookChance" => m.bookChanceBonus,
            "DynamiteChance" => m.dynamiteChanceBonus,
            "DiamondChance" => m.diamondPickaxeChanceBonus,
            "ReduceEmpty" => m.reduceEmptyChance,
            "ScorePerBlock" => m.bonusScorePerBlock,
            _ => 0f
        };
    }
}