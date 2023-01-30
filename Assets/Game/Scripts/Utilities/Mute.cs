using Game.Persistence;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mute : MonoBehaviour
{
    public AudioSource musicTrack;

    private void Awake()
    {
        MuteMusic();
    }
    public void MuteMusic()
    {
        musicTrack.mute = !GameData.music;
    }
}
