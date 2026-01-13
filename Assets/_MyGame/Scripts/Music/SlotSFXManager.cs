using UnityEngine;
using System.Collections;

public class SlotSFXManager : MonoBehaviour
{
    public static SlotSFXManager Instance;

    [Header("Sources")]
    public AudioSource loopSource;
    public AudioSource oneShotSource;

    [Header("Clips")]
    public AudioClip spinLoop;
    public AudioClip tick;
    public AudioClip stop;

    Coroutine tickRoutine;
    int activeSlots;

    void Awake()
    {
        Instance = this;
    }

    public void OnSlotStart()
    {
        activeSlots++;

        if (!loopSource.isPlaying)
        {
            loopSource.clip = spinLoop;
            loopSource.loop = true;
            loopSource.volume = 0.25f;
            loopSource.Play();
        }

        if (tickRoutine == null)
            tickRoutine = StartCoroutine(TickLoop());
    }

    public void OnSlotStop()
    {
        activeSlots--;

        oneShotSource.PlayOneShot(stop, 0.5f);

        if (activeSlots <= 0)
        {
            activeSlots = 0;

            if (tickRoutine != null)
            {
                StopCoroutine(tickRoutine);
                tickRoutine = null;
            }

            loopSource.Stop();
        }
    }

    IEnumerator TickLoop()
    {
        while (true)
        {
            oneShotSource.PlayOneShot(tick, 0.15f);
            yield return new WaitForSeconds(0.08f);
        }
    }
}
