using UnityEngine;

public class BuffApplier : MonoBehaviour
{
    public void Apply(BuffData buff)
    {
        var m = GameModifiers.Instance;

        switch (buff.type)
        {
            case BuffType.PickaxeDamage:
                m.bonusDamage += (int)buff.value;
                break;

            case BuffType.ExtraHit:
                m.extraHits += (int)buff.value;
                break;

            case BuffType.ScoreMultiplier:
                m.scoreMultiplier += buff.value;
                break;

            case BuffType.PickaxeChance:
                m.extraPickaxeChance += buff.value;
                break;

            case BuffType.EyeChance:
                m.extraEyeChance += buff.value;
                break;

            case BuffType.FreeSpin:
                m.extraFreeSpins += (int)buff.value;
                break;
        }

        GameStateManager.Instance.StartGame();
    }
}
