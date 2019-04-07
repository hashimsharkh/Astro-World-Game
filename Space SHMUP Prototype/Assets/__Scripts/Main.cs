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
    private int index = 0;

    [Header("Set in Inspector")]
    //weaponDefinitions variables
    public GameObject[] prefabEnemies;
    public GameObject[] prefabHeros;
    
    static public float enemySpawnPerSecond = 0.5f; //# of enemies per second
    public float enemyDefaultPadding = 1.5f;
    public WeaponDefinition[] weaponDefinitions;
    public PowerUpDefinition[] powerUpDefinitions;
    public GameObject prefabPowerUp;
    private GameObject _powerUp;//powerUp icon
    private Vector3[] positions= new Vector3[]{ new Vector3( 11, 1, 0) };//positions of power up icons
    [SerializeField]
    public GameObject[] powerUpIcon;//powerup icons in an array
    [SerializeField]
    public GameObject Canvas;//Canvas on hierarchy
    public Text levelText;
    static public int spawnUFO=1;

   
    private Image _radialTimer;//to show progress of powerUp icon

    //Array with all power ups that can drop
    public PowerUpType[] powerUpFrequency = new PowerUpType[]
    {
        PowerUpType.doublePoints,
        PowerUpType.invincibility,
        PowerUpType.slowTime,
        PowerUpType.nuke
     };

    private BoundsCheck _bndCheck;

    public void shipDestroyed(Enemy e)
    {
        if (Random.value <= e.powerUpDropChance)
        {
            //Choose which powerup to pick from
            //Get a random number to be able to pick from the possibilites in powerUpFrequency
            int _randomIndex = Random.Range(0, powerUpFrequency.Length);

            PowerUpType powerUpType = powerUpFrequency[_randomIndex];

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
        Invoke("DisableText", 3f);
        Invoke("SpawnEnemy", 1.5f / enemySpawnPerSecond);
        //creating a dictionary with WeaponType as the key
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }

        //Gets components to help the ship flash to show effect of invincibility powerup
        //flashingShip();


        //Creating a dictionary with poweruptype as the key
        POWERUP_DICT = new Dictionary<PowerUpType, PowerUpDefinition>();
        foreach (PowerUpDefinition def in powerUpDefinitions)
            POWERUP_DICT[def.powerUpType] = def;
    }

    void DisableText()
    {
        levelText.enabled = false;
    }
    /*public void FixedUpdate()
    {
        //Make color of ship flash if invincibility powerup is picked up
        for (int i = 0; i < _material.Length; i++)
            _material[i].color = _originalColors[i];

        if (Hero.invincibility)
        {
            foreach (Material material in _material)
                material.color = _colors[index % 2];
            index++;
        }
    }*/

     public void SpawnPowerUpIcons()
    {
        if (Hero.shouldSpawnDoublePoints())
        {
            _powerUp = Instantiate(powerUpIcon[0], Camera.main.ViewportToWorldPoint(positions[0]), Quaternion.identity) as GameObject;
            _powerUp.transform.SetParent(Canvas.transform);
            _radialTimer = GameObject.Find("RadialTimer").GetComponent<Image>();


            Hero.SetDoublePoints(false);
            Hero.doublePointsActive = true;
        }

    }
    void Update()
    {
        if (Hero.doublePointsActive && Hero.shouldSpawnDoublePoints())
        {  SpawnPowerUpIcons();

        }
        if (Hero.doublePointsActive)
        {
            _radialTimer.fillAmount = 1f;
            Hero.doublePointsActive = false;
        }
        if (_radialTimer != null)
        {
            //Reduce fill amount over 30 seconds   
            _radialTimer.fillAmount -= 1.0f / 7f * Time.deltaTime;

        }
        if(_radialTimer.fillAmount<=0f)
        {
            Destroy(_powerUp);//Destroy 
        }
    }
    public void SpawnHero()
    {
        GameObject _hero = Instantiate<GameObject>(prefabHeros[MainMenu.chosenHero]);
    }

    public void SpawnEnemy()
    {
        //instatiate random enemy type
        int _index;
        if (spawnUFO < 3 ) { _index = Random.Range(0, prefabEnemies.Length-1); } //if less than level 3, dont spawn UFOs
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
        //reset enemies for speed and spawning rate and UFO spawning
        Enemy.ResetSpeed();
        enemySpawnPerSecond = .5f;
        spawnUFO = 1; //reset count for level of the ufos
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
        Main.enemySpawnPerSecond += .5f;
    }

    //flashingShip is a formula which will help in making the ship flash and will be called in awake as GetComponent
    //is an expensive method on our processor
   /* public void flashingShip()
    {
        //variables which will be used to change color of ship(game objects and materials
        GameObject _barrel;
        GameObject _collar;
        GameObject _cube;
        GameObject _wing;
        GameObject _cube1;
        GameObject _cube2;
        GameObject _quad;
        GameObject _quad1;
        GameObject _quad2;

        Material _barr;//barr is barrel
        Material _cub;//cub is cube
        Material _cub1;
        Material _cub2;
        Material _coll;//coll is collar
        Material _wings;//wings is wing
        Material _q;//q stands for quad
        Material _q1;
        Material _q2;




        if (MainMenu.chosenHero == 0)
        {
            //Find the child game objects of hero and their renderers
            _barrel = prefabHeros[0].transform.Find("Weapon/Barrel").gameObject;
            _collar = prefabHeros[0].transform.Find("Weapon/Collar").gameObject;
            _cube = prefabHeros[0].transform.Find("Cockpit/Cube").gameObject;
            _wing = prefabHeros[0].transform.Find("Wing").gameObject;
            //find materials of the children
            _barr = _barrel.GetComponent<Renderer>().material;
            _coll = _collar.GetComponent<Renderer>().material;
            _cub = _cube.GetComponent<Renderer>().material;
            _wings = _wing.GetComponent<Renderer>().material;

            //find the colors of these children so they are not changed
            _originalColors = new Color[] { _barr.color, _coll.color, _cub.color, _wings.color };

            //assign the materials to the _material array
            _material = new Material[] { _barr, _coll, _cub, _wings };
        }
        else if (MainMenu.chosenHero == 1)
        {
            //Find the child game objects of hero and their renderers
            _barrel = prefabHeros[1].transform.Find("Weapon/Barrel").gameObject;
            _collar = prefabHeros[1].transform.Find("Weapon/Collar").gameObject;
            _cube = prefabHeros[1].transform.Find("Cockpit/Cube").gameObject;
            _cube1 = prefabHeros[1].transform.Find("Cockpit/Cube(1)").gameObject;
            _cube2 = prefabHeros[1].transform.Find("Cockpit/Cube(2)").gameObject;
            _wing = prefabHeros[1].transform.Find("Wing").gameObject;

            _barr = _barrel.GetComponent<Renderer>().material;
            _coll = _collar.GetComponent<Renderer>().material;
            _cub = _cube.GetComponent<Renderer>().material;
            _cub1 = _cube1.GetComponent<Renderer>().material;
            _cub2 = _cube2.GetComponent<Renderer>().material;
            _wings = _wing.GetComponent<Renderer>().material;

            //find the colors of these children so they are not changed
            _originalColors = new Color[] { _barr.color, _coll.color, _cub.color, _cub1.color, _cub2.color, _wings.color };
            //assign the materials to the _material array
            _material = new Material[] { _barr, _coll, _cub, _cub1, _cub2, _wings };
        }
        else
        {
            //Find the child game objects of hero and their renderers
            _barrel = prefabHeros[2].transform.Find("Weapon/Barrel").gameObject;
            _collar = prefabHeros[2].transform.Find("Weapon/Collar").gameObject;
            _cube = prefabHeros[2].transform.Find("Cockpit/Cube").gameObject;
            _quad = prefabHeros[2].transform.Find("Wing/Quad").gameObject;
            _quad1 = prefabHeros[2].transform.Find("Wing/Quad(1)").gameObject;
            _quad2 = prefabHeros[2].transform.Find("Wing/Quad(2)").gameObject;

            _barr = _barrel.GetComponent<Renderer>().material;
            _coll = _collar.GetComponent<Renderer>().material;
            _cub = _cube.GetComponent<Renderer>().material;
            _q = _quad.GetComponent<Renderer>().material;
            _q1 = _quad1.GetComponent<Renderer>().material;
            _q2 = _quad2.GetComponent<Renderer>().material;

            //find the colors of these children so they are not changed
            _originalColors = new Color[] { _barr.color, _coll.color, _cub.color, _q.color, _q1.color, _q2.color };
            //assign the materials to the _material array
            _material = new Material[] { _barr, _coll, _cub, _q, _q1, _q2 };
        }
    }*/
}