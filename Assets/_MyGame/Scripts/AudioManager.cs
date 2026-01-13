using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private void OnApplicationPause(bool pauseStatus)
    {
        AudioListener.pause = pauseStatus;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        AudioListener.pause = !hasFocus;
    }
}