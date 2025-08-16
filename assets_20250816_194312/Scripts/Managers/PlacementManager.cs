// Located at: Assets/Scripts/Managers/PlacementManager.cs
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using ProjectWitchcraft.BuildingSystem;
using ProjectWitchcraft.Core;
using UnityEngine.InputSystem;

namespace ProjectWitchcraft.Managers
{
    public class PlacementManager : Singleton<PlacementManager>
    {
        [Header("Dependencies")]
        [SerializeField] private WorldGridManager worldGridManager;
        [SerializeField] private ResourceManager resourceManager;
        [SerializeField] private ObjectPooler objectPooler;

        [Header("Configuration")]
        [SerializeField] private Transform placedObjectsParent;
        [SerializeField] private LayerMask buildingBlockLayer;
        [SerializeField] private LayerMask playerLayer;

        private PlaceableObject _previewObject;
        private BuildingData _currentBuildingData;
        private bool _isPlacing = false;
        private int _previewLayer;

        private bool _isPointerOverUI = false;

        protected override void Awake()
        {
            base.Awake();
            _previewLayer = LayerMask.NameToLayer("Preview");
        }

        private void OnEnable()
        {
            // Subscribe to all relevant events
            EventManager.AddListener<BuildingSelectedEvent>(OnBuildingSelected);
            EventManager.AddListener<PlaceActionTriggeredEvent>(HandlePlaceAction);
            EventManager.AddListener<RotateActionTriggeredEvent>(HandleRotateAction);
            EventManager.AddListener<CancelActionTriggeredEvent>(HandleCancelAction);
            EventManager.AddListener<DestroyActionTriggeredEvent>(HandleDestroyAction);
        }

        private void OnDisable()
        {
            // Unsubscribe from all events to prevent memory leaks
            EventManager.RemoveListener<BuildingSelectedEvent>(OnBuildingSelected);
            EventManager.RemoveListener<PlaceActionTriggeredEvent>(HandlePlaceAction);
            EventManager.RemoveListener<RotateActionTriggeredEvent>(HandleRotateAction);
            EventManager.RemoveListener<CancelActionTriggeredEvent>(HandleCancelAction);
            EventManager.RemoveListener<DestroyActionTriggeredEvent>(HandleDestroyAction);
        }

        private void Update()
        {
            _isPointerOverUI = EventSystem.current.IsPointerOverGameObject();

            if (_isPlacing)
            {
                UpdatePreview();
            }
        }

        private void OnBuildingSelected(BuildingSelectedEvent eventData)
        {
            BuildingData buildingData = eventData.BuildingData;
            if (buildingData?.prefab == null) return;

            if (_isPlacing && _currentBuildingData == buildingData)
            {
                StopPlacement();
                return;
            }

            StartPlacement(buildingData);
        }

        private void StartPlacement(BuildingData buildingData)
        {
            StopPlacement();

            _isPlacing = true;
            _currentBuildingData = buildingData;

            _previewObject = objectPooler.SpawnFromPool(_currentBuildingData.poolTag).GetComponent<PlaceableObject>();
            _previewObject.gameObject.layer = _previewLayer;
            foreach (Transform child in _previewObject.transform)
            {
                child.gameObject.layer = _previewLayer;
            }
            _previewObject.GetComponent<VisualsController>()?.SetIsTransparent(true, 0.5f);
        }

        private void StopPlacement()
        {
            _isPlacing = false;
            if (_previewObject != null)
            {
                objectPooler.ReturnToPool(_previewObject.gameObject);
            }
            _previewObject = null;
            _currentBuildingData = null;
        }

        // --- Event Handlers ---

        private void HandlePlaceAction(PlaceActionTriggeredEvent e)
        {
            if (!_isPlacing || _isPointerOverUI) return;

            var type = GetFinalPlacementType(_previewObject.transform.position, _previewObject.Size);
            if (IsPlacementValid(type, _previewObject.transform.position, _previewObject.Size))
            {
                ConsumeResources(_currentBuildingData.Cost);
                var finalObject = objectPooler.SpawnFromPool(_currentBuildingData.poolTag, _previewObject.transform.position, _previewObject.transform.rotation).GetComponent<PlaceableObject>();
                finalObject.transform.SetParent(placedObjectsParent);
                finalObject.GetComponent<VisualsController>()?.SetIsTransparent(false);
                finalObject.Placed = true;
                worldGridManager.PlaceObject(finalObject, type);
            }
        }

