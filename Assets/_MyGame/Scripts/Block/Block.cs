using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    public int maxHP;
    public int currentHP;
    public int reward;

    [Header("Damage Visuals")]
    public Image[] damageStages; // сюда нужно перетащить все 5 Image

    void OnDestroy()
    {
        if (transform != null)
            transform.DOKill();
    }

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

        PlayHitAnimation();
        UpdateVisual();

        if (currentHP <= 0)
        {
            DestroyBlock();
        }
    }

    void PlayHitAnimation()
    {
        transform.DOKill(true);

        Sequence seq = DOTween.Sequence();
        seq.SetTarget(transform);

        seq.Append(transform.DOScale(0.88f, 0.05f));
        seq.Append(transform.DOScale(1f, 0.12f).SetEase(Ease.OutBack));

        transform.DOShakePosition(0.15f, 8f, 25, 70);
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
        transform.DOKill(true); // ← ВАЖНО: true = complete

        Sequence seq = DOTween.Sequence();
        seq.SetTarget(transform); // ← связываем sequence с объектом

        seq.Append(transform.DOScale(1.2f, 0.12f));
        seq.Append(transform.DOScale(0f, 0.2f));

        seq.OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

}
