using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMusicVolume : MonoBehaviour
{
    public Slider volume;
    private static float _val; //value for audio source
    public AudioSource myMusic; //music clip

    // Update is called once per frame
    void Update()
    {
        myMusic.volume = volume.value; //sets music volume
        _val = volume.value;
    }
    static public float VolumeValue()
    {
        return _val; //returns the float value for volume of music
    }
}
