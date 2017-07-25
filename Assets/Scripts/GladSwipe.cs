using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

[RequireComponent(typeof(PlayerController))]
public class GladSwipe : MonoBehaviour {

    public float cooldown = 4f;
    public float degrees = 30f;
    public float range = 20f;
    public int push_force = 15;
    public LayerMask swipe_mask;
    public float swipe_speed = 2f;
    public float push_duration = 0.001f;
    public GamePad.Button swipe_button = GamePad.Button.B;
    public KeyCode swipe_test_button = KeyCode.C;
    PlayerController player_controller;
    float current_cooldwon = 0f;
    float current_active_time = 0f;
    GamePad.Index player_num = GamePad.Index.One;
    bool swipe = false;
    Vector3 current_dir = Vector3.right;
    Vector3 shoot_point = Vector3.zero;
    float angles_moved = 0f;
    void Start()
    {
        player_controller = GetComponent<PlayerController>();
        player_num = player_controller.player_num;
        shoot_point = player_controller.horizontal_shoot_point.transform.localPosition;
    }

    void Update()
    {
        if(swipe)
        {
            Swiping();
        }
        else
        {
            if (cooldown <= current_cooldwon && player_controller.AbilitiesUp())
            {
                if (GamePad.GetButtonDown(swipe_button, player_num) || Input.GetKeyDown(swipe_test_button))
                {
                    Vector2 inp = GamePad.GetAxis(GamePad.Axis.LeftStick, player_num, false);
                    float h = inp.x;
                    float v = inp.y;

                    Vector3 dir = new Vector3(Mathf.Abs(h), 0, Mathf.Abs(v));
                    Swipe(dir);
                }
            }
            else
                current_cooldwon += Time.deltaTime;
        }
    }

    void Swipe(Vector3 dir)
    {
        swipe = true;

        bool di_x = player_controller.GetDirection_x();
        bool di_z = player_controller.GetDirection_z();

        int x = di_x ? -1 : 1;
        int z = di_z ? -1 : 1;

        Vector3 shoot_tmp = shoot_point;
        if (dir.x < dir.z)
        {
            shoot_tmp.z = shoot_tmp.x;
            shoot_tmp.x = 0;
        }

        shoot_tmp.x *= x;
        shoot_tmp.z *= z;

        dir.x *= x;
        dir.z *= z;

        current_dir = shoot_tmp;

        Vector3 new_dir = current_dir;
        float cos = Mathf.Cos(x*degrees * Mathf.Deg2Rad * Time.deltaTime);
        float sin = Mathf.Sin(x*degrees * Mathf.Deg2Rad * Time.deltaTime);
        new_dir.x = current_dir.x * cos - current_dir.z * sin;
        new_dir.z = -current_dir.x * sin + current_dir.z * cos;
        current_dir = new_dir;
    }

    void Swiping()
    {
        Vector3 new_dir = current_dir;
        float cos = Mathf.Cos(swipe_speed * Mathf.Deg2Rad * Time.deltaTime);
        float sin = Mathf.Sin(swipe_speed * Mathf.Deg2Rad * Time.deltaTime );
        new_dir.x = current_dir.x * cos - current_dir.z * sin;
        new_dir.z = -current_dir.x * sin + current_dir.z * cos;
        current_dir = new_dir;
        angles_moved += (swipe_speed * Mathf.Deg2Rad * Time.deltaTime);

        if (angles_moved > (degrees * 2*Mathf.Deg2Rad))
        {
            swipe = false;
            angles_moved = 0f;
        }

        RaycastHit hit;
        Physics.Raycast(transform.position, current_dir.normalized, out hit, range, swipe_mask); // cast ray
        Debug.DrawRay(transform.position, current_dir, Color.black);

        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player")
            {
                Vector3 direction =  hit.collider.gameObject.transform.position;
                direction.y = 0;
                direction -= hit.point;
                hit.collider.gameObject.GetComponent<PlayerController>().PushMe(direction, push_force, push_duration);
            }
        }
    }
}

