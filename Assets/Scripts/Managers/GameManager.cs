// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sora.Events;
using Sora.Utility;

namespace Sora.Managers
{
    public enum EGameState
    {
        MAIN_MENU,
        RUNNING,
        FACET_SELECTION,
        PAUSED
    }

    public class GameManager : Singleton<GameManager>
    {
        [HideInInspector] public EGameState gameState;
        [SerializeField] private Events.SoraEvent gameStartEvent;
        [SerializeField] private Events.SoraEvent gameOverEvent;
        [SerializeField] private Events.SoraEvent gamePausedEvent;

        private void OnEnable()
        {
            Time.timeScale = 0.0f;
            gameState = EGameState.MAIN_MENU;
        }

        public void LoadGameScene()
        {
            SceneManager.instance.LoadGameScene(1);
        }

        public void OnGameOver()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0.0f;

            gameOverEvent.InvokeEvent();
        }

        public void OnPauseGame()
        {
            Time.timeScale = 0.0f;

            gameState = EGameState.PAUSED;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            gamePausedEvent.InvokeEvent();
        }

        public void ResumeGame()
        {
            Time.timeScale = 1.0f;

            gameState = EGameState.RUNNING;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;

            TimeKeeper.instance.OnGameResume();
            UIManager.instance.DisablePauseUI();
        }

        public void OnGameStart()
        {
            gameState = EGameState.RUNNING;
            gameStartEvent.InvokeEvent();
            Time.timeScale = 1.0f;

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }

        public float GetFinalScore()
        {
            return ScoreManager.instance.GetScore() * Mathf.Max(TimeKeeper.instance.currentTime, 1) * (EnemySpawner.instance.waveIndex + 1);
        }

        public void RestartGame()
        {
            Managers.SceneManager.instance.LoadScene(0);
        }

        public void PauseGame()
        {
            if (gameState == EGameState.MAIN_MENU)
                return;

            if (gameState == EGameState.PAUSED)
                ResumeGame();
            else
                OnPauseGame();
        }
    }
}