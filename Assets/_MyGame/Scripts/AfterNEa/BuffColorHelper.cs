using UnityEngine;

public static class BuffColorHelper
{
    public static Color GetColor(float value)
    {
        float t = Mathf.Clamp01(value / 10f);
        return Color.Lerp(Color.white, Color.red, t);
    }
}