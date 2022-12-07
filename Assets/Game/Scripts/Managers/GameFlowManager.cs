using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BumperBallGame
{
    public class GameFlowManager : MonoBehaviour
    {
        [Header("Parameters")]

        [Tooltip("Win game message")]
        public string WinGameMessage;

        [Tooltip("Duration of delay before the win message")]
        public float DelayBeforeWinMessage = 2f;

        [Tooltip("Sound played on win")] public AudioClip VictorySound;

        [Tooltip("Restart button")]
        public GameObject RestartButton;
        [Tooltip("Exit button")]
        public GameObject ExitButton;
        [Tooltip("Win Text UI")]
        public GameObject WinText;

        public List<GameObject> Players;
        private int playersRemaining;
        private bool isGameEnding;

        void Awake()
        {
            EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
            if (Players != null)
            {
                playersRemaining = Players.Count;
            }
            isGameEnding = false;
        }

        void Start()
        {
            AudioUtility.SetMasterVolume(1);
        }

        void Update()
        {
            if (playersRemaining < 2 && !isGameEnding)
            {
                string winner = "";
                foreach (GameObject p in Players)
                {
                    if (p.activeInHierarchy)
                    {
                        winner = p.name;
                    }
                }
                EndGame(winner);
            }
        }
        void OnPlayerDeath(PlayerDeathEvent evt) => UpdateGame(evt.Player);

        void UpdateGame(GameObject deadPlayer)
        {
            if (deadPlayer.GetComponent<PlayerController>())
            {
                RestartButton.SetActive(true);
                ExitButton.SetActive(true);
            }
            playersRemaining--;
        }

        void EndGame(string winnerName)
        {
            isGameEnding = true;
            // play a sound on win
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = VictorySound;
            audioSource.playOnAwake = false;
            audioSource.outputAudioMixerGroup = AudioUtility.GetAudioGroup(AudioUtility.AudioGroups.HUDVictory);
            audioSource.PlayScheduled(AudioSettings.dspTime + DelayBeforeWinMessage);

            WinText.GetComponent<TMP_Text>().SetText(winnerName + " " + WinGameMessage);
            RestartButton.SetActive(true);
            ExitButton.SetActive(true);
            WinText.SetActive(true);
        }

        void OnDestroy()
        {
            EventManager.RemoveListener<PlayerDeathEvent>(OnPlayerDeath);
        }
    }
}