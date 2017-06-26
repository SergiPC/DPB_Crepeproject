using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class NormalShoot : MonoBehaviour {

    public float bullet_force;
    public GameObject bullet;
    public float shoot_cooldown = 0.25f;
    public float bullet_life = 10.0f;
    float shoot_current_cooldown = 0.0f;
    string horizontal = "Horizontal";
    string vertical = "Vertical";
    string shoot_bullet = "Fire1";
    PlayerController player_controller;
    Vector3 shoot_point;
    bool di_x = false;
    bool di_z = false;
    void Start ()
    {
        player_controller = GetComponent<PlayerController>();
        shoot_point = player_controller.horizontal_shoot_point.transform.localPosition;
        horizontal = player_controller.GetHorizontal();
        vertical = player_controller.GetVertical();
        shoot_bullet = player_controller.GetShootButton();
    }
	
	void Update ()
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
    }

    void ShootBullet(Vector3 dir)
    {
        di_x = player_controller.GetDirection_x();
        di_z = player_controller.GetDirection_z();

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
        bull_tmp.GetComponent<Rigidbody>().AddForce(dir * bullet_force);
        Destroy(bull_tmp, bullet_life);
    }
}
