using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    public int maxHP;
    public int currentHP;
    public int reward;

    [Header("Damage Visuals")]
    public Image[] damageStages; // сюда нужно перетащить все 5 Image

    public void Init(int hp, int rewardValue)
    {
        maxHP = hp;
        currentHP = hp;
        reward = rewardValue;

        UpdateVisual();
    }

    public void TakeHit(int damage)
    {
        currentHP -= damage;
        UpdateVisual();

        if (currentHP <= 0)
        {
            DestroyBlock();
        }
    }

    void UpdateVisual()
    {
        // сколько уже нанесено урона
        int damageDone = maxHP - currentHP;

        for (int i = 0; i < damageStages.Length; i++)
        {
            // включаем картинки по количеству урона
            damageStages[i].enabled = (i < damageDone);
        }
    }

    void DestroyBlock()
    {
        Debug.Log($"Ѕлок разрушен. Ќаграда: {reward}");
        Destroy(gameObject);
    }
}
