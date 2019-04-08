using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GAME_IS_PAUSED = false; //determine if game is paused
    public GameObject PauseMenuUI; //store pause menu options


    // Update is called once per frame
    void Update()
    {
        //ensure that the game cannot be pasued on the main menu or game over menu
        if (!(GameOverMenu.GAME_OVER_ACTIVE && MainMenu.MAIN_MENU_ACTIVE))
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
        SceneManager.LoadScene("_Game_Over_Menu");
    }
}
