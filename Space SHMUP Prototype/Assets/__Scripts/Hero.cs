using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    //Singleton (Software Design pattern)
    static public Hero SINGLETON;

    //Set variables that will control the movement of the ship in inspector
    [Header("Set in Inspector")]
    public float velocity = 30;
    public float rollMult = -45;
    public float pitchMult = 30;

    void Awake()
    {
        //Set the the singleton for the hero class
        if (SINGLETON == null)
            SINGLETON = this;
        else
            Debug.LogError("Another instance of hero tries to exist and assign itself to Singleton");
    }

    // Update is called once per frame
    void Update()
    {
        //Read key inputs
        float _yPos = Input.GetAxis("Vertical");
        float _xPos = Input.GetAxis("Horizontal");


        Vector3 pos = transform.position;
        pos.y = pos.y+( velocity *_yPos * Time.deltaTime);
        pos.x += velocity * _xPos * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(_yPos * pitchMult, _xPos * rollMult, 0);
    }
}
