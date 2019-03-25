using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float radius = 4f;
    public bool keepOnScreen = true;

    [Header("Set Dynamically")]
    public bool isOnScreen = true;
    public float camWidth;
    public float camHeight;

    [HideInInspector]
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
            _pos.x = camWidth - radius;
            offRight = true;
        }

        if (_pos.x < -camWidth + radius)
        {
            _pos.x = -camWidth + radius;
            offLeft = true;
        }

        if (_pos.y > camHeight - radius)
        {
            _pos.y = camHeight - radius;
            offUp = true;
        }

        if (_pos.y < -camHeight + radius)
        {
            _pos.y = -camHeight + radius;
            offDown = true;
        }

        isOnScreen = !(offRight || offLeft || offUp || offDown);

        if (keepOnScreen && !isOnScreen)
        {
            transform.position = _pos;
            isOnScreen = true;
            offRight = offLeft = offUp = offDown = false;
        }


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
