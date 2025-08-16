// Located at: Assets/Scripts/Player/PlayerMovement.cs
using UnityEngine;
using ProjectWitchcraft.Player;

namespace ProjectWitchcraft.Player
{
    // The RequireComponent attribute has been removed, as the Rigidbody is on a child object.
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private float speed = 5f;

        private Rigidbody _rigidbody;
        private PlayerController _playerController;

        private void Awake()
        {
            // Use GetComponentInChildren to find the Rigidbody on the child object.
            _rigidbody = GetComponentInChildren<Rigidbody>();
            _playerController = GetComponent<PlayerController>();

            if (_rigidbody == null)
            {
                Debug.LogError("PlayerMovement script could not find a Rigidbody component on the Player or its children.", this);
            }
        }

        private void FixedUpdate()
        {
            if (_rigidbody == null || _playerController == null) return;

            Vector2 moveInput = _playerController.MoveInput;
            Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);

            _rigidbody.linearVelocity = movement * speed;
        }
    }
}
