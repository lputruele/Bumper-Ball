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
        public int scoreLimit;
        public int lives;

        public ConfigData(GameMode gameMode, int bots, int scoreLimit, int lives)
        {
            this.gameMode = gameMode;
            this.bots = bots;
            this.scoreLimit = scoreLimit;
            this.lives = lives;
        }
    }
}