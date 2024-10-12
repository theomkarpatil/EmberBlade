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
using UnityEngine.UI;

namespace Sora.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] protected Image fill;
        [SerializeField] protected Image fillBG;

        protected float fillAmount;
        protected const float fillDelta = 0.001f;
        protected Coroutine lerpFill;

        private float prevFillAmount;

        private void OnEnable()
        {
            fill.gameObject.SetActive(true);

            prevFillAmount = fillAmount;
        }

        private void FixedUpdate()
        {
            if (fillAmount != prevFillAmount)
            {
                OnFillAmountChange(fillAmount);
                prevFillAmount = fillAmount;
            }
        }


        protected void OnFillAmountChange(float value)
        {
            fill.fillAmount = value;

            if (lerpFill != null)
                StopCoroutine(lerpFill);

            lerpFill = StartCoroutine(LerpFillBG(value));
        }

        private IEnumerator LerpFillBG(float targetAmount)
        {
            while (fillBG.fillAmount >= targetAmount)
            {
                fillBG.fillAmount = Mathf.MoveTowards(fillBG.fillAmount, targetAmount, fillDelta);

                yield return null;
            }
        }
    }
}