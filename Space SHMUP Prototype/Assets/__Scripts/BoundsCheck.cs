using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float radius = 4f; //determines radius of object
    public bool keepOnScreen = true; //boolean that determines whether object needs to stay on screen

    [Header("Set Dynamically")]
    public bool isOnScreen = true; //determine if object is on screen
    public float camWidth; //stores camera width
    public float camHeight; //store camera height

    [HideInInspector]
    //determines if object is off the screen in the right, left, up, or down directions
    public bool offRight, offLeft, offUp, offDown; 
   

    void Awake()
    {
        //Sets up camera view 
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    void LateUpdate()
    {
        //Transforms the rocket and checking bounds 
        Vector3 _pos = transform.position;
        isOnScreen = true;
        offRight = offLeft = offUp = offDown = false;

        if (_pos.x > camWidth - radius)
        {
            //object is off the right side
            _pos.x = camWidth - radius;
            offRight = true;
        }

        if (_pos.x < -camWidth + radius)
        {
            //object is off the left side
            _pos.x = -camWidth + radius;
            offLeft = true;
        }

        if (_pos.y > camHeight - radius)
        {
            //object is off the top
            _pos.y = camHeight - radius;
            offUp = true;
        }

        if (_pos.y < -camHeight + radius)
        {
            //object is off the bottom
            _pos.y = -camHeight + radius;
            offDown = true;
        }

        //determine if object is off the screen in any direction
        isOnScreen = !(offRight || offLeft || offUp || offDown);

        if (keepOnScreen && !isOnScreen)
        {
            // return object to screen position
            transform.position = _pos;
            isOnScreen = true;
            offRight = offLeft = offUp = offDown = false;
        }

        //make object original position
        transform.position = _pos;

    }

    void OnDrawGizmos()
    {
        //Draw bounds on game console
        if (!Application.isPlaying) return;
        Vector3 _boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, _boundSize);
    }
}
