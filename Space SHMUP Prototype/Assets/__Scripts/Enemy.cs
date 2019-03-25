using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 1f;
    public int score = 100;
    public float showDamageDuration = 0.1f;

    private BoundsCheck _bndCheck;

   [Header("Set Dynamically: Enemy")]    
    public Color[] originalColors;
    public Material[] materials;
    // All the Materials of this & its children  
    public bool showingDamage = false;
    public float damageDoneTime; // Time to stop showing damage   
    public bool notifiedOfDestruction = false;

    void Awake()
    {
        _bndCheck = GetComponent<BoundsCheck>();

        // Get materials and colors for this GameObject and its children        
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];   
        for (int i = 0; i < materials.Length; i++)
            originalColors[i] = materials[i].color;

    }


    //position property
    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherObject = collision.gameObject;
       
        switch (otherObject.tag)
        {
            //print("Collison between" + otherObject.name + " and " + name);
            case "ProjectileHero":
                Projectile projectile = otherObject.GetComponent<Projectile>();
                if (!_bndCheck.isOnScreen) // if enemy is not in the screen, don't damage it
                {
                    Destroy(otherObject);
                    break;
                }

                health -= Main.GetWeaponDefinition(projectile.weaponType).damageOnHit;
                if (health <= 0)
                {
                    //change the score when the enemy is destroyed
                    ScoreCounter sc = new ScoreCounter();
                    sc.UpdateScore(this.gameObject.name);
                    //destroy the object
                    
                    Destroy(this.gameObject);
                }
                Destroy(otherObject);
                ShowDamage();//Hurt this enemy
                break;

            default:
                print("Enemy hit by non-ProjectileHero: " + otherObject.name);
                break;
        }
    }

        //Destroy enemy if out of bounds
        void Update()
        {
            Move();
        if (showingDamage && Time.time > damageDoneTime)
            UnShowDamage();       
        
        if (_bndCheck != null && _bndCheck.offDown)
            {
                Destroy(gameObject);
            }
        }


    //Move the enemy
    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    //make the enemy blink red
    void ShowDamage()
    {    
        foreach (Material m in materials)
        {
            m.color = Color.red;
        }
        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;
    }
    //stop making the enemy turn red/return enemy to original colour
    void UnShowDamage()
    {
      
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingDamage = false;
    }
}


