using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : Enemy
{
    public float waveFrequency = 1; //frequency of the wave path the ship will follow
    public float waveWidth = 2; // wdith of wave
    private float _initialPosition; //initial position of ship
    private float _birthTime; //stores time of instatiation
    bool _moveRight = true; //to help direct if the UFO is going left or right
    public GameObject bulletPrefab; //prefab that the UFO will drop
    public float timeBetweenBullets = 2f; // time between when the UFO will drop bullets

    void Start()
    {
        //get starting position
        _initialPosition = pos.x;
        //get time of instantiation of UFO
        _birthTime = Time.time;
        //drop bullet prefabs at the time interval
        Invoke("DropBullet", timeBetweenBullets);

    }

    //overriding move function in parent class Enemy
    public override void Move()
    {

        // move object in sine wave in y direction to give floating effect
        Vector3 _tempPos = pos;
        float _age = Time.time - _birthTime; //how long the UFO has been in the game for
        float _theta = Mathf.PI * _age / waveFrequency;
        float _sin = Mathf.Sin(_theta); //create the sine wave size
        _tempPos.y = _initialPosition + waveWidth * _sin; //set the UFO to move in a sine wave along the Y axis
        pos = _tempPos; //set the new position to the sine position

        //if the UFO is moving towards the right
        if (_moveRight)
        {
            // Spin object around Y-Axis counter-clockwise
            transform.Rotate(new Vector3(0, -30, 0) * Time.deltaTime);

            // move towards the right
            _tempPos.x += SPEED * Time.deltaTime;
            pos = _tempPos;
        }

        //if the UFO is moving towards the left
        else
        {
            // Spin object around Y-Axis clock-wise
            transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);

            // move towards the left
            _tempPos.x -= SPEED * Time.deltaTime;
            pos = _tempPos;
        }

        // if hit right bound, move left
        if (_tempPos.x >= 24)
            {
                _moveRight = false;
            }

        // if hit left bound, move right
        if (_tempPos.x <= -25)
            {
                _moveRight = true;
            }
    }

    //function to drop bullets from UFO
    void DropBullet()
    {
        //instantiate bullet prefabs
        GameObject bullet = Instantiate<GameObject>(bulletPrefab);
        //have the bullet prefab come from the UFO
        bullet.transform.position = transform.position;
        //invoke the function on a time interval
        Invoke("DropBullet", timeBetweenBullets);
    }


}
