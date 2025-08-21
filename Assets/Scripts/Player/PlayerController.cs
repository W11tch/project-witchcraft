// Located at: Assets/Scripts/Player/PlayerController.cs
using UnityEngine;
using UnityEngine.InputSystem;
using ProjectWitchcraft.Managers;
using ProjectWitchcraft.Core;

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
            if (GameManager.Instance.CurrentState != GameState.Playing) return;

            if (context.performed)
            {
                if (int.TryParse(context.control.name, out int keyNumber))
                {
                    int slotIndex = keyNumber - 1;
                    BuildingManager.Instance.SelectBuildingFromSlot(slotIndex);
                }
            }
        }

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

        public void OnPause(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            var gm = GameManager.Instance;
            if (gm.CurrentState == GameState.Playing) { gm.PauseGame(); }
            else if (gm.CurrentState == GameState.Paused) { gm.ResumeGame(); }
            else if (gm.CurrentState == GameState.InMenu) { gm.ExitMenuMode(); }
        }

        public void OnToggleInventory(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            var gm = GameManager.Instance;
            if (gm.CurrentState == GameState.Playing) { gm.EnterMenuMode(); }
            else if (gm.CurrentState == GameState.InMenu) { gm.ExitMenuMode(); }
        }
    }
}