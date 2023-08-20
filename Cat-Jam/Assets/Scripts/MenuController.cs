using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject controlPanel;
    public List<string> gameScenes = new List<string>();

    [Header("Toggle Music")]
    public Image imageMusicToggle;
    public Sprite activeMusicColor;
    public Sprite inactiveMusicColor;
    public bool toggleMusic = true;

    [Header("Toggle FX")]
    public Image imageFxToggle;
    public Sprite activeFxColor;
    public Sprite inactiveFxColor;
    public bool toggleFx = true;

    [Header("Slider")]
    public Slider musicSlider;
    public Slider fxSlider;


    void Start()
    {

        AudioManager.instance.ChangeMusicVolume(musicSlider.value);
        AudioManager.instance.ChangeFxVolume(fxSlider.value);
        musicSlider.onValueChanged.AddListener(val => AudioManager.instance.ChangeMusicVolume(val));
        fxSlider.onValueChanged.AddListener(val => AudioManager.instance.ChangeFxVolume(val));

    }

    public void play()
    {
        Debug.Log("play next btn");
        playClick();
        SceneManager.LoadScene("Game");
            /*
        int gameScenesIndex = gameScenes.IndexOf(SceneManager.GetActiveScene().ToString());
        gameScenesIndex++;
        if (gameScenesIndex > gameScenes.Count - 1)
            gameScenesIndex = 0;
        SceneManager.LoadScene(gameScenes[gameScenesIndex]);*/
    }
    public void controls()
    {
        playClick();
        controlPanel.SetActive(true);
    }
    public void menu()
    {
        playClick();
        controlPanel.SetActive(false);
    }

    public void ToggleFX()
    {
        AudioManager inst = AudioManager.instance;
        inst.ToggleFx();
        imageFxToggle.sprite = inst.FxIsOn() ? activeFxColor : inactiveFxColor;
        if (inst.FxIsOn())
            playClick();
    }
    public void ToggleMusic()
    {
        AudioManager inst = AudioManager.instance;
        inst.ToggleMusic();
        imageMusicToggle.sprite = inst.MusicIsOn() ? activeMusicColor : inactiveMusicColor;
        if (inst.FxIsOn())
            playClick();
    }

    private void playClick()
    {
        AudioManager inst = AudioManager.instance;
        inst.playFxSound();
    }
}
