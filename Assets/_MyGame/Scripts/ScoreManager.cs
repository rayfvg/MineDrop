using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int Score { get; private set; }
    public int Multiplier { get; private set; } = 1;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text multiplierText;

    void Awake()
    {
        Instance = this;
        UpdateUI();
    }

    public void AddScore(int baseAmount)
    {
        int finalAmount =
    Mathf.RoundToInt(baseAmount *
    Multiplier *
    GameModifiers.Instance.scoreMultiplier);

        Score += finalAmount;
        UpdateUI();
    }

    public void AddMultiplier(int value)
    {
        Multiplier *= value;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = Score.ToString();

        if (multiplierText != null)
            multiplierText.text = "x" + Multiplier;
    }

    public void ResetScore()
    {
        Score = 0;
        Multiplier = 1;
        UpdateUI();
    }
}
