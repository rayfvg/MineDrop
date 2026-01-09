using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int BaseScore { get; private set; }
    public List<int> pendingMultipliers = new();

    [Header("UI")]
    public TMP_Text scoreText;

    void Awake()
    {
        Instance = this;
        UpdateUI();
    }

    public void AddScore(int value)
    {
        BaseScore += value;
        UpdateUI();
    }

    public void ApplyMultiplier(int multiplier)
    {
        BaseScore *= multiplier;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = BaseScore.ToString();
    }

    public void ResetScore()
    {
        BaseScore = 0;
        pendingMultipliers.Clear();
        UpdateUI();
    }
}
