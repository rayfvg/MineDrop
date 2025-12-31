using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : MonoBehaviour
{
    public SymbolConfig config;

    int damage;
    int hitsLeft;
    bool hasHitThisFall;

    bool finished;

    public System.Action<PickaxeController> OnFinished;

    public void Init(SymbolConfig symbol, int hits)
    {
        config = symbol;
        damage = symbol.damage;
        hitsLeft = hits;

        GetComponent<UnityEngine.UI.Image>().sprite = symbol.sprite;

        StartCoroutine(FailsafeTimeout());
    }

    IEnumerator FailsafeTimeout()
    {
        float timeout = 6f; // с запасом
        float timer = 0f;

        while (hitsLeft > 0 && timer < timeout)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        Finish();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasHitThisFall) return;
        if (!collision.collider.CompareTag("Block")) return;


        Block block = collision.collider.GetComponent<Block>();
        if (block == null || block.currentHP <= 0)
            return;

        hasHitThisFall = true;

        StartCoroutine(HandleHit(block));
    }

    void Finish()
    {
        if (finished) return;
        finished = true;

        OnFinished?.Invoke(this);
        Destroy(gameObject);
    }

    IEnumerator HandleHit(Block block)
    {
        if (block == null)
            yield break;

        // 🔥 НАНОСИМ УРОН СРАЗУ
        block.TakeHit(damage);
        hitsLeft--;

        PlayHitAnimation();

        if (hitsLeft <= 0)
        {
            Finish();
        }
        else
        {
            yield return StartCoroutine(Bounce());
        }
    }


    void OnDestroy()
    {
        if (transform != null)
            transform.DOKill();
    }


    IEnumerator Bounce()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * 6f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.1f);

        hasHitThisFall = false;
    }

    public void PlayHitAnimation()
    {
        transform.DOKill(true);

        Sequence seq = DOTween.Sequence();
        seq.SetTarget(transform);

        seq.Append(transform
            .DORotate(new Vector3(0, 0, -75f), 0.06f)
            .SetEase(Ease.InQuad));

        seq.Append(transform
            .DORotate(new Vector3(0, 0, 10f), 0.08f)
            .SetEase(Ease.OutQuad));

        seq.Append(transform
            .DORotate(Vector3.zero, 0.12f)
            .SetEase(Ease.OutBack));
    }

}
