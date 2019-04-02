using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    static private float _sliderValue;
    public Slider slide;
    // Start is called before the first frame update
    void Start()
    {
        slide = (Slider)FindObjectOfType(typeof(Slider));
        slide.maxValue = 100;
        _sliderValue = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        slide.value = _sliderValue;
    }
    public void ResetSlider()
    {
        _sliderValue = 0f;
    }
    public void UpdateSlider()
    {
        _sliderValue += 5f;
    }
}
