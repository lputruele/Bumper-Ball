using BumperBallGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagBehavior : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (transform.position == Vector3.zero)
        {
            FlagGrabbedEvent flagGrabbedEvt = Events.FlagGrabbedEvent;
            flagGrabbedEvt.grabber = other.gameObject;
            EventManager.Broadcast(flagGrabbedEvt);
        }
    }

}
