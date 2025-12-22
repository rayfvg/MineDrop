using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudParallaxUI : MonoBehaviour
{
    [System.Serializable]
    public class CloudSettings
    {
        public Sprite sprite;

        [Header("Vertical position (Y)")]
        public float minY;
        public float maxY;

        [Header("Speed")]
        public float baseSpeed = 30f;
        public float randomSpeedOffset = 10f;

        [Header("Size")]
        public Vector2 size = new Vector2(200, 100);
    }

    [Header("Clouds")]
    public List<CloudSettings> clouds = new List<CloudSettings>();
    public int cloudsPerType = 2;

    private RectTransform canvasRect;
    private List<CloudInstance> activeClouds = new List<CloudInstance>();

    void Start()
    {
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        CreateClouds();
    }

    void Update()
    {
        foreach (var cloud in activeClouds)
        {
            cloud.Update();

            if (cloud.Rect.anchoredPosition.x > canvasRect.rect.width / 2 + 200)
            {
                cloud.ResetPosition(-canvasRect.rect.width / 2 - 200);
            }
        }
    }

    void CreateClouds()
    {
        foreach (var setting in clouds)
        {
            for (int i = 0; i < cloudsPerType; i++)
            {
                GameObject go = new GameObject("Cloud", typeof(Image));
                go.transform.SetParent(transform, false);

                Image img = go.GetComponent<Image>();
                img.sprite = setting.sprite;
                img.preserveAspect = true;

                RectTransform rect = img.rectTransform;
                rect.sizeDelta = setting.size;

                float x = Random.Range(-canvasRect.rect.width / 2, canvasRect.rect.width / 2);
                float y = Random.Range(setting.minY, setting.maxY);
                rect.anchoredPosition = new Vector2(x, y);

                CloudInstance instance = new CloudInstance(rect, setting);
                activeClouds.Add(instance);
            }
        }
    }

    private class CloudInstance
    {
        public RectTransform Rect;
        private CloudSettings settings;

        private float speed;
        private float speedTimer;

        public CloudInstance(RectTransform rect, CloudSettings settings)
        {
            Rect = rect;
            this.settings = settings;
            RandomizeSpeed();
        }

        public void Update()
        {
            speedTimer -= Time.deltaTime;
            if (speedTimer <= 0)
                RandomizeSpeed();

            Rect.anchoredPosition += Vector2.right * speed * Time.deltaTime;
        }

        void RandomizeSpeed()
        {
            speed = settings.baseSpeed +
                    Random.Range(-settings.randomSpeedOffset, settings.randomSpeedOffset);

            speed = Mathf.Max(5f, speed);
            speedTimer = Random.Range(1.5f, 4f);
        }

        public void ResetPosition(float startX)
        {
            float y = Random.Range(settings.minY, settings.maxY);
            Rect.anchoredPosition = new Vector2(startX, y);
            RandomizeSpeed();
        }
    }
}
