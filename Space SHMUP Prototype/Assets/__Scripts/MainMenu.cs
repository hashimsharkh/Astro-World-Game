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
    private static int FIRST_TIME = 1; // determine whether game is being played for the first time
    public static bool MAIN_MENU_ACTIVE; //check if main menu is active

    void Awake()
    {
        //FIRST_TIME = 1;
    }
    void Start()
    {
        chosenHero = 0; //default ship is the first 
        Enemy.speed = 10f;
        instructionsText.enabled = false;
        MAIN_MENU_ACTIVE = true; //set the main menu as active
    }

    void Update()
    {
        RectTransform _rectTransform = background.GetComponent<RectTransform>();
        _rectTransform.Rotate(new Vector3(0, 0, 45) * _backgroundSpeed);
    }
    public void PlayGame() //button for starting the game
    {
        if (FIRST_TIME == 1)
        {
            instructionsText.enabled = true;
            Invoke("DisplayScene", 5f);
        }
        else
        {
            DisplayScene();
        }
        mainMenuUI.SetActive(false);
        
        //SceneManager.LoadScene("_Scene_0");
    }
    void DisplayScene()
    {
        SceneManager.LoadScene("_Scene_0");
        instructionsText.enabled = false;
        FIRST_TIME++;
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
