using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck _bndCheck; //check if object is out of bounds of screen 
    private Renderer _rend; //renderer

    [Header("Set Dynamically")]
    public Rigidbody rigidBody; //rigid body object

    [SerializeField]
    private WeaponType _weaponType; //store weapon type

    //This public property masks the field _weaponType and takes action when it is set 
    public WeaponType weaponType
    {
        //allows get and set for the private field _weaponType
        get
        {
            return (_weaponType);
        }
        set
        {
            SetWeaponType(value);
        }
    }

    void Awake()
    {
        _bndCheck = GetComponent<BoundsCheck>(); // check bounds
        _rend = GetComponent<Renderer>(); //configure renderer
        rigidBody = GetComponent<Rigidbody>(); //configure rigidbody
    }

    void Update()
    {
        if (_bndCheck.offUp || _bndCheck.offLeft || _bndCheck.offRight) // if projectile is out of bounds, destroy object
        {
            Destroy(gameObject);
        }
    }

    //used to set weapon type and colours of projectile
    public void SetWeaponType(WeaponType eType)
    {
        _weaponType = eType; // set weapon type
        WeaponDefinition def = Main.GetWeaponDefinition(_weaponType);
        _rend.material.color = def.projectileColor;
    }
}
