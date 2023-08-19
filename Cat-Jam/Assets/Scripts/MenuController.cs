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
    public Toggle musicToggle;
    public Image imageMusicToggle;
    public Sprite activeMusicColor;
    public Sprite inactiveMusicColor;
    [SerializeField] private bool toggleMusic;

    [Header("Toggle FX")]
    public Toggle fxToggle;
    public Image imageFxToggle;
    public Sprite activeFxColor;
    public Sprite inactiveFxColor;
    [SerializeField] private bool toggleFx;

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
        int gameScenesIndex = gameScenes.IndexOf(SceneManager.GetActiveScene().ToString());
        gameScenesIndex++;
        if (gameScenesIndex > gameScenes.Count - 1)
            gameScenesIndex = 0;
        SceneManager.LoadScene(gameScenes[gameScenesIndex]);
    }
    public void controls()
    {
        controlPanel.SetActive(true);
    }
    public void menu()
    {
        controlPanel.SetActive(false);
    }

    public void Toggle()
    {
        var inst = AudioManager.instance;

        if (toggleFx)
        {
            inst.ToggleFx();
            imageFxToggle.sprite = inst.FxIsOn() ? activeFxColor : inactiveFxColor;
        }

        if (toggleMusic)
        {
            inst.ToggleMusic();
            imageMusicToggle.sprite = inst.MusicIsOn() ? activeMusicColor : inactiveMusicColor;
        }
    }

}
