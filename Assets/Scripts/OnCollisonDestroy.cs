using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisonDestroy : MonoBehaviour
{
    public float TimeToDestroy = 1.0f;
    // Use this for initialization
    public int touches_to_die = 2;
    public float bullet_speed = 6f;
    Rigidbody bod = null;
    int touch_num = 0;
    public int force = 1;
    void Start()
    {
        bod = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            bod.velocity = Vector3.zero;
            Invoke("DestroyMe", TimeToDestroy);
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            touch_num++;
            if (touch_num >= touches_to_die)
                Invoke("DestroyMe", TimeToDestroy);
        }

    }

    void FixedUpdate()
    {
        bod.velocity = bod.velocity.normalized * bullet_speed;
    }

    void DestroyMe()
    {
        Destroy(this.gameObject);
    }
}