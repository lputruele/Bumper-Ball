using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Persistence;
using Game.Gameplay;

namespace Game.UI
{
    public class MainMenuValuesToData : MonoBehaviour
    {
        public TMP_Dropdown gameModeUI;
        public TMP_Dropdown difficultyUI;
        public Slider botsUI;
        public Slider scoreLimitDM_UI;
        public Slider scoreLimitHTF_UI;
        public Slider livesUI;

        public GameObject survivalMenu;
        public GameObject deathMatchMenu;
        public GameObject holdTheFlagMenu;

        private ConfigData config;

        private void Start()
        {
            config = SaveSystem.LoadConfig();
            if (config != null)
            {
                gameModeUI.value = (int)config.gameMode;
                difficultyUI.value = (int)config.difficulty;
                botsUI.value = config.bots;
                scoreLimitDM_UI.value = config.scoreLimitDM;
                scoreLimitHTF_UI.value = config.scoreLimitHTF;
                livesUI.value = config.lives;
            }
            SetGameMode();
            SetDifficulty();
            SetBots();
            SetScoreLimitDM();
            SetScoreLimitHTF();
            SetLives();
        }

        public void SetGameMode()
        {
            GameData.gameMode = (GameMode)gameModeUI.value;
            deathMatchMenu.SetActive(false);
            survivalMenu.SetActive(false);
            holdTheFlagMenu.SetActive(false);
            if (GameData.gameMode == GameMode.SURVIVAL)
            {
                survivalMenu.SetActive(true);
            }
            if (GameData.gameMode == GameMode.DEATHMATCH)
            {
                deathMatchMenu.SetActive(true);
            }
            if (GameData.gameMode == GameMode.HOLD_THE_FLAG)
            {
                holdTheFlagMenu.SetActive(true);
            }
        }

        public void SetDifficulty()
        {
            GameData.difficulty = (Difficulty)difficultyUI.value;
        }

        public void SetBots()
        {
            GameData.bots = (int)botsUI.value;
        }

        public void SetScoreLimitDM()
        {
            GameData.scoreLimitDM = (int)scoreLimitDM_UI.value;
        }

        public void SetScoreLimitHTF()
        {
            GameData.scoreLimitHTF = (int)scoreLimitHTF_UI.value;
        }

        public void SetLives()
        {
            GameData.lives = (int)livesUI.value;
        }
        public void ValuesToData()
        {
            config = new ConfigData(GameData.gameMode, GameData.difficulty, GameData.bots, GameData.scoreLimitDM, GameData.lives, GameData.scoreLimitHTF);
            SaveSystem.SaveConfig(config);
        }
    }
}