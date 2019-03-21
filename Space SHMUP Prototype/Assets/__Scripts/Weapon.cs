using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enum of different weapon types and power-ups
public enum WeaponType
{
    none, //no weapon
    blaster, //simple blaster
    spread, //two simultaneous shots
    shield //adds shields
}

//allows setting the weapons' properties in the Inspector

    [System.Serializable]
    public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter; //letter to show up on power-up
    public Color color = Color.white; //coler of collar and power-up
    public GameObject projectilePrefab; //prefab for projectiles
    public Color projectileColor = Color.white; //color for prefab
    public float damageOnHit = 0; //amount of damage caused
    public float delayBetweenShots = 0; //delay
    public float velocity = 20; //speed of projectiles
}

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
