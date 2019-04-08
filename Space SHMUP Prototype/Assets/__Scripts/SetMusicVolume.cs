using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMusicVolume : MonoBehaviour
{
    public AudioSource myMusic; //stores music clip
    public AudioSource sicko;
    private static float _volume; //stores volume
    static bool SICKO=false;

    // Start is called before the first frame update
    void Start()
    {
        //change volume if needed and assign to clip
        _volume = ChangeMusicVolume.VolumeValue();
        if (SICKO == false)
        {
            myMusic.volume = _volume;
            sicko.volume = 0;
        }
        else
        {
            myMusic.volume = 0;
            sicko.volume = _volume;
        }
    }

    void Update()
    {
        //update volume
        if (SICKO == false)
        {
            myMusic.volume = _volume;
            sicko.volume = 0;
        }
        else
        {
            myMusic.volume = 0;
            sicko.volume = _volume;
        }
    }

    static public void ZeroVolume()
    {
        //reset volume
        _volume = 0.0f;
    }
    static public void SickoM()
    {
        SICKO = true;
    }
    static public void Reset()
    {
        SICKO = false;
    }

}
