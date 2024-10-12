// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sora.Managers;

namespace Sora.Managers
{
    public class TimeKeeper : Singleton<TimeKeeper>
    {
        [SerializeField] private float timePerLevel;
        [SerializeField] private Events.SoraEvent updateTime;
        [HideInInspector] public float currentTime;

        private Coroutine timer;
        
        public void OnGameStart(Component invoker, object data)
        {
            currentTime = timePerLevel;
            timer = StartCoroutine(Timer());
        }

        public void OnGamePaused(Component invoker, object data)
        {
            StopCoroutine(timer);
        }

        public void OnGameResume()
        {
            timer = StartCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            while (currentTime >= 0.0f)
            {
                currentTime -= 1.0f;
                updateTime.InvokeEvent();

                if (currentTime <= 0.0f)
                {
                    GameManager.instance.OnGameOver();
                    StopCoroutine(timer);
                }
                yield return new WaitForSeconds(1.0f);
            }
        }

        public void UpdateTimeForNextWave()
        {
            currentTime += timePerLevel;
        }

        public void UpdateCurrentTime()
        {
            currentTime += timePerLevel;
        }
    }
}