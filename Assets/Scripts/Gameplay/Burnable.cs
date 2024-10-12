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

namespace Sora.Gameplay
{
    [RequireComponent(typeof(IHealth))]
    public class Burnable : MonoBehaviour
    {
        [SerializeField] private GameObject burningParticles;

        private float currentBurnDuration;
        private Coroutine burningCoroutine;
        private bool burning;

        private IHealth healthComponent;

        private void OnEnable()
        {
            healthComponent = GetComponent<IHealth>();
        }

        private void FixedUpdate()
        {
            if (burning)
            {
                currentBurnDuration += Time.fixedDeltaTime;
            }
        }

        private IEnumerator TakeBurnDamage()
        {
            while (currentBurnDuration <= DamageValues.instance.burnDuration)
            {
                yield return new WaitForSeconds(DamageValues.instance.burnTick);

                healthComponent.TakeDamage(DamageValues.instance.burnDamage);
            }
            burningParticles.SetActive(false);
            burning = false;
        }

        public void StartBurning()
        {
            burningParticles.SetActive(true);
            burning = true;

            if (burningCoroutine != null)
                StopCoroutine(burningCoroutine);

            currentBurnDuration = 0.0f;
            burningCoroutine = StartCoroutine(TakeBurnDamage());
        }
    }
}