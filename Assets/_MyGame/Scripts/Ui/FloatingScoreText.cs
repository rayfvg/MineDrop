using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingScoreText : MonoBehaviour
{
    public TMP_Text text;

    public void Play(int amount, Vector3 worldPos)
    {
        text.text = "+" + amount;
        PlayCommon(worldPos);
    }

    public void PlayMultiplier(int multiplier, Vector3 worldPos)
    {
        text.text = "x" + multiplier;
        text.color = Color.yellow;
        PlayCommon(worldPos);
    }

    void PlayCommon(Vector3 worldPos)
    {
        RectTransform rect = transform as RectTransform;

        Canvas canvas = GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.transform as RectTransform;

        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out localPos
        );
        localPos += Random.insideUnitCircle * 12f;
        rect.anchoredPosition = localPos;

        Sequence seq = DOTween.Sequence();
        seq.Append(rect.DOAnchorPosY(rect.anchoredPosition.y + 80f, 0.6f));
        seq.Join(text.DOFade(0f, 0.6f));
        seq.OnComplete(() => Destroy(gameObject));
    }
}
