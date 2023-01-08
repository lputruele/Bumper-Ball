using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Audio
{
    public class InGameSounds : MonoBehaviour
    {
        public static InGameSounds Instance { get; private set; }

        public AudioClip BumpSound;
        public AudioClip FlagSound;
        public AudioClip SpawnSound;
        public AudioClip VictorySound;

        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }
    }
}