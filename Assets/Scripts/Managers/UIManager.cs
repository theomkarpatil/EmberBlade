// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Sora.Managers
{
    
    public class UIManager : Singleton<UIManager>
    {
        [Header("Score UI")]
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Animator scoreAnimator;

        [Space]
        [SerializeField] private GameObject crossHair;

        [Header("Score UI")]
        [SerializeField] private GameObject pauseUI;
        
        [Header("Time UI")]
        [SerializeField] private TMP_Text timer;

        [Header("GameEnd UI")]
        [SerializeField] private GameObject gameOverUI;
        [SerializeField] private GameObject waveUI;
        [SerializeField] private TMP_Text waveText;
        [SerializeField] private TMP_Text timeSavedText;
        [SerializeField] private TMP_Text finalScoreText;

        public void ShowWaveUI()
        {
            waveUI.SetActive(true);
            waveText.text = "Wave " + (EnemySpawner.instance.waveIndex + 1) + " Complete!";
            timeSavedText.text = "You saved " + TimeKeeper.instance.currentTime.ToString("0") + " seconds!";
        }

        public void ShowGameOverUI(Component invoker, object data)
        {
            gameOverUI.SetActive(true);
            finalScoreText.text = "Your Final Score\n" + GameManager.instance.GetFinalScore().ToString();
        }

        public void UpdateScoreUI()
        {
            scoreText.text = ScoreManager.instance.GetScore().ToString();
            scoreAnimator.Play("Punch", 0);
        }

        public void UpdateTimer(Component invoker, object data)
        {
            timer.text = TimeKeeper.instance.currentTime.ToString("0");
        }

        public void ShowPauseUI(Component invoker, object data)            
        {
            pauseUI.SetActive(true);
        }

        public void DisablePauseUI()
        {
            pauseUI.SetActive(false);
        }

        public void CrossHairEnabler(Component invoker, object data)
        {
            bool active = (bool)data;

            crossHair.SetActive(active);
        }
    }
}