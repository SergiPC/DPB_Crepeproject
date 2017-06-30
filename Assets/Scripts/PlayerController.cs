﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using GamepadInput;

public enum Player_M_states
{
    NORMAL,
    DASHING,
    SLEEPED,
    PUSHED,
    ROOTED,
    STUNED
}

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    public GameObject horizontal_shoot_point;
    public GamePad.Index player_num = GamePad.Index.One;
    public int velocity = 4;

    public float dash_cooldown = 0.25f;
    public float dash_force = 4f;
    public float dash_time = 4f;
    public LayerMask dash_mask;
    public GamePad.Button dash_button = GamePad.Button.X;
    public KeyCode dash_test_button = KeyCode.Z;
    bool can_throw_abilities = true;
    Rigidbody body;

    private Vector3 shoot_point;
    Collider curr_col;
    bool di_x = false;
    bool di_z = false;
    string horizontal = "Horizontal";
    string vertical = "Vertical";
    string shoot_bullet = "Fire1";
    float slowliness = 1.0f;

    float dash_current_cooldown = 0.0f;
    float last_time_pushed = 0.0f;
    public Player_M_states current_M_state;

    //Root/stun timers
    float root_time = 0f;
    float sleep_time = 0f;
    float current_root_time = 0f;

    // Use this for initialization
    void Awake ()
    {
        body = GetComponent<Rigidbody>();
        curr_col = GetComponent<Collider>();
        shoot_point = horizontal_shoot_point.transform.localPosition;
	}

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Bullet"))
        {
            OnCollisonDestroy c = col.gameObject.GetComponent<OnCollisonDestroy>();
            slowliness = 1 - c.force;
            sleep_time = c.slow_time;
            current_M_state = Player_M_states.SLEEPED;
            Invoke("ResetSlowliness", 5f);
        }

        else if(col.gameObject.CompareTag("ShotGun"))
        {
            Vector3 direction = col.transform.position;
            direction.y = 0;
            direction -= transform.position;
            int force = col.gameObject.GetComponent<OnCollisonDestroy>().force;
            PushMe(-direction.normalized, force);
        }

        else if (col.gameObject.CompareTag("SlowBullet"))
        {
            sleep_time = col.gameObject.GetComponent<OnCollisonDestroy>().slow_time;
            current_M_state = Player_M_states.SLEEPED;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("FuriHability"))
        {
            RootMe(2);
        }
    }

    void FixedUpdate()
    {
        Vector2 inp = GamePad.GetAxis(GamePad.Axis.LeftStick,player_num,false);
        float h = inp.x;
        float v = inp.y;

        if (Mathf.Abs(h) > 0.01f)
            di_x = (h > 0) ? false : true;
        if (Mathf.Abs(v) > 0.01f)
            di_z = (v > 0) ? false : true;

        if(AbilitiesUp())
            GetComponent<SpriteRenderer>().flipX = di_x;

        Vector3 vel = new Vector3(h, 0, v);

        switch(current_M_state)
        {
            case Player_M_states.NORMAL:
                vel = vel * velocity * Time.deltaTime * slowliness;
                body.velocity = vel;
                can_throw_abilities = true;
                break;
            case Player_M_states.DASHING:
                Dash(vel);
                can_throw_abilities = false;
                break;
            case Player_M_states.PUSHED:
                if (body.velocity.magnitude < 20f)
                    current_M_state = Player_M_states.NORMAL;
                can_throw_abilities = false;
                break;
            case Player_M_states.SLEEPED:
                Sleeped(vel);
                break;
            case Player_M_states.ROOTED:
                can_throw_abilities = true;
                if (root_time <= current_root_time)
                {
                    current_M_state = Player_M_states.NORMAL;
                    current_root_time = 0f;
                }
                else
                    current_root_time += Time.fixedDeltaTime;
                break;
            case Player_M_states.STUNED:
                can_throw_abilities = false;
                if (root_time <= current_root_time)
                {
                    current_M_state = Player_M_states.NORMAL;
                    current_root_time = 0f;
                }
                else
                    current_root_time += Time.fixedDeltaTime;
                break;

        }
        last_time_pushed += Time.fixedDeltaTime;
    }

    void Update()
    { 
        //DASH -------------------------------------
        if (dash_cooldown <= dash_current_cooldown && AbilitiesUp())
        {
            if (GamePad.GetButtonDown(dash_button, player_num) || Input.GetKeyDown(dash_test_button))
            {
                current_M_state = Player_M_states.DASHING;
                curr_col.enabled = false;
                dash_current_cooldown = 0.0f;
            }
        }
        else if(current_M_state == Player_M_states.NORMAL)
            dash_current_cooldown += Time.deltaTime;
    }

    void Dash(Vector3 dir)
    {
        dash_current_cooldown += Time.fixedDeltaTime;

        int x = di_x ? -1 : 1;
        int z = di_z ? -1 : 1;

        Vector3 shoot_tmp = shoot_point;

        if (Mathf.Abs(dir.x) < Mathf.Abs(dir.z))
        {
            shoot_tmp.z = shoot_tmp.x;
            shoot_tmp.x = 0;
        }
        shoot_tmp.x *= x;
        shoot_tmp.z *= z;

        Vector3 speed = dir.normalized * dash_force * velocity * Time.fixedDeltaTime;

        body.velocity = dir.normalized * dash_force * velocity * Time.fixedDeltaTime;


        if (dash_time <= dash_current_cooldown)
        {
            current_M_state = Player_M_states.NORMAL;
            curr_col.enabled = true;
            dash_current_cooldown = 0.0f;
        }
    }
    void Sleeped(Vector3 vel)
    {
        if(sleep_time <= current_root_time)
        {
            slowliness = 0f;
            current_M_state = Player_M_states.NORMAL;
            current_root_time = 0f;
            ResetSlowliness();
        }
        else
        {
            if(current_root_time < 1)
            {
                slowliness = 1 - current_root_time;
                can_throw_abilities = true;
            }
            else
                can_throw_abilities = false;

            current_root_time += Time.fixedDeltaTime;
        }

        vel = vel * velocity * Time.deltaTime * slowliness;
        body.velocity = vel;
    }
    void ResetSlowliness()
    {
        slowliness = 1.0f;
    }

    public void PushMe(Vector3 direction,int push_force)
    {
        if(current_M_state == Player_M_states.NORMAL && last_time_pushed > 0.2f)
        {
            body.AddForce(direction * 100000 * push_force);
            current_M_state = Player_M_states.PUSHED;
            last_time_pushed = 0.0f;
        }
    }

    public void RootMe(float time)
    {
        if(current_M_state == Player_M_states.NORMAL || current_M_state == Player_M_states.PUSHED)
        {
            body.velocity = Vector3.zero;
            current_M_state = Player_M_states.ROOTED;
            root_time = time;
            current_root_time = 0f;
        }

    }

    public void SlowMe(float slow, float time_slowed)
    {
        if(current_M_state == Player_M_states.NORMAL)
        {
            float new_slowliness = 1 - slow;
            if (slowliness < new_slowliness)
            {
                Invoke("ResetSlowliness", time_slowed);
            }
            else
            {
                slowliness = new_slowliness;
                Invoke("ResetSlowliness", time_slowed);
            }
        }
    }

    public string GetHorizontal()
    {
        return horizontal;
    }

    public string GetVertical()
    {
        return vertical;
    }

    public string GetShootButton()
    {
        return shoot_bullet;
    }
    public bool GetDirection_x()
    {
        return di_x;
    }
    public bool GetDirection_z()
    {
        return di_z;
    }

    public bool AbilitiesUp()
    {
        return can_throw_abilities;
    }
}
