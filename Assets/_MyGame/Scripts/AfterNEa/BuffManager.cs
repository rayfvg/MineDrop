using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance;

    public List<BuffData> allBuffs;

    void Awake()
    {
        Instance = this;
    }

    public List<BuffData> GetRandomBuffs(int count)
    {
        List<BuffData> copy = new(allBuffs);
        List<BuffData> result = new();

        for (int i = 0; i < count && copy.Count > 0; i++)
        {
            int index = Random.Range(0, copy.Count);
            result.Add(copy[index]);
            copy.RemoveAt(index);
        }

        return result;

    }
}
