// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sora.Gameplay.NPC
{   
    public class EnemyArmoured : Enemy
    {
        private ArmouredAnimatorController armouredAnimatorController;

        [Header("Armoured Variables")]
        public float armouredHealth = 50.0f;

        private bool isArmoured;
        private float initialArmouredHealth;

        protected override void InitializeReferences()
        {
            animatorController = GetComponentInChildren<ArmouredAnimatorController>();
            armouredAnimatorController = animatorController as ArmouredAnimatorController;

            player = FindObjectOfType<Player.TopDownPlayerController>().transform;
            rBody = GetComponent<Rigidbody>();
            burnable = GetComponentInChildren<Burnable>();
        }

        protected override void InitializeVariables()
        {
            isArmoured = true;
            initialArmouredHealth = armouredHealth;
            livingState = ECharacterLivingState.SPAWNING;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PlayerWeapon"))
            {
                if (isArmoured)
                {
                    armouredHealth -= DamageValues.instance.playerMeleeDamage;
                    Overheating.instance.IncrementOverHeating();

                    if (Overheating.instance.overheated && burnable)
                        burnable.StartBurning();

                    if (armouredHealth <= 0)
                    {
                        armouredAnimatorController.DeshieldEnemy();
                        isArmoured = false;
                    }
                }
                else
                {
                    TakeDamage(DamageValues.instance.playerMeleeDamage);
                    actionState = ECharacterActionState.TAKING_HIT;
                    animatorController.PerformHitAnimation();
                    Overheating.instance.IncrementOverHeating();

                    if (health <= 0)
                    {
                        GetComponent<Collider>().enabled = false;
                        DisableAnimationsForDeath();

                        Managers.EnemySpawner.instance.currentEnemies--;
                        Managers.ScoreManager.instance.UpdateScore(enemyType);

                        livingState = ECharacterLivingState.DEAD;
                    }
                }
            }
            else if (other.CompareTag("PlayerProjectile"))
            {
                if (isArmoured)
                {
                    armouredHealth -= DamageValues.instance.playerRangedDamage;

                    if (armouredHealth <= 0)
                    {
                        armouredAnimatorController.DeshieldEnemy();
                        isArmoured = false;
                    }
                }
                else
                {
                    TakeDamage(DamageValues.instance.playerRangedDamage);

                    if (health <= 0)
                    {
                        GetComponent<Collider>().enabled = false;
                        DisableAnimationsForDeath();

                        Managers.EnemySpawner.instance.currentEnemies--;
                        Managers.ScoreManager.instance.UpdateScore(enemyType);

                        livingState = ECharacterLivingState.DEAD;
                    }
                }
            }
        }

        public float GetArmourPercentage()
        {
            return armouredHealth / initialArmouredHealth;
        }

        protected override void DisableAnimationsForDeath()
        {
            animatorController.PlayDead();
            livingState = ECharacterLivingState.DEAD;
        }
    }
}