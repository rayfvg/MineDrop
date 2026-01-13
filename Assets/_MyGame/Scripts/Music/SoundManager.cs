using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource source;

    [Header("Pickaxe")]
    public AudioClip pickaxeHit;
    public AudioClip pickaxeBreak;

    [Header("Blocks")]
    public AudioClip blockHit;
    public AudioClip blockDestroy;

    [Header("Dynamite")]
    public AudioClip dynamiteExplosion;


    public AudioClip OpenChest;

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

    public void Play(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        source.PlayOneShot(clip, volume);
    }
}