using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ProjectWitchcraft.BuildingSystem; // Add this line

namespace ProjectWitchcraft.BuildingSystem
{
    /// <summary>
    /// The central manager for all building-related functionality.
    /// It handles input for placing, rotating, destroying, and selecting objects.
    /// </summary>
    public class BuildingSystem : MonoBehaviour
    {
        private static BuildingSystem _instance;
        public static BuildingSystem Instance => _instance;

        [System.Serializable]
        public class BuildingKey
        {
            public BuildingData buildingData;
            public KeyCode key;
        }

        [Header("Configuration")]
        [Tooltip("The LayerMask for the ground, used in raycasting for placement.")]
        [SerializeField] private LayerMask groundLayer;
        [Tooltip("List of BuildingData assets and their corresponding keybinds.")]
        [SerializeField] private BuildingKey[] buildingKeys;

        [Header("Component References")]
        [Tooltip("The main Input Actions asset for the project.")]
        [SerializeField] private InputActionAsset inputActionsAsset;
        [Tooltip("The Player Input component on the Player GameObject.")]
        [SerializeField] private PlayerInput playerInput;

        // Private fields
        private InputActionMap _buildingActionMap;
        private Camera _mainCamera;
        private bool _isInBuildMode = true;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }

            _mainCamera = Camera.main;

            if (inputActionsAsset == null)
            {
                Debug.LogError("Input Actions Asset is not assigned in the Inspector! Building functionality will be disabled.");
                return;
            }

            _buildingActionMap = inputActionsAsset.FindActionMap("Building");
            if (_buildingActionMap != null)
            {
                _buildingActionMap["Place"].performed += OnPlacePerformed;
                _buildingActionMap["Rotate"].performed += OnRotatePerformed;
                _buildingActionMap["Destroy"].performed += OnDestroyPerformed;
                _buildingActionMap["Cancel"].performed += OnCancelPerformed;
                _buildingActionMap["SelectBuilding"].performed += OnSelectBuildingPerformed;
            }
        }

        private void Start()
        {
            if (playerInput != null && playerInput.actions != null)
            {
                playerInput.actions.FindActionMap("Player").Enable();
                playerInput.actions.FindActionMap("Building").Enable();
            }

            // Automatically select the first building on start
            if (buildingKeys.Length > 0 && buildingKeys[0].buildingData != null)
            {
                BuildingEvents.TriggerObjectSelected(buildingKeys[0].buildingData);
            }
        }

        private void OnDestroy()
        {
            if (_buildingActionMap != null)
            {
                _buildingActionMap["Place"].performed -= OnPlacePerformed;
                _buildingActionMap["Rotate"].performed -= OnRotatePerformed;
                _buildingActionMap["Destroy"].performed -= OnDestroyPerformed;
                _buildingActionMap["Cancel"].performed -= OnCancelPerformed;
                _buildingActionMap["SelectBuilding"].performed -= OnSelectBuildingPerformed;
            }
        }

        public Vector3 GetMouseWorldPosition()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, groundLayer))
            {
                return hitInfo.point;
            }
            return Vector3.zero;
        }

        public void OnPlacePerformed(InputAction.CallbackContext context)
        {
            if (!context.performed || !_isInBuildMode) return;
            BuildingEvents.TriggerObjectPlaced();
        }

        public void OnRotatePerformed(InputAction.CallbackContext context)
        {
            if (!context.performed || !_isInBuildMode) return;
            BuildingEvents.TriggerObjectRotated();
        }
        
        public void OnDestroyPerformed(InputAction.CallbackContext context)
        {
            if (!context.performed || !_isInBuildMode) return;
            BuildingEvents.TriggerObjectDestroyed();
        }

        public void OnCancelPerformed(InputAction.CallbackContext context)
        {
            if (!context.performed || !_isInBuildMode) return;
            BuildingEvents.TriggerPlacementCanceled();
        }

        public void OnSelectBuildingPerformed(InputAction.CallbackContext context)
        {
            if (!context.performed || !_isInBuildMode) return;

            string controlPath = context.control.path;

            if (controlPath.EndsWith("/1"))
            {
                if (buildingKeys.Length > 0 && buildingKeys[0].buildingData != null)
                {
                    BuildingEvents.TriggerObjectSelected(buildingKeys[0].buildingData);
                }
            }
            else if (controlPath.EndsWith("/2"))
            {
                if (buildingKeys.Length > 1 && buildingKeys[1].buildingData != null)
                {
                    BuildingEvents.TriggerObjectSelected(buildingKeys[1].buildingData);
                }
            }
            else if (controlPath.EndsWith("/3"))
            {
                if (buildingKeys.Length > 2 && buildingKeys[2].buildingData != null)
                {
                    BuildingEvents.TriggerObjectSelected(buildingKeys[2].buildingData);
                }
            }
        }
    }
}