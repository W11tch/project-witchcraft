using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProjectWitchcraft.BuildingSystem;
using ProjectWitchcraft.Core;
// Note: "using ProjectWitchcraft.UI;" has been removed.

namespace ProjectWitchcraft.Managers
{
    public class PlacementManager : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private BuildingManager buildingManager;
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

        private void Awake()
        {
            _previewLayer = LayerMask.NameToLayer("Preview");
            BuildingEvents.OnObjectSelected += StartPlacement;
        }

        private void OnDestroy()
        {
            BuildingEvents.OnObjectSelected -= StartPlacement;
        }

        private void Update()
        {
            if (_isPlacing)
            {
                UpdatePreview();
                if (Input.GetMouseButtonDown(0)) HandleObjectPlaced();
                if (Input.GetKeyDown(KeyCode.R)) HandleObjectRotated();
                if (Input.GetKeyDown(KeyCode.Escape)) StopPlacement();
            }
            if (Input.GetKeyDown(KeyCode.F)) HandleObjectDestroyed();
        }

        private void StartPlacement(BuildingData buildingData)
        {
            if (buildingData?.prefab == null) return;
            if (_isPlacing && _currentBuildingData == buildingData)
            {
                StopPlacement();
                return;
            }
            StopPlacement();

            _isPlacing = true;
            _currentBuildingData = buildingData;

            _previewObject = objectPooler.SpawnFromPool(_currentBuildingData.poolTag).GetComponent<PlaceableObject>();

            _previewObject.gameObject.layer = _previewLayer;
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

        private void HandleObjectPlaced()
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

            var type = GetFinalPlacementType(_previewObject.transform.position, _previewObject.Size);

            if (IsPlacementValid(type, _previewObject.transform.position, _previewObject.Size))
            {
                ConsumeResources(_currentBuildingData.Cost);
                var finalObject = Instantiate(_currentBuildingData.prefab, _previewObject.transform.position, _previewObject.transform.rotation, placedObjectsParent)
                    .GetComponent<PlaceableObject>();
                finalObject.GetComponent<VisualsController>()?.SetIsTransparent(false);
                finalObject.Placed = true;
                worldGridManager.PlaceObject(finalObject, type);
            }
        }

        private void HandleObjectRotated() => _previewObject?.Rotate();

        private void HandleObjectDestroyed()
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
                    Destroy(objToDestroy.gameObject);
                }
            }
        }

        private void UpdatePreview()
        {
            if (_previewObject == null) return;
            Vector3 groundPos = buildingManager.GetMouseWorldPosition();
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
                return worldGridManager.IsAreaFree(position, size, BuildingData.PlacementType.UpperLevel)
                    ? BuildingData.PlacementType.UpperLevel
                    : BuildingData.PlacementType.GroundLevel;
            }
            return desiredType;
        }

        private bool IsPlacementValid(BuildingData.PlacementType type, Vector3 position, Vector3Int size)
        {
            if (!CanAffordCost(_currentBuildingData.Cost)) return false;
            Collider objectCollider = _currentBuildingData.prefab.GetComponent<Collider>();
            if (objectCollider != null)
            {
                if (Physics.CheckBox(position, objectCollider.bounds.extents, _previewObject.transform.rotation, playerLayer))
                {
                    return false;
                }
            }
            return type == BuildingData.PlacementType.UpperLevel
                ? worldGridManager.IsAreaFree(position, size, BuildingData.PlacementType.UpperLevel)
                : worldGridManager.IsAreaFree(position, size, BuildingData.PlacementType.GroundLevel);
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