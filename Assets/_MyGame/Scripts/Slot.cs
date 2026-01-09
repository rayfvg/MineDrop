using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

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

    [Header("Result")]
    public SymbolConfig currentSymbol;
    public int currentAmount; // 👈 ВОТ ОНО
    public Image icon;
    public TMP_Text amountText; // 👈 отдельный текст

    public Vector2Int GridPosition;

    public void Init(SymbolConfig[] configs)
    {
        pickaxes = configs;

        CreateItems();
    }

    public bool HasPickaxe()
    {
        return currentSymbol != null && currentSymbol.hasAmount;
    }

    public IEnumerator PlayUpgradeVisual(SymbolConfig newSymbol)
    {
        if (this == null) yield break;
        if (icon == null) yield break;
        if (newSymbol == null) yield break;

        icon.transform.DOKill();

        // вспышка
        icon.transform
            .DOScale(1.35f, 0.12f)
            .SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.12f);

        // смена кирки
        currentSymbol = newSymbol;
        icon.sprite = newSymbol.sprite;

        icon.transform
            .DOScale(1f, 0.15f)
            .SetEase(Ease.OutBack);
    }



    public List<Slot> GetCrossNeighbours()
    {
        List<Slot> result = new();

        TryAddNeighbour(GridPosition + Vector2Int.up, result);
        TryAddNeighbour(GridPosition + Vector2Int.down, result);
        TryAddNeighbour(GridPosition + Vector2Int.left, result);
        TryAddNeighbour(GridPosition + Vector2Int.right, result);
        Debug.Log($"{name} neighbours: {result.Count}");
        return result;

       
    }

    public void PlayBookVisual()
    {
        if (icon == null)
        {
            Debug.LogError($"❌ Slot {name}: icon не найден (книга)");
            return;
        }

        icon.transform.DOPunchScale(Vector3.one * 0.35f, 0.4f).SetEase(Ease.OutBack);
    }

    void TryAddNeighbour(Vector2Int pos, List<Slot> list)
    {
        if (SlotGridManager.Instance.TryGetSlot(pos, out Slot slot))
        {
            list.Add(slot);
        }
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
               if (this == null) return;

               SlotItem result = GetResultItem();
               if (result == null || result.CurrentSymbol == null)
                   return;

               currentSymbol = result.CurrentSymbol;
               currentAmount = result.Amount;

               icon = result.image;
               icon.enabled = true;

               if (amountText != null)
               {
                   amountText.text = currentAmount > 1 ? currentAmount.ToString() : "";
               }

               // 🔥 ВОЗВРАЩАЕМ КЛЮЧЕВОЙ ВЫЗОВ
               SlotResultManager.Instance.AddResult(
                   currentSymbol,
                   currentAmount,
                   this
               );
           });
    }

    public void PlayEyeVisual()
    {
        if (icon == null) return;

        icon.transform.DOKill();

        Sequence seq = DOTween.Sequence();

        seq.Append(
            icon.transform
                .DOScale(1.35f, 0.2f)
                .SetEase(Ease.OutBack)
        );

        seq.Append(
            icon.transform
                .DORotate(new Vector3(0, 0, 15f), 0.1f)
        );

        seq.Append(
            icon.transform
                .DORotate(new Vector3(0, 0, -15f), 0.1f)
        );

        seq.Append(
            icon.transform
                .DORotate(Vector3.zero, 0.1f)
        );

        seq.Append(
            icon.transform
                .DOScale(1f, 0.2f)
                .SetEase(Ease.InOutBack)
        );
    }

    public void ClearVisual()
    {
        // очищаем ТОЛЬКО reel (прокрутку)
        foreach (Transform child in reel)
        {
            SlotItem item = child.GetComponent<SlotItem>();
            if (item == null) continue;

            item.image.enabled = false;
            item.amountText.text = "";
        }

        // ❗ icon НЕ ТРОГАЕМ
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

    public RectTransform GetSpawnPoint()
    {
        return viewport;
    }
}
