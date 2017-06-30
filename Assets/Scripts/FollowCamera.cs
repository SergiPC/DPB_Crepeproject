using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {


    public GameObject to_follow;
    Vector3 offset;
	// Use this for initialization
	void Start () {
        offset = transform.position -  to_follow.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = to_follow.transform.position + offset;
	}
}
