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
    public float gameRestartDelay = 2f;//Restart delay of 2 seconds used after hero ship is destroyed

    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;

    //This variable holds a reference to the last triggering GameObject
    private GameObject _lastTriggerGo = null;

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

    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        //print("Triggered : " + go.name); used to print when an object collides with hero ship

        //Make sure it is not the same triggering go as last time 
        //if it is it will be ignored as a duplicate and function wil; exit
        if (go == _lastTriggerGo)
            return;

        //go, the enmy GameObject is destroyed by hitting the shield
        _lastTriggerGo = go;

        //If the shield was triggered by an enemy
        if(go.tag =="Enemy")
        {
            shieldLevel--; //Decrease the level of the shield by 1
            Destroy(go); //And destroy the enemy
        }
        else
            ////Wont happen in our case but if a non enemy collides with the ship; it will be printed on console
            print("Triggered by non-enemy: " + go.name);

    }

    //shieldLevel property
    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);//Ensures that _shieldLevel is never higher than 4
            if (value < 0)
            {
                Destroy(this.gameObject);//If value passed is less than 0,_Hero is destroyed
                Main.S.DelayedRestart(gameRestartDelay);//Tell Main.S to restart the game after a delay
            }
        }
    }
}
