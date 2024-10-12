// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sora.Gameplay;
using UnityEngine.UI;

namespace Sora.UI
{
    public class HealthBarEnemy : HealthBar
    {
        protected Gameplay.NPC.Enemy enemy;

        private void Start()
        {
            enemy = transform.parent.GetComponentInParent<Gameplay.NPC.Enemy>();
        }

        private void Update()
        {
            fillAmount = enemy.GetHealthPercentage();

            if (enemy.GetHealthPercentage() <= 0)
                gameObject.SetActive(false);
        }
    }
}