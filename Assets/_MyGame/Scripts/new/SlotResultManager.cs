using System.Collections.Generic;
using UnityEngine;

public class SlotResultManager : MonoBehaviour
{
    public static SlotResultManager Instance;

    public List<SlotResult> results = new();

    private int expectedResults;
    private int receivedResults;


    void Awake()
    {
        Instance = this;
    }

    public void StartCollect(int totalSlots)
    {
        results.Clear();
        expectedResults = totalSlots;
        receivedResults = 0;
    }

    public void AddResult(SymbolConfig symbol, int amount, Slot sourceSlot)
    {
        results.Add(new SlotResult
        {
            symbol = symbol,
            amount = amount,
            sourceSlot = sourceSlot
        });

        receivedResults++;

        if (receivedResults >= expectedResults)
        {
            OnAllResultsCollected();
        }
    }



    void OnAllResultsCollected()
    {
        Debug.Log("ВСЕ СЛОТЫ ОСТАНОВИЛИСЬ");

        foreach (var r in results)
        {
            Debug.Log($"{r.symbol.id} x {r.amount}");
        }


        var groups = GetPickaxeGroupsSorted();

        Debug.Log("КИРКИ ПО ВОЗРАСТАНИЮ:");

        foreach (var g in groups)
        {
            Debug.Log($"{g.pickaxe.id} x{g.count} | dmg {g.Damage}");
        }


        ApplyBookUpgrade(groups);

        Debug.Log("КИРКИ ПОСЛЕ КНИГИ:");

        foreach (var g in groups)
        {
            Debug.Log($"{g.pickaxe.id} x{g.count} | dmg {g.Damage}");
        }

      //  PickaxeSpawner.Instance.SpawnGroups(groups);

        foreach (var r in results)
        {
            if (!r.IsPickaxe)
                continue;

            SpawnPickaxeFromSlot(r);
        }
        // ❗ позже отсюда пойдёт логика кирок
    }

    void SpawnPickaxeFromSlot(SlotResult result)
    {
        RectTransform slotPoint = result.sourceSlot.GetSpawnPoint();

        GameObject obj = Instantiate(
            PickaxeSpawner.Instance.pickaxePrefab,
            PickaxeSpawner.Instance.spawnParent
        );

        RectTransform rect = obj.GetComponent<RectTransform>();

        result.sourceSlot.ClearVisual();


        // позиция слота → локальная позиция в spawnParent
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            PickaxeSpawner.Instance.spawnParent,
            RectTransformUtility.WorldToScreenPoint(null, slotPoint.position),
            null,
            out localPos
        );

        rect.anchoredPosition = localPos;
        rect.sizeDelta = new Vector2(120, 120); // хардкод — ок

        obj.GetComponent<PickaxeController>()
    .Init(result.symbol, result.amount);
    }



    public List<PickaxeGroup> GetPickaxeGroupsSorted()
    {
        Dictionary<SymbolConfig, int> map = new();

        foreach (var r in results)
        {
            if (!r.IsPickaxe)
                continue;

            if (!map.ContainsKey(r.symbol))
                map[r.symbol] = 0;

            map[r.symbol] += r.amount;
        }

        List<PickaxeGroup> groups = new();

        foreach (var kvp in map)
        {
            groups.Add(new PickaxeGroup
            {
                pickaxe = kvp.Key,
                count = kvp.Value
            });
        }

        groups.Sort((a, b) => a.Damage.CompareTo(b.Damage));

        return groups;
    }

    public bool HasBook()
    {
        foreach (var r in results)
        {
            if (r.IsBook)
                return true;
        }
        return false;
    }

    public void ApplyBookUpgrade(List<PickaxeGroup> groups)
    {
        if (!HasBook())
            return;

        Debug.Log("📘 КНИГА ВЫПАЛА — УЛУЧШАЕМ КИРКИ");

        // получаем все конфиги кирок, отсортированные по damage
        List<SymbolConfig> allPickaxes = new();

        foreach (var g in groups)
            allPickaxes.Add(g.pickaxe);

        allPickaxes.Sort((a, b) => a.damage.CompareTo(b.damage));

        for (int i = 0; i < groups.Count; i++)
        {
            SymbolConfig current = groups[i].pickaxe;

            SymbolConfig upgraded = GetNextPickaxe(current, allPickaxes);
            groups[i].pickaxe = upgraded;
        }
    }

    SymbolConfig GetNextPickaxe(SymbolConfig current, List<SymbolConfig> all)
    {
        int index = all.IndexOf(current);

        if (index < 0)
            return current;

        if (index >= all.Count - 1)
            return current;

        return all[index + 1];
    }
}
