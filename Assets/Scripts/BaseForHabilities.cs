using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

[RequireComponent(typeof(PlayerController))]
public class BaseForHabilities : MonoBehaviour {

    public float cooldown = 4f;
    public float active_time = 2f;
    public GamePad.Button furi_button = GamePad.Button.B;
    public KeyCode furi_test_button = KeyCode.C;
    PlayerController player_controller;
    float current_cooldwon = 0f;
    float current_active_time = 0f;
    GameObject shield;
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

    void Update()
    {

        if (cooldown <= current_cooldwon && player_controller.AbilitiesUp())
        {
            if (GamePad.GetButtonDown(furi_button, player_num) || Input.GetKeyDown(furi_test_button))
            {
                //DO HABILITY STUFF
            }
        }
        else
            current_cooldwon += Time.deltaTime;
    }

}
