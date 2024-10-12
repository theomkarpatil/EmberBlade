// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sora.Gameplay.NPC;

namespace Sora.Gameplay
{
    public class DamageValues : Managers.Singleton<DamageValues>
    {
        [Header("Player Damage")]
        public float playerMeleeDamage;
        public float playerRangedDamage;
        public float playerDamageMultiplier;
        public float burnDamage;
        public float burnTick;
        public float burnDuration;

        [Header("Enemy Damage")]
        [SerializeField] private SerializedDictionary<EEnemyType, float> enemyDamage;

        public float GetEnemyDamage(EEnemyType enemyType)
        {
            if (enemyDamage.ContainsKey(enemyType))
                return enemyDamage[enemyType];
            else
                return 0.0f;
        }
    }
}