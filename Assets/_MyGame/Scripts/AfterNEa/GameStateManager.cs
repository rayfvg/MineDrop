using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GameObject menuCanvas;
    public GameObject gameCanvas;
    public GameObject loseCanvas;
    public GameObject victoryCanvas;

    public GameState State { get; private set; }

    void Awake()
    {
        Instance = this;
        GoToMenu();
    }

    public void GoToMenu()
    {
        State = GameState.Menu;
        menuCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        loseCanvas.SetActive(false);
        victoryCanvas.SetActive(false);

        GameModifiers.Instance.ResetAll();
    }

    public void StartGame()
    {
        State = GameState.Playing;
        menuCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        loseCanvas.SetActive(false);
        victoryCanvas.SetActive(false);

        ScoreManager.Instance.ResetScore();
        BlockRefreshManager.Instance.RefreshFieldFirst();
    }

    public void Lose()
    {
        State = GameState.Lose;
        loseCanvas.SetActive(true);
    }

    public void Victory()
    {
        State = GameState.Victory;
        victoryCanvas.SetActive(true);
    }

    public bool CanStartSpin()
    {
        return State == GameState.Playing;
    }
}
