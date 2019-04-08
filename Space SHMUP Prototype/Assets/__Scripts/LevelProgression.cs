using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelProgression : MonoBehaviour
{
    static private int _curLevel; //counts the current level
    static private bool _nextLevel; //detirmines when the next level change occurs
    public Text level; //text displaying current level
    public Slider slider; //the progress bar
    static private float _counter; //a counter to hold values for the progress bar
    public Text levelText;
    public Text levelFeaturesText; 

    //Getter function to return current level
    public static int getCurLevel()
    {
        return _curLevel;
    }
    // Start is called before the first frame update
    void Start()
    {//presets
        _curLevel = 1; //set level to 1 at start
        _nextLevel = false; 
        level.text = "Level "+_curLevel;
        _counter = 0f;
        levelFeaturesText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_nextLevel == true) //if a change level is requested
        {
            _curLevel++; //increase the level
            levelText.enabled = true;
            levelText.text = "LEVEL " + _curLevel;
            Invoke("DisableText", 3f);
            _nextLevel = false; //reset the condition
            level.text = "Level " + _curLevel; //update text
            if(_curLevel > 10)
            {
                SceneManager.LoadScene("_Game_Over_Menu");
            }
            if (_curLevel == 3)
            {
                levelFeaturesText.enabled = true;
                Invoke("DisableText", 3f);
            }
        }
        
        slider.value = _counter; //keeps slider updated in real time
        
    }
    void DisableText()
    {
        levelText.enabled = false;
        levelFeaturesText.enabled = false;
    }
    public static void IncLevel() //increases the level by one
    {
        _nextLevel = true; //condition to detirmine whether or not to change level
        if (_curLevel % 2 == 0)
        {
            Enemy.ChangeSpeed(); //makes enemies faster every two levels
        }
        Main.spawnUFO++; //increment for the start of spawning UFOs
        Main.SpawnFaster(); //makes more enemies spawn
    }
    public static void ResetSlider()
    {
        LevelProgression._counter = 0f; //set the slider to 0 when next level starts
    }
    public static void UpdateSlider()
    {
        LevelProgression._counter += .05f; //adds 5% to the bar 
    }
}