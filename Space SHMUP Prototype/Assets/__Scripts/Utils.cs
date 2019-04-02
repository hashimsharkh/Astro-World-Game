using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    //function can be called be any other function because it is static
    static public Material[] GetAllMaterials(GameObject go)
    {
        Renderer[] _rends = go.GetComponentsInChildren<Renderer>(); //get component

        List<Material> _mats = new List<Material>(); //list that will collect the objects that need to be changed

        foreach (Renderer rend in _rends) //change the material of all the objects in the list
            _mats.Add(rend.material);

        return _mats.ToArray(); //convert list to an array and return    

    }
}
