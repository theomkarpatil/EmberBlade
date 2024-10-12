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

namespace Sora 
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float timeToDisable;

        private Coroutine disableRoutine;
        private void OnEnable()
        {
            disableRoutine = StartCoroutine(DisableObject());
        }

        private IEnumerator DisableObject()
        {
            yield return new WaitForSecondsRealtime(timeToDisable);

            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            if (disableRoutine != null)
                StopCoroutine(disableRoutine);
        }
    }
}