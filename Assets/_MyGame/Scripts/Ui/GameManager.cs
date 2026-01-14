using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string rewardID;

    public GameObject BuffMenu;
    public GameObject VinnerLable;


    void Awake()
    {
        Instance = this;
    }

    public void MyRewardAdvShow()
    {
        YG2.RewardedAdvShow(rewardID, () =>
        {
            // Получение вознаграждения
            WinnerAfterAds();

            // По желанию, воспользуйтесь ID вознаграждения
            if (rewardID == "AddCoin")
                WinnerAfterAds();
        });
    }


    public void WinnerAfterAds()
    {
        GameStateManager.Instance.VictoryAfterAds();
        BuffMenu.SetActive(false);
    }

    public void GetMeAds()
    {
        YG2.InterstitialAdvShow();
    }

    public void GetReview()
    {
        YG2.ReviewShow();
    }
}
