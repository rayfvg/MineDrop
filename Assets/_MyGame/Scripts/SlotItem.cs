using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotItem : MonoBehaviour
{
    public Image image;
    public TMP_Text amountText;

    public RectTransform Rect => transform as RectTransform;

    public SymbolConfig CurrentSymbol { get; private set; }
    public int Amount { get; private set; }

    public void SetRandom(SymbolConfig[] symbols)
    {
        CurrentSymbol = WeightedRandom.GetRandom(symbols);

        if (CurrentSymbol.isEmpty)
        {
            image.enabled = false;
            amountText.text = "";
            return;
        }

        image.enabled = true;
        image.sprite = CurrentSymbol.sprite;

        if (CurrentSymbol.hasAmount)
        {
            Amount = Random.Range(1, 6);
            amountText.text = Amount.ToString();
        }
        else
        {
            Amount = 0;
            amountText.text = "";
        }
    }

}
