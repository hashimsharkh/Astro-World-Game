using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgression : MonoBehaviour
{
    static private int _curLevel; //counts the current level
    static private bool _nextLevel; //detirmines when the next level change occurs
    public Text level; //text displaying current level
    public Slider slider; //the progress bar
    static private float _counter; //a counter to hold values for the progress bar

    // Start is called before the first frame update
    void Start()
    {//presets
        _curLevel = 0; //set level to 0...increases right away
        _nextLevel = false; 
        level.text = "";
        _counter = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_nextLevel == true) //if a change level is requested
        {
            _curLevel++; //increase the level
            _nextLevel = false; //reset the condition
            level.text = "Level " + _curLevel; //update text
        }
        slider.value = _counter; //keeps slider updated in real time
    }
    public void IncLevel() //increases the level by one
    {
        _nextLevel = true; //condition to detirmine whether or not to change level
    }
    public void ResetSlider()
    {
        _counter = 0f; //set the slider to 0 when next level starts
    }
    public void UpdateSlider()
    {
        _counter += .05f; //adds 5% to the bar 
    }
}