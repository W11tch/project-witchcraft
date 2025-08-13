using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private InputActionMap buildingActionMap;
    private Camera mainCamera;
    private bool isInBuildMode = true;
    private Dictionary<string, BuildingData> keyToBuildingDataMap;

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

        mainCamera = Camera.main;

        if (inputActionsAsset == null)
        {
            Debug.LogError("Input Actions Asset is not assigned in the Inspector! Building functionality will be disabled.");
            return;
        }

        buildingActionMap = inputActionsAsset.FindActionMap("Building");
        if (buildingActionMap != null)
        {
            buildingActionMap["Place"].performed += OnPlacePerformed;
            buildingActionMap["Rotate"].performed += OnRotatePerformed;
            buildingActionMap["Destroy"].performed += OnDestroyPerformed;
            buildingActionMap["Cancel"].performed += OnCancelPerformed;
            buildingActionMap["SelectBuilding"].performed += OnSelectBuildingPerformed;
        }

        keyToBuildingDataMap = new Dictionary<string, BuildingData>();
        foreach (var buildingKey in buildingKeys)
        {
            if (buildingKey.buildingData == null) continue;
            string controlPath = $"<Keyboard>/{buildingKey.key.ToString()}";
            keyToBuildingDataMap[controlPath] = buildingKey.buildingData;
        }
    }

    private void Start()
    {
        if (playerInput != null && playerInput.actions != null)
        {
            playerInput.actions.FindActionMap("Player").Enable();
            playerInput.actions.FindActionMap("Building").Enable();
        }

        if (buildingKeys.Length > 0 && buildingKeys[0].buildingData != null)
        {
            BuildingEvents.TriggerObjectSelected(buildingKeys[0].buildingData);
        }
    }

    private void OnDestroy()
    {
        if (buildingActionMap != null)
        {
            buildingActionMap["Place"].performed -= OnPlacePerformed;
            buildingActionMap["Rotate"].performed -= OnRotatePerformed;
            buildingActionMap["Destroy"].performed -= OnDestroyPerformed;
            buildingActionMap["Cancel"].performed -= OnCancelPerformed;
            buildingActionMap["SelectBuilding"].performed -= OnSelectBuildingPerformed;
        }
    }

    public Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, groundLayer))
        {
            return hitInfo.point;
        }
        return Vector3.zero;
    }

    public void OnPlacePerformed(InputAction.CallbackContext context)
    {
        if (!context.performed || !isInBuildMode) return;
        BuildingEvents.TriggerObjectPlaced();
    }

    public void OnRotatePerformed(InputAction.CallbackContext context)
    {
        if (!context.performed || !isInBuildMode) return;
        BuildingEvents.TriggerObjectRotated();
    }

    /// <summary>
    /// Handles the destruction of a building. It performs a raycast to select and destroy the specific object.
    /// </summary>
    public void OnDestroyPerformed(InputAction.CallbackContext context)
    {
        if (!context.performed || !isInBuildMode) return;

        // Create a ray from the mouse position.
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast to find the object to destroy.
        // We use a LayerMask to ensure we only hit buildable objects, not the ground.
        if (Physics.Raycast(ray, out hit, 100f, ~groundLayer))
        {
            // The raycast successfully hit an object. Destroy it.
            // This ensures we only destroy the specific object that was clicked.
            Destroy(hit.transform.gameObject);
        }
    }

    public void OnCancelPerformed(InputAction.CallbackContext context)
    {
        if (!context.performed || !isInBuildMode) return;
        BuildingEvents.TriggerPlacementCanceled();
    }

    public void OnSelectBuildingPerformed(InputAction.CallbackContext context)
    {
        if (!context.performed || !isInBuildMode) return;

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
        else
        {
            Debug.LogWarning($"No building key found for control path: {controlPath}");
        }
    }
}