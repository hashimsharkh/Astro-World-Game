using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    static Dictionary<PowerUpType, PowerUpDefinition> POWERUP_DICT; //Dictionary of power up types and their description

    //Variables used to change color when invisible
    private Material[] _material;
    private Color[] _colors = new Color[] { Color.cyan, Color.black };
    private Color[] _originalColors;

    [Header("Set in Inspector")]
    //weaponDefinitions variables
    public GameObject[] prefabEnemies;
    public GameObject[] prefabHeros;

    static public float ENEMY_SPAWN_PER_SEC = 0.5f; //# of enemies per second
    public float enemyDefaultPadding = 1.5f; //enemy padding
    public WeaponDefinition[] weaponDefinitions; //store weapons
    public PowerUpDefinition[] powerUpDefinitions; //store power ups
    public GameObject prefabPowerUp; //store power up prefab
    private GameObject _powerUp, _powerUp1, _powerUp2, _powerUp3;//powerUp prefab icon
    private Vector3[] _positions = new Vector3[] { new Vector3(11, 1, 0), new Vector3(9.5f, 1, 0), new Vector3(8f, 1, 0), new Vector3(6.5f, 1, 0) };//positions of power up icons

    [SerializeField]
    public GameObject[] powerUpIcon;//powerup icons in an array

    [SerializeField]
    public GameObject Canvas;//Canvas on hierarchy
    public Text levelText; //display first level
    static public int SPAWN_UFO = 1;

    private Image _radialTimer, _radialTimer1, _radialTimer2;//to show progress of powerUp icon

    //Array with all power ups that can drop
    public PowerUpType[] powerUpFrequency = new PowerUpType[]
    {
        PowerUpType.doublePoints,
        PowerUpType.invincibility,
        PowerUpType.slowTime,
        PowerUpType.nuke
    };

    private BoundsCheck _bndCheck; //check if object is off the screen

    public void ShipDestroyed(Enemy e)
    {
        if (Random.value <= e.powerUpDropChance)
        {
            //Choose which powerup to pick from
            //Get a random number to be able to pick from the possibilites in powerUpFrequency
            int _randomIndex = Random.Range(0, powerUpFrequency.Length);

            PowerUpType powerUpType = powerUpFrequency[_randomIndex]; //get power up randomly

            GameObject go = Instantiate(prefabPowerUp) as GameObject;

            PowerUp _powerUp = go.GetComponent<PowerUp>();

            _powerUp.SetType(powerUpType);

            //Set it to the position of the destroyed enemy
            _powerUp.transform.position = e.transform.position;

        }

    }

    void Awake()
    {
        SINGLETON = this;
        //spawn in the selected hero
        SpawnHero();

        //calling spawn enemy after rocket is created
        SINGLETON = this;
        _bndCheck = GetComponent<BoundsCheck>();
        Invoke("DisableText", 3f); //disable text after 3 seconds
        Invoke("SpawnEnemy", 1.5f / ENEMY_SPAWN_PER_SEC); //begin spawning enemies
        //creating a dictionary with WeaponType as the key
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }


        //Creating a dictionary with poweruptype as the key
        POWERUP_DICT = new Dictionary<PowerUpType, PowerUpDefinition>();
        foreach (PowerUpDefinition def in powerUpDefinitions)
            POWERUP_DICT[def.powerUpType] = def;
    }

    void DisableText()
    {
        levelText.enabled = false; //disable level text
    }


    public void SpawnDoublePointIcon()
    {
        //Spawn power up icons 
        if (Hero.ShouldSpawnDoublePoints())
        {
            _powerUp = Instantiate(powerUpIcon[0], Camera.main.ViewportToWorldPoint(_positions[0]), Quaternion.identity) as GameObject;
            _powerUp.transform.SetParent(Canvas.transform);
            _radialTimer = GameObject.Find("RadialTimer").GetComponent<Image>();


            Hero.SetDoublePoints(false);
            Hero.DOUBLE_POINTS_ACTIVE = true;
        }



    }

    public void SpawnInvincibilityIcon()
    {
        //spawn invincibility icons
        if (Hero.ShouldSpawnInvincibility())
        {
            _powerUp1 = Instantiate(powerUpIcon[1], Camera.main.ViewportToWorldPoint(_positions[1]), Quaternion.identity) as GameObject;
            _powerUp1.transform.SetParent(Canvas.transform);
            _radialTimer1 = GameObject.Find("RadialTimer1").GetComponent<Image>();


            Hero.SetInvincibility(false);
            Hero.INVINCIBILITY_ACTIVE = true;
        }
    }

    public void SpawnSlowDownIcon()
    {
        //spawn slow down icons
        if (Hero.ShouldSpawnSlowDown())
        {
            _powerUp2 = Instantiate(powerUpIcon[2], Camera.main.ViewportToWorldPoint(_positions[2]), Quaternion.identity) as GameObject;
            _powerUp2.transform.SetParent(Canvas.transform);
            _radialTimer2 = GameObject.Find("RadialTimer2").GetComponent<Image>();


            Hero.SetSlowDown(false);
            Hero.SLOW_DOWN_ACTIVE = true;
        }
    }
    public void SpawnNukeIcon()
    {
        //spawn nuke icon
        if (Hero.ShouldSpawnNuke())
        {
            _powerUp3 = Instantiate(powerUpIcon[3], Camera.main.ViewportToWorldPoint(_positions[3]), Quaternion.identity) as GameObject;
            _powerUp3.transform.SetParent(Canvas.transform);

            Hero.SetNuke(false);
            Hero.NUKE_ACTIVE = true;
        }
    }
    void Update()
    {
        //Double point power up icon radial timer
        if (Hero.DOUBLE_POINTS_ACTIVE && Hero.ShouldSpawnDoublePoints())
        {
            SpawnDoublePointIcon();

        }
        if (Hero.DOUBLE_POINTS_ACTIVE)
        {
            _radialTimer.fillAmount = 1f;
            Hero.DOUBLE_POINTS_ACTIVE = false;
        }
        if (_radialTimer != null)
        {
            //Reduce fill amount over 30 seconds   
            _radialTimer.fillAmount -= 1.0f / 7f * Time.deltaTime;

        }
        if ((_radialTimer != null) && _radialTimer.fillAmount <= 0f)
        {
            Destroy(_powerUp);//Destroy 
        }


        //Invincibility power up icon
        if (Hero.INVINCIBILITY_ACTIVE && Hero.ShouldSpawnInvincibility())
        {
            SpawnInvincibilityIcon();

        }
        if (Hero.INVINCIBILITY_ACTIVE)
        {
            _radialTimer1.fillAmount = 1f;
            Hero.INVINCIBILITY_ACTIVE = false;
        }

        if (_radialTimer1 != null)
        {
            //Reduce fill amount over 30 seconds   
            _radialTimer1.fillAmount -= 1.0f / 7f * Time.deltaTime;

        }
        if ((_radialTimer1 != null) && _radialTimer1.fillAmount <= 0f)
        {
            Destroy(_powerUp1);//Destroy 
        }


        //Slowing down radial timer and power up icon spawn

        if (Hero.SLOW_DOWN_ACTIVE && Hero.ShouldSpawnSlowDown())
        {
            SpawnSlowDownIcon();

        }
        if (Hero.SLOW_DOWN_ACTIVE)
        {
            _radialTimer2.fillAmount = 1f;
            Hero.SLOW_DOWN_ACTIVE = false;
        }

        if (_radialTimer2 != null)
        {
            //Reduce fill amount over 30 seconds   
            _radialTimer2.fillAmount -= 1.0f / 7f * Time.deltaTime;

        }
        if ((_radialTimer2 != null) && _radialTimer2.fillAmount <= 0f)
        {
            Destroy(_powerUp2);//Destroy 
        }

        //Spawn nuke powerup
        if (Hero.NUKE_ACTIVE && Hero.ShouldSpawnNuke())
        {
            SpawnNukeIcon();
        }
        if (_powerUp3 != null && !Hero.NUKE)
            Destroy(_powerUp3);
    }
    public void SpawnHero()
    {
        GameObject _hero = Instantiate<GameObject>(prefabHeros[MainMenu.CHOSEN_HERO]);
        //spawn a hero if not default
    }

    public void SpawnEnemy()
    {
        //instatiate random enemy type
        int _index;
        if (SPAWN_UFO < 3) { _index = Random.Range(0, prefabEnemies.Length - 1); } //if less than level 3, dont spawn UFOs
        else { _index = Random.Range(0, prefabEnemies.Length); } //spawn UFOs if level is at least 3
        GameObject _enemy = Instantiate<GameObject>(prefabEnemies[_index]); //spawn the enemy

        //bounds check
        float _enemyPadding = enemyDefaultPadding;
        if (_enemy.GetComponent<BoundsCheck>() != null)
            _enemyPadding = Mathf.Abs(_enemy.GetComponent<BoundsCheck>().radius);

        Vector3 _pos = Vector3.zero;
        float _xMin = -_bndCheck.camWidth + _enemyPadding;
        float _xMax = _bndCheck.camWidth - _enemyPadding;
        _pos.x = Random.Range(_xMin, _xMax);
        if (_index != 1) { _pos.y = _bndCheck.camHeight + _enemyPadding; }
        else { _pos.y = 400; }
        _enemy.transform.position = _pos;

        Invoke("SpawnEnemy", 1.5f / ENEMY_SPAWN_PER_SEC);
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
        //reset enemies for speed and spawning rate and UFO spawning
        Enemy.ResetSpeed();
        ENEMY_SPAWN_PER_SEC = .5f;
        SPAWN_UFO = 1; //reset count for level of the ufos
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
        Main.ENEMY_SPAWN_PER_SEC += .5f;
    }
}