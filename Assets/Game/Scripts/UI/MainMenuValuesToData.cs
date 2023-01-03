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
        public Slider livesUI;

        public GameObject survivalMenu;
        public GameObject deathMatchMenu;

        private ConfigData config;

        private void Start()
        {
            config = SaveSystem.LoadConfig();
            if (config != null)
            {
                gameModeUI.value = (int)config.gameMode;
                botsUI.value = config.bots;
                fragLimitUI.value = config.fragLimit;
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
            if (GameData.gameMode == GameMode.SURVIVAL)
            {
                deathMatchMenu.SetActive(false);
                survivalMenu.SetActive(true);
            }
            else
            {
                deathMatchMenu.SetActive(true);
                survivalMenu.SetActive(false);
            }

        }

        public void SetBots()
        {
            GameData.bots = (int)botsUI.value;
        }

        public void SetFragLimit()
        {
            GameData.fragLimit = (int)fragLimitUI.value;
        }

        public void SetLives()
        {
            GameData.lives = (int)livesUI.value;
        }
        public void ValuesToData()
        {
            config = new ConfigData(GameData.gameMode, GameData.bots, GameData.fragLimit, GameData.lives);
            SaveSystem.SaveConfig(config);
        }
    }
}