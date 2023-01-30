using Game.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Persistence
{
    [System.Serializable]
    public class ConfigData
    {
        public GameMode gameMode;
        public Difficulty difficulty;
        public int bots;
        public int scoreLimitDM;
        public int scoreLimitHTF;
        public int lives;
        public bool sound;
        public bool music;

        public ConfigData(GameMode gameMode, Difficulty difficulty, int bots, int scoreLimitDM, int lives, int scoreLimitHTF, bool sound, bool music)
        {
            this.gameMode = gameMode;
            this.difficulty = difficulty;
            this.bots = bots;
            this.scoreLimitDM = scoreLimitDM;
            this.scoreLimitHTF = scoreLimitHTF;
            this.lives = lives;
            this.sound = sound;
            this.music = music;
        }
    }
}