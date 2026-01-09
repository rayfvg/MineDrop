using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class Block : MonoBehaviour
{
    public int maxHP;
    public int currentHP;
    public int reward;

    [Header("Damage Visuals")]
    public Image[] damageStages; // сюда нужно перетащить все 5 Image

    bool isDestroyed;

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

        BlockRefreshManager.Instance.Register(this);
    }

    public void TakeHit(int damage)
    {
        if (isDestroyed) return;

        currentHP -= damage;

        if (currentHP > 0)
        {
            PlayHitAnimation();
            UpdateVisual();
        }
        else
        {
            isDestroyed = true;
            DestroyBlock();
        }
    }


    public void PlayHitAnimation()
    {
        transform.DOKill(true);

        Sequence seq = DOTween.Sequence();
        seq.Append(transform
            .DORotate(new Vector3(0, 0, -75f), 0.06f)
            .SetEase(Ease.InQuad));

        seq.Append(transform
            .DORotate(new Vector3(0, 0, 10f), 0.08f)
            .SetEase(Ease.OutQuad));

        seq.Append(transform
            .DORotate(Vector3.zero, 0.12f)
            .SetEase(Ease.OutBack));
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
        transform.DOKill(true);

        int finalReward = reward + GameModifiers.Instance.bonusScorePerBlock;
        ScoreManager.Instance.AddScore(finalReward);

        FloatingTextSpawner.Instance.Spawn(
            finalReward,
            transform.position
        );

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(1.2f, 0.12f));
        seq.Append(transform.DOScale(0f, 0.2f));
        seq.OnComplete(() =>
        {
            BlockRefreshManager.Instance.Unregister(this);
            Destroy(gameObject);
        });
    }

}
