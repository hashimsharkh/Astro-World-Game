using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    static public int chosenHero; //chosen hero from options menu
    public Dropdown dropdown; //options of heros

    void Start()
    {
        chosenHero = 0; //default ship is the first 
        Enemy.speed = 10f;
    }
    public void PlayGame() //button for starting the game
    {
        SceneManager.LoadScene("_Scene_0");
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
