using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    private Vector3 vec; //vector to control direction
    private float x0; //used to find side the enemy is on
    private bool mRight; //detirmes side for movement
    Rigidbody rb; //used to control movement
    void Start()
    {
        x0 = pos.x;
        //detirmine whether to move left or right
        if (x0 < 0)
        {
            mRight = true;
            vec = new Vector3(.2f, -.2f, 0); //the .2 controls the speed of the enemy
        }
        else
        {
            mRight = false;
            vec = new Vector3(-.2f, -.2f, 0);
        }
        rb = GetComponent<Rigidbody>();
    }
    public override void Move()
    {
        if (mRight == true)
        {
            transform.position = rb.position + vec; //moves the enemy along the vector specified
        }
        else
        {
            transform.position = rb.position + vec;
        }
    }


}
