using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisonDestroy : MonoBehaviour
{
    public float TimeToDestroy = 1.0f;
    // Use this for initialization

    void OnCollisionEnter(Collision collision)
    {
        Invoke("DestroyMe", TimeToDestroy);
    }

    void DestroyMe()
    {
        Destroy(this.gameObject);
    }
}