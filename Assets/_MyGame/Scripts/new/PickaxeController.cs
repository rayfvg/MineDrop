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
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasHitThisFall) return;
        if (!collision.collider.CompareTag("Block")) return;

        Block block = collision.collider.GetComponent<Block>();
        if (block == null) return;

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
        PlayHitAnimation();

        yield return new WaitForSeconds(0.06f);

        if (block == null)
        {
            Finish();
            yield break;
        }

        block.TakeHit(damage);
        hitsLeft--;

        if (hitsLeft <= 0)
        {
            Finish();
        }
        else
        {
            StartCoroutine(Bounce());
        }
    }



    void Start()
    {
        Invoke(nameof(ForceFinish), 3f);
    }

    void ForceFinish()
    {
        Finish();
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
