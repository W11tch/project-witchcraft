// Located at: Assets/Scripts/Managers/InteractionManager.cs
using ProjectWitchcraft.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace ProjectWitchcraft.Managers
{
    public class InteractionManager : Singleton<InteractionManager>
    {
        [Header("Configuration")]
        [Tooltip("The maximum distance from the camera to check for interactable objects.")]
        [SerializeField] private float _interactionDistance = 100f; // Increased for top-down view
        [Tooltip("The layers that contain interactable objects.")]
        [SerializeField] private LayerMask _interactableLayers; // **MODIFIED**: Changed from int to LayerMask
        [Tooltip("The thickness of the outline when an object is highlighted.")]
        [SerializeField] private float _outlineThickness = 0.05f;

        private IInteractable _currentTarget;
        private Camera _mainCamera;
        private Renderer _currentTargetRenderer;

        // **MODIFIED**: This now refers to the thickness property in our shader.
        private static readonly int OutlineThicknessProperty = Shader.PropertyToID("_OutlineThickness");

        // This will hold the original material of the sprite to avoid instancing new materials unnecessarily.
        private MaterialPropertyBlock _propertyBlock;

        protected override void Awake()
        {
            // --- DEBUG: Add a log here to catch duplicate instances ---
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("DUPLICATE InteractionManager instance was created and is being destroyed.", this.gameObject);
            }
            base.Awake();
            _mainCamera = Camera.main;
            _propertyBlock = new MaterialPropertyBlock(); // **NEW**: Initialize the property block.
        }

        private void OnEnable()
        {
            EventManager.AddListener<InteractActionTriggeredEvent>(OnInteractPressed);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<InteractActionTriggeredEvent>(OnInteractPressed);
        }

        private void Update()
        {
            // **NEW**: Prevents interaction raycasting while over UI elements.
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // If we were highlighting something, stop.
                if (_currentTarget != null)
                {
                    ToggleOutline(false);
                    _currentTarget = null;
                    _currentTargetRenderer = null;
                }
                return;
            }

            HandleTargeting();
        }


        private void HandleTargeting()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            IInteractable newTarget = null;

            RaycastHit hit;

            // **MODIFIED**: The raycast now uses the flexible LayerMask.
            if (Physics.Raycast(ray, out hit, _interactionDistance, _interactableLayers))
            {
                newTarget = hit.collider.GetComponent<IInteractable>();

                //if (newTarget == null)
                //{
                    // DEBUG: The object was hit, but it's missing the IInteractable component (like ChestController).
                   // Debug.Log($"Object {hit.collider.name} was hit, but it has no IInteractable component.", hit.collider.gameObject);
                //}
            }

            if (newTarget != _currentTarget)
            {
                // Target has changed, hide outline on the old target
                if (_currentTargetRenderer != null)
                {
                    ToggleOutline(false);
                }

                _currentTarget = newTarget;

                // Show outline on the new target, if it's valid
                if (_currentTarget != null)
                {
                    // DEBUG: A new valid target has been acquired.
                    Debug.Log($"New interactable target acquired: {((MonoBehaviour)_currentTarget).name}");
                    // Try to get the Renderer from the hit collider's GameObject.
                    _currentTargetRenderer = hit.collider.GetComponentInChildren<Renderer>();
                    ToggleOutline(true);
                }
            }
        }


        private void OnInteractPressed(InteractActionTriggeredEvent e)
        {
            // DEBUG: Check if the 'E' key press is being received by the manager.
            Debug.Log("InteractionManager received InteractActionTriggeredEvent!");

            if (_currentTarget != null)
            {
                // DEBUG: A valid target exists, attempting to interact.
                Debug.Log($"Attempting to interact with {_currentTarget}.");
                _currentTarget.Interact();
            }
            else
            {
                // DEBUG: 'E' was pressed, but no valid target was being hovered over.
                Debug.Log("Interact pressed, but no target was found by InteractionManager.");
            }
        }

        private void ToggleOutline(bool show)
        {
            if (_currentTargetRenderer == null) return;

            // **MODIFIED LOGIC**: We now use a MaterialPropertyBlock to change the shader property.
            // This is the most efficient way to change material properties at runtime without creating new material instances.
            _currentTargetRenderer.GetPropertyBlock(_propertyBlock);
            float outlineValue = show ? _outlineThickness : 0f;
            _propertyBlock.SetFloat(OutlineThicknessProperty, outlineValue);
            _currentTargetRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}