using Game.Audio;
using Game.Persistence;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Gameplay
{
    public enum GameMode
    {
        SURVIVAL,
        DEATHMATCH,
        HOLD_THE_FLAG
    }

    public enum Difficulty
    {
        EASY,
        NORMAL,
        HARD
    }

    public class GameFlowManager : MonoBehaviour
    {
        [Header("Parameters")]

        public GameObject botPrefab;
        public GameObject playerPrefab;
        public List<Material> ballMaterials;
        public List<Material> arenaMaterials;
        public GameObject arena;
        public GameObject flag;
        public GameObject player;

        public GameModeManager GameModeHandler { get; private set; }

        private Vector3[] playerPositions = { new Vector3(-3, 0, -3), new Vector3(3, 0, -3), new Vector3(3, 0, 3), new Vector3(-3, 0, 3) };
        private string[] botNames = {"Zeus", "Hera", "Ares"};

        void Awake()
        {
            Time.timeScale = 1;
            InitializePlayers();
            switch (GameData.gameMode)
            {
                case GameMode.SURVIVAL: gameObject.AddComponent<SurvivalManager>(); break;
                case GameMode.DEATHMATCH: gameObject.AddComponent<DeathMatchManager>(); break;
                case GameMode.HOLD_THE_FLAG: gameObject.AddComponent<HoldTheFlagManager>(); flag.SetActive(true); break;
            }
            GameModeHandler = GetComponent<GameModeManager>();
            arena.GetComponentInChildren<Renderer>().material = arenaMaterials[Random.Range(0,arenaMaterials.Count)];
            EventManager.AddListener<GameOverEvent>(OnGameOver);
        }

        void Start()
        {
            AudioUtility.SetMasterVolume(1);         
        }

        

        void OnGameOver(GameOverEvent evt)
        {
            // play a sound on win
            if (InGameSounds.Instance.VictorySound)
                AudioUtility.CreateSFX(InGameSounds.Instance.VictorySound, transform.position, AudioUtility.AudioGroups.HUDVictory, 0f);
        }

        void OnDestroy()
        {
            EventManager.RemoveListener<GameOverEvent>(OnGameOver);
        }

        private void InitializePlayers()
        {
            //GameObject player = Instantiate(playerPrefab, playerPositions[0], Quaternion.identity);
            player.GetComponentInChildren<Renderer>().material = ballMaterials[0];
            player.name = "P1";
            for (int i = 0; i < GameData.bots; i++)
            { 
                GameObject bot = Instantiate(botPrefab, playerPositions[i+1], Quaternion.identity);
                bot.GetComponentInChildren<Renderer>().material = ballMaterials[i+1];
                bot.name = botNames[i];
            }
        }
    }
}