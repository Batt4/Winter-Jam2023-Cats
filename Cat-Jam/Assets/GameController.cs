using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public Slider sliderTimer;
    float timer;
    public float startingTimer = 60;

    void Start()
    {
        timer = startingTimer;
    }


    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            // end game
            Debug.Log("game over");
        } else
        {

        }

    }
}
