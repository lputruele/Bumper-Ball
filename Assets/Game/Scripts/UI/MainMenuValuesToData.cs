using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BumperBallGame;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;

namespace Game.UI
{
    public class MainMenuValuesToData : MonoBehaviour
    {
        public TMP_Dropdown gameModeUI;
        public Slider botsUI;
        public Slider fragLimitUI;
        public Slider scoreFlagLimitUI;
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
                botsUI.value = config.bots;
                fragLimitUI.value = config.scoreLimit;
                scoreFlagLimitUI.value = config.scoreLimit;
                livesUI.value = config.lives;
            }
            SetGameMode();
            SetBots();
            SetFragLimit();
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

        public void SetBots()
        {
            GameData.bots = (int)botsUI.value;
        }

        public void SetFragLimit()
        {
            if (GameData.gameMode == GameMode.DEATHMATCH)
            {
                GameData.scoreLimit = (int)fragLimitUI.value;
            }
            else
            {
                GameData.scoreLimit = (int)scoreFlagLimitUI.value;
            }
        }

        public void SetLives()
        {
            GameData.lives = (int)livesUI.value;
        }
        public void ValuesToData()
        {
            config = new ConfigData(GameData.gameMode, GameData.bots, GameData.scoreLimit, GameData.lives);
            SaveSystem.SaveConfig(config);
        }
    }
}