using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    static private int HIGH_SCORE; //store high score
    static private int CURR_SCORE; //store current score
    public Text curScore;
    public Text highscore;

    //updates the score depending on the enemy killed
    public void UpdateScore(string name){
        //first easy enemy gives 25
        if (name == "Enemy_0(Clone)") //first enemy
        {
            CURR_SCORE += 25;
        }
        else if(name == "Enemy_1(Clone)") //second enemy
        {
            CURR_SCORE += 50;
        }
        else //third enemy
        {
            CURR_SCORE += 100;
        }
    }
    public void ResetScore()
    {
        //resets the score when the game is over
        CURR_SCORE = 0;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //set the score to 0
        CURR_SCORE = 0;
        //set the score text
        curScore.text = "Score: " + CURR_SCORE;
        //get the highscore from data
        HIGH_SCORE = PlayerPrefs.GetInt("highscore", HIGH_SCORE);
        //set the highscore text
        highscore.text = "Highscore: " + HIGH_SCORE;
    }

    // Update is called once per frame
    void Update()
    {
        //updates current score
        curScore.text = "Score: " + CURR_SCORE;
        //updates highscore when needed
        if (CURR_SCORE > HIGH_SCORE)
        {
            HIGH_SCORE = CURR_SCORE;
            //set the highscore in database
            PlayerPrefs.SetInt("highscore", HIGH_SCORE);
        }
    }
}
