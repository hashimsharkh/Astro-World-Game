using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]
    static public float SPEED = 10f; //stores speed of enemies
    public float fireRate = 0.3f; //store rate of fire
    public float health = 1f; //stores enemy health
    public int score = 50; //stores enemy points given
    public float showDamageDuration = 0.1f; //stores damage duration
    public float powerUpDropChance = 1f;//chance to drop a power-up

    private BoundsCheck _bndCheck; //check if object is off screen

   [Header("Set Dynamically: Enemy")]    
    public Color[] originalColors; //stores colours
    public Material[] materials; //stores materials
    // All the Materials of this & its children  
    public bool showingDamage = false; //stores if enemy shows damage or not
    public float damageDoneTime; // Time to stop showing damage   
    public bool notifiedOfDestruction = false; //stores if enemy showed damage

    void Awake()
    {
        _bndCheck = GetComponent<BoundsCheck>(); //check if object is off screen

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
        GameObject _otherObject = collision.gameObject;
       
        switch (_otherObject.tag)
        {
            //print("Collison between" + otherObject.name + " and " + name);
            case "ProjectileHero":
                Projectile projectile = _otherObject.GetComponent<Projectile>();
                if (!_bndCheck.isOnScreen) // if enemy is not in the screen, don't damage it
                {
                    Destroy(_otherObject);
                    break;
                }

                health -= Main.GetWeaponDefinition(projectile.weaponType).damageOnHit;
                health -= Main.GetWeaponDefinition(projectile.weaponType).continuousDamage;
                if (health <= 0)
                {
                    //Immediately before this enemy is destroyed , it notifies the Main singleton by calling ShipDestroyed()
                    //this only happens once for each enemy ship which is enforced by the nothifiedOfDestruction bool
                    if (!notifiedOfDestruction)
                        Main.SINGLETON.ShipDestroyed(this);

                    notifiedOfDestruction = true;

                    //change the score when the enemy is destroyed
                    ScoreCounter.UpdateScore(this.gameObject.name);
                    //destroy the object
                    Hero.SINGLETON.destroySource1.Play();

                    Destroy(this.gameObject);
                }
                Destroy(_otherObject);
                ShowDamage();//Hurt this enemy
                break;

            default:
                print("Enemy hit by non-ProjectileHero: " + _otherObject.name);
                break;
        }
    }

    //Destroy enemy if out of bounds
    void Update()
    {
        Move();
        //check damage and if the enemy should be blinking red
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
        Vector3 _tempPos = pos;
        _tempPos.y -= SPEED * Time.deltaTime;
        pos = _tempPos;
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
    static public void ChangeSpeed() //when level changes, makes enemies faster
    {
        Enemy.SPEED += 5f; //makes enemies 1.5x faster
    }
    static public void ResetSpeed() //reset enemies speed for level 1 on restart
    {
        Enemy.SPEED = 10f;
    }
}


