using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : MonoBehaviour
{
    float speed=.2f;
    int direction;
    float timeCount=0;
    // Start is called before the first frame update
    void Start()
    {
        direction = Random.Range(0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;
        if (direction == 0)
        {
            float x=(float)Mathf.Sin(timeCount);
            float y=-.5f;
            float z=0;
            Vector3 vec = new Vector3(x, y, z);
            transform.position += vec * speed;
        }
        else if (direction == 1)
        {
            float x = (float)Mathf.Sin(timeCount);
            float y = -.5f;
            float z = 0;
            Vector3 vec = new Vector3(-x,y,z);
            transform.position += vec * speed;
        }
        transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);
    }
}
