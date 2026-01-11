using UnityEngine;

public class ChanceTableUI : MonoBehaviour
{
    public static ChanceTableUI Instance;
    public ChanceRow[] rows;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        TryRefresh(); // 🔥 ВАЖНО
    }

    void OnEnable()
    {
        TryRefresh();
    }

    public void TryRefresh()
    {
        if (GridManager.Instance == null)
        {
            Debug.Log("ChanceTableUI: ждём GridManager");
            return;
        }

        Refresh();
    }

    public void Refresh()
    {
        if (GridManager.Instance == null)
            return;

        var chances = GridManager.Instance.GetCurrentChances();

        foreach (var row in rows)
        {
            if (row != null)
                row.Refresh(chances);
        }
    }
}
