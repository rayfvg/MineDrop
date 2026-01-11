using TMPro;
using UnityEngine;

public class BuffStatRow : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text valueText;
    public string symbolId;

    public void Refresh()
    {
        var chances = GridManager.Instance.GetCurrentChances();

        if (chances.TryGetValue(symbolId, out float value))
            valueText.text = value.ToString("0.0") + "%";
    }
}
