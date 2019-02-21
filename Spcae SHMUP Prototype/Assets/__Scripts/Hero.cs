using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    //Singleton (Software Design pattern)
    static public Hero Singleton;

    //Set variables that will control the movement of the ship in inspector
    [Header("Set in Inspector")]
    public float velocity = 30;
    public float rollMult = -45;
    public float pitchMult = 30;

    void awake()
    {
        //Set the the singleton for the hero class
        if (Singleton == null)
            Singleton = this;
        else
            Debug.LogError("Another instance of hero tries to exist and assign itself to Singleton");
    }

    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        //Read
        float y = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");

        Vector3 pos = transform.position;
        pos.y = pos.y+( velocity * y * Time.deltaTime);
        pos.x += velocity * x * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(y * pitchMult, x * rollMult, 0);
    }
}
