using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

[RequireComponent(typeof(PlayerController))]
public class ShotGun_shoot : MonoBehaviour
{
    public float bullet_force;
    public GameObject bullet;
    public float shoot_cooldown = 0.25f;
    public float bullet_life = 10.0f;
    public float time_between_bullets = 0.01f;
    public float degrees_of_direction = 20f;
    public int bullet_num = 3;
    public GamePad.Button shotgun_button = GamePad.Button.A;
    public KeyCode shotgun_test_shoot_button = KeyCode.X;
    float shoot_current_cooldown = 0.0f;
    float timer_between_bullets = 0f;
    float real_timer_between_bullets = 0f;
    string horizontal = "Horizontal";
    string vertical = "Vertical";
    string shoot_bullet = "Fire1";
    PlayerController player_controller;
    GamePad.Index player_num = 0;
    Vector3 shoot_point;
    bool di_x = false;
    bool di_z = false;
    bool shooting = false;
    int bullets_shoot = 0;

    void Start()
    {
        player_controller = GetComponent<PlayerController>();
        shoot_point = player_controller.horizontal_shoot_point.transform.localPosition;
        horizontal = player_controller.GetHorizontal();
        vertical = player_controller.GetVertical();
        shoot_bullet = player_controller.GetShootButton();
        real_timer_between_bullets = Random.Range(0, time_between_bullets);
        player_num = player_controller.player_num;
    }

    void Update()
    {
        if (shooting)
        {
            if (real_timer_between_bullets <= timer_between_bullets)
            {
                Vector2 inp = GamePad.GetAxis(GamePad.Axis.LeftStick, (GamePad.Index)player_num, false);
                float h = inp.x;
                float v = inp.y;

                Vector3 dir = new Vector3(Mathf.Abs(h), 0, Mathf.Abs(v));

                if (dir.sqrMagnitude < 0.2)
                    dir = transform.right;

                float new_degrees_of_direction = Random.Range(-degrees_of_direction, degrees_of_direction);
                new_degrees_of_direction *= Mathf.Deg2Rad;
                Vector3 new_dir = dir;
                float cos = Mathf.Cos(new_degrees_of_direction);
                float sin = Mathf.Sin(new_degrees_of_direction);
                new_dir.x = dir.x * cos + dir.z * sin;
                new_dir.z = -dir.x * sin + dir.z * cos;

                ShootBullet(new_dir.normalized);

                bullets_shoot++;

                if(bullets_shoot >= bullet_num)
                {
                    shoot_current_cooldown = 0.0f;
                    bullets_shoot = 0;
                    shooting = false;
                }

                timer_between_bullets = 0.0f;
                real_timer_between_bullets = Random.Range(0, time_between_bullets);
            }
            else
                timer_between_bullets += Time.deltaTime;


        }
        else
        {
     //SHOOT -------------------------------------
            if (shoot_cooldown <= shoot_current_cooldown && player_controller.AbilitiesUp())
            {
                if (GamePad.GetButtonDown(shotgun_button, player_num) || Input.GetKeyDown(shotgun_test_shoot_button))
                {
                    shooting = true;
                }
            }
            else
                shoot_current_cooldown += Time.deltaTime;

        }
       
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
