using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

[RequireComponent(typeof(PlayerController))]
public class Shield : MonoBehaviour {


    public float cooldown = 4f;
    public float active_time = 2f;
    public GamePad.Button shield_button = GamePad.Button.B;
    public KeyCode shield_test_button = KeyCode.C;
    public float time_to_desactivate = 1f;
    PlayerController player_controller;
    float current_cooldwon = 0f;
    float current_active_time = 0f;
    GameObject shield;
    bool shield_active = false;
    GamePad.Index player_num = GamePad.Index.One;

    void Start()
    {
        player_controller = GetComponent<PlayerController>();
        player_num = player_controller.player_num;

        foreach (Transform tr in transform)
        {
            if (tr.gameObject.CompareTag("Shield"))
            {
                shield = tr.gameObject;
            }
        }

        if (shield == null)
            Destroy(this);
        else
            shield.SetActive(false);

    }
    void OnCollisionEnter(Collision col)
    {
        if (shield_active)
        {
            if (col.gameObject.tag != "Player" && col.gameObject.tag != "Untagged" && col.gameObject.tag != "Floor" && col.gameObject.tag != "Shield")
            {
                Debug.Log(col.gameObject.tag);
                Invoke("DeactivateShield", time_to_desactivate);
            }
        }
    }
    void Update()
    {

        if (shield_active)
        {
            if (active_time <= current_active_time)
            {
                Invoke("DeactivateShield", time_to_desactivate);
            }
            else
            {
                current_active_time += Time.deltaTime;
                if (player_controller.current_M_state == Player_M_states.STUNED || player_controller.current_M_state == Player_M_states.SLEEPED)
                {
                    Invoke("DeactivateShield", time_to_desactivate);
                }
            }

        }
        else
        {
            if (cooldown <= current_cooldwon && player_controller.AbilitiesUp())
            {
                if (GamePad.GetButtonDown(shield_button, player_num) || Input.GetKeyDown(shield_test_button))
                {
                    ActivateShield();
                }
            }
            else
                current_cooldwon += Time.deltaTime;
        }
    }

    void ActivateShield()
    {
        shield.SetActive(true);
        shield_active = true;
        current_active_time = 0f;
    }

    void DeactivateShield()
    {
        shield.SetActive(false);
        shield_active = false;
        current_cooldwon = 0f;
    }
}
