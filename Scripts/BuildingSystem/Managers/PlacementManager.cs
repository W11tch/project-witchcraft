using UnityEngine;
using System.Collections;
using ProjectWitchcraft.BuildingSystem;
using ProjectWitchcraft.Core;

namespace ProjectWitchcraft.BuildingSystem
{
    public class PlacementManager : MonoBehaviour
    {
        private static PlacementManager _instance;
        public static PlacementManager Instance => _instance;

        [SerializeField] private Transform placedObjectsParent;
        [SerializeField] private LayerMask buildingBlockLayer;

        private PlaceableObject objectToPlace;

        private void Awake()
        {
            if (_instance != null && _instance != this) { Destroy(gameObject); }
            else { _instance = this; }

            BuildingEvents.OnObjectSelected += SelectObject;
            BuildingEvents.OnObjectPlaced += PlaceObject;
            BuildingEvents.OnObjectRotated += RotateObject;
            BuildingEvents.OnObjectDestroyed += DestroyObject;
            BuildingEvents.OnPlacementCanceled += CancelPlacement;
        }

        private void OnDestroy()
        {
            BuildingEvents.OnObjectSelected -= SelectObject;
            BuildingEvents.OnObjectPlaced -= PlaceObject;
            BuildingEvents.OnObjectRotated -= RotateObject;
            BuildingEvents.OnObjectDestroyed -= DestroyObject;
            BuildingEvents.OnPlacementCanceled -= CancelPlacement;
        }

        private void Update()
        {
            if (objectToPlace == null) return;
            if (BuildingSystem.Instance == null) return;

            Vector3 newPosition = BuildingSystem.Instance.GetMouseWorldPosition();
            newPosition = WorldGridManager.Instance.SnapToGrid(newPosition);

            BuildingData.PlacementType finalPlacementType = GetFinalPlacementType(newPosition, objectToPlace.Size);
            bool canPlace = IsPlacementValid(finalPlacementType, newPosition, objectToPlace.Size);

            objectToPlace.GetComponent<VisualsController>()?.SetIsTransparent(true, canPlace ? 0.5f : 0.2f);

            Collider objectCollider = objectToPlace.GetComponent<Collider>();
            if (objectCollider != null)
            {
                float halfHeight = objectCollider.bounds.extents.y;

                if (finalPlacementType == BuildingData.PlacementType.GroundLevel)
                {
                    newPosition.y = 0.01f - halfHeight;
                }
                else // UpperLevel
                {
                    var gridCell = WorldGridManager.Instance.GetGridData(newPosition);
                    if (gridCell != null && gridCell.groundObject != null)
                    {
                        var groundCollider = gridCell.groundObject.GetComponent<Collider>();
                        float topOfGroundObject = gridCell.groundObject.transform.position.y + groundCollider.bounds.extents.y;
                        newPosition.y = topOfGroundObject + halfHeight;
                    }
                    else
                    {
                        newPosition.y = 0.01f - halfHeight;
                    }
                }
            }
            objectToPlace.SetPosition(newPosition);
        }

        public void SelectObject(BuildingData buildingData)
        {
            if (buildingData?.prefab == null) return;
            if (objectToPlace != null) Destroy(objectToPlace.gameObject);

            GameObject obj = Instantiate(buildingData.prefab);
            obj.transform.position = BuildingSystem.Instance.GetMouseWorldPosition();
            objectToPlace = obj.GetComponent<PlaceableObject>();
            objectToPlace.GetComponent<VisualsController>()?.SetIsTransparent(true, 0.5f);
            obj.transform.SetParent(placedObjectsParent);
        }

        private void PlaceObject()
        {
            if (objectToPlace == null) return;

            var finalPlacementType = GetFinalPlacementType(objectToPlace.transform.position, objectToPlace.Size);

            if (IsPlacementValid(finalPlacementType, objectToPlace.transform.position, objectToPlace.Size))
            {
                objectToPlace.GetComponent<VisualsController>()?.SetIsTransparent(false);
                WorldGridManager.Instance.PlaceObject(objectToPlace, finalPlacementType);
                objectToPlace.Placed = true;

                BuildingData originalData = objectToPlace.BuildingData;
                objectToPlace = null;

                StartCoroutine(SelectObjectNextFrame(originalData));
            }
        }

        private IEnumerator SelectObjectNextFrame(BuildingData data)
        {
            yield return new WaitForEndOfFrame();
            SelectObject(data);
        }

        private void RotateObject() => objectToPlace?.Rotate();

        private void DestroyObject()
        {
            if (Camera.main == null) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 100f, buildingBlockLayer))
            {
                var objToDestroy = hit.collider.GetComponentInParent<PlaceableObject>();
                if (objToDestroy != null)
                {
                    var gridData = WorldGridManager.Instance.GetGridData(objToDestroy.transform.position);
                    if (gridData != null)
                    {
                        bool isGroundObject = gridData.groundObject == objToDestroy;
                        if (isGroundObject && gridData.upperObject != null)
                        {
                            return;
                        }
                    }

                    WorldGridManager.Instance.RemoveObject(objToDestroy);
                    Destroy(objToDestroy.gameObject);
                }
            }
        }

        private void CancelPlacement()
        {
            if (objectToPlace != null) Destroy(objectToPlace.gameObject);
            objectToPlace = null;
        }

        private BuildingData.PlacementType GetFinalPlacementType(Vector3 pos, Vector3Int size)
        {
            if (objectToPlace == null) return BuildingData.PlacementType.GroundLevel;
            var placementType = objectToPlace.BuildingData.placementType;
            if (placementType == BuildingData.PlacementType.Either)
            {
                return WorldGridManager.Instance.AreCellsFreeForUpper(pos, size) ? BuildingData.PlacementType.UpperLevel : BuildingData.PlacementType.GroundLevel;
            }
            return placementType;
        }

        private bool IsPlacementValid(BuildingData.PlacementType type, Vector3 pos, Vector3Int size)
        {
            if (type == BuildingData.PlacementType.UpperLevel) return WorldGridManager.Instance.AreCellsFreeForUpper(pos, size);
            return WorldGridManager.Instance.AreCellsFreeForGround(pos, size);
        }
    }
}