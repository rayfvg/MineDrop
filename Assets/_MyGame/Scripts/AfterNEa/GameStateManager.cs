using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GameObject menuCanvas;
    public GameObject gameCanvas;
    public GameObject loseCanvas;
    public GameObject victoryCanvas;

    public UIAnimatedPopup losePopup;
    public UIAnimatedPopup victoryPopup;
    public GameState State { get; private set; }

    void Awake()
    {
        Instance = this;
        GoToMenu();
    }

    public void SetState(GameState newState)
    {
        State = newState;
    }

    public void GoToMenu()
    {
        State = GameState.Menu;
        menuCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        loseCanvas.SetActive(false);
        victoryCanvas.SetActive(false);

        GameModifiers.Instance.ResetAll();

        DebtManager.Instance.UpdateUI();
    }

    public void StartGame()
    {
        State = GameState.Playing;

        menuCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        loseCanvas.SetActive(false);
        victoryCanvas.SetActive(false);

        ScoreManager.Instance.ResetScore();
        RunManager.Instance.ResetRun();   // 🔥 ВОТ ЭТО
        GridManager.Instance.ResetFreeSpins(); // 👈 см. ниже
        BlockRefreshManager.Instance.RefreshFieldFirst();
        ChestManager.Instance.ResetAll();
    }

    public void Lose()
    {
        State = GameState.Lose;
        losePopup.Show();
    }


    public void Victory()
    {
        State = GameState.Victory;

        DebtManager.Instance.IncreaseDebt();
        victoryPopup.Show();

        BuffSelectionUI.Instance.Show();
    }


    public bool CanStartSpin()
    {
        return State == GameState.Playing;
    }
}
