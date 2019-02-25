using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    private Vector3 _vec; //vector to control direction
    private float _x0; //used to find side the enemy is on
    private bool _mRight; //detirmes side for movement
    Rigidbody rb; //used to control movement
    void Start()
    {
        _x0 = pos.x;
        //detirmine whether to move left or right
        if (_x0 < 0)
        {
            _mRight = true;
            _vec = new Vector3(.2f, -.2f, 0); //the .2 controls the speed of the enemy
        }
        else
        {
            _mRight = false;
            _vec = new Vector3(-.2f, -.2f, 0);
        }
        rb = GetComponent<Rigidbody>();
    }
    public override void Move()
    {
        if (_mRight == true)
        {
            transform.position = rb.position + _vec; //moves the enemy along the vector specified
        }
        else
        {
            transform.position = rb.position + _vec;
        }
    }


}
