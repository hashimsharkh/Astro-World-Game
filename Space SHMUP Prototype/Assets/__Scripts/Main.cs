using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main SINGLETON;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT; //dictionary of weapon types and their descriptions

    [Header("Set in Inspector")]
    //weaponDefinitions variables
    public GameObject[] prefabEnemies;
    public GameObject[] prefabHeros;
    static public float enemySpawnPerSecond = 0.5f; //# of enemies per second
    public float enemyDefaultPadding = 1.5f;
    public WeaponDefinition[] weaponDefinitions;

    private BoundsCheck _bndCheck;

    void Awake()
    {
        SpawnHero();
        //calling spawn enemy after rocket is created
        SINGLETON = this;
        _bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1.5f / enemySpawnPerSecond);
        //creating a dictionary with WeaponType as the key
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
    }

    public void SpawnHero()
    {
        GameObject _hero = Instantiate<GameObject>(prefabHeros[MainMenu.chosenHero]);
    }

    public void SpawnEnemy()
    {
        //instatiate random enemy type
        int _index = Random.Range(0, prefabEnemies.Length);
        GameObject _enemy = Instantiate<GameObject>(prefabEnemies[_index]);

        //bounds check
        float _enemyPadding = enemyDefaultPadding;
        if (_enemy.GetComponent<BoundsCheck>() != null)
            _enemyPadding = Mathf.Abs(_enemy.GetComponent<BoundsCheck>().radius);

        Vector3 _pos = Vector3.zero;
        float _xMin = -_bndCheck.camWidth + _enemyPadding;
        float _xMax = _bndCheck.camWidth - _enemyPadding;
       _pos.x = Random.Range(_xMin, _xMax);
        _pos.y = _bndCheck.camHeight + _enemyPadding;
        _enemy.transform.position = _pos;

        Invoke("SpawnEnemy", 1.5f / enemySpawnPerSecond);
    }

    public void DelayedRestart(float delay)
    {
        //Invoke the Restart() method in delay seconds
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        //reset the score when game is over
        ScoreCounter.ResetScore();
        //Reload _Scene_0 to restart the game
        SceneManager.LoadScene("_Scene_0");
    }

    static public WeaponDefinition GetWeaponDefinition(WeaponType weaponType)
    {
        //ensuring the key being retrieved exists to avoid error
        if (WEAP_DICT.ContainsKey(weaponType))
        {
            return (WEAP_DICT[weaponType]);
        }
        //failed to find the right weapon, return WeaponType.none
        return (new WeaponDefinition());
    }
    static public void SpawnFaster() //makes enemies spawn faster when level changes
    {
        Main.enemySpawnPerSecond += .5f;
    }
}