using TMPro;
using UnityEngine;

public class DebtManager : MonoBehaviour
{
    public static DebtManager Instance;

    [Header("Debt Progression")]
    public int startDebt = 100;

    public AnimationCurve debtGrowthCurve;
    // X = номер победы (0,1,2,3Е)
    // Y = множитель

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
        debtText.text = $"¬аш долг составл€ет:\n{currentDebt}";
        recordText.text = $"–екорд: {PlayerPrefs.GetInt(RecordKey, 0)}";
    }

    public void IncreaseDebt()
    {
        winsCount++;
        PlayerPrefs.SetInt(WinsKey, winsCount);

        float multiplier = debtGrowthCurve.Evaluate(winsCount);
        currentDebt = Mathf.RoundToInt(startDebt * multiplier);

        PlayerPrefs.SetInt(DebtKey, currentDebt);
        PlayerPrefs.Save();
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
}
