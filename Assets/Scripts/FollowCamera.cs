using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {


    public GameObject to_follow;
    Vector3 offset;
    float x = 0f;
    float y = 0f;
    float z = 0f;

	// Use this for initialization
	void Start ()
    {
        offset = transform.position - to_follow.transform.position;
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        float t_x = x + offset.x;
        if (t_x < transform.position.x)
            t_x = transform.position.x;
        transform.position = new Vector3(t_x,y, z);
	}

    public void SetFloat(float n_x)
    {
        x = n_x;
    }
}
