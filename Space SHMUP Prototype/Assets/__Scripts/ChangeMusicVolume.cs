using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMusicVolume : MonoBehaviour
{
    public Slider volume;
    private static float val; //value for audio source
    public AudioSource myMusic;

    // Update is called once per frame
    void Update()
    {
        myMusic.volume = volume.value;
        val = volume.value;
    }
    static public float VolumeValue()
    {
        return val;
    }
}
