using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BumperBallGame
{


    public static class Events
    {
        public static PlayerDeathEvent PlayerDeathEvent = new PlayerDeathEvent();
    }


    public class PlayerDeathEvent : GameEvent 
    {
        public GameObject Player;
    }

}