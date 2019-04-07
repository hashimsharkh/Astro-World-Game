using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    static public int chosenHero; //chosen hero from options menu
    public Dropdown dropdown; //options of heros
    public Image background; //holds background image
    private float _backgroundSpeed = 0.001f; //controls speed of background
    public Text instructionsText;
    public GameObject mainMenuUI;
    

   void Start()
    {
        chosenHero = 0; //default ship is the first 
        Enemy.speed = 10f;
        instructionsText.enabled = false;
    }

    void Update()
    {
        RectTransform _rectTransform = background.GetComponent<RectTransform>();
        _rectTransform.Rotate(new Vector3(0, 0, 45) * _backgroundSpeed);
        
        
    }
    public void PlayGame() //button for starting the game
    {
        instructionsText.enabled = true;
        mainMenuUI.SetActive(false);
        Invoke("DisplayScene", 5f);
        //SceneManager.LoadScene("_Scene_0");
    }
    void DisplayScene()
    {
        SceneManager.LoadScene("_Scene_0");
        instructionsText.enabled = false;
    }

    public void QuitGame() //quits the application
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void ChangeHero() //updates the chosen hero from drop down menu
    {
        chosenHero = dropdown.value;
    }

}
