using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGenerator : MonoBehaviour {

    public GameObject beggining;
    public GameObject ending;
    public GameObject[] scene_parts;

    public int number_of_sections = 10;

    float x_size = 0f;
	// Use this for initialization
	void Awake ()
    {
        GameObject go = Instantiate(beggining, transform);

        x_size = go.GetComponent<Collider>().bounds.size.x;
        Debug.Log(go.GetComponent<Collider>().bounds.size);
        Vector3 pos = transform.position;
        for(int i = 0; i< number_of_sections; i++)
        {
            pos.x += x_size;
            int x = Random.Range(0, scene_parts.Length);
            go = Instantiate(scene_parts[x], pos, transform.rotation);
            x_size = go.GetComponent<Collider>().bounds.size.x;
        }
        pos.x += x_size;

       Instantiate(ending, pos, transform.rotation);

    }
}
