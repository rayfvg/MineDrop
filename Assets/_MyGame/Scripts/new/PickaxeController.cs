using UnityEngine;

public class PickaxeController : MonoBehaviour
{
    public SymbolConfig config;

    public void Init(SymbolConfig symbol)
    {
        config = symbol;
        GetComponent<UnityEngine.UI.Image>().sprite = symbol.sprite;
    }
}
