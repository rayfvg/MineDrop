using System.Collections;
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
        if (GridManager.Instance.FreeSpins > 0)
            return;

        StartCoroutine(FinishRunDelayed());
    }

    IEnumerator FinishRunDelayed()
    {
        yield return new WaitForSeconds(0.6f); // 🔥 пауза осознания

        int score = ScoreManager.Instance.Score;
        int debt = DebtManager.Instance.currentDebt;

        DebtManager.Instance.SaveRecord(score);

        if (score >= debt)
            GameStateManager.Instance.Victory();
        else
            GameStateManager.Instance.Lose();
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
