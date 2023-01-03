using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BumperBallGame
{
    public static class Events
    {
        public static PlayerDeathEvent PlayerDeathEvent = new PlayerDeathEvent();
        public static GameOverEvent GameOverEvent = new GameOverEvent();
        public static PlayerOutOfLivesEvent PlayerOutOfLivesEvent = new PlayerOutOfLivesEvent();
    }


    public class PlayerDeathEvent : GameEvent 
    {
        public GameObject Killed;
        public GameObject Killer;
    }

    public class GameOverEvent : GameEvent
    {
        public GameObject Winner;
    }

    public class PlayerOutOfLivesEvent : GameEvent
    {
        public GameObject Killed;
    }

}