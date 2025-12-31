using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChestMultiplierChance
{
    public int multiplier;
    public float chance;
}

public class ChestMultiplier : MonoBehaviour
{
    public GameObject closedVisual;
    public GameObject openedVisual;

    public List<ChestMultiplierChance> chances;

    bool activated;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        PickaxeController pickaxe = other.GetComponent<PickaxeController>();
        if (pickaxe == null) return;

        activated = true;

        int multiplier = RollMultiplier();

        OpenChest(multiplier);

        // ✅ корректно завершаем кирку
        pickaxe.ForceFinish();
    }

    int RollMultiplier()
    {
        float total = 0;
        foreach (var c in chances)
            total += c.chance;

        float roll = Random.Range(0, total);
        float current = 0;

        foreach (var c in chances)
        {
            current += c.chance;
            if (roll <= current)
                return c.multiplier;
        }

        return 2;
    }

    void OpenChest(int multiplier)
    {
        closedVisual.SetActive(false);
        openedVisual.SetActive(true);

        // ✅ множитель УМНОЖАЕТСЯ, а не затирается
        ScoreManager.Instance.AddMultiplier(multiplier);

        FloatingTextSpawner.Instance.SpawnMultiplier(
            multiplier,
            transform.position
        );
    }
}
