using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChanceRow : MonoBehaviour
{
    public string symbolId;
    public TMP_Text valueText;

    public void Refresh(Dictionary<string, float> chances)
    {
        if (chances.TryGetValue(symbolId, out float value))
            valueText.text = value.ToString("0.0") + "%";
    }
}
