using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
 
public class Enemy_2 : Enemy
{ 
    [Header("Set in Inspector: Enemy_2")]
    
    public float waveFrequency = 2;
   
    public float waveWidth = 4;
    public float waveRotY = 45;
    private float _x0;
     
    private float _birthTime;
    
    void Start()
    {     
        _x0 = pos.x;

        _birthTime = Time.time;
    }                                        

    public override void Move()
    { 

        Vector3 tempPos = pos;
    
        float age = Time.time - _birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.x = _x0 + waveWidth* sin;
        pos = tempPos;  
     
        Vector3 rot = new Vector3(0, sin * waveRotY, 0);     
        this.transform.rotation = Quaternion.Euler(rot);  
   
        base.Move();
    }
}


