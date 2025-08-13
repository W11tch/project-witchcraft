using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This script handles the physical, physics-based movement of the player character.
/// It receives input from the PlayerController script.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Component References")]
    [Tooltip("The Rigidbody component of the player model.")]
    [SerializeField] private Rigidbody playerRigidbody;

    [Header("Movement Settings")]
    [Tooltip("The speed at which the player moves.")]
    [SerializeField] private float moveSpeed = 5f;

    // Private variable to store input
    private Vector2 moveInput;

    private void Awake()
    {
        // This script requires a Rigidbody, but it's on a child object.
        // We ensure the reference is set.
        if (playerRigidbody == null)
        {
            Debug.LogError("Player Rigidbody is not assigned! Please assign the Rigidbody component from the child object in the Inspector.");
        }
    }

    /// <summary>
    /// Receives the move input from the PlayerController and stores it.
    /// This method is called by the PlayerController's input callback.
    /// </summary>
    /// <param name="inputVector">The 2D vector representing the player's movement input.</param>
    public void ReceiveMoveInput(Vector2 inputVector)
    {
        moveInput = inputVector;
    }

    /// <summary>
    /// Called every fixed-rate frame, which is ideal for physics.
    /// </summary>
    private void FixedUpdate()
    {
        if (playerRigidbody == null) return;

        // Correctly map 2D input (X, Y) to a 3D movement vector (X, Z).
        // moveInput.x -> X-axis (left/right)
        // moveInput.y -> Z-axis (forward/backward)
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);

        // Apply movement to the Rigidbody.
        playerRigidbody.linearVelocity = moveDirection * moveSpeed;
    }
}