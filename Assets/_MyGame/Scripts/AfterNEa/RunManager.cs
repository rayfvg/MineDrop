using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance;

    bool spinUsed;

    void Awake()
    {
        Instance = this;
    }

    public void ResetRun()
    {
        spinUsed = false;
    }

    public bool CanSpin()
    {
        return !spinUsed && GameStateManager.Instance.State == GameState.Playing;
    }

    public void MarkSpinUsed()
    {
        spinUsed = true;
    }

    public void TryFinishRun()
    {
        // ❗ если есть фриспины — НЕЛЬЗЯ заканчивать ран
        if (GridManager.Instance.FreeSpins > 0)
            return;

        FinishRun();
    }

    void FinishRun()
    {
        int score = ScoreManager.Instance.Score;
        int debt = DebtManager.Instance.currentDebt;

        DebtManager.Instance.SaveRecord(score);

        if (score >= debt)
            GameStateManager.Instance.Victory();
        else
            GameStateManager.Instance.Lose();
    }
}
