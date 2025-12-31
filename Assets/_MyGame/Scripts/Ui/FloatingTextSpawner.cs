using UnityEngine;

public class FloatingTextSpawner : MonoBehaviour
{
    public static FloatingTextSpawner Instance;

    public FloatingScoreText prefab;
    public Canvas canvas;

    void Awake()
    {
        Instance = this;
    }

    public void Spawn(int amount, Vector3 worldPos)
    {
        FloatingScoreText txt =
            Instantiate(prefab, canvas.transform);

        txt.Play(amount, worldPos);
    }

    public void SpawnMultiplier(int multiplier, Vector3 worldPos)
    {
        FloatingScoreText txt =
            Instantiate(prefab, canvas.transform);

        txt.PlayMultiplier(multiplier, worldPos);
    }

}
