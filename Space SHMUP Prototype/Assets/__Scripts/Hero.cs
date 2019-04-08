using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public Weapon[] weapons;

    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1; // default shield level is 1
    public static bool invincibility = false;//invincibility is a variable that will be used to determine if ship is invincible
    public static bool NUKE = false;//shows nuke power up is active
    public static bool DOUBLE_POINTS_ACTIVE = false, INVINCIBILITY_ACTIVE = false, SLOW_DOWN_ACTIVE = false, NUKE_ACTIVE = false;// show that power up is active
    private static bool _DOUBLE_POINTS = true,_INVINCIBILITY=true,_SLOW_DOWN,_NUKE = true;//Used to instantiate power up icons
    private static bool _ENEMY_SLOW;//Used to instantiate power up icons
    private float _currentEnemySpeed;
    private float _timer = 0;
    //This variable holds a reference to the last triggering GameObject
    private GameObject _lastTriggerGo = null;
    public delegate void WeaponFireDelegate(); //new delegate type
    public WeaponFireDelegate fireDelegate;

    [Header("Audio Effects")]
    public AudioSource shootingSource1; //audio source for the first weapon
    public AudioSource shootingSource2; //audio source for the second weapon
    public AudioSource shootingSource3; //audio source for the third weapon
    public AudioSource destroySource1; //audio source when enemies are destroyed
    public AudioSource powerDownSource; //audio source when shield level decreases 

    void Start()
    {
        Time.timeScale = 1f; //make screen go at regular time
    }
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

        _timer += Time.deltaTime;//find how many seconds passed by from beginning of frame

        //if more than 7 seconds have passed revert everything to normal settings
        if (_timer > 7f)
        {
            _timer = 0;
            PowerUp.multiplier = 1;
            invincibility = false;
            Enemy.SPEED = _currentEnemySpeed;
            _ENEMY_SLOW = false;
            SLOW_DOWN_ACTIVE = false;
            _SLOW_DOWN = true;
            DOUBLE_POINTS_ACTIVE = false;
            _DOUBLE_POINTS = true;
            INVINCIBILITY_ACTIVE = false;
            _INVINCIBILITY = true;
            NUKE_ACTIVE = false;
            _NUKE = true;

        }
        //call fireDelegate() if the delegate is not empty
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }
        if (!_ENEMY_SLOW)
        {
            _currentEnemySpeed = Enemy.SPEED;
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
        if (_gameObjectRoot.tag == "Enemy")
        {
            if (!invincibility)
            {
                shieldLevel--; //Decrease the level of the shield by 1
                powerDownSource.Play();
                Destroy(_gameObjectRoot); //And destroy the enemy
            }
        }
        else if (_gameObjectRoot.tag == "PowerUp")
            AbsorbPowerUp(_gameObjectRoot);

        else
            print("Triggered by non-enemy: " + _gameObjectRoot.name);

    }
    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp _powerUp = go.GetComponent<PowerUp>();
        _timer = 0;

        //Instantiate a smoke puff when user collides with powerup and destroy it after 1.5 seconds
        GameObject _smokePuff = Instantiate(_powerUp.pickUpEffect, transform.position, transform.rotation) as GameObject;
        Destroy(_smokePuff, 1.5f);

        switch (_powerUp.powerUpType)
        {
            case PowerUpType.doublePoints:
                //these variables show that double points is active
                PowerUp.multiplier = 2;
                DOUBLE_POINTS_ACTIVE = true ;

                break;

            case PowerUpType.invincibility:
                //Invincibility is true
                invincibility = true;
                INVINCIBILITY_ACTIVE = true;

                break;

            case PowerUpType.nuke:
                NUKE_ACTIVE = true;
                //make nuke power up true
                _NUKE = true;
                NUKE = true;

                break;

            case PowerUpType.slowTime:
                //make enemy speed slower
                Enemy.SPEED = 2f;
                _ENEMY_SLOW = true;
                SLOW_DOWN_ACTIVE = true;
                break;
        }

        //Destroy the powerup
        if(_powerUp!=null)
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
                Time.timeScale = 0f; //freeze game
                SetMusicVolume.ZeroVolume(); //stops the game volume
                //make game over screen appear on top of main scene
                SceneManager.LoadScene("_Game_Over_Menu", LoadSceneMode.Additive);

            }
        }
    }

    //Check if powerups have been picked up
    public static bool ShouldSpawnDoublePoints()
    {
        //check if double points is picked up
        return _DOUBLE_POINTS;
    }

    public static bool ShouldSpawnInvincibility()
    {
        //check if invincibility is picked up
        return _INVINCIBILITY;
    }

    public static bool ShouldSpawnNuke()
    {
        //check if nuke is picked up
        return _NUKE;
    }

    public static bool ShouldSpawnSlowDown()
    {
        //check if slow down is picked up
        return _SLOW_DOWN;
    }


    //Setter functions
    public static void SetDoublePoints(bool value)
    {
        //set double points
        _DOUBLE_POINTS = value;
    }

    public static void SetInvincibility(bool value)
    {
        //set invicibility
        _INVINCIBILITY = value;
    }

    public static void SetSlowDown(bool value)
    {
        //set slow down
        _SLOW_DOWN = value;
    }

    public static void SetNuke(bool value)
    {
        //set nuke
        _NUKE = value;
    }
}
