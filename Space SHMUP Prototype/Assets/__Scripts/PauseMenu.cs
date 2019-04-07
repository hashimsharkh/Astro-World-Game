using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GAME_IS_PAUSED = false;
    public GameObject PauseMenuUI;


    // Update is called once per frame
    void Update()
    {
        // check if P is pressed so game can pause
        if (!(GameOverMenu.GAME_OVER_ACTIVE))
        {
            PauseClicked();
        }
            
    }

    void PauseClicked()
    {
        //If P is clicked 
        if (Input.GetKeyDown(KeyCode.P))
        {
            // if game is already paused and P is clicked, resume game
            if (GAME_IS_PAUSED)
            {
                Resume();
            }
            // if game is not paused and P is clicked, pause game
            else
            {
                Pause();
            }
        }
    }

    // resume game
    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        //un freezes time
        Time.timeScale = 1f;
        GAME_IS_PAUSED = false;

    }

    // pauses game when P is clicked or pause button on screen
    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        // freezes time
        Time.timeScale = 0f;
        GAME_IS_PAUSED = true;
    }

    // quits game and restarts it
    public void QuitGame()
    {
        //returns to main menu
        SceneManager.LoadScene("_Main_Menu");
    }
}
