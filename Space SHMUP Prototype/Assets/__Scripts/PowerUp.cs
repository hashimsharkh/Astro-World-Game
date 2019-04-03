using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUpDefinition
{
    public PowerUpType powerUpType = PowerUpType.none;
    public string letter; //letter to show up on power-up
    public Color color = Color.white; //color of  power-up
    public Color letterColor = Color.white; //Color of letter on power-up
}
public class PowerUp : MonoBehaviour
{

    [Header("Set in Inspector")]
    //Vector2s.x holds a min value and y a max for a Random.Range() that will be called later
    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(.25f, 2);
    public float lifeTime = 6f;//Seconds the powerup exists
    public float fadeTime = 4f; //Seconds it will then fade
    public float duration = 3f;//Duration before powerup effect is gone
    public GameObject pickUpEffect;//Pickup effect used after picking up powerup
    public static int multiplier=1;//Used for points multiplier

    [Header("Set Dynamically")]
    public PowerUpType powerUpType; //The type of the powerup
    public WeaponType type;
    public GameObject cube; //Reference to the Cube child

    public TextMesh letter; //Reference to the text mesh

    public Vector3 rotPerSecond; //Euler rotation speed
    public float birthTime; //time of birth

    private Rigidbody _rigid;
    private BoundsCheck _bndCheck;
    private Renderer _cubeRend;

    //Getter functions
    public Rigidbody getRigid()
    {
        return _rigid;
    }
    void Awake()
    {
        //Find the cube reference
        cube = transform.Find("Cube").gameObject;
        //Find the textmesh and other components
        letter = GetComponent<TextMesh>();
        _rigid = GetComponent<Rigidbody>();
        _bndCheck = GetComponent<BoundsCheck>();
        _cubeRend = cube.GetComponent<Renderer>();
        
        //Set a random velocity
        Vector3 vel = Random.onUnitSphere; //get Random XYZ velocity
        //Random.onUnisSphere gives you a vector point that is somewhere on 
        //The surface of the sphere with a radius of 1m around the origin
        vel.z = 0; //Flatten the vel to the XY plane
        vel.Normalize(); //Normalizing a Vector3 makes it length 1m

        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        _rigid.velocity = vel;

        //Set the rotation of this GameObject to R:[0,0,0]
        transform.rotation = Quaternion.identity;

        //Quaternion.identity is equal to no rotation

        //Set up the rotPerSecond for the cube child using rotMinMax x &y
        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y));


        birthTime = Time.time;//Time.time is the birth of the powerup
            
     }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

        //Fade out the powerup over time
        //Given the default values, a PowerUp will exist for 10 seconds
        //And fades out over 4 seconds
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
        //For lifeTime seconds, u will be <=0. Then it will transition to 
        //1 over the course of fadeTime seconds
        //if u>=1 destroy this powerup
        if(u>=1)
        {
            Destroy(this.gameObject);
            return;
        }

        //use u to determine the alpha value of the Cube and letter
        if(u>0)
        {
            Color c = _cubeRend.material.color;
            c.a = 1f - u;

            //Fade the letter too just not as much
            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }

        if(!_bndCheck.isOnScreen)
        {
            //If the powerup has drifted entirely off the screen,destroy it
            Destroy(gameObject);
        }
    }
    
    public void SetType(PowerUpType powerUpType)
    {
        //Grab the WeaponDefinition from main
        PowerUpDefinition def = Main.GetPowerUpDefinition(powerUpType);
        //Set the color of the Cube Child 
        _cubeRend.material.color = def.color;

        letter.color = def.letterColor; //We could colarize the letter too
        letter.text = def.letter;//Set the letter that is shown
        this.powerUpType = powerUpType;//Finally actually set the type
    
    }

    public void AbsorbedBy (GameObject target)
    {
        //This function is called by the Hero Class when a powerup is collected
        Destroy(this.gameObject);
    }
}
