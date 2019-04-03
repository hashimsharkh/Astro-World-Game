using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMusicVolume : MonoBehaviour
{
    public AudioSource myMusic;
    private float _volume;
    // Start is called before the first frame update
    void Start()
    {
        _volume = ChangeMusicVolume.VolumeValue();
        myMusic.volume = _volume;
    }

}
