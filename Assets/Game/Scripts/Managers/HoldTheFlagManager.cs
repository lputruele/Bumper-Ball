using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BumperBallGame
{
    public class HoldTheFlagManager : GameModeManager
    {
        public const float BUMP_TIME = 0.1f;
        private float bumpTimer;
        public int scoreLimit = 50;
        private bool gameOver;
        public GameObject flag;
        private GameObject flagHolder;

        void Start()
        {
            players = new List<GameObject>();
            scoreLimit = GameData.scoreLimit;
            bumpTimer = float.NegativeInfinity;
            EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
            EventManager.AddListener<BumpEvent>(OnBump);
            EventManager.AddListener<FlagGrabbedEvent>(OnFlagGrabbed);
            BallController[] balls = FindObjectsOfType<BallController>();
            flag = GetComponent<GameFlowManager>().flag;            
            foreach (BallController ball in balls)
            {
                players.Add(ball.gameObject);
            }
            for (int i = 0; i < players.Count; i++)
            {
                scores[players[i]] = 0;
            }
            StartCoroutine(HolderScoreUpdateRoutine());
        }

        private void Update()
        {
            if (flagHolder != null)
            {
                Vector3 holderPos = flagHolder.transform.position;
                flag.transform.position = new Vector3(holderPos.x + 0.5f, holderPos.y + 1, holderPos.z);
            }            
        }

        private void HolderScoreUpdate()
        {
            if (flagHolder != null)
            {
                scores[flagHolder] += 1;
                if (scores[flagHolder] >= scoreLimit){
                    GameOverEvent gameOverEvt = Events.GameOverEvent;
                    gameOverEvt.Winner = flagHolder;
                    EventManager.Broadcast(gameOverEvt);
                    gameOver = true;
                }
                UpdateScoreEvent updateScoreEvt = Events.UpdateScoreEvent;
                EventManager.Broadcast(updateScoreEvt);
            }
        }

        private IEnumerator HolderScoreUpdateRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(1.0f);

            while (!gameOver)
            {
                yield return wait;
                HolderScoreUpdate();
            }
        }

        void OnPlayerDeath(PlayerDeathEvent evt)
        {
            if (!gameOver)
            {
                if (evt.Killed == flagHolder)
                {
                    flag.transform.position = Vector3.zero;
                    flagHolder = null;
                }                
                evt.Killed.SetActive(true);
            }            
        }

        void OnBump(BumpEvent evt)
        {
            if (!gameOver && Time.time >= bumpTimer)
            {
                if (evt.Bumped == flagHolder)
                {
                    flagHolder = evt.Bumper;
                    bumpTimer = Time.time + BUMP_TIME;
                }
                else
                {
                    if (evt.Bumper == flagHolder)
                    {
                        flagHolder = evt.Bumped;
                        bumpTimer = Time.time + BUMP_TIME;
                    }
                }
            }
        }

        void OnFlagGrabbed(FlagGrabbedEvent evt)
        {
            flagHolder = evt.grabber;
        }

        void OnDestroy()
        {
            EventManager.RemoveListener<PlayerDeathEvent>(OnPlayerDeath);
            EventManager.RemoveListener<BumpEvent>(OnBump);
            EventManager.RemoveListener<FlagGrabbedEvent>(OnFlagGrabbed);
        }
    }
}