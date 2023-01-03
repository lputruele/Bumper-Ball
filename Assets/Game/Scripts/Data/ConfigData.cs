using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BumperBallGame
{
    [System.Serializable]
    public class ConfigData
    {
        public GameMode gameMode;
        public int bots;
        public int fragLimit;
        public int lives;

        public ConfigData(GameMode gameMode, int bots, int fragLimit, int lives)
        {
            this.gameMode = gameMode;
            this.bots = bots;
            this.fragLimit = fragLimit;
            this.lives = lives;
        }
    }
}