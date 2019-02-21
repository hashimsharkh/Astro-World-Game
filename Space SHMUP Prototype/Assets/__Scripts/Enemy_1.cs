using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : MonoBehaviour
{
    public float speed=.2f;
    int direction;
    // Start is called before the first frame update
    void Start()
    {
        direction = Random.Range(0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (direction == 0)
        {
            Vector3 vec = new Vector3(0.5f, -0.5f, 0);
            transform.position += vec * speed;
        }
        else if (direction == 1)
        {
            Vector3 vec = new Vector3(-0.5f, -0.5f, 0);
            transform.position += vec * speed;
        }
    }
}
