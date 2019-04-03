using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PowerUpType
{
    none, //no powerup
    doublePoints, //doubles points
    nuke, //nuke which will destroy all enemies on screen
    slowTime, //slows time 
    invincibility //gives invincibility to user
}

public class Main : MonoBehaviour
{
   

    static public Main SINGLETON;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT; //dictionary of weapon types and their descriptions
    static Dictionary<PowerUpType, PowerUpDefinition> POWERUP_DICT; //Dictionary of power up types and their descriptions

    [Header("Set in Inspector")]
    //weaponDefinitions variables
    public GameObject[] prefabEnemies;
    static public float enemySpawnPerSecond = 0.5f; //# of enemies per second
    public float enemyDefaultPadding = 1.5f;
    public WeaponDefinition[] weaponDefinitions;
    public PowerUpDefinition[] powerUpDefinitions;
    public GameObject prefabPowerUp;



    //Array with all power ups that can drop
    public PowerUpType[] powerUpFrequency = new PowerUpType[]
    {
        PowerUpType.doublePoints,
        PowerUpType.invincibility
        //,PowerUpType.slowTime,
        //PowerUpType.nuke
     };


    private BoundsCheck _bndCheck;
    public void shipDestroyed(Enemy e)
    {
       if(Random.value<= e.powerUpDropChance) 
        {
            //Choose which powerup to pick from
            //Get a random number to be able to pick from the possibilites in powerUpFrequency
            int _randomIndex = Random.Range(0,powerUpFrequency.Length);

            PowerUpType powerUpType = powerUpFrequency[_randomIndex];

            GameObject go = Instantiate (prefabPowerUp) as GameObject;

            PowerUp _powerUp = go.GetComponent<PowerUp>();

            _powerUp.SetType(powerUpType);

            //Set it to the position of the destroyed enemy
            _powerUp.transform.position = e.transform.position;

        }

}
    void Awake()
    {
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

        //Creating a dictionary with poweruptype as the key
        POWERUP_DICT = new Dictionary<PowerUpType, PowerUpDefinition>();
        foreach (PowerUpDefinition def in powerUpDefinitions)
            POWERUP_DICT[def.powerUpType] = def;
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

        //Reset enemyspawnpersecond
        Main.enemySpawnPerSecond = 0.5f;
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

    static public PowerUpDefinition GetPowerUpDefinition(PowerUpType pt)//pt is powerUpType
    {
        //ensuring the key being retrieved exists to avoid error
        if (POWERUP_DICT.ContainsKey(pt))
        {
            return (POWERUP_DICT[pt]);
        }
        //failed to find the right weapon, return PowerUpType.none
        return (new PowerUpDefinition());
    }
    static public void SpawnFaster() //makes enemies spawn faster when level changes
    {
        Main.enemySpawnPerSecond += .5f;
    }
}