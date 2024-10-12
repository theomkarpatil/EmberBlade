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
    public class WarlordAnimatorController : EnemyAnimatorController
    {
        private EnemyWarlord enemyWarlord;

        protected override void InitializeReferences()
        {
            enemy = GetComponentInParent<EnemyWarlord>();
            enemyWarlord = enemy as EnemyWarlord;
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            base.animator = animator;
        }


        protected override void InitializeVariables()
        {
            currentAttackTime = enemy.attackRate;
        }

        private void Update()
        {
            if (enemy.actionState == ECharacterActionState.HALTING)
            {
                currentHaltDuration += Time.deltaTime;
                if (currentHaltDuration >= haltToMoveDelay)
                {
                    enemy.SetState(ECharacterActionState.MOVING);
                    currentHaltDuration = 0.0f;
                }
            }

            if ((enemy.readyToAttack || enemyWarlord.readyToHeavyAttack) && (enemy.actionState == ECharacterActionState.MOVING || enemy.actionState == ECharacterActionState.HALTING))
            {
                currentAttackTime += Time.deltaTime;

                if (currentAttackTime >= enemy.attackRate)
                {
                    if (enemy.readyToAttack)
                        Attack();
                    else if (enemyWarlord.readyToHeavyAttack)
                        HeavyAttack();                        
                }
            }
        }

        private void FixedUpdate()
        {
            animator.SetFloat("speed", enemy.GetMovementSpeed());
        }

        private void HeavyAttack()
        {
            currentAttackTime = 0.0f;
            animator.SetBool("attacking", false);
            animator.Play("AttackSpin", 0);
        }
    }
}