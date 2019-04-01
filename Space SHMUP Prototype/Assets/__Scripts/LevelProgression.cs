using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgression : MonoBehaviour
{
    static private int _curLevel; //counts the current level
    static private bool _nextLevel; //detirmines when the next level change occurs
    public Text level;

    // Start is called before the first frame update
    void Start()
    {
        _curLevel = 0; //set level to 0...increases right away
        _nextLevel = false;
        level.text = "";
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

    }
    public void incLevel()
    {
        _nextLevel = true; //condition to detirmine whether or not to change level
    }
}