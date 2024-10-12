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
    public class ScoreManager : Singleton<ScoreManager>
    {
        private int score;
        [Header("Enemy Scores")]
        [SerializeField] private SerializedDictionary<Gameplay.NPC.EEnemyType, int> enemyScores;
        [Space]
        [SerializeField] private int waveCompletionScore;

        public int GetScore()
        {
            return score;
        }

        public void UpdateScore(Gameplay.NPC.EEnemyType enemyType)
        {
            score += enemyScores[enemyType];
            OnScoreUpdate();
        }

        public void UpdateScoreOnWaveCompletion()
        {
            score += waveCompletionScore;
            OnScoreUpdate();
        }

        private void OnScoreUpdate()
        {
            UIManager.instance.UpdateScoreUI();
        }
    }
}