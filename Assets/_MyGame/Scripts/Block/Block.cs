using UnityEngine;

public class Block : MonoBehaviour
{
    public int maxHP;
    public int currentHP;
    public int reward;

    public void Init(int hp, int rewardValue)
    {
        maxHP = hp;
        currentHP = hp;
        reward = rewardValue;
    }

    public void TakeHit()
    {
        currentHP--;

        if (currentHP <= 0)
        {
            DestroyBlock();
        }
    }

    void DestroyBlock()
    {
        Debug.Log($"Блок разрушен. Награда: {reward}");
        Destroy(gameObject);
    }
}
