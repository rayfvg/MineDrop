using UnityEngine;

[System.Serializable]
public class SymbolConfig
{
    public string id;          // "Pickaxe1", "Empty", "Book", "Eye", "Dynamite"
    public Sprite sprite;

    public int damage;   // сила удара кирки


    public bool isEmpty;
    public bool hasAmount;     // кирки = true, остальное = false

    [Range(0f, 100f)]
    public float weight;       // шанс выпадения

}
