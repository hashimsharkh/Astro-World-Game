using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public static bool GAME_OVER = false;
    public GameObject gameOverMenuUI;
    public GameObject[] finishObjects;
    public Text scoreText;
    //public Button mainMenuBtn, shareScoreBtn;
    //private bool isButtonPressed = false;

    void Start()
    {
        //all objects with this tag will be added
        finishObjects = GameObject.FindGameObjectsWithTag("ShowOnFinish");
        HideFinished();
        //gameOverMenuUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Hero.SINGLETON.shieldLevel < 0)
        {
        ShowFinished();
        gameOverMenuUI.SetActive(true);
        //    //Main.SINGLETON.DelayedRestart(10f);
        }
        //ManualQuit();

    }

    public void Restart()
    {
        SceneManager.LoadScene("_Main_Menu");
    }

    void ManualQuit()
    {
        //If Q is clicked 
        if (Input.GetKeyDown(KeyCode.Q))
            ShowFinished();
    }

    void ShowFinished()
    {
        gameOverMenuUI.SetActive(true);
        foreach (GameObject go in finishObjects)
            go.SetActive(true);
        //Time.timeScale = 1f;
        //Main.SINGLETON.DelayedRestart(10f);
        scoreText.text = "YOUR SCORE IS: " + ScoreCounter.CURR_SCORE;
    }

    void HideFinished()
    {
        foreach (GameObject go in finishObjects)
            go.SetActive(false);
    }
}
