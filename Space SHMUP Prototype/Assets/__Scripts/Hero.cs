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


    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1; // default shield level is 1

    //This variable holds a reference to the last triggering GameObject
    private GameObject _lastTriggerGo = null;
    public delegate void WeaponFireDelegate(); //new delegate type
    public WeaponFireDelegate fireDelegate;

    [Header("Audio Effects")]
    public AudioClip shootingSound1;
    public AudioClip shootingSound2;
    public AudioClip shootingSound3;
    public AudioSource shootingSource1;
    public AudioSource shootingSource2;
    public AudioSource shootingSource3;
    public AudioClip destroySound1;
    public AudioSource destroySource1;

    void Start()
    {
        Time.timeScale = 1f;
        //attach a sound to each source
        shootingSource1.clip = shootingSound1;
        shootingSource2.clip = shootingSound2;
        shootingSource3.clip = shootingSound3;
        destroySource1.clip = destroySound1;
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
            AbsorbPowerUp(_gameObjectRoot);
        
        else
            print("Triggered by non-enemy: " + _gameObjectRoot.name);

    }
    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp _powerUp= go.GetComponent<PowerUp>();

        switch (_powerUp.type)
        {

        }
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
                destroySource1.Play();
                Destroy(this.gameObject);//If value passed is less than 0,_Hero is destroyed
                Time.timeScale = 0f;
                SceneManager.LoadScene("_Game_Over_Menu", LoadSceneMode.Additive);
                //Main.SINGLETON.DelayedRestart(gameRestartDelay);//Tell Main.S to restart the game after a delay
            }
        }
    }
}
