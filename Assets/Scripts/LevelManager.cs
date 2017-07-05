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
    List<GameObject> dead_players;
    List<GameObject> winners;

    public static LevelManager current = null;
    // Use this for initialization

    void Awake()
    {
        if (current != this && current != null)
            Destroy(current);

        current = this;
    }

    void Start ()
    {
        current_status = IN_GAME_STATUS.STARTING;
        players_in_game = GameObject.FindGameObjectsWithTag("Player");
        dead_players = new List<GameObject>();
        winners = new List<GameObject>();
    }

    public void ChangeStatus(IN_GAME_STATUS new_status)
    {

    }


    public void AddFinishedPlayer(GameObject go)
    {
        winners.Add(go);
        go.GetComponent<PlayerController>().current_M_state = Player_M_states.PAUSED;
    }

    public void AddDeadPlayer(GameObject go)
    {
        dead_players.Add(go);
        go.GetComponent<PlayerController>().current_M_state = Player_M_states.PAUSED;
        go.SetActive(false);
    }
}
