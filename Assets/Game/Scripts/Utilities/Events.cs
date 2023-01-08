using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events
{
    public static PlayerDeathEvent PlayerDeathEvent = new PlayerDeathEvent();
    public static GameOverEvent GameOverEvent = new GameOverEvent();
    public static PlayerOutOfLivesEvent PlayerOutOfLivesEvent = new PlayerOutOfLivesEvent();
    public static UpdateScoreEvent UpdateScoreEvent = new UpdateScoreEvent();
    public static BumpEvent BumpEvent = new BumpEvent();
    public static FlagGrabbedEvent FlagGrabbedEvent = new FlagGrabbedEvent();
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

public class UpdateScoreEvent : GameEvent
{
}

public class BumpEvent : GameEvent
{
    public GameObject Bumped;
    public GameObject Bumper;
}

public class FlagGrabbedEvent : GameEvent
{
    public GameObject Grabber;
}
