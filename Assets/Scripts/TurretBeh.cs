using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBeh : MonoBehaviour {

    public GameObject bullet;
    List<GameObject> players_inside;
    public float cooldown = 1f;
    public float bullet_life = 4.0f;
    public float bullet_force = 4.0f;
    float current_cooldown = 0f;

    void Awake()
    {
        players_inside = new List<GameObject>();
    }

	// Use this for initialization
	void OnTriggerEnter(Collider col)
    {
		if(col.gameObject.CompareTag("Player"))
        {
            
            if (!players_inside.Contains(col.gameObject))
                players_inside.Add(col.gameObject);
        }
	}

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            players_inside.Remove(col.gameObject);
        }
    }

    void Update ()
    {
        if (cooldown <= current_cooldown)
        {
            float min_distance = 9000000000000f;
            int current_target = -1;
            int i = 0;

            foreach (GameObject g in players_inside)
            {
                float dist_tmp = Vector3.Distance(transform.position,g.transform.position);
                if(dist_tmp < min_distance)
                {
                    min_distance = dist_tmp;
                    current_target = i;
                }
                i++;
            }

            if(current_target != -1)
            {
               Vector3 dir = players_inside[current_target].transform.position -  transform.position;
               dir.y = 0f;
               ShootBullet(dir.normalized);
               current_cooldown = 0.0f;
            }
            
        }
        else
            current_cooldown += Time.deltaTime;
    }

    void ShootBullet(Vector3 dir)
    { 
        GameObject bull_tmp = Instantiate(bullet, transform.position, Quaternion.identity);
        bull_tmp.GetComponent<Rigidbody>().AddForce(dir * bullet_force);
        Destroy(bull_tmp, bullet_life);
    }
}
