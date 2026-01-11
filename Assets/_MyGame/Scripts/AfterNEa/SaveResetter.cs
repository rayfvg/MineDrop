using UnityEngine;

public class SaveResetter : MonoBehaviour
{
    public void ResetAllSaves()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // Бафы
        if (GameModifiers.Instance != null)
            GameModifiers.Instance.ResetAll();

        // Очки / долг / рекорд
        ScoreManager.Instance.ResetScore();
        DebtManager.Instance.ResetAll();


        // UI
        ChanceTableUI.Instance.Refresh();
        DebtManager.Instance.UpdateUI();
        ScoreManager.Instance.UpdateUI();

        Debug.Log("🔥 ВСЕ СОХРАНЕНИЯ И UI СБРОШЕНЫ");
    }
}
