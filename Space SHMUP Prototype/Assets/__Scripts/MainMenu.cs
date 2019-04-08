using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    static public int CHOSEN_HERO; //chosen hero from options menu
    public Dropdown dropdown; //options of heros
    public Image background; //holds background image
    private float _backgroundSpeed = 0.001f; //controls speed of background
    public Text instructionsText; //show instructions
    public GameObject mainMenuUI; //store main menu options
    private static int _FIRST_TIME = 1; // determine whether game is being played for the first time
    public static bool MAIN_MENU_ACTIVE; //check if main menu is active

    void Start()
    {
        CHOSEN_HERO = 0; //default ship is the first 
        Enemy.SPEED = 10f; //restart enemy speed
        instructionsText.enabled = false; //dont know instructions
        MAIN_MENU_ACTIVE = true; //set the main menu as active
    }

    void Update()
    {
        //rotate stars background
        RectTransform _rectTransform = background.GetComponent<RectTransform>();
        _rectTransform.Rotate(new Vector3(0, 0, 45) * _backgroundSpeed);
    }
    public void PlayGame() //button for starting the game
    {
        //if it is the first time, show instructions
        if (_FIRST_TIME == 1)
        {
            instructionsText.enabled = true;
            //show scene after 5 seconds
            Invoke("DisplayScene", 5f);
        }
        else
        {
            //if its not the first time display the scene
            DisplayScene();
        }
        mainMenuUI.SetActive(false); //disable main menu
        
    }
    void DisplayScene()
    {
        //switch to main scene and disable menu options
        SceneManager.LoadScene("_Scene_0");
        instructionsText.enabled = false;
        _FIRST_TIME++;
        MAIN_MENU_ACTIVE = false; //main menu is not active
    }

    public void QuitGame() //quits the application
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void ChangeHero() //updates the chosen hero from drop down menu
    {
        CHOSEN_HERO = dropdown.value;
    }

}
