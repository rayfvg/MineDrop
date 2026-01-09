using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffButton : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public Button button;

    [Header("Particles")]
    public GameObject weakFX;
    public GameObject mediumFX;
    public GameObject strongFX;

    BuffData buff;

    public void Setup(BuffData data)
    {
        buff = data;

        titleText.text = data.title;
        descriptionText.text = data.description;

        weakFX.SetActive(false);
        mediumFX.SetActive(false);
        strongFX.SetActive(false);

        switch (data.power)
        {
            case BuffPower.Weak: weakFX.SetActive(true); break;
            case BuffPower.Medium: mediumFX.SetActive(true); break;
            case BuffPower.Strong: strongFX.SetActive(true); break;
        }

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
