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

namespace Sora.UI
{
    public class ArmourBarEnemy : HealthBarEnemy
    {
        private Gameplay.NPC.EnemyArmoured armouredEnemy;

        private void Start()
        {
            enemy = transform.parent.GetComponentInParent<Gameplay.NPC.EnemyArmoured>();
            armouredEnemy = enemy as Gameplay.NPC.EnemyArmoured;
        }

        private void Update()
        {
            fillAmount = armouredEnemy.GetArmourPercentage();

            if (armouredEnemy.GetArmourPercentage() <= 0)
                gameObject.SetActive(false);
        }
    }
}