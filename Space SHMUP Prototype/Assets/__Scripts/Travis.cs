using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Travis : MonoBehaviour
{
    public Text sickText;
    public void SickoM()
    {
        SetMusicVolume.SickoM();
        sickText.enabled = true;
        sickText.gameObject.SetActive(true);
    }
}
