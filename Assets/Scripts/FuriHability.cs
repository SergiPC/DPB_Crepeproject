using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class FuriHability : MonoBehaviour {

    public float cooldown = 4f;
    public float active_time = 2f;
    PlayerController player_controller;
    float current_cooldwon = 0f;
    float current_active_time = 0f;
    GameObject furi_object;
    string horizontal = "Horizontal";
    string vertical = "Vertical";
    string shoot_bullet = "Fire1";
    bool furi_active = false;
    void Start ()
    {
        player_controller = GetComponent<PlayerController>();

        horizontal = player_controller.GetHorizontal();
        vertical = player_controller.GetVertical();
        shoot_bullet = player_controller.GetShootButton();

        foreach (Transform tr in transform)
        {
            if(tr.gameObject.CompareTag("FuriHability"))
            {
                furi_object = tr.gameObject;
            }
        }

        if (furi_object == null)
            Destroy(this);
        else
            furi_object.SetActive(false);

    }
	
	void Update ()
    {
        if(furi_active)
        {
            if(active_time <= current_active_time)
            {
                DeactivateFuri();
            }
            else
            {
                current_active_time += Time.deltaTime;
                if(player_controller.current_M_state == Player_M_states.ROOTED)
                {
                    DeactivateFuri();
                }
            }

        }
        else
        {
                if (cooldown <= current_cooldwon && player_controller.current_M_state != Player_M_states.ROOTED)
                {
                    if (Input.GetButtonDown(shoot_bullet))
                    {
                        ActivateFuri();
                    }
                }
                else
                    current_cooldwon += Time.deltaTime;
        }
        
	}

    void ActivateFuri()
    {
        furi_object.SetActive(true);
        furi_active = true;
        current_active_time = 0f;
    }

    void DeactivateFuri()
    {
        furi_object.SetActive(false);
        furi_active = false;
        current_cooldwon = 0f;
    }
}
