using System.Collections.Generic;
using UnityEngine;

public class BlockRow : MonoBehaviour
{
    public List<RectTransform> spawnPoints;
    public List<BlockChance> chances;

    public void FillRow()
    {
        ClearRow();

        foreach (RectTransform point in spawnPoints)
        {
            BlockType selected = GetRandomBlock();

            GameObject blockObj = Instantiate(
                selected.prefab,
                point
            );

            RectTransform blockRect = blockObj.GetComponent<RectTransform>();
            blockRect.anchorMin = Vector2.zero;
            blockRect.anchorMax = Vector2.one;
            blockRect.offsetMin = Vector2.zero;
            blockRect.offsetMax = Vector2.zero;

            Block block = blockObj.GetComponent<Block>();
            block.Init(selected.hp, selected.reward);
        }
    }

    void ClearRow()
    {
        foreach (RectTransform point in spawnPoints)
        {
            for (int i = point.childCount - 1; i >= 0; i--)
            {
                Destroy(point.GetChild(i).gameObject);
            }
        }
    }

    BlockType GetRandomBlock()
    {
        float total = 0f;
        foreach (var c in chances)
            total += c.chance;

        float rand = Random.Range(0f, total);
        float current = 0f;

        foreach (var c in chances)
        {
            current += c.chance;
            if (rand <= current)
                return c.blockType;
        }

        return chances[0].blockType;
    }
}
