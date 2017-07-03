using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public int player_num;
    GameObject[] players_in_game;
	// Use this for initialization
	void Start ()
    {
        players_in_game = GameObject.FindGameObjectsWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {

    }
}
