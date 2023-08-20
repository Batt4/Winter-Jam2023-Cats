using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public GameObject fxPrefab;

    bool muteFx = false;

    [SerializeField] private AudioSource musicSource;
    private List<AudioSource> fxList = new List<AudioSource>();

    [Header("Music")]
    [SerializeField] public AudioClip musicaMenu, musicaGame;


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

    public void changeMusic(AudioClip audio)
    {
        musicSource.loop = true;
        musicSource.Stop();
        musicSource.PlayOneShot(audio);
    }

    public void playFxSound()
    {
        GameObject fxp = Instantiate(fxPrefab);
        fxp.transform.parent = gameObject.transform;
        AudioSource audioSource = fxp.GetComponent<AudioSource>();
        fxList.Add(audioSource);

        if (audioSource != null)
        {
            audioSource.PlayOneShot(meow);
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
        muteFx = !muteFx;
        if (fxList.Count > 0)
            foreach (AudioSource audioSource in fxList)
                audioSource.mute = muteFx;
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
        yield return new WaitForSeconds(1);
        fxList.Remove(audioSource);
        Destroy(audioSource.gameObject);
    }


}
