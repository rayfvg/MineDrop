using UnityEngine;
using System.Collections.Generic;

public class BuffSelectionUI : MonoBehaviour
{
    public static BuffSelectionUI Instance;

    public BuffButton[] buttons;

    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        List<BuffData> buffs =
            BuffManager.Instance.GetRandomBuffs(3);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].Setup(buffs[i]);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
