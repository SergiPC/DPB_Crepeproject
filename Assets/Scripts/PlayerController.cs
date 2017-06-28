using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
    public int player_num = 1;
    public int velocity = 4;

    public float dash_cooldown = 0.25f;
    public float dash_force = 4f;
    public float dash_time = 4f;
    public LayerMask dash_mask;
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
    float current_root_time = 0f;

    // Use this for initialization
    void Awake ()
    {
        body = GetComponent<Rigidbody>();
        curr_col = GetComponent<Collider>();
        shoot_point = horizontal_shoot_point.transform.localPosition;

        switch(player_num)
        {
            case 1:
                horizontal = "Horizontal";
                vertical = "Vertical";
                shoot_bullet = "Fire1";
                break;
            case 2:
                horizontal = "Horizontal_2";
                vertical = "Vertical_2";
                shoot_bullet = "Fire2";
                break;
            case 3:
                horizontal = "Horizontal_3";
                vertical = "Vertical_3";
                shoot_bullet = "Fire3";
                break;
            case 4:
                horizontal = "Horizontal_4";
                vertical = "Vertical_4";
                shoot_bullet = "Fire4";
                break;
        }
	}

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Bullet"))
        {
            slowliness = 0.6f;
            Invoke("ResetSlowliness", 5f);
        }

        if(col.gameObject.CompareTag("ShotGun"))
        {
            Vector3 direction = col.transform.position;
            direction.y = 0;
            direction -= transform.position;
            int force = col.gameObject.GetComponent<OnCollisonDestroy>().force;
            PushMe(-direction.normalized, force);
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
        float h = Input.GetAxis(horizontal);
        float v = Input.GetAxis(vertical);

        if (Mathf.Abs(h) > 0.01f)
            di_x = (h > 0) ? false : true;
        if (Mathf.Abs(v) > 0.01f)
            di_z = (v > 0) ? false : true;

        if(current_M_state != Player_M_states.ROOTED)
            GetComponent<SpriteRenderer>().flipX = di_x;

        Vector3 vel = new Vector3(h, 0, v);

        switch(current_M_state)
        {
            case Player_M_states.NORMAL:
                vel = vel * velocity * Time.deltaTime * slowliness;
                body.velocity = vel;
                break;
            case Player_M_states.DASHING:
                Dash(vel);
                break;
            case Player_M_states.PUSHED:
                if (body.velocity.magnitude < 20f)
                    current_M_state = Player_M_states.NORMAL;
                break;
            case Player_M_states.ROOTED:
               
            case Player_M_states.STUNED:

                if (root_time <= current_root_time)
                {
                    current_M_state = Player_M_states.NORMAL;
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
        if (dash_cooldown <= dash_current_cooldown && current_M_state == Player_M_states.NORMAL)
        {
            if (Input.GetButtonDown(shoot_bullet))
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
}
