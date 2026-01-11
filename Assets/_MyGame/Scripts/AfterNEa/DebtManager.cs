using TMPro;
using UnityEngine;

public class DebtManager : MonoBehaviour
{
    public static DebtManager Instance;

    [Header("Debt Progression")]
    public int startDebt = 100;

    public AnimationCurve debtGrowthCurve;
    // X = номер победы (0,1,2,3…)
    // Y = множитель
    public float debtGrowthPerWin = 1.35f;
    int winsCount;
    const string WinsKey = "WINS";

    public int currentDebt;

    [Header("UI")]
    public TMP_Text debtText;
    public TMP_Text recordText;

    const string DebtKey = "DEBT";
    const string RecordKey = "RECORD";

    void Awake()
    {
        Instance = this;

        winsCount = PlayerPrefs.GetInt(WinsKey, 0);
        currentDebt = PlayerPrefs.GetInt(DebtKey, startDebt);

        UpdateUI();
    }

    public void UpdateUI()
    {
        debtText.text = $"Ваш долг составляет:\n{currentDebt}";
        recordText.text = $"Рекорд: {PlayerPrefs.GetInt(RecordKey, 0)}";
    }

    public void IncreaseDebt()
    {
        winsCount++;
        PlayerPrefs.SetInt(WinsKey, winsCount);

        currentDebt = Mathf.RoundToInt(startDebt * Mathf.Pow(debtGrowthPerWin, winsCount));

        PlayerPrefs.SetInt(DebtKey, currentDebt);
        PlayerPrefs.Save();

        UpdateUI();
    }

    public void SaveRecord(int score)
    {
        int record = PlayerPrefs.GetInt(RecordKey, 0);
        if (score > record)
        {
            PlayerPrefs.SetInt(RecordKey, score);
            PlayerPrefs.Save();
        }
    }

    public void ResetAll()
    {
        winsCount = 0;
        currentDebt = startDebt;

        PlayerPrefs.SetInt(WinsKey, winsCount);
        PlayerPrefs.SetInt(DebtKey, currentDebt);
        PlayerPrefs.Save();

        UpdateUI();
    }
}
