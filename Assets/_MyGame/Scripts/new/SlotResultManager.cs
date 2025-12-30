using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotResultManager : MonoBehaviour
{
    public static SlotResultManager Instance;

    public List<SlotResult> results = new();

    private int expectedResults;
    private int receivedResults;
    bool pipelineStarted;
    RunPhase currentPhase;

    public GridManager gridManager;


    public bool IsRunFinished { get; private set; }
    void Awake()
    {
        Instance = this;
    }

    List<SymbolConfig> GetAllPickaxeConfigs()
    {
        if (gridManager == null)
        {
            Debug.LogError("❌ GridManager НЕ назначен в SlotResultManager");
            return new List<SymbolConfig>();
        }

        List<SymbolConfig> list = new(gridManager.pickaxes);
        list.Sort((a, b) => a.damage.CompareTo(b.damage));
        return list;
    }


    public void StartCollect(int totalSlots)
    {
        pipelineStarted = false;
        results.Clear();
        expectedResults = totalSlots;
        receivedResults = 0;

        IsRunFinished = false; // 👈 важно
    }

    public void AddResult(SymbolConfig symbol, int amount, Slot sourceSlot)
    {
        if (pipelineStarted)
            return;

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
        if (pipelineStarted)
            return;

        pipelineStarted = true;

        Debug.Log("🎰 ВСЕ СЛОТЫ ОСТАНОВИЛИСЬ");
        StartCoroutine(RunPipeline());
    }

    IEnumerator RunPipeline()
    {
        // 📘 ЭТАП КНИГИ
        currentPhase = RunPhase.Book;

        if (HasBook())
        {
            Debug.Log("📘 ЭТАП КНИГИ: улучшение кирок");
            yield return StartCoroutine(BookPhase());
        }
        else
        {
            Debug.Log("📘 ЭТАП КНИГИ: пропущен");
        }

        // ⛏ ЭТАП КИРОК
        currentPhase = RunPhase.PickaxeFall;
        Debug.Log("⛏ ЭТАП КИРОК: падение");

        yield return StartCoroutine(
     PickaxeFallManager.Instance.StartFall(results)
 );

        // 💣 ДИНАМИТ (позже)
        if (HasDynamite())
            Debug.Log("💣 ЭТАП ДИНАМИТА");

        // 👁 ГЛАЗ (позже)
        if (HasEye())
            Debug.Log("👁 ЭТАП ГЛАЗА");

        currentPhase = RunPhase.End;
        Debug.Log("🏁 РАН ЗАВЕРШЁН");

        IsRunFinished = true; // 👈 вот он
    }

    IEnumerator BookPhase()
    {
        List<(Slot slot, SymbolConfig upgraded)> upgrades = new();

        // 1️⃣ СОБИРАЕМ ВСЕ АПГРЕЙДЫ
        foreach (var r in results)
        {
            if (!r.IsBook)
                continue;


            Slot bookSlot = r.sourceSlot;
            List<Slot> targets = bookSlot.GetCrossNeighbours();

            r.sourceSlot.PlayBookVisual();

            foreach (var slot in targets)
            {
                SlotResult targetResult =
                    results.Find(x => x.sourceSlot == slot);

                if (targetResult == null || !targetResult.IsPickaxe)
                    continue;

                SymbolConfig upgraded = GetNextPickaxe(
                    targetResult.symbol,
                    GetAllPickaxeConfigs()
                );

                if (upgraded == targetResult.symbol)
                    continue;

                // 🔥 обновляем ДАННЫЕ СРАЗУ
                targetResult.symbol = upgraded;
                slot.currentSymbol = upgraded;


               
                upgrades.Add((slot, upgraded));
            }
        }

        if (upgrades.Count == 0)
            yield break;

        // ⏸ ПАУЗА — игрок понял, что книга сработала
        yield return new WaitForSeconds(0.25f);

        // 2️⃣ ВИЗУАЛЬНЫЙ ПОКАЗ (ВСЕ ОДНОВРЕМЕННО)
        foreach (var u in upgrades)
        {
            StartCoroutine(u.slot.PlayUpgradeVisual(u.upgraded));
        }

        // ⏸ ждём, пока игрок УВИДИТ апгрейд
        yield return new WaitForSeconds(0.5f);
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

    public bool HasDynamite()
    {
        foreach (var r in results)
        {
            if (r.symbol.id == "Dynamite")
                return true;
        }
        return false;
    }

    public bool HasEye()
    {
        foreach (var r in results)
        {
            if (r.symbol.id == "Eye")
                return true;
        }
        return false;
    }
}
