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
    public class EnemyAnimatorController : MonoBehaviour
    {
        [SerializeField] protected float haltToMoveDelay;
        protected float currentHaltDuration;
        [HideInInspector] public bool dead;
        
        protected Enemy enemy;
        protected Animator animator;
        protected AudioSource audioSource;

        // attacking
        [SerializeField] private Collider weaponCollider;
        protected float currentAttackTime;

        private void OnEnable()
        {
            InitializeReferences();
            InitializeVariables();
        }

        protected virtual void InitializeReferences()
        {
            enemy = GetComponentInParent<Enemy>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }

        protected virtual void InitializeVariables()
        {
            currentAttackTime = enemy.attackRate;
            currentHaltDuration = 0.0f;
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

            if (enemy.readyToAttack && (enemy.actionState == ECharacterActionState.MOVING || enemy.actionState == ECharacterActionState.HALTING))
            {
                currentAttackTime += Time.deltaTime;

                if (currentAttackTime >= enemy.attackRate)
                    Attack();
            }
        }

        private void FixedUpdate()
        {
            animator.SetFloat("speed", enemy.GetMovementSpeed());
        }

        protected void Attack()
        {
            currentAttackTime = 0.0f;
            animator.SetBool("attacking", false);
            animator.Play("Attack", 0);

            Managers.AudioManager.instance.PlayRollingSFX(audioSource, EAudioType.ENEMY_MELEE);
        }

        // called from animation
        protected virtual void SetAttackComplete()
        {
            animator.SetBool("attacking", false);
            weaponCollider.enabled = false;
            enemy.SetState(ECharacterActionState.HALTING);
        }

        private void SetAttackStart()
        {
            enemy.SetState(ECharacterActionState.ATTACKING);
            weaponCollider.enabled = true;
        }

        public void PerformHitAnimation()
        {
            if(enemy.actionState == ECharacterActionState.ATTACKING)
                SetAttackComplete();

            Managers.AudioManager.instance.PlayRollingSFX(audioSource, EAudioType.MELEE_HIT);

            enemy.SetState(ECharacterActionState.TAKING_HIT);
            animator.Play("TakeHit", 0);
        }

        public void PlayDead()
        {
            animator.SetBool("dead", true);
            animator.Play("Death", 0);
            enabled = false;
        }

        private void SetSpawningComplete()
        {
            enemy.SetSpawningComplete();
        }

        private void SetHitComplete()
        {
            enemy.SetState(ECharacterActionState.HALTING);

            if (enemy.livingState == ECharacterLivingState.SPAWNING)
                enemy.SetState(ECharacterLivingState.ALIVE);
        }

        private void PlayFootStep()
        {
            Managers.AudioManager.instance.PlayRollingSFX(audioSource, EAudioType.FOOTSTEPS);
        }
    }
}