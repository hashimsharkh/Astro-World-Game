using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    public static bool GAME_OVER = false;
    public GameObject gameOverMenuUI;
    public GameObject[] finishObjects;

    void Start()
    {
        //all objects with this tag will be added
        finishObjects = GameObject.FindGameObjectsWithTag("ShowOnFinish");
        HideFinished();
    }

    // Update is called once per frame
    void Update()
    {
        if (Hero.SINGLETON.shieldLevel < 0)
                ShowFinished();
            ManualQuit();

    }

    public void Restart()
    {
        Main.SINGLETON.Restart();
    }

    void ManualQuit()
    {
        //If Q is clicked 
        if (Input.GetKeyDown(KeyCode.Q))
            ShowFinished();
    }

    void ShowFinished()
    {
        foreach (GameObject go in finishObjects)
            go.SetActive(true);
       Time.timeScale = 1f;
    }

    void HideFinished()
    {
        foreach (GameObject go in finishObjects)
            go.SetActive(false);
    }
}
