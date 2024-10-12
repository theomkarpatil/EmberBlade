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
    [RequireComponent(typeof(Rigidbody))]
    public class TopDownPlayerController : MonoBehaviour, IHealth
    {   
        [Header("Movement Variables")]
        [SerializeField] private float acceleration;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float maxSprintSpeed;
        [SerializeField] private float smoothingRate;
        [SerializeField] private float decellerationRate;
        [SerializeField] private float turnSpeed;

        [Header("Dash Variables")]
        [SerializeField] private float dashForce;
        [SerializeField] private float dashCooldown;
        [HideInInspector] public bool dashing;
        private bool dashOnCD;
        private float currentDashTime;

        [Space]
        public float health;
        [SerializeField] private Events.SoraEvent takeDamage;
        [HideInInspector] public float initialHealth;

        private Vector3 targetVelocity;
        private Vector3 currentVelocity;
        private Vector3 moveDir;
        private Vector2 lookDir;

        private PlayerInput pInput;
        private Rigidbody rBody;
        private Camera mainCam;
        private PlayerAnimatorController animatorController;
        private AudioSource audioSource;

        private const float timeScaleAdjustment = 100.0f;

        private void OnEnable()
        {
            pInput = GetComponent<PlayerInput>();
            rBody = GetComponent<Rigidbody>();
            animatorController = GetComponentInChildren<PlayerAnimatorController>();
            audioSource = GetComponentInChildren<AudioSource>();
            mainCam = Camera.main;

            initialHealth = health;
        }

        private void Update()
        {
            if(dashOnCD)
            {
                currentDashTime += Time.deltaTime;

                if (currentDashTime >= dashCooldown)
                    dashOnCD = false;
            }
        }

        private void FixedUpdate()
        {
            moveDir = new Vector3(pInput.movementDir.x, 0.0f, pInput.movementDir.y);
            lookDir = pInput.lookDir;            
            
            LookAtMouse();
            if (moveDir.magnitude >= 0.01f && !dashing)
                MovePlayer();
            else
                DecelleratePlayer();                        
        }

        private void MovePlayer()
        {
            targetVelocity = Vector3.SmoothDamp(rBody.velocity, acceleration * Time.fixedDeltaTime * timeScaleAdjustment * moveDir, ref currentVelocity, smoothingRate);

            if (pInput.sprinting && targetVelocity.magnitude <= maxSprintSpeed)
                rBody.velocity = targetVelocity;
            else if (targetVelocity.magnitude <= maxSpeed)
                rBody.velocity = targetVelocity;
        }

        private void DecelleratePlayer()
        {
            rBody.velocity *= decellerationRate;
        }

        private void LookAtMouse()
        {
            Vector3 _mouseToWorld = mainCam.ScreenToWorldPoint(new Vector3(lookDir.x, lookDir.y, mainCam.transform.position.y));

            Vector3 _dir = (_mouseToWorld - transform.position).normalized;
            _dir.y = 0.0f;

            Quaternion _targetRotation = Quaternion.LookRotation(_dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, turnSpeed);
        }

        public void Dash()
        {
            if (!dashOnCD)
            {
                currentDashTime = 0.0f;
                dashing = true;
                dashOnCD = true;

                if (moveDir.x == 0 && moveDir.z == 0)
                    rBody.AddForce(Time.fixedDeltaTime * timeScaleAdjustment * dashForce * transform.forward, ForceMode.Impulse);
                else
                    rBody.AddForce(Time.fixedDeltaTime * timeScaleAdjustment * dashForce * moveDir, ForceMode.Impulse);

                animatorController.Dash(lookDir, moveDir);
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("EnemyWeapon"))
            {
                TakeDamage(DamageValues.instance.GetEnemyDamage(other.GetComponentInParent<NPC.Enemy>().GetEnemyType()));
                AudioManager.instance.PlayRollingSFX(audioSource, EAudioType.MELEE_HIT);
                takeDamage.InvokeEvent();

                if (health <= 0.0f)
                    Managers.GameManager.instance.OnGameOver();
            }

        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("EnemyProjectile"))
            {
                TakeDamage(DamageValues.instance.GetEnemyDamage(NPC.EEnemyType.RANGED));
                AudioManager.instance.PlayOneShotSFX(audioSource, EAudioType.RANGED_HIT);
                takeDamage.InvokeEvent();

                if (health <= 0.0f)
                    Managers.GameManager.instance.OnGameOver();
            }
        }

        public float GetSpeedNormalized()
        {
            return rBody.velocity.magnitude / maxSprintSpeed;
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

        public float GetHealthPercentage()
        {
            return health / initialHealth;
        }

        public void UpdateMaxHealth(float value)
        {
            initialHealth *= value;
        }

        public void ModifyDashCooldown(float value)
        {
            dashCooldown += value;
        }

        public void ModifyMovementSpeed(float value)
        {
            maxSpeed *= value;
        }
    }
}