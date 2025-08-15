using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ProjectWitchcraft.BuildingSystem;

namespace ProjectWitchcraft.Managers
{
    public class BuildingManager : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private List<BuildingMenuItem> buildingMenuItems;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        // **THE FIX**: The Start method is back. It selects the first building in the list when the game starts.
        private void Start()
        {
            if (buildingMenuItems != null && buildingMenuItems.Count > 0 && buildingMenuItems[0] != null)
            {
                BuildingEvents.TriggerObjectSelected(buildingMenuItems[0].BuildingData);
            }
        }

        public Vector3 GetMouseWorldPosition()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, LayerMask.GetMask("Ground")))
            {
                return hitInfo.point;
            }
            return Vector3.zero;
        }

        // This is the only input this script handles.
        public void OnSelectBuildingPerformed(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            // This is a robust way to get the name of the key that was pressed (e.g., "1", "2").
            string controlName = context.control.name;

            foreach (var item in buildingMenuItems)
            {
                if (item != null && item.KeyIdentifier == controlName)
                {
                    BuildingEvents.TriggerObjectSelected(item.BuildingData);
                    return; // Found the match, no need to check further.
                }
            }
        }
    }
}