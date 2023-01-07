using BumperBallGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BumperBallGame
{
    public class SurvivalManager : GameModeManager
    {
        
        private int lives;
        private int playersRemaining;
        private bool gameOver;      

        void Start()
        {
            players = new List<GameObject>();
            lives = GameData.lives;
            EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
            BallController[] balls = FindObjectsOfType<BallController>();
            foreach (BallController ball in balls)
            {
                players.Add(ball.gameObject);
            }
            for (int i = 0; i < players.Count; i++)
            {
                scores[players[i]] = lives;
            }
            playersRemaining = players.Count;
        }

        void OnPlayerDeath(PlayerDeathEvent evt)
        {
            if (!gameOver)
            {
                scores[evt.Killed] -= 1;
                if (scores[evt.Killed] == 0)
                {
                    playersRemaining--;
                    if (evt.Killed.GetComponent<PlayerController>())
                    {
                        PlayerOutOfLivesEvent playerOutOfLivesEvt = Events.PlayerOutOfLivesEvent;
                        playerOutOfLivesEvt.Killed = evt.Killed;
                        EventManager.Broadcast(playerOutOfLivesEvt);
                    }
                }
                if (playersRemaining < 2)
                {
                    foreach (GameObject p in players)
                    {
                        if (p.activeInHierarchy)
                        {
                            GameOverEvent gameOverEvt = Events.GameOverEvent;
                            gameOverEvt.Winner = p;
                            EventManager.Broadcast(gameOverEvt);
                            gameOver = true;
                        }
                    }

                }
                if (scores[evt.Killed] > 0)
                {
                    evt.Killed.SetActive(true);
                }
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