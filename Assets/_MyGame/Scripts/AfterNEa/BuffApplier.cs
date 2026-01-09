using UnityEngine;

public class BuffApplier : MonoBehaviour
{
    public static BuffApplier Instance;

    void Awake()
    {
        Instance = this;
    }

    public void Apply(BuffData buff)
    {
        var m = GameModifiers.Instance;

        switch (buff.type)
        {
            case BuffType.PickaxeDamage:
                m.bonusPickaxeDamage += (int)buff.value;
                break;

            case BuffType.EyeChance:
                m.eyeChanceBonus += buff.value;
                break;

            case BuffType.BookChance:
                m.bookChanceBonus += buff.value;
                break;

            case BuffType.DynamiteChance:
                m.dynamiteChanceBonus += buff.value;
                break;

            case BuffType.DiamondPickaxeChance:
                m.diamondPickaxeChanceBonus += buff.value;
                break;

            case BuffType.ReduceEmpty:
                m.reduceEmptyChance += buff.value;
                break;

            case BuffType.ScorePerBlock:
                m.bonusScorePerBlock += (int)buff.value;
                break;
        }

        GameStateManager.Instance.StartGame();
    }
}