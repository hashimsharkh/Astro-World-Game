using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
public class PowerUpDrawer : MonoBehaviour
{
    private static string powerUpIconPrefabPath = "_Prefabs/PowerUpIcon";//This is where the prefab is stored

    private static GameObject powerUpIconPanel;

    static Dictionary<PowerUp, PowerUpInfo> powerUps = new Dictionary<PowerUp, PowerUpInfo>();

    private static List<PowerUp> keys = new List<PowerUp>();
    
    // Start is called before the first frame update
    void Start()
    {
        //Find powerUp panel
        powerUpIconPanel = GameObject.Find("Canvas").transform.Find("PowerUpPanel").gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimers();
    }

    public static void AddIcon(PowerUp powerUp)
    {
       

        if(!effects.ContainsKey(e))
        {
            GameObject icon = ObjectPooler.GetPooledObject(powerUpIconPrefabPath);

            GameObject powerUpSprite = icon.transform.GetChild(0).GetChild(1).gameObject;

            powerUpSprite.GetComponent<Image>().sprite = powerUp.GetComponent<SpriteRenderer>().sprite;

            EffectInfo info = new EffectInfo(icon, e);

            icon.transform.SetParent(powerUpIconPanel.transform);

        }
        else
        {
            effects[e].maxDuration += e.length;
            effects[e].timeLeft += e.length;
        }

        keys = new List<Effect>(effects.Keys);

    }

    public static void RemoveIcon(Effect e)
    {
        effects[e].icon.SetActive(false); 
        effects.Remove(e);
    }

    private static void UpdateTimers()
    {
        bool changed = false;

        if(effects.Count>0)
        {
            foreach(Effect effect in keys)
            {
                EffectInfo effectInfo = effects[effect];

                if(effectInfo.timeLeft>0)
                {
                    effectInfo.timeLeft -= Time.deltaTime;
                }
                else
                {
                    changed = true;
                    RemoveIcon(effect);
                }

                if(effects.ContainsKey(effect))
                {
                    effectInfo.icon.transform.GetChild(0).GetComponent<Image>().fillAmount = effectInfo.timeLeft / effects[effect].maxDuration;
                }
            }
        }

        if(changed)
        {
            keys = new List<Effect>(effects.Keys);
        }
    }
}
*/