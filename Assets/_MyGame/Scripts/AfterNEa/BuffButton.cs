using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffButton : MonoBehaviour
{
    public TMP_Text titleText;
    public GameObject fxRoot;
    public Button button;

    BuffData buff;

    public void Setup(BuffData data)
    {
        buff = data;

        titleText.text = data.title;

        // FX
        foreach (Transform c in fxRoot.transform)
            c.gameObject.SetActive(false);

        Transform fx = fxRoot.transform.Find(data.particleId);
        if (fx != null)
            fx.gameObject.SetActive(true);
        else
            Debug.LogWarning($"❌ FX {data.particleId} не найден");

        // 🔥 ВАЖНО
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Apply);
    }

    void Apply()
    {
        if (buff == null)
        {
            Debug.LogError("❌ BuffButton.Apply вызван, но buff == null");
            return;
        }

        GameModifiers.Instance.AddModifier(
            buff.symbolId,
            buff.weightDelta
        );

        BuffSelectionUI.Instance.Hide();
        GameStateManager.Instance.GoToMenu();

        ChanceTableUI.Instance.Refresh();
    }
}
