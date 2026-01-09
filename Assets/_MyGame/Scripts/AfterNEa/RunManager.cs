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

        StartCoroutine(FinishRunFlow());
    }

    IEnumerator FinishRunFlow()
    {
        yield return new WaitForSeconds(0.4f);

        if (ScoreManager.Instance.pendingMultipliers.Count > 0)
        {
            yield return StartCoroutine(PlayChestMultipliers());
        }

        FinishRun();
    }

    IEnumerator PlayChestMultipliers()
    {
        GameStateManager.Instance.SetState(GameState.Paused);

        foreach (int mult in ScoreManager.Instance.pendingMultipliers)
        {
            FloatingTextSpawner.Instance.SpawnMultiplier(mult, Vector3.zero);

            yield return new WaitForSeconds(0.4f);

            ScoreManager.Instance.ApplyMultiplier(mult);

            yield return new WaitForSeconds(0.3f);
        }

        ScoreManager.Instance.pendingMultipliers.Clear();

        GameStateManager.Instance.SetState(GameState.Playing);
    }

    void FinishRun()
    {
        int score = ScoreManager.Instance.BaseScore;
        int debt = DebtManager.Instance.currentDebt;

        DebtManager.Instance.SaveRecord(score);

        if (score >= debt)
            GameStateManager.Instance.Victory();
        else
            GameStateManager.Instance.Lose();
    }
}
