using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [Header("Pickaxes")]
    public PickaxeConfig[] pickaxes;

    [Header("Slots (по колонкам)")]
    public List<Slot[]> columns = new List<Slot[]>();


    [Header("Roll Settings")]
    public float columnDelay = 0.3f;
    public float rollTime = 1.5f;

    void Start()
    {
        foreach (var column in columns)
        {
            foreach (var slot in column)
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
        foreach (var column in columns)
        {
            foreach (var slot in column)
            {
                slot.StartRoll(rollTime);
            }

            yield return new WaitForSeconds(columnDelay);
        }
    }
}
