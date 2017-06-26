using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    public GameObject bullet;
    public float bullet_force;
    public GameObject horizontal_shoot_point;
    public int player_num = 1;
    public int velocity = 4;

    Rigidbody body;

    private Vector3 shoot_point;
    bool di_x = false;
    bool di_z = false;
    string horizontal = "Horizontal";
    string vertical = "Vertical";
    string shoot_bullet = "Fire1";
    float slowliness = 1.0f;

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
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float h = Input.GetAxis(horizontal);
        float v = Input.GetAxis(vertical);
        Vector3 vel = new Vector3(h, 0, v);
        vel = vel.normalized * velocity * Time.deltaTime * slowliness;
        Debug.Log("horizontal: " + h + "Vertical: " + v);
        di_x = (h > 0) ? false : true;
        di_z = (v > 0) ? false : true;

        GetComponent<SpriteRenderer>().flipX = di_x;
        body.velocity = vel;
    }

    void Update()
    {
        if(Input.GetButtonDown(shoot_bullet))
        {
            float h = Input.GetAxis(horizontal);
            float v = Input.GetAxis(vertical);
            Vector3 vel = new Vector3(Mathf.Abs(h), 0, Mathf.Abs(v));

            if (vel.sqrMagnitude > 0.2 )
                ShootBullet(vel.normalized);
            else
                ShootBullet(transform.right);
        }
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
        Destroy(bull_tmp, 10.0f);
    }
}
