// Located at: Assets/Scripts/Managers/BuildingManager.cs
using ProjectWitchcraft.BuildingSystem;
using ProjectWitchcraft.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectWitchcraft.Managers
{
    public class BuildingManager : Singleton<BuildingManager>
    {
        [Header("Configuration")]
        // FIX: Replaced the old list with a direct list of BuildingData assets.
        // This list represents the player's building hotbar.
        [SerializeField] private List<BuildingData> buildingHotbar;

        private Camera _mainCamera;

        protected override void Awake()
        {
            base.Awake();
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            // Automatically select the first item in the hotbar on game start.
            if (buildingHotbar != null && buildingHotbar.Count > 0 && buildingHotbar[0] != null)
            {
                EventManager.TriggerEvent(new BuildingSelectedEvent { BuildingData = buildingHotbar[0] });
            }
        }

        public Vector3 GetMouseWorldPosition()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, LayerMask.GetMask("Ground")))
            {
                return hitInfo.point;
            }
            return Vector3.zero;
        }

        // FIX: This method now accepts a slot index (0 for key "1", 1 for key "2", etc.).
        public void SelectBuildingFromSlot(int slotIndex)
        {
            // Check if the requested slot is valid.
            if (slotIndex >= 0 && slotIndex < buildingHotbar.Count)
            {
                BuildingData selectedBuilding = buildingHotbar[slotIndex];
                if (selectedBuilding != null)
                {
                    EventManager.TriggerEvent(new BuildingSelectedEvent { BuildingData = selectedBuilding });
                }
            }
        }
    }
}