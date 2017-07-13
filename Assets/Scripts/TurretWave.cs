using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretWave : MonoBehaviour {

    public float cooldown = 4f;
    public float active_time = 2f;
    public int force = 15;

    float current_cooldwon = 0f;
    float current_active_time = 0f;

    bool furi_active = false;
    Collider col;
    SpriteRenderer sprite;
    
    void Start()
    {
        col = GetComponent<Collider>();
        sprite = GetComponent<SpriteRenderer>();
        DeactivateFuri();
    }

    void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.CompareTag("Player"))
        {
            Vector3 direction = c.transform.position;
            direction.y = 0;
            direction -= transform.position;
            c.gameObject.GetComponent<PlayerController>().PushMe(direction, force);
        }
    }

    void Update()
    {
        if (furi_active)
        {
            if (active_time <= current_active_time)
            {
                DeactivateFuri();
            }
            else
            {
                current_active_time += Time.deltaTime;
            }

        }
        else
        {
            if (cooldown <= current_cooldwon)
            {
               ActivateFuri();
            }
            else
                current_cooldwon += Time.deltaTime;
        }

    }

    void ActivateFuri()
    {
        col.enabled = true;
        sprite.enabled = true;
        furi_active = true;
        current_active_time = 0f;
    }

    void DeactivateFuri()
    {
        col.enabled = false;
        sprite.enabled = false;
        furi_active = false;
        current_cooldwon = 0f;
    }
}
