using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMusicVolume : MonoBehaviour
{
    public AudioSource myMusic; //stores music clip
    private static float _volume; //stores volume

    // Start is called before the first frame update
    void Start()
    {
        //change volume if needed and assign to clip
        _volume = ChangeMusicVolume.VolumeValue();
        myMusic.volume = _volume;
    }

    void Update()
    {
        //update volume
        myMusic.volume = _volume;
    }

    static public void ZeroVolume()
    {
        //reset volume
        _volume = 0.0f;
    }

}
