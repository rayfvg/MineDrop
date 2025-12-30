using System.Collections.Generic;
using UnityEngine;

public class SlotGridManager : MonoBehaviour
{
    public static SlotGridManager Instance;


    private Dictionary<Vector2Int, Slot> slots = new();

    void Awake()
    {
        Instance = this;
    }


    public void RegisterSlot(Slot slot, Vector2Int position)
    {
        slot.GridPosition = position;
        slots[position] = slot;
    }

    public bool TryGetSlot(Vector2Int position, out Slot slot)
    {
        return slots.TryGetValue(position, out slot);
    }
}
