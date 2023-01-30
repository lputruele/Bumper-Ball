using Game.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public List<GameObject> players;
    public Dictionary<GameObject, int> scores = new Dictionary<GameObject, int>();

    public void StopPlayers(bool stop)
    {
        foreach (GameObject player in players)
        {
            player.GetComponent<BallController>().enabled = !stop;
        }
    }
}
