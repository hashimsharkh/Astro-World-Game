﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enum of different weapon types and power-ups
public enum WeaponType
{
    none, //no weapon
    blaster, //simple blaster
    spread, //two simultaneous shots
    laser, //[NI] Damage over time
    shield, //adds shields
    destroyer // destorys all enemies on screen
}


//allows setting the weapons' properties in the Inspector

[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none; //initial weapon type
    public string letter; //letter to show up on power-up
    public Color color = Color.white; //coler of collar and power-up
    public GameObject projectilePrefab; //prefab for projectiles
    public Color projectileColor = Color.white; //color for prefab
    public float damageOnHit = 0; //amount of damage caused
    public float continuousDamage = 0;//Damage per second
    public float delayBetweenShots = 0; //delay
    public float velocity = 20; //speed of projectiles
}

public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR; //transform the weapon

    [Header("Set In Inspector")]
    [SerializeField]
    public GameObject explosionPrefab;//prefab for explosion effect;
    
    [Header("Set Dynamically")]
    [SerializeField]
    private WeaponType _weaponType = WeaponType.none;
    public WeaponDefinition def; //weapon definition
    public GameObject collar; //stores weapon collar
    public float lastShotTime; //time last shot was fired
    private Renderer _collarRend; //stores collar renderer
    private int _weaponChange = 0; //default weapon is spread
    public AudioSource explosionSound;


    WeaponType currentWeapon; //keeps track of which weapon is currently in use

    void Start()
    {
        //find collar and renderer
        collar = transform.Find("Collar").gameObject;
        _collarRend = collar.GetComponent<Renderer>();
        SetWeaponType(_weaponType); //call for deafault _weaponType WeaponType.none

        if (PROJECTILE_ANCHOR == null){
            GameObject _gameobject = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = _gameobject.transform;
        }
        //attach to game object
        GameObject _rootGameObject = transform.root.gameObject;
        if (_rootGameObject.GetComponent<Hero>() != null)
        {
            _rootGameObject.GetComponent<Hero>().fireDelegate += Fire;
        }
    }


    void Update()
    {
        //If nuke powerup is activated user has the chance to press space bar to destroy everything around him/her to save himself/herself
        //Then resets to previous weapon being used
        if(Hero.NUKE)
        {
            SetWeaponType(WeaponType.destroyer);
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Hero.NUKE = false;
                //Instantiating a big explosion effect
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                
                explosionSound.Play();
                Destroy(explosion, 4f);

                SetWeaponType(currentWeapon);//Resets weapon to previous weapon used
            }
        }

        if (Input.GetKeyDown(KeyCode.X)) // switch weapon if x is pressed
        {
            _weaponChange++;
            if (_weaponChange % 3 == 0) {
                SetWeaponType(WeaponType.spread); // if there is no remainder when divided by 3,spread is active
                currentWeapon = WeaponType.spread; //keep track that the current weapon is spread
            }
            else if (_weaponChange % 3 == 1 || (_weaponChange % 3 == 2 && LevelProgression.GetCurLevel() < 3))
            {
                SetWeaponType(WeaponType.blaster); // if the remainder when divided by 3 is 2 then use blaster
                currentWeapon = WeaponType.blaster; // keep track that the current weapon is blaster
            }
            else
            {
                _weaponChange = -1;
                SetWeaponType(WeaponType.laser);//Otherwise use blaster if it is above level 10
                currentWeapon = WeaponType.laser; //keep track that the current weapon is laser
            }
        }

    }
    //property to set and get private _weaponType variable
    public WeaponType weaponType
    {
        get
        {
            return (_weaponType);
        }
        set
        {
            SetWeaponType(value);
        }
    }

    public void SetWeaponType(WeaponType wType)
    {
        //sets value of _weaponType to wType
        _weaponType = wType;
        if(weaponType == WeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        def = Main.GetWeaponDefinition(_weaponType);
        _collarRend.material.color = def.color;
        lastShotTime = 0;
    }

    public void Fire()
    {
        if (!gameObject.activeInHierarchy) return; //if this.gameObject is inactive, return
        if(Time.time - lastShotTime < def.delayBetweenShots) //if it hasn't been enough time between shots, return
        {
            return;
        }
        Projectile _projectile;
        Vector3 _velocity = Vector3.up * def.velocity;
        if (transform.up.y < 0)
        {
            _velocity.y = -_velocity.y;
        }

        switch (weaponType)
        {
            case WeaponType.blaster:
                _projectile = MakeProjectile(); //make middle projectile
                _projectile.rigidBody.velocity = _velocity;
                _projectile = MakeProjectile(); //make right projectile
                _projectile.transform.rotation = Quaternion.AngleAxis(30, Vector3.back);
                _projectile.rigidBody.velocity = _projectile.transform.rotation * _velocity;
                _projectile = MakeProjectile(); //make left projectile
                _projectile.transform.rotation = Quaternion.AngleAxis(-30, Vector3.back);
                _projectile.rigidBody.velocity = _projectile.transform.rotation * _velocity;
                Hero.SINGLETON.shootingSource1.Play();
                break;

            case WeaponType.spread: //spread weapon properties
                _projectile = MakeProjectile();
                _projectile.rigidBody.velocity = _velocity;
                Hero.SINGLETON.shootingSource2.Play();
                break;
            case WeaponType.laser: //laser weapon properties
                _projectile = MakeProjectile();
                _projectile.rigidBody.velocity = _velocity;
                Hero.SINGLETON.shootingSource3.Play();
                break;
            case WeaponType.destroyer: //destroyer weapon properties
                DestroyAllEnemies();
                break;

        }
    }

    public Projectile MakeProjectile()
    {

        GameObject _gameobject = Instantiate<GameObject>(def.projectilePrefab);

        if (transform.parent.gameObject.tag == "Hero")
        {
            _gameobject.tag = "ProjectileHero";
            _gameobject.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            _gameobject.tag = "ProjectileEnemy";
            _gameobject.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        _gameobject.transform.position = collar.transform.position;
        _gameobject.transform.SetParent(PROJECTILE_ANCHOR, true);

        Projectile _projectile = _gameobject.GetComponent<Projectile>();
        _projectile.weaponType = weaponType;
        lastShotTime = Time.time;
        return (_projectile);
    }

    // function to destroy all enemies on screen
    public void DestroyAllEnemies()
    {
        // array that contains all enemies
        GameObject[] enemyArray;

        // finding all gameObjects that are enemies
        enemyArray = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in enemyArray)
        {
            ScoreCounter.UpdateScore(enemy.name);
            Destroy(enemy);
        }
    }

}
