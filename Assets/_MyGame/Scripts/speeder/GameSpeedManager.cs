using UnityEngine;

public class GameSpeedManager : MonoBehaviour
{
    public static GameSpeedManager Instance;

    [Header("Speed Settings")]
    public float baseSpeed = 1f;
    public float firstFreeSpinSpeed = 1.5f;
    public float speedStep = 0.5f;
    public float maxSpeed = 5f;

    int freeSpinChain;

    void Awake()
    {
        Instance = this;
        ResetSpeed();
    }

    public void OnFreeSpinTriggered()
    {
        freeSpinChain++;

        float targetSpeed =
            firstFreeSpinSpeed + (freeSpinChain - 1) * speedStep;

        Time.timeScale = Mathf.Min(targetSpeed, maxSpeed);

        Debug.Log($"⚡ GAME SPEED x{Time.timeScale}");
    }

    public void OnFreeSpinEnded()
    {
        freeSpinChain--;

        //if (freeSpinChain <= 0)
        //{
        //    ResetSpeed();
        //}
    }

    public void ResetSpeed()
    {
        freeSpinChain = 0;
        Time.timeScale = baseSpeed;
        Debug.Log("⏱ GAME SPEED RESET");
    }
}
