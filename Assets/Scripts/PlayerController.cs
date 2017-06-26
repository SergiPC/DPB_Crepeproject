using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject bullet;
    public float bullet_force;
    public GameObject horizontal_shoot_point;
    public int player_num = 1;
    public int velocity = 4;
    public float shoot_cooldown = 0.25f;
    public float dash_cooldown = 0.25f;
    public float bullet_life = 10.0f;
    public float dash_force = 4f;
    public float dash_time = 4f;
    public LayerMask dash_mask;
    Rigidbody body;

    private Vector3 shoot_point;
    bool di_x = false;
    bool di_z = false;
    string horizontal = "Horizontal";
    string vertical = "Vertical";
    string shoot_bullet = "Fire1";
    float slowliness = 1.0f;
    float shoot_current_cooldown = 0.0f;
    float dash_current_cooldown = 0.0f;
    bool dashing = false;

    // Use this for initialization
    void Start ()
    {
        body = GetComponent<Rigidbody>();
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
    }
    void FixedUpdate()
    {
        float h = Input.GetAxis(horizontal);
        float v = Input.GetAxis(vertical);

        if (Mathf.Abs(h) > 0.01f)
            di_x = (h > 0) ? false : true;
        if (Mathf.Abs(v) > 0.01f)
            di_z = (v > 0) ? false : true;

        GetComponent<SpriteRenderer>().flipX = di_x;
        Vector3 vel = new Vector3(h, 0, v);
        if (dashing == false)
        {
            
            vel = vel * velocity * Time.deltaTime * slowliness;
            body.velocity = vel;
        }
        else
        {
            Dash(vel);
        }
    }
    void Update()
    {
        //SHOOT -------------------------------------
        if (shoot_cooldown <= shoot_current_cooldown)
        {
            if (Input.GetButtonDown(shoot_bullet))
            {
                float h = Input.GetAxis(horizontal);
                float v = Input.GetAxis(vertical);

                Vector3 dir = new Vector3(Mathf.Abs(h), 0, Mathf.Abs(v));

                if (dir.sqrMagnitude > 0.2)
                    ShootBullet(dir.normalized);
                else
                    ShootBullet(transform.right);
                shoot_current_cooldown = 0.0f;
            }
        }
        else
            shoot_current_cooldown += Time.deltaTime;

        //DASH -------------------------------------

        if (dash_cooldown <= dash_current_cooldown && dashing == false)
        {
            if (Input.GetButtonDown(shoot_bullet))
            {
                dashing = true;
                dash_current_cooldown = 0.0f;
            }
        }
        else if(dashing == false)
            dash_current_cooldown += Time.deltaTime;
    }

    void ShootBullet(Vector3 dir)
    {
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
        GameObject bull_tmp = Instantiate(bullet, transform.position + shoot_tmp, Quaternion.identity);
        bull_tmp.GetComponent<Rigidbody>().AddForce(dir  * bullet_force);
        Destroy(bull_tmp, bullet_life);
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

        Collider[] col = Physics.OverlapCapsule(transform.position + shoot_tmp, transform.position + shoot_tmp + (dir.normalized * (7200*dash_time*dash_force)), 25, dash_mask, QueryTriggerInteraction.Ignore);
        if (col.Length > 0)
        {
            Vector3 hit_point = col[0].ClosestPointOnBounds(transform.position + shoot_tmp);
            hit_point.y = transform.position.y;
            if (Vector3.Distance(hit_point, transform.position) > 40)
            {
                body.velocity = Vector3.zero;
                body.MovePosition(hit_point);
            }

            Debug.Log("touching");
        }
        else
        {
            body.velocity = dir.normalized * dash_force * velocity;
        }
        if (dash_time <= dash_current_cooldown)
        {
            dashing = false;
            dash_current_cooldown = 0.0f;
        }

    }

    void ResetSlowliness()
    {
        slowliness = 1.0f;
    }
}
