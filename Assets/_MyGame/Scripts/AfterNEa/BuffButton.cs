using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffButton : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public Button button;

    BuffData buff;

    public void Setup(BuffData data)
    {
        buff = data;
        titleText.text = data.title;
        descriptionText.text = data.description;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Apply);
    }

    void Apply()
    {
        BuffApplier.Instance.Apply(buff);

        BuffSelectionUI.Instance.Hide();
        GameStateManager.Instance.GoToMenu();
    }
}
