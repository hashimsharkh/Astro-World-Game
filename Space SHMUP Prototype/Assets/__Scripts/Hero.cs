using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using UnityEngine.UI;
=======
using UnityEngine.SceneManagement;
>>>>>>> master

public class Hero : MonoBehaviour
{
    //Singleton (Software Design pattern)
    static public Hero SINGLETON;

    public GameObject Hero1;
    private GameObject barrel;
    private GameObject collar;
    private GameObject cube;
    private GameObject wing;
    private Material b;
    private Material cu;
    private Material co;
    private Material w;
    private int size = 4;
    private Material[] _material;
    private Color[] _colors = new Color[] { Color.cyan, Color.black };
    private Color[] _originalColors;
    private int index = 0;
    public RawImage doublePoints, inv;//Declare 2 images for powerups

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

<<<<<<< HEAD
    private bool _invincibility = false;//invincibility is a variable that will be used to determine if ship is invincible
    
=======
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
        //attach a sound to each source
        shootingSource1.clip = shootingSound1;
        shootingSource2.clip = shootingSound2;
        shootingSource3.clip = shootingSound3;
        destroySource1.clip = destroySound1;
    }
>>>>>>> master
    void Awake()
    {
        //Set the the singleton for the hero class
        if (SINGLETON == null)
            SINGLETON = this;
        else
            Debug.LogError("Another instance of hero tries to exist and assign itself to Singleton");

        //Find the child game objects of hero and their renderers
        barrel = Hero1.transform.Find("Weapon/Barrel").gameObject;
        collar = Hero1.transform.Find("Weapon/Collar").gameObject;
        cube = Hero1.transform.Find("Cockpit/Cube").gameObject;
        wing = Hero1.transform.Find("Wing").gameObject;

        //find materials of the children
        b = barrel.GetComponent<Renderer>().material;
        co = collar.GetComponent<Renderer>().material;
        cu = cube.GetComponent<Renderer>().material;
        w = wing.GetComponent<Renderer>().material;

        //find the colors of these children so they are not changed
        _originalColors = new Color[] { b.color, co.color, cu.color, w.color };

        //assign the materials to the _material array
        _material = new Material[] { b, co, cu, w };

       
    }
    void Start()
    {
        GameObject go = GameObject.Find("DoublePoints");

        if (go != null)
        {
            doublePoints = go.GetComponent<RawImage>();
            doublePoints.gameObject.SetActive(false);
        }


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
<<<<<<< HEAD
            if (_invincibility == false)
            {
                shieldLevel--; //Decrease the level of the shield by 1
                Destroy(_gameObjectRoot); //And destroy the enemy
            }
=======
            
            shieldLevel--; //Decrease the level of the shield by 1
            Destroy(_gameObjectRoot); //And destroy the enemy
            
>>>>>>> master
        }
        else if(_gameObjectRoot.tag == "PowerUp")
            StartCoroutine(AbsorbPowerUp(_gameObjectRoot));
        
        else
            print("Triggered by non-enemy: " + _gameObjectRoot.name);

    }
    IEnumerator AbsorbPowerUp(GameObject go)
    {
<<<<<<< HEAD
        int _flag = 0; //used to indicate which powerup was used so effect can be reverted immediately afterwards
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

        Destroy(smokePuff, 1.5f);
        
        switch (_powerUp.powerUpType)
=======
        PowerUp _powerUp= go.GetComponent<PowerUp>();

        switch (_powerUp.type)
>>>>>>> master
        {
            case PowerUpType.doublePoints:
                _flag = 1;
                PowerUp.multiplier = 2;
                //Instantiate(PowerUp.powerUpPrefab, transform.position, Quaternion.identity);
                break;

            case PowerUpType.invincibility:
                //Invincibility is true
                _flag = 2;
                _invincibility = true;

                break;
                
               

        }
    
        yield return new WaitForSeconds(_powerUp.duration);

        if(_flag ==1)
        { 
             PowerUp.multiplier = 1;
             doublePoints.gameObject.SetActive(false);
        }
        if (_flag == 2)
        {
            _invincibility = false;
        }
<<<<<<< HEAD
        //Destroy the powerup
        if (_powerUp != null)
            _powerUp.AbsorbedBy(this.gameObject);
       
    }

    public void FixedUpdate()
    {
        //Make color of ship flash if invincibility powerup is picked up
        for (int i = 0; i < size; i++)
            _material[i].color = _originalColors[i];

        if (_invincibility)
        {
            foreach (Material material in _material)
                material.color = _colors[index % 2];
            index++;
        }

=======
        _powerUp.AbsorbedBy(this.gameObject);
>>>>>>> master
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
                
                //Main.SINGLETON.DelayedRestart(gameRestartDelay);//Tell Main.S to restart the game after a delay
            }
        }
    }
}
