// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAudioType
{
    FOOTSTEPS,
    ENEMY_MELEE,
    PLAYER_MELEE,
    ENEMY_RANGED,
    PLAYER_RANGED,
    MELEE_HIT,
    RANGED_HIT,
    PLAYER_OVERHEATED
}

namespace Sora.Managers
{
    [System.Serializable]
    public class RollingSFX
    {
        public List<AudioClip> clips;
        private int currentIndex = 0;

        public void PlaySFX(AudioSource source)
        {
            source.PlayOneShot(clips[currentIndex % clips.Count]);
            currentIndex++;
        }
    }

    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private SerializedDictionary<string, AudioClip> bgMusic = new SerializedDictionary<string, AudioClip>();
        [SerializeField] private SerializedDictionary<EAudioType, AudioClip> oneShotSFX = new SerializedDictionary<EAudioType, AudioClip>();
        [SerializeField] private SerializedDictionary<EAudioType, RollingSFX> rollingSFX = new SerializedDictionary<EAudioType, RollingSFX>();
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();

            audioSource.volume = PlayerPrefs.GetFloat("music", 0.1f);
            PlayMusic();            
        }

        private void PlayMusic()
        {
            if (bgMusic.ContainsKey("theme"))
            {
                audioSource.clip = bgMusic["theme"];
                audioSource.Play();
            }
            else
                Debug.Log("AudioManager:: Theme song not added. Make sure the clip is added or the clip's name matches the following: \"theme\".");
        }

        public void PlayOneShotSFX(AudioSource source, EAudioType type)
        {
            source.PlayOneShot(oneShotSFX[type]);
        }

        public void PlayRollingSFX(AudioSource source, EAudioType type)
        {
            rollingSFX[type].PlaySFX(source);
        }

        public void PauseBackgroundMusic()
        {
            audioSource.Pause();
        }

        public void ResumeBackgroundMusic()
        {
            audioSource.Play();
        }

        public void SetBackgroundVolume(float volume)
        {
            audioSource.volume = volume;
        }

        public void MuteMusic(bool value)
        {
            if (!value)
            {
                audioSource.volume = 0.0f;
                PlayerPrefs.SetFloat("music", 0.0f);
            }
            else
            {
                audioSource.volume = 0.1f;
                PlayerPrefs.SetFloat("music", 0.1f);
            }
        }

        public void OnGameStart()
        {
            audioSource.clip = bgMusic["game"];
            audioSource.Play();
        }

        public void OnWaveCompletion()
        {
            audioSource.clip = bgMusic["wave"];
            audioSource.Play();
        }
    }
}