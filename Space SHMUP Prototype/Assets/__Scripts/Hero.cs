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
    private float _shieldLevel = 1; // default shield level is 1

    //This variable holds a reference to the last triggering GameObject
    private GameObject _lastTriggerGo = null;
    public delegate void WeaponFireDelegate(); //new delegate type
    public WeaponFireDelegate fireDelegate;

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
        //Read key inputs from horizontal and vertical axis
        float _yPos = Input.GetAxis("Vertical");
        float _xPos = Input.GetAxis("Horizontal");

        //change position of x and y 
        Vector3 _pos = transform.position;
        _pos.y = _pos.y + (velocity *_yPos * Time.deltaTime);
        _pos.x += velocity * _xPos * Time.deltaTime;
        transform.position = _pos;

        //change rotation of object when it is moving
        transform.rotation = Quaternion.Euler(_yPos * pitchMult, _xPos * rollMult, 0);
        
        //call fireDelegate() if the delegate is not empty
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }
    }
    

    void OnTriggerEnter(Collider other)
    {
        Transform _rootTransform = other.gameObject.transform.root;
        GameObject _gameObjectRoot = _rootTransform.gameObject;
        
        //Make sure it is not the same triggering go as last time 
        //if it is it will be ignored as a duplicate and function wil; exit
        if (_gameObjectRoot == _lastTriggerGo)
            return;

        //go, the enmy GameObject is destroyed by hitting the shield
        _lastTriggerGo = _gameObjectRoot;

        //If the shield was triggered by an enemy
        if(_gameObjectRoot.tag =="Enemy")
        {
            shieldLevel--; //Decrease the level of the shield by 1
            Destroy(_gameObjectRoot); //And destroy the enemy
        }
        else if(_gameObjectRoot.tag == "PowerUp")
            StartCoroutine(AbsorbPowerUp(_gameObjectRoot));
        
        else
            print("Triggered by non-enemy: " + _gameObjectRoot.name);

    }
    IEnumerator AbsorbPowerUp(GameObject go)
    {
        int flag = 0; //used to indicate which powerup was used so effect can be reverted immediately afterwards
        PowerUp _powerUp = go.GetComponent<PowerUp>();

        ////Instantiate a smoke puff when user collides with powerup and destroy it after 1.5 seconds
        GameObject smokePuff = Instantiate(_powerUp.pickUpEffect, transform.position, transform.rotation) as GameObject;

        //Turn off mesh renderer/ colliders of powerup and its child so it does not appear on the screen
        Renderer[] rend = _powerUp.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rend)
            r.enabled = false;

        //Turning off rigidbody collisions
        _powerUp.getRigid().detectCollisions = false;
        Collider[] coll = _powerUp.GetComponentsInChildren<Collider>();

        //Turning off box collider for cube
        foreach (Collider c in coll)
            c.enabled = false;


        switch (_powerUp.powerUpType)
        {
            case PowerUpType.doublePoints:
                flag = 1;
                Destroy(smokePuff, 1.5f);
                PowerUp.multiplier = 2;
                break;

            case PowerUpType.invincibility:
                flag = 2;
                Collider[] collider = GetComponentsInChildren<Collider>();
                foreach (Collider c in collider)
                    c.enabled = false;
                break;
                
               

        }
        yield return new WaitForSeconds(_powerUp.duration);

        if(flag ==1)
             PowerUp.multiplier = 1;
        if (flag ==2)
        {
            Collider[] collider = GetComponentsInChildren<Collider>();
            foreach (Collider c in collider)
                c.enabled = true;
        }
        //Destroy the powerup
        if (_powerUp != null)
            _powerUp.AbsorbedBy(this.gameObject);
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
                Main.SINGLETON.DelayedRestart(gameRestartDelay);//Tell Main.S to restart the game after a delay
            }
        }
    }
}
