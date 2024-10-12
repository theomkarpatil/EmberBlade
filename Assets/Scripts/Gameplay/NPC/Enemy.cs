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
    public enum EEnemyType
    {
        SKELETON,
        RANGED,
        ARMOURED,
        WARLORD
    }

    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Enemy : MonoBehaviour, IHealth
    {
        [Header("Enemy Variables")]
        [SerializeField] protected EEnemyType enemyType;
        public float health = 100.0f;
        [SerializeField] protected float movementSpeed;
        public float attackRate;
        [SerializeField] protected float attackRange;
        [SerializeField] private float smoothingRate;

        protected Burnable burnable;

        protected EnemyAnimatorController animatorController;
        protected Transform player;
        protected Rigidbody rBody;

        // movement
        protected Vector3 moveDir;
        protected const float turnSpeed = 0.1f;
        protected const float movementSpeedAdjustment = 100.0f;

        // attacking
        public bool readyToAttack;

        public ECharacterLivingState livingState { get; protected set; }
        public ECharacterActionState actionState { get; protected set; }
        protected float initialHealth;

        private void OnEnable()
        {
            InitializeReferences();
            InitializeVariables();
        }

        protected virtual void InitializeVariables() { }

        protected virtual void InitializeReferences()
        {
            animatorController = GetComponentInChildren<EnemyAnimatorController>();
            player = FindObjectOfType<Player.TopDownPlayerController>().transform;
            rBody = GetComponent<Rigidbody>();
            burnable = GetComponentInChildren<Burnable>();
        }
        
        private void Start()
        {
            initialHealth = health;
        }

        private void FixedUpdate()
        {
            if (livingState == ECharacterLivingState.DEAD)
            {
                rBody.velocity = Vector3.zero;

                enabled = false;
                return;
            }
            else if (livingState == ECharacterLivingState.ALIVE && (actionState != ECharacterActionState.MOVING))
            {
                rBody.velocity = Vector3.zero;
            }
            else if (livingState == ECharacterLivingState.ALIVE && actionState == ECharacterActionState.MOVING)
            {
                MoveTowardsPlayer();
            }
        }

        protected virtual void MoveTowardsPlayer()
        {
            Vector3 _targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);

            if (Vector3.Distance(transform.position, _targetPosition) >= attackRange)
            {
                readyToAttack = false;

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

                readyToAttack = true;
                rBody.velocity *= 0.5f;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PlayerWeapon"))
            {
                TakeDamage(DamageValues.instance.playerMeleeDamage * DamageValues.instance.playerDamageMultiplier);
                Overheating.instance.IncrementOverHeating();

                if (Overheating.instance.overheated && burnable)
                    burnable.StartBurning();

                if(enemyType != EEnemyType.WARLORD)
                    animatorController.PerformHitAnimation();

                if (health <= 0)
                {
                    GetComponent<Collider>().enabled = false;
                    DisableAnimationsForDeath();

                    livingState = ECharacterLivingState.DEAD;
                    Managers.ScoreManager.instance.UpdateScore(enemyType);

                    Managers.EnemySpawner.instance.currentEnemies--;
                }
            }
            else if (other.CompareTag("PlayerProjectile"))
            {
                TakeDamage(DamageValues.instance.playerRangedDamage);

                if (enemyType != EEnemyType.WARLORD)
                    animatorController.PerformHitAnimation();

                if (health <= 0)
                {
                    GetComponent<Collider>().enabled = false;
                    DisableAnimationsForDeath();

                    Managers.ScoreManager.instance.UpdateScore(enemyType);
                    livingState = ECharacterLivingState.DEAD;
                    Managers.EnemySpawner.instance.currentEnemies--;
                }
            }
        }

        public void SetState(ECharacterLivingState state)
        {
            livingState = state;
        }

        public void SetState(ECharacterActionState state)
        {
            actionState = state;
        }

        public  EEnemyType GetEnemyType()
        {
            return enemyType;
        }

        public float GetHealthPercentage()
        {
            return health / initialHealth;
        }

        public float GetHealth()
        {
            return health;
        }

        public void TakeDamage(float damage)
        {
            health -= damage;
        }

        public void GainHealth(float value)
        {
            health += value;
        }

        public float GetMovementSpeed()
        {
            return rBody.velocity.magnitude / movementSpeed;
        }

        // called from animator
        public void SetSpawningComplete()
        {
            livingState = ECharacterLivingState.ALIVE;
            actionState = ECharacterActionState.HALTING;
        }

        protected virtual void DisableAnimationsForDeath()
        {
            animatorController.PlayDead();
            livingState = ECharacterLivingState.DEAD;
        }
    }
}