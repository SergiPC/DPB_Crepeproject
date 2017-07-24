using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BOOST_ZONES
{
    SPEED_UP,
    RECHARGE,
    SHIELD,
    LESS_COST
}


public class BoostZone : MonoBehaviour {

    public BOOST_ZONES type;
    public float effect = 0.2f;
    public float time_of_effect = 2f;
    public float boost_cd = 2f;
    bool active = true;
    
    float current_cd = 0f;

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            if(active)
            {
                switch(type)
                {
                    case BOOST_ZONES.SPEED_UP:
                        c.gameObject.GetComponent<PlayerController>().SpeedMe(effect, time_of_effect);
                        break;
                    case BOOST_ZONES.RECHARGE:
                        break;
                    case BOOST_ZONES.SHIELD:
                        break;
                    case BOOST_ZONES.LESS_COST:
                        break;
                }

                foreach(Renderer r in GetComponentsInChildren<Renderer>())
                {
                    r.enabled = false;
                }

                active = false;
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
	    if(!active)
        {
            if (boost_cd <= current_cd)
            {
                active = true;
                current_cd = 0f;
                foreach (Renderer r in GetComponentsInChildren<Renderer>())
                {
                    r.enabled = true;
                }
            }
            else
                current_cd += Time.deltaTime;
        }	
	}
}
