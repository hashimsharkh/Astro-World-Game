using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    static private int _highscore;
    static private int _score;
    public Text curScore;
    public Text highscore;

    //updates the score depending on the enemy killed
    public void UpdateScore(string name){
        //first easy enemy gives 25
        if (name == "Enemy_0(Clone)") //first enemy
        {
            _score += 25;
        }
        else if(name == "Enemy_1(Clone)") //second enemy
        {
            _score += 50;
        }
        else //third enemy
        {
            _score += 100;
        }
    }
    public void ResetScore()
    {
        //resets the score when the game is over
        _score = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        //set the score to 0
        _score = 0;
        //set the score text
        curScore.text = "Score: " + _score;
        //get the highscore from data
        _highscore = PlayerPrefs.GetInt("highscore", _highscore);
        //set the highscore text
        highscore.text = "Highscore: " + _highscore;
    }

    // Update is called once per frame
    void Update()
    {
        //updates current score
        curScore.text = "Score: " + _score;
        //updates highscore when needed
        if (_score > _highscore)
        {
            _highscore = _score;
            //set the highscore in database
            PlayerPrefs.SetInt("highscore", _highscore);
        }
    }
}