        private void HandleRotateAction(RotateActionTriggeredEvent e)
        {
            if (!_isPlacing) return;
            _previewObject?.Rotate();
        }

        private void HandleCancelAction(CancelActionTriggeredEvent e)
        {
            if (!_isPlacing) return;
            StopPlacement();
        }

        private void HandleDestroyAction(DestroyActionTriggeredEvent e)
        {
            if (_isPointerOverUI) return;

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hit, 100f, buildingBlockLayer))
            {
                var objToDestroy = hit.collider.GetComponentInParent<PlaceableObject>();
                if (objToDestroy != null)
                {
                    var gridData = worldGridManager.GetGridData(objToDestroy.transform.position);
                    if (gridData != null && gridData.groundObject == objToDestroy && gridData.upperObject != null)
                    {
                        return;
                    }
                    worldGridManager.RemoveObject(objToDestroy);
                    objectPooler.ReturnToPool(objToDestroy.gameObject);
                }
            }
        }

        // --- Private Helper Methods ---

        private void UpdatePreview()
        {
            if (_previewObject == null) return;
            Vector3 groundPos = BuildingManager.Instance.GetMouseWorldPosition();
            Vector3 snappedPos = worldGridManager.SnapToGrid(groundPos);
            var type = GetFinalPlacementType(snappedPos, _previewObject.Size);
            bool canPlace = IsPlacementValid(type, snappedPos, _previewObject.Size);
            _previewObject.GetComponent<VisualsController>()?.SetIsTransparent(true, canPlace ? 0.5f : 0.2f);

            float halfHeight = _previewObject.GetComponent<Collider>().bounds.extents.y;
            if (type == BuildingData.PlacementType.GroundLevel)
            {
                snappedPos.y = 0.01f - halfHeight;
            }
            else
            {
                var gridCell = worldGridManager.GetGridData(snappedPos);
                if (gridCell?.groundObject != null)
                {
                    var groundCollider = gridCell.groundObject.GetComponent<Collider>();
                    float topOfGroundObject = gridCell.groundObject.transform.position.y + groundCollider.bounds.extents.y;
                    snappedPos.y = topOfGroundObject + halfHeight;
                }
                else
                {
                    snappedPos.y = 0.01f - halfHeight;
                }
            }
            _previewObject.transform.position = snappedPos;
        }

        private BuildingData.PlacementType GetFinalPlacementType(Vector3 position, Vector3Int size)
        {
            var desiredType = _previewObject.BuildingData.placementType;
            if (desiredType == BuildingData.PlacementType.Either)
            {
                return worldGridManager.IsAreaFree(position, size, BuildingData.PlacementType.UpperLevel, _previewObject)
                    ? BuildingData.PlacementType.UpperLevel
                    : BuildingData.PlacementType.GroundLevel;
            }
            return desiredType;
        }

        private bool IsPlacementValid(BuildingData.PlacementType type, Vector3 position, Vector3Int size)
        {
            if (_currentBuildingData == null || !CanAffordCost(_currentBuildingData.Cost))
            {
                return false;
            }

            Collider objectCollider = _currentBuildingData.prefab.GetComponent<Collider>();
            if (objectCollider != null)
            {
                if (Physics.CheckBox(position, objectCollider.bounds.extents, _previewObject.transform.rotation, playerLayer))
                {
                    return false;
                }
            }

            bool isAreaFree = type == BuildingData.PlacementType.UpperLevel
                ? worldGridManager.IsAreaFree(position, size, BuildingData.PlacementType.UpperLevel, _previewObject)
                : worldGridManager.IsAreaFree(position, size, BuildingData.PlacementType.GroundLevel, _previewObject);

            return isAreaFree;
        }

        private bool CanAffordCost(List<ResourceCost> costs)
        {
            foreach (var cost in costs)
            {
                if (!resourceManager.HasResource(cost.ResourceType, cost.Amount)) return false;
            }
            return true;
        }

        private void ConsumeResources(List<ResourceCost> costs)
        {
            foreach (var cost in costs)
            {
                resourceManager.RemoveResource(cost.ResourceType, cost.Amount);
            }
        }
    }
}
