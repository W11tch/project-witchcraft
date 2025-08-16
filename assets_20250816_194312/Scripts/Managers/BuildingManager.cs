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
        [SerializeField] private List<BuildingMenuItem> buildingMenuItems;

        private Camera _mainCamera;

        protected override void Awake()
        {
            base.Awake();
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            if (buildingMenuItems != null && buildingMenuItems.Count > 0 && buildingMenuItems[0] != null)
            {
                EventManager.TriggerEvent(new BuildingSelectedEvent { BuildingData = buildingMenuItems[0].BuildingData });
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

        // This method is now called by the PlayerController.
        public void SelectBuildingFromInput(string controlName)
        {
            foreach (var item in buildingMenuItems)
            {
                if (item != null && item.KeyIdentifier == controlName)
                {
                    EventManager.TriggerEvent(new BuildingSelectedEvent { BuildingData = item.BuildingData });
                    return;
                }
            }
        }
    }
}
