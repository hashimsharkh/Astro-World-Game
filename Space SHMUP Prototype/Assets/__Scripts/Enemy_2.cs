using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
 
public class Enemy_2 : Enemy
{ 
    [Header("Set in Inspector: Enemy_2")]
    
    public float waveFrequency = 2;
   
    public float waveWidth = 4;
    public float waveRotY = 45;
    private float _initialPosition;
     
    private float _birthTime;
    
    void Start()
    {     
        // assign intial position and time of object
        _initialPosition = pos.x;

        _birthTime = Time.time;
    }                                        

    public override void Move()
    { 
        // move object in sine wave in x direction
        Vector3 _tempPos = pos;
    
        float _age = Time.time - _birthTime;
        float _theta = Mathf.PI * 2 * _age / waveFrequency;
        float _sin = Mathf.Sin(_theta);
        _tempPos.x = _initialPosition + waveWidth* _sin;
        pos = _tempPos;  
     
        Vector3 _rotation = new Vector3(0, _sin * waveRotY, 0);     
        this.transform.rotation = Quaternion.Euler(_rotation);  
   
        // move object in sine wave in y direction
        base.Move();
    }
}


