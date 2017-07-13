using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IN_GAME_STATUS
{
    STARTING,
    IN_GAME,
    PAUSE,
    FINISHING,
    WINNING,
    LOSING
}


public class LevelManager : MonoBehaviour {

    public int player_num;
    List<GameObject> players_in_game;
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
        GameObject[] g = GameObject.FindGameObjectsWithTag("Player");
        players_in_game = new List<GameObject>(g);
        dead_players = new List<GameObject>();
        winners = new List<GameObject>();
        ChangeStatus(IN_GAME_STATUS.IN_GAME);
    }


    void Update()
    {

        if(current_status == IN_GAME_STATUS.IN_GAME)
        {
            float x = players_in_game[0].transform.position.x;
            foreach (GameObject c in players_in_game)
            {
                if (x < c.transform.position.x)
                {
                    x = c.transform.position.x;
                }
            }
            Camera.main.GetComponent<FollowCamera>().SetFloat(x);
        }
    }

    public void ChangeStatus(IN_GAME_STATUS new_status)
    {
        current_status = new_status;
        switch (new_status)
        {
            case IN_GAME_STATUS.STARTING:
                break;
            case IN_GAME_STATUS.PAUSE:
                break;
            case IN_GAME_STATUS.IN_GAME:
                break;
            case IN_GAME_STATUS.FINISHING:
                CheckWinStatus();
                break;
            case IN_GAME_STATUS.WINNING:
                break;
            case IN_GAME_STATUS.LOSING:
                break;
        }
    }


    public void AddFinishedPlayer(GameObject go)
    {
        if(!winners.Contains(go))
        {
            winners.Add(go);
            go.GetComponent<PlayerController>().current_M_state = Player_M_states.PAUSED;
            RemovePlayer(go);
        }

    }

    public void AddDeadPlayer(GameObject go)
    {
        if (!dead_players.Contains(go))
        {
            dead_players.Add(go);
            go.GetComponent<PlayerController>().current_M_state = Player_M_states.PAUSED;
            go.SetActive(false);
            RemovePlayer(go);
        }
    }

    void RemovePlayer(GameObject go)
    {
        if(players_in_game.Contains(go))
        {
            players_in_game.Remove(go);
        }

        if(players_in_game.Count < 1)
        {
            ChangeStatus(IN_GAME_STATUS.FINISHING);
        }
    }

    void CheckWinStatus()
    {
        if (winners.Count > 0)
            ChangeStatus(IN_GAME_STATUS.WINNING);
        else
            ChangeStatus(IN_GAME_STATUS.LOSING);
    }
}
