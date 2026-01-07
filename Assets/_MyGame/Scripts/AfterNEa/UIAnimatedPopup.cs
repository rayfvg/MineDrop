using DG.Tweening;
using UnityEngine;

public class UIAnimatedPopup : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeTime = 0.25f;
    public float scaleTime = 0.3f;

    void Awake()
    {
        canvasGroup.alpha = 0;
        transform.localScale = Vector3.one * 0.8f;
    }

    public void Show()
    {
        gameObject.SetActive(true);

        canvasGroup.DOFade(1, fadeTime);
        transform.DOScale(1f, scaleTime).SetEase(Ease.OutBack);
    }

    public void Hide()
    {
        canvasGroup.DOFade(0, fadeTime);
        transform.DOScale(0.8f, scaleTime)
            .OnComplete(() => gameObject.SetActive(false));
    }
}
