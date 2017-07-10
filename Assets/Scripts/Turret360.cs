using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret360 : MonoBehaviour {


    public GameObject bullet;
    public float cooldown = 1f;
    public float bullet_life = 4.0f;
    public float bullet_force = 4.0f;
    public int bullet_num = 8;
    float current_cooldown = 0f;

    void Update()
    {
        if (cooldown <= current_cooldown)
        {
            Vector3 dir = Vector3.right;
            dir.y = 0f;
            float degrees = 360/bullet_num;
            for (int i = 0; i< bullet_num; i++)
            {
                Vector3 new_dir = dir;
                float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
                float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
                new_dir.x = dir.x * cos - dir.z * sin;
                new_dir.z = -dir.x * sin + dir.z * cos;

                ShootBullet(new_dir.normalized);
                degrees += 360 / bullet_num;
            }
            current_cooldown = 0.0f;
        }
        else
            current_cooldown += Time.deltaTime;
    }

    void ShootBullet(Vector3 dir)
    {
        GameObject bull_tmp = Instantiate(bullet, transform.position, Quaternion.identity);
        bull_tmp.GetComponent<Rigidbody>().AddForce(dir * bullet_force);
        Destroy(bull_tmp, bullet_life);
    }
}
