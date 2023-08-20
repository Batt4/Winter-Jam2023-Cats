using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [Header("Possible Levels")]
    public List<string> gameScenes = new List<string>();

    [Header("Coins")]
    public GameObject coinPrefab;
    public int numberOfSpawns = 5;
    public List<Transform> coinsSpawns = new List<Transform>();
    List<GameObject> coins = new List<GameObject>();


    void Start()
    {
        timer = startingTimer;
        sliderTimer.maxValue = startingTimer;
        updateTimer(startingTimer);
        gameover = false;
        if (numberOfSpawns > coinsSpawns.Count)
            numberOfSpawns = coinsSpawns.Count;

        List<int> selectedIndexes = new List<int>();
        while (selectedIndexes.Count < numberOfSpawns)
        {
            int randomIndex = Random.Range(0, coinsSpawns.Count-1);
            if (!selectedIndexes.Contains(randomIndex))
            {
                selectedIndexes.Add(randomIndex);
            }
        }
        Debug.Log("count: " + selectedIndexes.Count);
        for (int index = 0; index <= selectedIndexes.Count-1; index++)
        {
            GameObject inst = Instantiate(coinPrefab, coinsSpawns[selectedIndexes[index]].position, Quaternion.identity);
            coins.Add(inst);
        }

    }


    void Update()
    {
        if (Input.GetKeyDown(pauseKey1) || Input.GetKeyDown(pauseKey2))
        {
            TogglePause();
        }

        timer -= Time.deltaTime;
        updateTimer(timer); ;

        if (timer <= 0)
        {
            gameover = true;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            panelLose.SetActive(true);
            StopAllCoroutines();
        }

    }

    private void updateTimer(float t)
    {
        //float normalizedTime = t / sliderTimer.maxValue;
        sliderTimer.value = t;
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
        SceneManager.LoadScene("Menu");
    }

    public void playBtn()
    {
        Debug.Log("play again btn");
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
 
    }
    public void playNextBtn()
    {
        Debug.Log("play next btn");/*
        int gameScenesIndex = gameScenes.IndexOf(SceneManager.GetActiveScene().ToString());
        gameScenesIndex++;
        if (gameScenesIndex > gameScenes.Count - 1)
            gameScenesIndex = 0;
        SceneManager.LoadScene(gameScenes[gameScenesIndex]);*/
        SceneManager.LoadScene("Game");
    }
}
