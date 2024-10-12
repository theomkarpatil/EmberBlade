// Developed by Sora Arts
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using UnityEngine;
using Sora.InputSystem;

namespace Sora.Gameplay.Player
{
	public enum E_TestPlayerStates
    {
		IDLE,
		WALKING,
		RUNNING
    }

    public class PlayerInput : MonoBehaviour
    {
		[Header("Test Input")]
		// sample InputSytem usage
		[SerializeField] private GameplayInputReader gpInputReader;
		[HideInInspector] public Vector2 movementDir;
		[HideInInspector] public Vector2 lookDir;
		public bool sprinting;

		private void OnEnable()
        {
			gpInputReader.Enable();

			// Input system usage
			gpInputReader.moveEvent += OnMove;
			gpInputReader.lookEvent += OnLook;
			gpInputReader.sprintEvent += SprintStart;
			gpInputReader.sprintCanceledEvent += SprintStopped;
		}

		private void OnMove(Vector2 val)
        {
			movementDir = val;
        }

		private void OnLook(Vector2 val)
        {
			lookDir = val;
        }

		private void SprintStart()
        {
			sprinting = true;
        }

		private void SprintStopped()
        {
			sprinting = false;
        }
	}
}

