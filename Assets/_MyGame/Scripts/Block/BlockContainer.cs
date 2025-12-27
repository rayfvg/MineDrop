using System.Collections.Generic;
using UnityEngine;

public class BlockContainer : MonoBehaviour
{
    public List<BlockRow> rows;

    void Start()
    {
        FillAll();
    }

    public void FillAll()
    {
        foreach (BlockRow row in rows)
        {
            row.FillRow();
        }
    }
}
