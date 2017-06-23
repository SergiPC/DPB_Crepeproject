using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadows : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
        GetComponent<Renderer>().receiveShadows = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
