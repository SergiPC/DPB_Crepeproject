using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

[RequireComponent(typeof(PlayerController))]
public class SideShield : MonoBehaviour
{


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
    bool di_x = false;
    bool di_z = false;
    bool rotated = false;
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
                    Vector2 inp = GamePad.GetAxis(GamePad.Axis.LeftStick, player_num, false);
                    float h = inp.x;
                    float v = inp.y;

                    Vector3 dir = new Vector3(Mathf.Abs(h), 0, Mathf.Abs(v));

                    ActivateShield(dir);
                }
            }
            else
                current_cooldwon += Time.deltaTime;
        }
    }

    void ActivateShield(Vector3 dir)
    {
        di_x = player_controller.GetDirection_x();
        di_z = player_controller.GetDirection_z();

        int x = di_x ? -1 : 1;
        int z = di_z ? -1 : 1;

        Vector3 shoot_tmp = player_controller.horizontal_shoot_point.transform.localPosition;
        shoot_tmp.y = shield.transform.localPosition.y;
        if (dir.x < dir.z)
        {
            shield.transform.Rotate(Vector3.up, 90);
            rotated = true;
            shoot_tmp.z = shoot_tmp.x;
            shoot_tmp.x = 0;
        }

        shoot_tmp.x *= x;
        shoot_tmp.z *= z;

        dir.x *= x;
        dir.z *= z;

        shield.transform.localPosition = shoot_tmp;

        shield.SetActive(true);
        shield_active = true;
        current_active_time = 0f;
    }

    void DeactivateShield()
    {
        shield.SetActive(false);
        shield_active = false;
        current_cooldwon = 0f;
        if(rotated)
            shield.transform.Rotate(Vector3.up, 90);

        rotated = false;
    }
}