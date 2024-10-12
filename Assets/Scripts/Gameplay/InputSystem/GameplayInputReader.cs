// Developed by Sora Arts
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


namespace Sora.InputSystem
{
	[CreateAssetMenu(fileName = "GameplayInputReader", menuName = "Sora /Input System /GameplayInputReader")]
	public class GameplayInputReader : ScriptableObject, KeyBindings.IPlayerMovementActions, KeyBindings.IInteractionsActions, KeyBindings.ICombatActions
	{
		// Movement Events
		public event UnityAction<Vector2> moveEvent;
		public event UnityAction sprintEvent;
		public event UnityAction sprintCanceledEvent;
		public event UnityAction dashPerformedEvent;

		// Look Events
		public event UnityAction<Vector2> lookEvent;

		// Interaction Events
		public event UnityAction pickUpEvent;

		// Combat Events
		public event UnityAction meleeAttackEvent;
		public event UnityAction rangedAttackEvent;

		[Space]
		[SerializeField] private BindingsContainer bindingsContainer;

		public void Enable()
        {
			bindingsContainer.keyBindings.PlayerMovement.Enable();
			bindingsContainer.keyBindings.PlayerMovement.SetCallbacks(this);

			bindingsContainer.keyBindings.Interactions.Enable();
			bindingsContainer.keyBindings.Interactions.SetCallbacks(this);

			bindingsContainer.keyBindings.Combat.Enable();
			bindingsContainer.keyBindings.Combat.SetCallbacks(this);

			bindingsContainer.keyBindings.Enable();
		}

		public void OnMovement(InputAction.CallbackContext context)
		{
			if (moveEvent != null)
				moveEvent.Invoke(context.ReadValue<Vector2>());
		}

		public void OnLook(InputAction.CallbackContext context)
        {
			if (lookEvent != null)
				lookEvent.Invoke(context.ReadValue<Vector2>());
        }

		public void OnPickUp(InputAction.CallbackContext context)
        {
			if (pickUpEvent != null && context.phase == InputActionPhase.Performed)
				pickUpEvent.Invoke();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
			if (sprintEvent != null && context.phase == InputActionPhase.Performed)
				sprintEvent.Invoke();

			if (sprintEvent != null && context.phase == InputActionPhase.Canceled)
				sprintCanceledEvent.Invoke();
		}

        public void OnMelee(InputAction.CallbackContext context)
        {
			if (meleeAttackEvent != null && context.phase == InputActionPhase.Started)
				meleeAttackEvent.Invoke();
        }

        public void OnRanged(InputAction.CallbackContext context)
        {
			if (rangedAttackEvent != null && context.phase == InputActionPhase.Started)
				rangedAttackEvent.Invoke();
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (dashPerformedEvent != null && context.phase == InputActionPhase.Performed)
                dashPerformedEvent.Invoke();
        }

        public void OnPause(InputAction.CallbackContext context)
        {
			Managers.GameManager.instance.PauseGame();
        }
    }
}
