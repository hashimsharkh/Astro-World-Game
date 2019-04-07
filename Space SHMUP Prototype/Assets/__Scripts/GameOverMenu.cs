using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public Text scoreText; //display score
    public Text gameOverText; // display game over message
    public AudioSource gameOverSource; // indicate game is over

    void Start()
    {
        gameOverSource.Play();
    }
    void Update()
    {
        scoreText.text = "YOUR SCORE IS: " + ScoreCounter.CURR_SCORE; //update score text with current score
        if(LevelProgression.getCurLevel() > 5)
        {
            gameOverText.text = "GAME OVER. YOU WIN!"; //player wins at level 5
        }
        else
        {
            gameOverText.text = "GAME OVER. YOU LOSE."; //player loses if less than level 5
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("_Main_Menu"); //reload main menu when restart
    }
}
