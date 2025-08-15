using UnityEngine;
using ProjectWitchcraft.Player;

namespace ProjectWitchcraft.Player
{
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
        private Vector2 _moveInput;

        private void Awake()
        {
            if (playerRigidbody == null)
            {
                Debug.LogError("Player Rigidbody is not assigned! Please assign the Rigidbody component from the child object in the Inspector.");
                return; // Stop execution if the rigidbody is missing
            }

            // *** THIS IS THE FIX ***
            // By setting interpolation, we smooth the Rigidbody's rendered position between physics frames.
            // This gives the camera a smooth target to follow, eliminating jitter.
            playerRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        }

        public void ReceiveMoveInput(Vector2 inputVector)
        {
            _moveInput = inputVector;
        }

        private void FixedUpdate()
        {
            if (playerRigidbody == null) return;

            Vector3 moveDirection = new Vector3(_moveInput.x, 0f, _moveInput.y);
            playerRigidbody.linearVelocity = moveDirection * moveSpeed;
        }
    }
}