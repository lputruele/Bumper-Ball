using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace BumperBallGame
{
    public class AudioManager : MonoBehaviour
    {
        public AudioMixer audioMixer;
        public bool CanPlay { get; set; }

        private void Awake()
        {
            CanPlay = true;
        }

        public void EnsureSFXDestruction(AudioSource source)
        {
            StartCoroutine("DelayedSFXDestruction", source);
        }

        public void HandleMultipleSounds()
        {
            CanPlay = false;
            StartCoroutine("Reset");
        }


        private IEnumerator DelayedSFXDestruction(AudioSource source)
        {
            while (source.isPlaying)
            {
                yield return null;
            }

            GameObject.Destroy(source.gameObject);
        }

        private IEnumerator Reset()
        {
            yield return new WaitForSeconds(.03f);
            CanPlay = true;
        }

    }
}