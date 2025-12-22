using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BirdParallaxUI : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 60f;

    [Header("Vertical movement")]
    public float floatAmplitude = 20f;
    public float floatFrequency = 1.5f;

    [Header("Spawn Y")]
    public float minY = -100f;
    public float maxY = 200f;

    [Header("Random variation")]
    public float speedRandomOffset = 10f;
    public float amplitudeRandomOffset = 5f;

    [Header("Delays")]
    public float startDelay = 2f;
    public float delayBetweenFlights = 5f;

    private RectTransform rect;
    private RectTransform canvasRect;
    private Image image;

    private float currentSpeed;
    private float baseY;
    private float timeOffset;

    private bool isFlying;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        image = GetComponent<Image>();

        image.enabled = false;
        StartCoroutine(StartWithDelay());
    }

    void Update()
    {
        if (!isFlying)
            return;

        float x = rect.anchoredPosition.x + currentSpeed * Time.deltaTime;
        float y = baseY + Mathf.Sin(Time.time * floatFrequency + timeOffset) * floatAmplitude;

        rect.anchoredPosition = new Vector2(x, y);

        if (x > canvasRect.rect.width / 2 + rect.rect.width)
        {
            EndFlight();
        }
    }

    IEnumerator StartWithDelay()
    {
        yield return new WaitForSeconds(startDelay);
        StartFlight();
    }

    void StartFlight()
    {
        Respawn();
        image.enabled = true;
        isFlying = true;
    }

    void EndFlight()
    {
        isFlying = false;
        image.enabled = false;
        StartCoroutine(WaitForNextFlight());
    }

    IEnumerator WaitForNextFlight()
    {
        yield return new WaitForSeconds(delayBetweenFlights);
        StartFlight();
    }

    void Respawn()
    {
        baseY = Random.Range(minY, maxY);
        timeOffset = Random.Range(0f, 10f);

        currentSpeed = speed + Random.Range(-speedRandomOffset, speedRandomOffset);
        currentSpeed = Mathf.Max(10f, currentSpeed);

        floatAmplitude += Random.Range(-amplitudeRandomOffset, amplitudeRandomOffset);
        floatAmplitude = Mathf.Max(5f, floatAmplitude);

        rect.anchoredPosition = new Vector2(
            -canvasRect.rect.width / 2 - rect.rect.width,
            baseY
        );
    }
}
