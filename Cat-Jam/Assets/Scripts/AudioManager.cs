using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public GameObject fxPrefab;

    [SerializeField] private AudioSource musicSource;
    private List<AudioSource> fxList = new List<AudioSource>();

    [Header("Music")]
    [SerializeField] private AudioClip music;


    [Header("SFX")]
    [SerializeField] public AudioClip btnClick;
    [SerializeField] public AudioClip playBtn, meow;

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

    public void playFxSound(AudioClip clip)
    {
        GameObject fxp = Instantiate(fxPrefab);
        fxp.transform.parent = gameObject.transform;
        AudioSource audioSource = fxp.GetComponent<AudioSource>();
        fxList.Add(audioSource);

        if (audioSource != null)
        {
            audioSource.PlayOneShot(clip);
            StartCoroutine(RemoveAudioSource(audioSource));
        }
    }


    public void ChangeMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    public void ChangeFxVolume(float value)
    {
        foreach(AudioSource audioSource in fxList)
            audioSource.volume = value;
    }

    public void ToggleFx()
    {
        foreach (AudioSource audioSource in fxList)
            audioSource.mute = !audioSource.mute;
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public bool FxIsOn()
    {
        foreach (AudioSource audioSource in fxList)
            if (audioSource.mute)
                return true;
        return false;

    }
    public bool MusicIsOn()
    {
        return !musicSource.mute;
    }

    IEnumerator RemoveAudioSource(AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        fxList.Remove(audioSource);
        Destroy(audioSource.gameObject);
    }


}
