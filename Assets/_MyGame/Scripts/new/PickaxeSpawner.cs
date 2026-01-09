using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeSpawner : MonoBehaviour
{
    public static PickaxeSpawner Instance;

    public GameObject pickaxePrefab;
    public RectTransform spawnParent;
    public float spawnDelay = 0.1f;
    public float groupDelay = 0.5f;

    void Awake()
    {
        Instance = this;
    }

    public void SpawnGroups(List<PickaxeGroup> groups)
    {
        StartCoroutine(SpawnRoutine(groups));
    }

    IEnumerator SpawnRoutine(List<PickaxeGroup> groups)
    {
        foreach (var group in groups)
        {
            SpawnPickaxe(group.pickaxe, group.count);
            yield return new WaitForSeconds(spawnDelay);

            yield return new WaitForSeconds(groupDelay);
        }
    }

    void SpawnPickaxe(SymbolConfig symbol, int hits)
    {
        GameObject obj = Instantiate(pickaxePrefab, spawnParent);
        obj.transform.localPosition = Vector3.zero;

        obj.GetComponent<PickaxeController>()
            .Init(symbol, hits);
    }
}
