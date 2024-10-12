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

namespace Sora.Gameplay.Player
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        [SerializeField] private InputSystem.GameplayInputReader gpInputReader;
        [SerializeField] private float comboInterval;
        [SerializeField] private GameObject dashTrail;        

        [Space]
        [SerializeField] private Collider swordCollider;
        [SerializeField] private GameObject projectile;
        [SerializeField] private GameObject slashTrail;
        [SerializeField] private GameObject slashTrailOverheated;
        [SerializeField] private Transform projectileSpawnPosition;
        [SerializeField] private float projectileForce;
        [SerializeField] private int projectilePoolSize;
        private List<GameObject> projectilePool = new List<GameObject>();
        private int projectileIndex;

        private Animator animator;
        private AudioSource audioSource;
        private TopDownPlayerController playerController;
        private bool overHeated;

        private int meleeIndex;
        private bool attacking;
        private bool attackingRange;
        private float attackRate = 0.4f;
        private float currentAttackTime;
        private bool startComboTimer;
        private float currentComboInterval;

        private void OnEnable()
        {
            animator = GetComponent<Animator>();
            playerController = GetComponentInParent<TopDownPlayerController>();
            audioSource = GetComponent<AudioSource>();

            gpInputReader.meleeAttackEvent += MeleeAttack;
            gpInputReader.rangedAttackEvent += RangedAttack;
            gpInputReader.dashPerformedEvent += playerController.Dash;

            meleeIndex = 1;
            projectileIndex = 0;
            attacking = false;

            InstantiateProjectilePool();
        }

        public void OnGameOver()
        {
            gpInputReader.meleeAttackEvent -= MeleeAttack;
            gpInputReader.rangedAttackEvent -= RangedAttack;
            gpInputReader.dashPerformedEvent -= playerController.Dash;
        }

        private void Update()
        {
            animator.SetFloat("speed", playerController.GetSpeedNormalized());

            if (startComboTimer)
            {
                currentComboInterval += Time.deltaTime;

                if (currentComboInterval >= comboInterval)
                {
                    startComboTimer = false;
                    meleeIndex = 1;
                }
            }

            if(attackingRange)
            {
                currentAttackTime += Time.deltaTime;

                if (currentAttackTime >= attackRate)
                {
                    attacking = false;
                    attackingRange = false;
                }
            }
        }

        private void MeleeAttack()
        {
            if (attacking || GameManager.instance.gameState != EGameState.RUNNING)
                return;

            currentComboInterval = 0.0f;
            startComboTimer = true;

            AudioManager.instance.PlayRollingSFX(audioSource, EAudioType.PLAYER_MELEE);
            animator.Play("Melee" + meleeIndex, 0);
            animator.SetBool("attacking", true);
            attacking = true;

            meleeIndex++;
            if (meleeIndex >= 4)
                meleeIndex = 1;
        }
        
        private void RangedAttack()
        {
            if (attacking || GameManager.instance.gameState != EGameState.RUNNING)
                return;

            if(!overHeated)
            {
                UI.OverheatingUI.instance.ShowUnderheatedError();
                return;
            }

            animator.Play("Ranged", 0);
            animator.SetBool("attacking", true);
            attacking = true;
            attackingRange = true;
            currentAttackTime = 0.0f;
        }

        //called from animations
        private void LaunchProjectile()
        {
            GameObject _go = projectilePool[projectileIndex % projectilePoolSize];
            projectileIndex++;
            AudioManager.instance.PlayOneShotSFX(audioSource, EAudioType.PLAYER_RANGED);

            _go.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _go.transform.position = projectileSpawnPosition.position;
            _go.transform.rotation = projectileSpawnPosition.rotation;

            _go.SetActive(true);
            _go.GetComponent<Rigidbody>().AddForce(Time.deltaTime * 100.0f * projectileForce * transform.parent.forward, ForceMode.Impulse);
        }

        //called from animations
        private void AttackComplete()
        {
            attacking = false;
            animator.SetBool("attacking", false);
            swordCollider.enabled = false;

            slashTrail.SetActive(false);
            slashTrailOverheated.SetActive(false);
        }

        //called from animations
        private void AttackStart()
        {
            swordCollider.enabled = true;
            if(overHeated)
                slashTrailOverheated.SetActive(true);
            else
                slashTrail.SetActive(true);
        }

        public void Dash(Vector3 lookDir, Vector3 moveDir)
        {
            dashTrail.SetActive(true);
            AttackComplete();

            string _animation = GetDashAnimation(lookDir, moveDir);
            animator.Play(_animation, 0);
        }
        
        public void SetOverHeating(bool value)
        {
            overHeated = value;
        }

        private void InstantiateProjectilePool()
        {
            for(int i = 0; i < projectilePoolSize; ++i)
            {
                GameObject _go = Instantiate(projectile, null);
                _go.SetActive(false);

                projectilePool.Add(_go);
            }
        }

        private string GetDashAnimation(Vector3 lookDir, Vector3 moveDir)
        {
            animator.SetBool("dashing", true);

            float _angle = Vector3.SignedAngle(lookDir, moveDir, Vector3.up);

            if(moveDir.x == 0 && moveDir.z == 0)
                return "DashForward";


            if (_angle > -45 && _angle <= 45)
            {
                return "DashForward";
            }
            else if (_angle > 45 && _angle <= 135)
            {
                return "DashRight";
            }
            else if (_angle > 135 || _angle <= -135)
            {
                return "DashBackward";
            }
            else if (_angle > -135 && _angle <= -45)
            {
                return "DashLeft";
            }

            // Default to dashForward if something goes wrong
            return "dashForward";
        }

        // called from animations
        private void DisableDashing()
        {
            animator.SetBool("dashing", false);
            playerController.dashing = false;
            dashTrail.SetActive(false);
        }

        private void PlayFootStep()
        {
            AudioManager.instance.PlayRollingSFX(audioSource, EAudioType.FOOTSTEPS);
        }
    }
}