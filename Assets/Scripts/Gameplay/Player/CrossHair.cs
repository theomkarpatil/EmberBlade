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
    public class CrossHair : MonoBehaviour
    {
        private PlayerInput pInput;

        private void Start()
        {
            pInput = FindObjectOfType<PlayerInput>();
        }

        private void Update()
        {
            transform.position = pInput.lookDir;
        }
    }
}