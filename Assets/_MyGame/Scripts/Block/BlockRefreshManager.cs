using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class BlockRefreshManager : MonoBehaviour
{
    public static BlockRefreshManager Instance;

    [Header("Bonus")]
    public int refreshBonus = 500;
    public TMP_Text centerText;

    private readonly List<Block> activeBlocks = new();

    void Awake()
    {
        Instance = this;
    }

    // ➕ вызывается при создании блока
    public void Register(Block block)
    {
        if (!activeBlocks.Contains(block))
            activeBlocks.Add(block);
    }

    // ➖ вызывается при уничтожении блока
    public void Unregister(Block block)
    {
        activeBlocks.Remove(block);

        if (activeBlocks.Count == 0)
        {
            RefreshField();
        }
    }

    public void RefreshField()
    {
        BlockContainer container =
            FindObjectOfType<BlockContainer>();

        container.FillAll();

        // 🔥 ВОТ ЭТОГО НЕ ХВАТАЛО
        ChestManager.Instance.ResetAll();

        GiveBonus();
    }


    public void RefreshFieldFirst()
    {
        BlockContainer container =
            FindObjectOfType<BlockContainer>();

        container.FillAll();
    }

    void GiveBonus()
    {
        ScoreManager.Instance.AddScore(refreshBonus);

        centerText.text = $"Ресурсы обновились\n+{refreshBonus}";
        centerText.alpha = 1;
        centerText.transform.localScale = Vector3.zero;

        Sequence seq = DOTween.Sequence();
        seq.Append(centerText.transform
            .DOScale(1f, 0.3f)
            .SetEase(Ease.OutBack));
        seq.AppendInterval(1f);
        seq.Append(centerText.DOFade(0f, 0.4f));
    }
}
