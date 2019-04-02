using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Parallax : MonoBehaviour
{
    // move stars background

    [Header("Set in Inspector")]
    public GameObject pointOfInterest;
    public GameObject[] panels;
    public float scrollSpeed = -30f;
    public float motionMult = 0.25f;
    private float _panelHeight;
    private float _depth;

    void Start()
    {
        //set stars background
        _panelHeight = panels[0].transform.localScale.y;
        _depth = panels[0].transform.position.z;

        panels[0].transform.position = new Vector3(0, 0, _depth);
        panels[1].transform.position = new Vector3(0, _panelHeight, _depth);
    }

    void Update()
    {
        //make image of stars in the background float
        float _tY, _tX = 0;
        _tY = Time.time * scrollSpeed % _panelHeight + (_panelHeight * 0.5f);
        if (pointOfInterest != null)
        {
            _tX = -pointOfInterest.transform.position.x * motionMult;
        }
        
        panels[0].transform.position = new Vector3(_tX, _tY, _depth);
        
        if (_tY >= 0)
        {
            panels[1].transform.position = new Vector3(_tX, _tY - _panelHeight, _depth);
        }
        else
        {
            panels[1].transform.position = new Vector3(_tX, _tY + _panelHeight, _depth);
        }
    }
}