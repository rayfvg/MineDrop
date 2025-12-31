using DG.Tweening;
using System.Collections;
using UnityEngine;

public class DynamiteController : MonoBehaviour
{
    public int damage = 5;

    bool hasLanded;
    bool finished;

    public System.Action<DynamiteController> OnFinished;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasLanded) return;
        if (!collision.collider.CompareTag("Block")) return;

        hasLanded = true;
        Block block = collision.collider.GetComponent<Block>();

        StartCoroutine(ExplosionSequence(block));
    }

    IEnumerator ExplosionSequence(Block block)
    {
        // 1️⃣ подпрыгивания
        for (int i = 0; i < 2; i++)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * 4f, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.15f);
        }

        // 2️⃣ "надувается"
        transform.DOScale(1.4f, 0.2f).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(0.2f);

        // 3️⃣ взрыв
        transform.DOScale(0f, 0.15f).SetEase(Ease.InBack);

        block.TakeHit(damage);

        yield return new WaitForSeconds(0.15f);

        Finish();
    }

    void Finish()
    {
        if (finished) return;
        finished = true;

        OnFinished?.Invoke(this);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (transform != null)
            transform.DOKill();
    }
}
