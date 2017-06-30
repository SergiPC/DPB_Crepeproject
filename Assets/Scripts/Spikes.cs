using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Spikes : MonoBehaviour {

    public float time_to_appear = 3f;
    public float time_to_disappear = 3f;

    float current_time_to_appear = 3f;
    float current_time_to_disappear = 3f;

    bool appearing = false;
    Animator anim;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        current_time_to_disappear = time_to_disappear;

    }
	void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerController>().SlowMe(0.8f, 0.5f);
        }
    }
	// Update is called once per frame
	void Update ()
    {
        if (appearing)
        {
            Appear();
        }
        else
            Disappear();
	}

    void Appear()
    {
        if (time_to_appear <= current_time_to_appear)
        {
            anim.SetBool("Appear", true);
            appearing = false;
            current_time_to_appear = 0f;
        }
        else
            current_time_to_appear += Time.deltaTime;
    }

    void Disappear()
    {
        if (time_to_disappear <= current_time_to_disappear)
        {
            anim.SetBool("Appear", false);
            appearing = true;
            current_time_to_disappear = 0f;
        }
        else
            current_time_to_disappear += Time.deltaTime;
    }
}
