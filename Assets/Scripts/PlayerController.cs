using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Rigidbody body;
    public GameObject bullet;
    Collider col;
    public float bullet_force;
    public GameObject horizontal_shoot_point;
    public int velocity = 4;
    private Vector3 shoot_point;
    bool di_x = false;
    bool di_z = false;
    // Use this for initialization
    void Start ()
    {
        body = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        shoot_point = horizontal_shoot_point.transform.localPosition;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 vel = new Vector3(h, 0,v) * velocity;
        Debug.Log("horizontal: " + h + "Vertical: " + v);
        di_x = (h > 0) ? false : true;
        di_z = (v > 0) ? false : true;

        GetComponent<SpriteRenderer>().flipX = di_x;
        body.velocity = vel;
    }
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 vel = new Vector3(Mathf.Abs(h), 0, Mathf.Abs(v));

            if (vel.sqrMagnitude > 0.2 )
                ShootBullet(vel);
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
