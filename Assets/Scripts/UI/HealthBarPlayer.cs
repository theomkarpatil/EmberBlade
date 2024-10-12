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

namespace Sora.UI
{
    public class HealthBarPlayer : HealthBar
    {
        [SerializeField] private Color fullColor;
        [SerializeField] private Color mediumColor;
        [SerializeField] private Color lowColor;

        private Animator anim;
        private Gameplay.Player.TopDownPlayerController player;

        private void Start()
        {
            anim = GetComponent<Animator>();
            player = FindObjectOfType<Gameplay.Player.TopDownPlayerController>();
        }

        private void Update()
        {
            fillAmount = player.GetHealthPercentage();

            if (fill.fillAmount >= 0.6f)
                fill.color = fullColor;
            else if (fill.fillAmount <= 0.6f && fill.fillAmount >= 0.3f)
                fill.color = mediumColor;
            else if (fill.fillAmount <= 0.3f)
                fill.color = lowColor;

            if (fillAmount <= 0.3f)
            {
                anim.Play("Indicate", 0);
            }
            else
                anim.Play("Default", 0);
        }
    }
}