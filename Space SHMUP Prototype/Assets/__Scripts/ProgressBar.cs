using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    static private float _sliderValue; //value of slider
    public Slider slide; //store slider

    // Start is called before the first frame update
    void Start()
    {
        slide = (Slider)FindObjectOfType(typeof(Slider)); //find slider
        slide.maxValue = 100; //set max value
        _sliderValue = 0f; //set initial value
    }

    // Update is called once per frame
    void Update()
    {
        slide.value = _sliderValue; //declare value of slider
    }

    public void ResetSlider()
    {
        _sliderValue = 0f; //resets slider
    }

    public void UpdateSlider()
    {
        _sliderValue += 5f; //update slider 
    }
}
