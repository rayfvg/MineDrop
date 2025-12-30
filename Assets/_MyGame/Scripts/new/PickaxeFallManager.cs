using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeFallManager : MonoBehaviour
{
    public static PickaxeFallManager Instance;

    public RectTransform spawnParent;
    public GameObject pickaxePrefab;

    public bool IsFinished { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public IEnumerator StartFall(List<SlotResult> results)
    {
        IsFinished = false;
        yield return StartCoroutine(FallRoutine(results));
        IsFinished = true;
    }

    IEnumerator FallRoutine(List<SlotResult> results)
    {
        // 1️⃣ оставляем только кирки
        List<SlotResult> pickaxes = new();
        foreach (var r in results)
            if (r.IsPickaxe)
                pickaxes.Add(r);

        // 2️⃣ сортируем по tier
        pickaxes.Sort((a, b) =>
            a.symbol.tier.CompareTo(b.symbol.tier)
        );

        // 3️⃣ идём по tier-группам
        int index = 0;

        while (index < pickaxes.Count)
        {
            int currentTier = pickaxes[index].symbol.tier;

            List<SlotResult> sameTier = new();

            while (index < pickaxes.Count &&
                   pickaxes[index].symbol.tier == currentTier)
            {
                sameTier.Add(pickaxes[index]);
                index++;
            }

            // ⬇ ПАДАЕТ ЦЕЛАЯ ГРУППА
            yield return StartCoroutine(FallTierGroup(sameTier));
        }
        Debug.Log("✅ ВСЕ КИРКИ ОТРАБОТАЛИ");
    }

    IEnumerator FallTierGroup(List<SlotResult> group)
    {
        List<PickaxeController> active = new();

        foreach (var result in group)
        {
            // ❗ убираем иконку из слота
            result.sourceSlot.ClearVisual();

            RectTransform slotRect = result.sourceSlot.GetSpawnPoint();

            PickaxeController pc = SpawnPickaxe(
                result.symbol,
                slotRect,
                result.amount
            );

            active.Add(pc);
            pc.OnFinished += p => active.Remove(p);

        }

        // ждём, пока вся группа исчезнет
        float timeout = Time.time + 4f;
        yield return new WaitUntil(() =>
            active.Count == 0 || Time.time > timeout
        );
    }


    IEnumerator FallFromSlot(SlotResult result)
    {
        // 🔥 УДАЛЯЕМ ВИЗУАЛ ИЗ СЛОТА
        result.sourceSlot.ClearVisual();

        RectTransform slotRect = result.sourceSlot.GetSpawnPoint();

        PickaxeController pickaxe = SpawnPickaxe(
            result.symbol,
            slotRect,
            result.amount
        );

        bool finished = false;
        pickaxe.OnFinished += _ => finished = true;

        yield return new WaitUntil(() => finished);
    }

    PickaxeController SpawnPickaxe(
    SymbolConfig symbol,
    RectTransform slotRect,
    int hits
)
    {
        GameObject obj = Instantiate(pickaxePrefab, spawnParent);
        RectTransform rect = obj.GetComponent<RectTransform>();

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            spawnParent,
            RectTransformUtility.WorldToScreenPoint(null, slotRect.position),
            null,
            out localPos
        );

        rect.anchoredPosition = localPos;

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);

        rect.sizeDelta = new Vector2(120, 120);

        PickaxeController pc = obj.GetComponent<PickaxeController>();
        pc.Init(symbol, hits);

        return pc;
    }



}
