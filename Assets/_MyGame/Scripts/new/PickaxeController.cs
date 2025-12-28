using UnityEngine;

public class PickaxeController : MonoBehaviour
{
    public SymbolConfig config;

    int damage;
    int hitsLeft;
    bool hasHitThisFall = false;

    public void Init(SymbolConfig symbol, int amount)
    {
        config = symbol;

        damage = symbol.damage;   // <-- добавим в SymbolConfig
        hitsLeft = amount;

        GetComponent<UnityEngine.UI.Image>().sprite = symbol.sprite;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasHitThisFall) return;

        if (!collision.collider.CompareTag("Block"))
            return;

        Block block = collision.collider.GetComponent<Block>();

        if (block == null) return;

        hasHitThisFall = true;

        block.TakeHit(damage);
        hitsLeft--;

        if (hitsLeft <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            // позже здесь будет подпрыгивание
            ResetHit();
        }
    }

    void ResetHit()
    {
        hasHitThisFall = false;
    }
}
