using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource musicSource, fxSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void playSound(AudioClip clip)
    {
        fxSource.PlayOneShot(clip);
    }

    public void ChangeMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    public void ChangeFxVolume(float value)
    {
        fxSource.volume = value;
    }

    public void ToggleFx()
    {
        fxSource.mute = !fxSource.mute;
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public bool FxIsOn()
    {
        return !fxSource.mute;
    }
    public bool MusicIsOn()
    {
        return !musicSource.mute;
    }
}
