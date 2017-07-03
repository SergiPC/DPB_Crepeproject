using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IN_GAME_STATUS
{
    STARTING,
    IN_GAME,
    PAUSE,
    WINNING,
    LOSING
}


public class LevelManager : MonoBehaviour {

    public int player_num;
    GameObject[] players_in_game;
    IN_GAME_STATUS current_status;
    // Use this for initialization
    void Start ()
    {
        current_status = IN_GAME_STATUS.STARTING;
        players_in_game = GameObject.FindGameObjectsWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    public void ChangeStatus(IN_GAME_STATUS new_status)
    {

    }
}
