using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Pickaxes")]
    public SymbolConfig[] pickaxes;

    [Header("Columns")]
    public SlotColumn[] columns = new SlotColumn[5];

    [Header("Roll Settings")]
    public float columnDelay = 0.3f;
    public float rollTime = 1.5f;

    bool isRolling;
    int freeSpins;

    public int FreeSpins => freeSpins; // 👈 ВОТ ОН

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int x = 0; x < columns.Length; x++)
        {
            var column = columns[x];

            for (int y = 0; y < column.slots.Length; y++)
            {
                Slot slot = column.slots[y];

                slot.Init(pickaxes);
                Vector2Int gridPos = new Vector2Int(x, y);
                slot.GridPosition = gridPos;

                SlotGridManager.Instance.RegisterSlot(slot, gridPos);
            }
        }
    }

    public Dictionary<string, float> GetCurrentChances()
    {
        Dictionary<string, float> result = new();

        float total = 0f;

        foreach (var s in pickaxes)
        {
            float w = SlotWeightCalculator.GetWeight(s);
            result[s.id] = w;
            total += w;
        }

        // переводим в проценты
        var keys = new List<string>(result.Keys);
        foreach (var k in keys)
        {
            result[k] = result[k] / total * 100f;
        }

        return result;
    }

    public void ResetFreeSpins()
    {
        freeSpins = 0;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && RunManager.Instance.CanSpin())
        {
            RunManager.Instance.MarkSpinUsed();
            StartCoroutine(RollByColumns());
        }
    }

    public void StartGameButton()
    {
        if (RunManager.Instance.CanSpin() == false)
            return;

        RunManager.Instance.MarkSpinUsed();
        StartCoroutine(RollByColumns());
    }

    public void AddFreeSpin()
    {
        // ❗ если фриспин уже есть — игнорируем
        if (freeSpins > 0)
            return;

        freeSpins = 1;

        if (!isRolling)
            StartCoroutine(RollByColumns());
    }

    IEnumerator RollByColumns()
    {
        isRolling = true;

        SlotResultManager.Instance.StartCollect(5 * 3);

        foreach (var column in columns)
        {
            foreach (var slot in column.slots)
                slot.StartRoll(rollTime);

            yield return new WaitForSeconds(columnDelay);
        }

        yield return new WaitUntil(() =>
            SlotResultManager.Instance.IsRunFinished
        );

        isRolling = false;

        if (freeSpins > 0)
        {
            freeSpins--;

            // 👇 один фриспин закончился
            GameSpeedManager.Instance.OnFreeSpinEnded();

            yield return new WaitForSeconds(0.25f);
            StartCoroutine(RollByColumns());
        }
       
    }
}