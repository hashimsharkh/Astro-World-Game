using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    private Vector3 _vec; //vector to control direction
    private float _initialPos; //used to find side the enemy is on
    private bool _moveRight; //determines side for movement
    private Rigidbody _rigidBody; //used to control movement

    void Start()
    {
        _initialPos = pos.x;

        //determine whether to move left or right
        if (_initialPos < 0)
        {
            _moveRight = true;
            _vec = new Vector3(.2f, -.2f, 0); //the .2 controls the speed of the enemy
        }
        else
        {
            _moveRight = false;
            _vec = new Vector3(-.2f, -.2f, 0);
        }
        _rigidBody = GetComponent<Rigidbody>();
    }
    public override void Move()
    {
        if (_moveRight == true)
        {
            transform.position = _rigidBody.position + _vec; //moves the enemy along the vector specified
        }
        else
        {
            transform.position = _rigidBody.position + _vec;
        }
    }


}
