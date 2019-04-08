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
    public static bool GAME_OVER_ACTIVE; //check whether game over screen is active

    void Start()
    {
        gameOverSource.Play(); //play game over sound
        GAME_OVER_ACTIVE = true; //set that the game over menu is active
    }
    void Update()
    {
        scoreText.text = "YOUR SCORE IS: " + ScoreCounter.CURR_SCORE; //update score text with current score
        if(LevelProgression.GetCurLevel() > 10)
        {
            gameOverText.text = "GAME OVER. YOU WIN!"; //player wins at level 10
        }
        else
        {
            gameOverText.text = "GAME OVER. YOU LOSE."; //player loses if less than level 10
        }
    }

    public void Restart()
    {
        //reset the score when game is over
        ScoreCounter.ResetScore();
        //reset enemies for speed and spawning rate and UFO spawning
        Enemy.ResetSpeed();
        SetMusicVolume.Reset();
        Main.ENEMY_SPAWN_PER_SEC = .5f;
        Main.SPAWN_UFO = 1; //reset count for level of the ufos
        SceneManager.LoadScene("_Main_Menu"); //reload main menu when restart
        GAME_OVER_ACTIVE = false;
    }
}
