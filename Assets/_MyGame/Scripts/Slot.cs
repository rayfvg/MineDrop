using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class Slot : MonoBehaviour
{
    [Header("UI")]
    public RectTransform viewport;
    public RectTransform reel;
    public SlotItem itemPrefab;

    [Header("Reel Settings")]
    public int visibleItems = 3;
    public float itemHeight = 100f;
    public float startSpeed = 1500f;
    public float slowSpeed = 300f;

    private List<SlotItem> items = new();
    private SymbolConfig[] pickaxes;
    private float currentSpeed;
    private bool isRolling;

    public void Init(SymbolConfig[] configs)
    {
        pickaxes = configs;

        CreateItems();
    }

    void CreateItems()
    {
        for (int i = 0; i < visibleItems + 4; i++)
        {
            var item = Instantiate(itemPrefab, reel);
            item.SetRandom(pickaxes);
            item.Rect.anchoredPosition = new Vector2(0, -i * itemHeight);
            items.Add(item);
        }

        reel.sizeDelta = new Vector2(
            reel.sizeDelta.x,
            items.Count * itemHeight
        );
    }

    public void StartRoll(float duration)
    {
        currentSpeed = startSpeed;
        isRolling = true;

        DOTween.To(
            () => currentSpeed,
            x => currentSpeed = x,
            slowSpeed,
            duration
        ).SetEase(Ease.OutCubic)
         .OnComplete(StopRoll);
    }

    void Update()
    {
        if (!isRolling) return;

        reel.anchoredPosition += Vector2.down * currentSpeed * Time.deltaTime;

        foreach (var item in items)
        {
            if (item.Rect.anchoredPosition.y + reel.anchoredPosition.y < -itemHeight)
            {
                MoveItemToTop(item);
            }
        }
    }

    void MoveItemToTop(SlotItem item)
    {
        float topY = GetTopY() + itemHeight;
        item.Rect.anchoredPosition = new Vector2(0, topY);
        item.SetRandom(pickaxes);
    }

    float GetTopY()
    {
        float max = float.MinValue;
        foreach (var item in items)
            max = Mathf.Max(max, item.Rect.anchoredPosition.y);
        return max;
    }

    void StopRoll()
    {
        isRolling = false;

        float snapY = Mathf.Round(reel.anchoredPosition.y / itemHeight) * itemHeight;

        reel.DOAnchorPosY(snapY, 0.25f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                SlotItem result = GetResultItem();

                if (result != null && result.CurrentSymbol != null)
                {
                    Debug.Log(
                        $"[SLOT RESULT] {result.CurrentSymbol.id}"
                    );
                }
            });
    }


    SlotItem GetResultItem()
    {
        float centerY = viewport.position.y;
        SlotItem closest = null;
        float minDistance = float.MaxValue;

        foreach (var item in items)
        {
            float itemY = item.Rect.position.y;
            float distance = Mathf.Abs(itemY - centerY);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = item;
            }
        }

        return closest;
    }
}
