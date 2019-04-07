using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMusicVolume : MonoBehaviour
{
    public AudioSource myMusic;
    private static float _volume;
    // Start is called before the first frame update
    void Start()
    {
        _volume = ChangeMusicVolume.VolumeValue();
        myMusic.volume = _volume;
    }

    void Update()
    {
        myMusic.volume = _volume;
    }

    static public void zeroVolume()
    {
        _volume = 0.0f;
    }

}
