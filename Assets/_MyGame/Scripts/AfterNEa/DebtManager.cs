using TMPro;
using UnityEngine;

public class DebtManager : MonoBehaviour
{
    public static DebtManager Instance;

    public int currentDebt;

    [Header("UI")]
    public TMP_Text debtText;
    public TMP_Text recordText;

    const string DebtKey = "DEBT";
    const string RecordKey = "RECORD";

    void Awake()
    {
        Instance = this;

        currentDebt = PlayerPrefs.GetInt(DebtKey, 100);
        UpdateUI();
    }

    public void UpdateUI()
    {
        debtText.text = $"Ваш долг составляет:\n{currentDebt}";
        recordText.text = $"Рекорд: {PlayerPrefs.GetInt(RecordKey, 0)}";
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
