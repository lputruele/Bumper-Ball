using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BumperBallGame
{
    public class DeathMatchManager : GameModeManager
    {
        public int scoreLimit = 10;
        private bool gameOver;

        void Start()
        {
            players = new List<GameObject>();
            scoreLimit = GameData.scoreLimit;
            EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
            BallController[] balls = FindObjectsOfType<BallController>();
            foreach (BallController ball in balls)
            {
                players.Add(ball.gameObject);
            }
            for (int i = 0; i < players.Count; i++)
            {
                scores[players[i]] = 0;
            }
        }

        void OnPlayerDeath(PlayerDeathEvent evt)
        {
            if (!gameOver)
            {
                scores[evt.Killer] += 1;
                if (scores[evt.Killer] >= scoreLimit)
                {
                    GameOverEvent gameOverEvt = Events.GameOverEvent;
                    gameOverEvt.Winner = evt.Killer;
                    EventManager.Broadcast(gameOverEvt);
                    gameOver = true;
                }
                evt.Killed.SetActive(true);
            }
            UpdateScoreEvent updateScoreEvt = Events.UpdateScoreEvent;
            EventManager.Broadcast(updateScoreEvt);
        }

        void OnDestroy()
        {
            EventManager.RemoveListener<PlayerDeathEvent>(OnPlayerDeath);
        }
    }
}