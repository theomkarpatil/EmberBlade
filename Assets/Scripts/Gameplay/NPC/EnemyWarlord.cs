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
    public class EnemyWarlord : Enemy
    {
        public bool readyToHeavyAttack;

        protected override void InitializeVariables()
        {
            readyToHeavyAttack = false;
            livingState = ECharacterLivingState.SPAWNING;
        }

        protected override void MoveTowardsPlayer()
        {
            Vector3 _targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);

            if (Vector3.Distance(transform.position, _targetPosition) >= attackRange)
            {
                readyToAttack = false;
                readyToHeavyAttack = false;

                moveDir = (_targetPosition - transform.position).normalized;

                Quaternion _targetRotation = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, turnSpeed);

                rBody.velocity = Time.deltaTime * movementSpeed * movementSpeedAdjustment * moveDir;
            }
            else
            {
                moveDir = (_targetPosition - transform.position).normalized;
                Quaternion _targetRotation = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, turnSpeed);

                if (health >= initialHealth / 2.0f)
                    readyToAttack = true;
                else
                    readyToHeavyAttack = true;

                rBody.velocity *= 0.5f;
            }
        }
    }
}