using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteFallManager : MonoBehaviour
{
    public static DynamiteFallManager Instance;

    public RectTransform spawnParent;
    public GameObject dynamitePrefab;

    public bool IsFinished { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public IEnumerator StartFall(List<SlotResult> results)
    {
        IsFinished = false;

        List<SlotResult> dynamites = new();
        foreach (var r in results)
            if (r.symbol.id == "Dynamite")
                dynamites.Add(r);

        List<DynamiteController> active = new();

        foreach (var result in dynamites)
        {
            result.sourceSlot.ClearVisual();

            RectTransform slotRect = result.sourceSlot.GetSpawnPoint();
            DynamiteController dc = SpawnDynamite(slotRect);

            active.Add(dc);
            dc.OnFinished += d => active.Remove(d);
        }

        yield return new WaitUntil(() => active.Count == 0);

        IsFinished = true;
    }

    DynamiteController SpawnDynamite(RectTransform slotRect)
    {
        GameObject obj = Instantiate(dynamitePrefab, spawnParent);
        RectTransform rect = obj.GetComponent<RectTransform>();

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            spawnParent,
            RectTransformUtility.WorldToScreenPoint(null, slotRect.position),
            null,
            out localPos
        );

        rect.anchoredPosition = localPos;
        rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(120, 120);

        return obj.GetComponent<DynamiteController>();
    }
}
