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

    void Start()
    {
        foreach (var column in columns)
        {
            foreach (var slot in column.slots)
            {
                slot.Init(pickaxes);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(RollByColumns());
        }
    }

    IEnumerator RollByColumns()
    {
        SlotResultManager.Instance.StartCollect(5 * 3);

        foreach (var column in columns)
        {
            foreach (var slot in column.slots)
            {
                slot.StartRoll(rollTime);
            }

            yield return new WaitForSeconds(columnDelay);
        }
    }

}
