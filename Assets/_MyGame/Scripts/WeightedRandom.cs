using UnityEngine;

public static class WeightedRandom
{
    public static SymbolConfig GetRandom(SymbolConfig[] symbols)
    {
        float totalWeight = 0f;
        foreach (var s in symbols)
            totalWeight += s.weight;

        float random = Random.Range(0f, totalWeight);
        float current = 0f;

        foreach (var s in symbols)
        {
            current += s.weight;
            if (random <= current)
                return s;
        }

        return symbols[0]; // fallback
    }
}
