using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : Enemy
{
    public float waveFrequency = 1;
    public float waveWidth = 2;
    private float _initialPosition;
    private float _birthTime; //stores time of instatiation
    bool _moveRight = true;

    void Start()
    {
        _initialPosition = pos.x;

        _birthTime = Time.time;
    }

    public override void Move()
    {
       
    // move object in sine wave in y direction to give floating effect
    Vector3 _tempPos = pos;
        float _age = Time.time - _birthTime;
        float _theta = Mathf.PI * _age / waveFrequency;
        float _sin = Mathf.Sin(_theta);
        _tempPos.y = _initialPosition + waveWidth * _sin;
        pos = _tempPos;

        if (_moveRight)
        {
            // Spin object around Y-Axis counter-clockwise
            transform.Rotate(new Vector3(0, -30, 0) * Time.deltaTime);

            // move towards the right
            _tempPos.x += speed * Time.deltaTime;
            pos = _tempPos;
        }

        else
        {
            // Spin object around Y-Axis clock-wise
            transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);

            // move towards the left
            _tempPos.x -= speed * Time.deltaTime;
            pos = _tempPos;
        }

        if (_tempPos.x >= 24)
            {
                _moveRight = false;
            }
            
        if (_tempPos.x <= -25)
            {
                _moveRight = true;
            }

    }


}
