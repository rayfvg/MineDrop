using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [Header("View")]
    public Image pickaxeRenderer;
    public TMP_Text amountText;

    [Header("Settings")]
    public float fastSpeed = 0.05f;
    public float slowSpeed = 0.15f;

    private PickaxeConfig[] pickaxes;
    private Tween rollTween;

    public void Init(PickaxeConfig[] configs)
    {
        pickaxes = configs;
    }

    public void StartRoll(float rollDuration)
    {
        rollTween?.Kill();

        rollTween = DOVirtual.DelayedCall(fastSpeed, ChangeSprite)
            .SetLoops(-1)
            .SetEase(Ease.Linear);

        DOVirtual.DelayedCall(rollDuration, StopRoll);
    }

    void ChangeSprite()
    {
        var p = pickaxes[Random.Range(0, pickaxes.Length)];
        pickaxeRenderer.sprite = p.sprite;
        amountText.text = Random.Range(1, 6).ToString();
    }

    void StopRoll()
    {
        rollTween.Kill();

        // финальный результат
        var finalPickaxe = pickaxes[Random.Range(0, pickaxes.Length)];
        int amount = Random.Range(1, 6);

        pickaxeRenderer.sprite = finalPickaxe.sprite;
        amountText.text = amount.ToString();

        // небольшой punch эффект
        transform.DOPunchScale(Vector3.one * 0.15f, 0.2f);
    }
}
