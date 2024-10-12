// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sora.Gameplay
{
    public class Overheating : Managers.Singleton<Overheating>
    {
        [HideInInspector] public bool overheated;

        [SerializeField] private float overHeatThreshold;
        [SerializeField] private float heatingPerHit;
        [HideInInspector] public float overHeatingMultiplier;

        [SerializeField] private float resetCoolDown;
        [SerializeField] private float resetRate;
        [SerializeField] private float resetTick;

        [SerializeField] private Material glowMaterial;
        [SerializeField] private Color initialColor;
        [SerializeField] private Color overheatedColor;
        [SerializeField] private GameObject meltingParticles;
        [SerializeField] private Events.SoraEvent enableCrosshair;

        private float currentHeat;
        private float currentCD;
        private bool startHeatingCD;
        private Coroutine resetHeating;
        private float currentEmissionLerpDelta;
        private Player.PlayerAnimatorController player;

        private void OnEnable()
        {            
            player = GetComponentInChildren<Player.PlayerAnimatorController>();
            InitializeOverheating();
        }

        public void InitializeOverheating()
        {
            currentHeat = 0.0f;
            overHeatingMultiplier = 1.0f;
            currentEmissionLerpDelta = 0.0f;
            glowMaterial.SetColor("_BASE_COLOR", initialColor);
        }

        private void Update()
        {
            if (startHeatingCD)
            {
                currentCD += Time.deltaTime;

                if (currentCD >= resetCoolDown)
                {
                    startHeatingCD = false;

                    resetHeating = StartCoroutine(ResetHeating());
                }
            }

            if (currentHeat < overHeatThreshold)
            {
                meltingParticles.SetActive(false);
                enableCrosshair.InvokeEvent(this, false);

                player.SetOverHeating(false);
                overheated = false;
            }

            if (currentEmissionLerpDelta > 1.0f)
                currentEmissionLerpDelta = 1.0f;
        }

        public void IncrementOverHeating()
        {
            currentHeat += heatingPerHit;
            currentEmissionLerpDelta += heatingPerHit / 100.0f;
            Color _color = Color.Lerp(initialColor, overheatedColor, currentEmissionLerpDelta);
            glowMaterial.SetColor("_BASE_COLOR", _color);

            currentCD = 0.0f;
                        
            if(resetHeating != null)
                StopCoroutine(resetHeating);
            
            startHeatingCD = true;

            if (currentHeat >= overHeatThreshold)
            {
                currentHeat = overHeatThreshold;
                enableCrosshair.InvokeEvent(this, true);

                player.SetOverHeating(true);
                overheated = true;
                meltingParticles.SetActive(true);
            }
        }

        private IEnumerator ResetHeating()
        {
            while(currentHeat > 0.0f)
            {
                yield return new WaitForSecondsRealtime(resetTick);
                currentHeat -= resetRate;

                currentEmissionLerpDelta -= heatingPerHit / 100.0f;
                Color _color = Color.Lerp(initialColor, overheatedColor, currentEmissionLerpDelta);
                glowMaterial.SetColor("_BASE_COLOR", _color);


                if (currentEmissionLerpDelta < 0)
                    currentEmissionLerpDelta = 0.0f;
            }
        }

        private void OnDisable()
        {
            glowMaterial.SetColor("_BASE_COLOR", initialColor);
        }



        public float GetOverheatingPercentage()
        {
            return currentHeat / overHeatThreshold;
        }    
    }
}