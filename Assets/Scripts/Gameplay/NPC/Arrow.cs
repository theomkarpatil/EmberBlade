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

namespace Sora.Gameplay.NPC
{
    /// You may delete all of the stuff inside here. 
    /// Just remember to stick to the formating
    public class Arrow : MonoBehaviour
    {
        private Rigidbody rBody;

        private void OnEnable()
        {
            transform.parent = null;
        }

        private void Start()
        {
            rBody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            transform.parent = collision.transform;
            rBody.velocity = Vector3.zero;
            StartCoroutine(DisableArrow());
        }

        private IEnumerator DisableArrow()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            gameObject.SetActive(false);
        }
    }
}