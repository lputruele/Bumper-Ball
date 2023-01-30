using Game.Gameplay;
using Game.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.UI
{
    public class InGameMenu : MonoBehaviour
    {

        [Tooltip("Win game message")]
        public string WinGameMessage;

        [Tooltip("Duration of delay before the win message")]
        public float DelayBeforeWinMessage = 2f;

        [Tooltip("Pause button")]
        public GameObject PauseButton;
        public GameObject PauseMenu;
        [Tooltip("Restart button")]
        public GameObject RestartButton;
        [Tooltip("Exit button")]
        public GameObject ExitButton;
        [Tooltip("Win Text UI")]
        public GameObject WinText;
        public GameObject WinTextShadow;
        public GameObject CountdownText;
        public GameObject CountdownTextShadow;
        public GameObject PlayerScoreText;
        public GameObject PlayerNameText;
        public GameObject ScoreText;
        public GameObject EventUpdateText;
        public GameModeManager GameModeManager;

        private List<string> eventList = new List<string>();
        private List<Tuple<string,int>> scoreList = new List<Tuple<string, int>>();

        void Awake()
        {
            EventManager.AddListener<PlayerOutOfLivesEvent>(OnPlayerOutOfLives);
            EventManager.AddListener<GameOverEvent>(OnGameOver);
            EventManager.AddListener<UpdateScoreEvent>(OnUpdateScore);
            EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
        }

        private void Start()
        {
            GameModeManager = GetComponent<GameModeManager>();
            foreach (GameObject p in GameModeManager.players)
            {
                scoreList.Add(new Tuple<string, int>(p.transform.parent.name, GameModeManager.scores[p]));
            }
            PlayerScoreText.GetComponent<TMP_Text>().SetText(JoinScoreList(1));
            PlayerNameText.GetComponent<TMP_Text>().SetText(JoinScoreList(0));
            StartCoroutine(StartCountDown());
        }


        void OnPlayerOutOfLives(PlayerOutOfLivesEvent evt)
        {
            eventList.Add(evt.Killed.transform.parent.name + " " + "is out of lives!");
            EventUpdateText.GetComponent<TMP_Text>().SetText(JoinEventList());
            StartCoroutine(HideEventUpdateText());
            if (evt.Killed.GetComponent<BallController>().IsPlayer)
            {
                RestartButton.SetActive(true);
                ExitButton.SetActive(true);
            }
        }

        void OnGameOver(GameOverEvent evt)
        {
            WinText.GetComponent<TMP_Text>().SetText(evt.Winner.transform.parent.name + " " + WinGameMessage);
            WinTextShadow.GetComponent<TMP_Text>().SetText(evt.Winner.transform.parent.name + " " + WinGameMessage);
            RestartButton.SetActive(true);
            ExitButton.SetActive(true);
            WinText.SetActive(true);
        }

        void OnPlayerDeath(PlayerDeathEvent evt)
        {
            eventList.Add(evt.Killer.transform.parent.name + " bumped " + evt.Killed.transform.parent.name + " out.");
            EventUpdateText.GetComponent<TMP_Text>().SetText(JoinEventList());
            StartCoroutine(HideEventUpdateText());
        }

        private string JoinEventList()
        {
            string result = "";
            for (int i = 0; i < eventList.Count; i++)
            {
                result += eventList[i] + "\n";
            }
            return result;
        }

        private string JoinScoreList(int pos)
        {
            string result = "";
            for (int i = scoreList.Count - 1; i >= 0; i--)
            {
                if (pos == 0)
                    result += scoreList[i].Item1 + "\n";
                else
                    result += scoreList[i].Item2 + "\n";
            }
            return result;
        }

        IEnumerator HideEventUpdateText()
        {
            WaitForSeconds wait = new WaitForSeconds(4.0f);
            yield return wait;
            if (eventList.Count > 0)
            {
                eventList.RemoveAt(0);
                EventUpdateText.GetComponent<TMP_Text>().SetText(JoinEventList());
            }
        }
        void OnUpdateScore(UpdateScoreEvent evt)
        {
            scoreList = new List<Tuple<string, int>>();
            foreach (GameObject p in GameModeManager.players)
            {
                scoreList.Add(new Tuple<string, int>(p.transform.parent.name, GameModeManager.scores[p]));
            }
            scoreList.Sort((x, y) => x.Item2.CompareTo(y.Item2));
            PlayerScoreText.GetComponent<TMP_Text>().SetText(JoinScoreList(1));
            PlayerNameText.GetComponent<TMP_Text>().SetText(JoinScoreList(0));
        }

        void OnDestroy()
        {
            EventManager.RemoveListener<PlayerOutOfLivesEvent>(OnPlayerOutOfLives);
            EventManager.RemoveListener<GameOverEvent>(OnGameOver);
            EventManager.RemoveListener<UpdateScoreEvent>(OnUpdateScore);
            EventManager.RemoveListener<PlayerDeathEvent>(OnPlayerDeath);
        }

        public void HideUI()
        {
            PlayerScoreText.SetActive(false);
            PlayerNameText.SetActive(false);
            ScoreText.SetActive(false);
            EventUpdateText.SetActive(false);
            WinText.SetActive(false);
            WinTextShadow.SetActive(false);
            ExitButton.SetActive(false);
            RestartButton.SetActive(false);
            PauseButton.SetActive(false);
            PauseMenu.SetActive(false);
            CountdownText.SetActive(false);
            CountdownTextShadow.SetActive(false);
        }

        public void OpenPauseMenu()
        {
            switch (Time.timeScale)
            {
                case 0: ResumeGame(); break;
                case 1: PauseGame(); break;
            }
        }


        public void PauseGame()
        {
            Time.timeScale = 0;
            PauseMenu.SetActive(true);
        }
        public void ResumeGame()
        {
            Time.timeScale = 1;
            PauseMenu.SetActive(false);
        }

        private void OnApplicationPause(bool paused) 
        { 
            if (paused) PauseGame(); 
        }

        private IEnumerator StartCountDown()
        {
            GetComponent<GameFlowManager>().GameModeHandler.StopPlayers(true);
            CountdownText.SetActive(true);
            CountdownTextShadow.SetActive(true);
            CountdownText.GetComponent<TMP_Text>().text = "3";
            CountdownTextShadow.GetComponent<TMP_Text>().text = "3";
            WaitForSeconds wait = new WaitForSeconds(1.0f);
            yield return wait;
            CountdownText.GetComponent<TMP_Text>().text = "2";
            CountdownTextShadow.GetComponent<TMP_Text>().text = "2";
            yield return wait;
            CountdownText.GetComponent<TMP_Text>().text = "1";
            CountdownTextShadow.GetComponent<TMP_Text>().text = "1";
            yield return wait;
            CountdownText.GetComponent<TMP_Text>().text = "GO!";
            CountdownTextShadow.GetComponent<TMP_Text>().text = "GO";
            GetComponent<GameFlowManager>().GameModeHandler.StopPlayers(false);
            yield return wait;
            CountdownText.SetActive(false);
            CountdownTextShadow.SetActive(false);
        }
    }
}