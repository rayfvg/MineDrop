using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour
{
    [Header("Pickaxes")]
    public SymbolConfig[] pickaxes;

    [Header("Columns")]
    public SlotColumn[] columns = new SlotColumn[5];

    [Header("Roll Settings")]
    public float columnDelay = 0.3f;
    public float rollTime = 1.5f;

    bool isRolling;
    int freeSpins;

    void Start()
    {
        for (int x = 0; x < columns.Length; x++)
        {
            var column = columns[x];

            for (int y = 0; y < column.slots.Length; y++)
            {
                Slot slot = column.slots[y];

                // 1️⃣ инициализация слота
                slot.Init(pickaxes);

                // 2️⃣ задаём позицию в сетке
                Vector2Int gridPos = new Vector2Int(x, y);
                slot.GridPosition = gridPos;

                // 3️⃣ регистрируем слот
                SlotGridManager.Instance.RegisterSlot(slot, gridPos);
            }
        }
    }



    void Update()
    {
        if (
     Input.GetKeyDown(KeyCode.Space) &&
     GameStateManager.Instance.CanStartSpin() &&
     !isRolling &&
     freeSpins == 0
 )
        {
            StartCoroutine(RollByColumns());
        }

    }

    public void AddFreeSpins(int count)
    {
        freeSpins += count;

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

        // 👁 автофриспины
        if (freeSpins > 0)
        {
            freeSpins--;
            yield return new WaitForSeconds(0.25f);
            StartCoroutine(RollByColumns());
        }
    }

}
