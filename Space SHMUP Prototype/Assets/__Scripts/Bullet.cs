using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private BoundsCheck _bndCheck;

    void Awake()
    {
        _bndCheck = GetComponent<BoundsCheck>(); // check bounds
    }

    // Update is called once per frame
    void Update()
    {
        if (_bndCheck != null && _bndCheck.offDown) // if bullet is out of bounds, destroy object
        {
            Destroy(gameObject);
            print("destroyed");
        }

    }


}

