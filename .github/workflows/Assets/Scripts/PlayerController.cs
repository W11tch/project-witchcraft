using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This script acts as the central hub for all player input.
/// It takes input from the Player Input component and delegates
/// the corresponding actions to other, more specialized scripts.
/// </summary>
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    [Header("Script References")]
    [Tooltip("The PlayerMovement script, which handles the character's physics-based movement.")]
    [SerializeField] private PlayerMovement playerMovement;

    /// <summary>
    /// Called by the Unity Input System when the "Move" action is performed.
    /// This method passes the input value directly to the PlayerMovement script.
    /// </summary>
    /// <param name="context">The context from the input action, containing the input value.</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        // Check for a valid reference to the movement script before calling it.
        if (playerMovement != null)
        {
            playerMovement.ReceiveMoveInput(context.ReadValue<Vector2>());
        }
    }
}