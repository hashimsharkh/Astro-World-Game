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

    private BoundsCheck _bndCheck;

    void Awake()
    {
        _bndCheck = GetComponent<BoundsCheck>();
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

}
