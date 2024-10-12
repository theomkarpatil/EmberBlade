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
    public class RangedEnemyAnimatorController : EnemyAnimatorController
    {
        [Header("Ranged Variables")]
        [SerializeField] private GameObject projectile;
        [SerializeField] private float projectileForce;
        [SerializeField] private Transform projectileSpawnPosition;
        [SerializeField] private int projectilePoolSize;
        private int projectileIndex;
        private Quaternion projectileInitialRotation;

        private Transform player;

        private List<GameObject> projectilePool = new List<GameObject>();

        protected override void InitializeReferences()
        {
            enemy = GetComponentInParent<EnemyRanged>();
            animator = GetComponent<Animator>();
            player = FindObjectOfType<Gameplay.Player.TopDownPlayerController>().transform;
            audioSource = GetComponent<AudioSource>();
        }

        protected override void InitializeVariables()
        {
            currentAttackTime = enemy.attackRate;

            InstantiateProjectilePool();
            projectileIndex = 0;
            projectileInitialRotation = projectile.transform.rotation;
        }

        private void FixedUpdate()
        {
            animator.SetFloat("speed", enemy.GetMovementSpeed());
        }

        // called from animation
        private void LaunchProjectile()
        {
            GameObject _go = projectilePool[projectileIndex % projectilePoolSize];
            projectileIndex++;

            Managers.AudioManager.instance.PlayOneShotSFX(audioSource, EAudioType.ENEMY_RANGED);

            _go.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _go.transform.position = projectileSpawnPosition.position;
            _go.transform.rotation = projectileInitialRotation;

            Vector3 _dir = player.position - _go.transform.position;

            _go.SetActive(true);
            _go.GetComponent<Rigidbody>().AddForce(Time.deltaTime * 100.0f * projectileForce * _dir.normalized, ForceMode.Impulse);
        }

        protected override void SetAttackComplete()
        {
            animator.SetBool("attacking", false);
            enemy.SetState(ECharacterActionState.HALTING);
        }

        private void InstantiateProjectilePool()
        {
            for (int i = 0; i < projectilePoolSize; ++i)
            {
                GameObject _go = Instantiate(projectile, null);
                _go.SetActive(false);

                projectilePool.Add(_go);
            }
        }
    }
}