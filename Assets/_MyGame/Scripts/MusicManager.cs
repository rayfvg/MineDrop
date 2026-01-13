using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioSource source;

    [Header("Music")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMenu()
    {
        Play(menuMusic);
    }

    public void PlayGame()
    {
        Play(gameMusic);
    }

    void Play(AudioClip clip)
    {
        if (source.clip == clip)
            return;

        StopAllCoroutines();
        StartCoroutine(FadeTo(clip));
    }

    IEnumerator FadeTo(AudioClip newClip)
    {
        float t = 0f;

        while (t < 0.5f)
        {
            t += Time.unscaledDeltaTime;
            source.volume = Mathf.Lerp(0.1f, 0f, t / 0.1f);
            yield return null;
        }

        source.clip = newClip;
        source.Play();

        t = 0f;
        while (t < 0.5f)
        {
            t += Time.unscaledDeltaTime;
            source.volume = Mathf.Lerp(0f, 0.1f, t / 0.1f);
            yield return null;
        }
    }

    public void StopMusic()
    {
        source.Stop();
    }
}
