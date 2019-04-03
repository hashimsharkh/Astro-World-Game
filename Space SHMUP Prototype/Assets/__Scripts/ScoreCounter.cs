using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    static private int HIGH_SCORE; //store high score
    static private int CURR_SCORE; //store current score
    public Text curScore; //text displaying the current score
    public Text highscore; //text displaying the high score
    private int levelCount; //this int is compared to score to detirmine level switches
    private int pbTracker; //this int tracks when the progress bar is updated
    LevelProgression lp; //counts levels from score

    //updates the score depending on the enemy killed
    public static void UpdateScore(string name){
        //first easy enemy gives 25
        if (name == "Enemy_0(Clone)") //first enemy
        {
            CURR_SCORE += 25*PowerUp.multiplier;
        }
        else if(name == "Enemy_1(Clone)") //second enemy
        {
            CURR_SCORE += 50*PowerUp.multiplier;
        }
        else //third enemy
        {
            CURR_SCORE += 100 * PowerUp.multiplier;
        }
    }
    public static void ResetScore()
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
        //set a new instance of levelprogression
        //lp = new LevelProgression();
        //set the tracker to send a signal every 50
        pbTracker = 50;
        //sets the level count to change when score reaches 1000
        levelCount = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        //updates current score
        curScore.text = "Score: " + CURR_SCORE;
        //updates highscore when needed
        if (CURR_SCORE >= HIGH_SCORE)
        {
            HIGH_SCORE = CURR_SCORE;
            //set the highscore in database
            PlayerPrefs.SetInt("highscore", HIGH_SCORE);
            highscore.text = "Highscore: " + HIGH_SCORE;
        }
        if (CURR_SCORE >= levelCount) //if the score reaches a 1000 point threshold, the level will change
        {
            levelCount = levelCount + 1000; //sets the next level 1000 points away
            LevelProgression.IncLevel(); //increases the level
            LevelProgression.ResetSlider(); //resets the progress bar for the next level
        }
        if (CURR_SCORE >= pbTracker)
        {
            LevelProgression.UpdateSlider(); //updates slider 
            pbTracker += 50; //sets the next update 50 points ahead
        }
    }
}
