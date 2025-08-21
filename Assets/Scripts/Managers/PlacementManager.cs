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
        public ObjectPooler objectPooler;
        [SerializeField] private WorldGridManager worldGridManager;
        [SerializeField] private InventoryManager inventoryManager;

        [Header("Configuration")]
        [SerializeField] private Transform placedObjectsParent;
        public Transform PlacedObjectsParent => placedObjectsParent;

        [SerializeField] private LayerMask buildingBlockLayer;
        [SerializeField] private LayerMask playerLayer;

        private PlaceableObject _previewObject;
        private BuildingData _currentBuildingData;
        private bool _isPlacing = false;
        private int _previewLayer;
        private bool _isPointerOverUI = false;
        private bool _isDestroyModeActive = true;
        private bool _isFrozen = false;

        protected override void Awake()
        {
            base.Awake();
            _previewLayer = LayerMask.NameToLayer("Preview");
        }

        private void OnEnable()
        {
            EventManager.AddListener<GameStateChangedEvent>(OnGameStateChanged);
            EventManager.AddListener<BuildingSelectedEvent>(OnBuildingSelected);
            EventManager.AddListener<PlaceActionTriggeredEvent>(HandlePlaceAction);
            EventManager.AddListener<RotateActionTriggeredEvent>(HandleRotateAction);
            EventManager.AddListener<CancelActionTriggeredEvent>(HandleCancelAction);
            EventManager.AddListener<DestroyActionTriggeredEvent>(HandleDestroyAction);
            EventManager.AddListener<ToggleDestroyModeEvent>(OnToggleDestroyMode);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<GameStateChangedEvent>(OnGameStateChanged);
            EventManager.RemoveListener<BuildingSelectedEvent>(OnBuildingSelected);
            EventManager.RemoveListener<PlaceActionTriggeredEvent>(HandlePlaceAction);
            EventManager.RemoveListener<RotateActionTriggeredEvent>(HandleRotateAction);
            EventManager.RemoveListener<CancelActionTriggeredEvent>(HandleCancelAction);
            EventManager.RemoveListener<DestroyActionTriggeredEvent>(HandleDestroyAction);
            EventManager.RemoveListener<ToggleDestroyModeEvent>(OnToggleDestroyMode);
        }

        private void OnToggleDestroyMode(ToggleDestroyModeEvent e)
        {
            _isDestroyModeActive = e.IsDestroyModeActive;
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
            if (_previewObject == null)
            {
                _isPlacing = false;
                return;
            }
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

        private void HandlePlaceAction(PlaceActionTriggeredEvent e)
        {
            if (_isFrozen) return;
            if (!_isPlacing || _isPointerOverUI || _previewObject == null) return;
            Vector3 finalPosition = _previewObject.transform.position;
            if (IsPlacementValid(finalPosition, _previewObject.Size))
            {
                ConsumeResources(_currentBuildingData.Cost);
                var finalObject = objectPooler.SpawnFromPool(_currentBuildingData.poolTag, finalPosition, _previewObject.transform.rotation).GetComponent<PlaceableObject>();
                finalObject.transform.SetParent(placedObjectsParent);
                finalObject.GetComponent<VisualsController>()?.SetIsTransparent(false);
                finalObject.Placed = true;
                worldGridManager.PlaceObject(finalObject, _currentBuildingData.Category);
            }
        }

        private void HandleRotateAction(RotateActionTriggeredEvent e)
        {
            if (_isFrozen) return;
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
            if (_isFrozen) return;
            if (!_isDestroyModeActive) return;
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

        private void UpdatePreview()
        {
            if (_previewObject == null) return;
            Vector3 snappedPos = worldGridManager.SnapToGridCenter(BuildingManager.Instance.GetMouseWorldPosition());
            Vector3 finalPreviewPosition = GetPreviewPosition(snappedPos);
            _previewObject.transform.position = finalPreviewPosition;
            bool canPlace = IsPlacementValid(finalPreviewPosition, _previewObject.Size);
            _previewObject.GetComponent<VisualsController>()?.SetIsTransparent(true, canPlace ? 0.5f : 0.2f);
        }

        private Vector3 GetPreviewPosition(Vector3 snappedPos)
        {
            var category = _currentBuildingData.Category;
            Collider objectCollider = _previewObject.GetComponent<Collider>();
            float halfHeight = objectCollider.bounds.extents.y;
            var gridCell = worldGridManager.GetGridData(snappedPos);

            switch (category)
            {
                case BuildingData.ObjectCategory.Block:
                    if (gridCell?.groundObject != null && gridCell.groundObject.BuildingData.canPlaceOnTop)
                    {
                        var groundCollider = gridCell.groundObject.GetComponent<Collider>();
                        float topOfGroundObject = groundCollider.bounds.center.y + groundCollider.bounds.extents.y;
                        snappedPos.y = topOfGroundObject + halfHeight;
                    }
                    else
                    {
                        snappedPos.y = 0.01f - halfHeight;
                    }
                    break;
                case BuildingData.ObjectCategory.Bridge:
                    snappedPos.y = 0.01f - halfHeight;
                    break;
                case BuildingData.ObjectCategory.Wall:
                case BuildingData.ObjectCategory.Furniture:
                    if (gridCell?.groundObject != null && gridCell.groundObject.BuildingData.canPlaceOnTop)
                    {
                        var groundCollider = gridCell.groundObject.GetComponent<Collider>();
                        float topOfGroundObject = groundCollider.bounds.center.y + groundCollider.bounds.extents.y;
                        snappedPos.y = topOfGroundObject + halfHeight;
                    }
                    else
                    {
                        snappedPos.y = -10f;
                    }
                    break;
            }
            return snappedPos;
        }

        private bool IsPlacementValid(Vector3 position, Vector3Int size)
        {
            if (_currentBuildingData == null || !CanAffordCost(_currentBuildingData.Cost))
            {
                return false;
            }
            var category = _currentBuildingData.Category;
            var gridCell = worldGridManager.GetGridData(position);
            bool isCellEmpty = gridCell == null || (gridCell.groundObject == null && gridCell.upperObject == null);
            switch (category)
            {
                case BuildingData.ObjectCategory.Block:
                    if (isCellEmpty) return true;
                    return gridCell.groundObject != null && gridCell.groundObject.BuildingData.canPlaceOnTop && gridCell.upperObject == null;
                case BuildingData.ObjectCategory.Bridge:
                    if (isCellEmpty) return true;
                    return gridCell.groundObject == null;
                case BuildingData.ObjectCategory.Wall:
                    if (isCellEmpty) return false;
                    return gridCell.groundObject != null && gridCell.groundObject.BuildingData.canPlaceOnTop && gridCell.upperObject == null;
                case BuildingData.ObjectCategory.Furniture:
                    if (isCellEmpty) return false;
                    bool hasFoundation = gridCell.groundObject != null && gridCell.groundObject.BuildingData.canPlaceOnTop;
                    bool canStackOnFurniture = gridCell.upperObject != null && gridCell.upperObject.BuildingData.canPlaceOnTop;
                    if (!hasFoundation) return false;
                    if (gridCell.upperObject != null && !canStackOnFurniture) return false;
                    if (_currentBuildingData.RequiresWallBehind && !worldGridManager.HasWallBehind(position)) return false;
                    break;
            }
            return true;
        }

        private bool CanAffordCost(List<ResourceCost> costs)
        {
            foreach (var cost in costs)
            {
                if (!inventoryManager.HasResource(cost.ResourceType, cost.Amount))
                {
                    return false;
                }
            }
            return true;
        }

        private void ConsumeResources(List<ResourceCost> costs)
        {
            foreach (var cost in costs)
            {
                inventoryManager.RemoveResource(cost.ResourceType, cost.Amount);
            }
        }

        private void OnGameStateChanged(GameStateChangedEvent e)
        {
            _isFrozen = (e.NewState == GameState.Paused || e.NewState == GameState.InMenu);
        }
    }
}