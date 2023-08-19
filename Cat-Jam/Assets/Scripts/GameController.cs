using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public Slider sliderTimer;
    float timer;
    public float startingTimer = 60;
    bool gameover;

    public GameObject panelWin;
    public GameObject panelLose;

    [Header("Pause")]
    private bool isPaused = false;
    public KeyCode pauseKey1 = KeyCode.Escape;
    public KeyCode pauseKey2 = KeyCode.P;
    public GameObject panelPause; 



    void Start()
    {
        timer = startingTimer;
        gameover = false;
    }


    void Update()
    {
        if (Input.GetKeyDown(pauseKey1) || Input.GetKeyDown(pauseKey2))
        {
            TogglePause();
        }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            gameover = true;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            panelLose.SetActive(true);
        }

    }

    private void TogglePause()
    {
        if (!gameover)
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                panelPause.SetActive(true);
            }
            else
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                panelPause.SetActive(false);
            }
        }
            
    }
    public void continueBtn()
    {
        Debug.Log("play btn");
        TogglePause();
    }

    public void menuBtn()
    {
        Debug.Log("menu btn");
        //go to menu
    }

    public void playBtn()
    {
        Debug.Log("play btn");
        //reload scene
    }
    public void playNextBtn()
    {
        Debug.Log("play next btn");
        //play meow sound u otro lvl
    }
}
