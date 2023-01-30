using Game.Persistence;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public AudioMixer audioMixer;

        public void EnsureSFXDestruction(AudioSource source)
        {
            StartCoroutine("DelayedSFXDestruction", source);
        }


        private IEnumerator DelayedSFXDestruction(AudioSource source)
        {
            while (source.isPlaying)
            {
                yield return null;
            }

            GameObject.Destroy(source.gameObject);
        }


    }
}