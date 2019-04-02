using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Set in inspector")]
    public float rotationsPerSecond = 0.1f;

    [Header("Set Dynamically")]
    public int levelShown = 0;

    //this non public variable will appear in the inspector and will no be accessed outside the shield class
    private Material _mat;

    // Start is called before the first frame update
    void Start()
    {
        //mat is defined as the material of the renderer component on this GameObject.
        _mat = GetComponent<Renderer>().material;

    }

    // Update is called once per frame
    void Update()
    {
        //Read the current shield level from the Hero Singleton
        int currLevel = Mathf.FloorToInt(Hero.SINGLETON.shieldLevel);
        //If this is different from the level shown
        if(levelShown !=currLevel)
        {
            levelShown = currLevel; //assign the level to the current level

            //Adjust the texture offset to show different shield level
            _mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);

        }

        //Rotate the shield a bit every frame in a time based way;
        float rZ = -(rotationsPerSecond * Time.time*360) % 360f;//rZ is rotation about z axis
        transform.rotation = Quaternion.Euler(0, 0, rZ);
    }

}
