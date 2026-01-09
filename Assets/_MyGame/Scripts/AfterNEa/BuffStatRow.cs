using TMPro;
using UnityEngine;

public class BuffStatRow : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text valueText;

    public string buffId;

    public void Refresh()
    {
        float value = BuffStatsProvider.GetValue(buffId);
        valueText.text = value.ToString("0.##");
        valueText.color = BuffColorHelper.GetColor(value);
    }
}