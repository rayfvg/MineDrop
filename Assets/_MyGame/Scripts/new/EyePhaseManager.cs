using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EyePhaseManager : MonoBehaviour
{
    public static EyePhaseManager Instance;

    public CanvasGroup canvasGroup;
    public RectTransform labelTransform;
    public TMP_Text labelText;

    [TextArea]
    public string baseText = "Фри\nспинн!!!";

    void Awake()
    {
        Instance = this;
        Debug.Log("👁 EyePhaseManager READY");

        canvasGroup.alpha = 0;
        labelTransform.localScale = Vector3.zero;
    }

    public IEnumerator Play(int eyeCount)
    {
        Time.timeScale = 0f;

        labelText.text = $"+{eyeCount}\n{baseText}";

        canvasGroup.alpha = 1;
        labelTransform.localScale = Vector3.zero;

        labelTransform
            .DOScale(1f, 0.4f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);

        yield return new WaitForSecondsRealtime(1.2f);

        labelTransform
            .DOScale(0f, 0.25f)
            .SetEase(Ease.InBack)
            .SetUpdate(true);

        yield return new WaitForSecondsRealtime(0.3f);

        canvasGroup.alpha = 0;

        Time.timeScale = 1f;
    }
}
