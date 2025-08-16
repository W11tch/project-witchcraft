// Located at: Assets/Scripts/Player/PlayerController.cs
using UnityEngine;
using UnityEngine.InputSystem;
using ProjectWitchcraft.Managers;
using ProjectWitchcraft.Core; // Add this to use the new events

namespace ProjectWitchcraft.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        public Vector2 MoveInput { get; private set; }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log("Attack action performed.");
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log("Interact action performed.");
            }
        }

        public void OnSelectBuilding(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                BuildingManager.Instance.SelectBuildingFromInput(context.control.name);
            }
        }

        // --- Updated Methods: Broadcast Events Instead of Direct Calls ---

        public void OnPlace(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                EventManager.TriggerEvent(new PlaceActionTriggeredEvent());
            }
        }

        public void OnRotate(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                EventManager.TriggerEvent(new RotateActionTriggeredEvent());
            }
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                EventManager.TriggerEvent(new CancelActionTriggeredEvent());
            }
        }

        public void OnDestroyObject(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                EventManager.TriggerEvent(new DestroyActionTriggeredEvent());
            }
        }
    }
}
