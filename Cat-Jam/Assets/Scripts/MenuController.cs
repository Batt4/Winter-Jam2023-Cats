using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject controlPanel;
    public void play()
    {

    }
    public void controls()
    {
        controlPanel.SetActive(true);
    }
    public void menu()
    {
        controlPanel.SetActive(false);
    }
    public void soundToggle()
    {

    }
}
