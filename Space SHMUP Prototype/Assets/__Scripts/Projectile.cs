using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck bndCheck;
    private Renderer _rend;

    [Header("Set Dynamically")]
    public Rigidbody rigidBody;
    [SerializeField]
    private WeaponType _weaponType;

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

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        _rend = GetComponent<Renderer>();
        rigidBody = GetComponent<RigidBody>();
    }

    void Update()
    {
        if (bndCheck.offUp)
        {
            Destroy(gameObject);
        }
    }

    public void SetWeaponType(WeaponType eType)
    {
        _weaponType = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_weaponType);
        _rend.material.color = def.projectileColor;
    }
}
