using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    private static PlacementManager _instance;
    public static PlacementManager Instance => _instance;

    [SerializeField] private Transform placedObjectsParent;
    [SerializeField] private LayerMask buildingBlockLayer;

    private PlaceableObject objectToPlace;

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

        VisualsController visualsController = objectToPlace.GetComponent<VisualsController>();
        if (visualsController != null)
        {
            visualsController.SetIsTransparent(true, canPlace ? 0.5f : 0.2f);
        }

        Collider objectCollider = objectToPlace.GetComponent<Collider>();
        if (objectCollider != null)
        {
            float height = objectCollider.bounds.size.y;
            if (finalPlacementType == BuildingData.PlacementType.GroundLevel)
            {
                newPosition.y -= height / 2f;
            }
            else
            {
                newPosition.y += height / 2f;
            }
        }

        objectToPlace.SetPosition(newPosition);
    }

    public void SelectObject(BuildingData buildingData)
    {
        if (buildingData.prefab == null)
        {
            Debug.LogError($"The prefab field is not assigned in the {buildingData.name} BuildingData asset.");
            return;
        }

        if (objectToPlace != null)
        {
            Destroy(objectToPlace.gameObject);
        }

        GameObject obj = Instantiate(buildingData.prefab);

        if (BuildingSystem.Instance != null)
        {
            obj.transform.position = BuildingSystem.Instance.GetMouseWorldPosition();
        }
        else
        {
            obj.transform.position = Vector3.zero;
        }

        objectToPlace = obj.GetComponent<PlaceableObject>();

        if (objectToPlace == null)
        {
            Debug.LogError("Prefab does not have a PlaceableObject script attached!");
            Destroy(obj);
            return;
        }

        VisualsController visualsController = obj.GetComponent<VisualsController>();
        if (visualsController != null)
        {
            visualsController.SetIsTransparent(true, 0.5f);
        }

        obj.transform.SetParent(placedObjectsParent);
    }

    private void PlaceObject()
    {
        if (objectToPlace == null) return;

        Vector3 snappedPosition = objectToPlace.transform.position;
        BuildingData.PlacementType finalPlacementType = GetFinalPlacementType(snappedPosition, objectToPlace.Size);

        bool canPlace = IsPlacementValid(finalPlacementType, snappedPosition, objectToPlace.Size);

        if (canPlace)
        {
            VisualsController visualsController = objectToPlace.GetComponent<VisualsController>();
            if (visualsController != null)
            {
                visualsController.SetIsTransparent(false);
            }

            WorldGridManager.Instance.PlaceObject(objectToPlace, finalPlacementType);
            objectToPlace.Placed = true;

            objectToPlace = null;
        }
        else
        {
            // NEW: Trigger the cancel event instead of destroying the object directly
            BuildingEvents.TriggerPlacementCanceled();
        }
    }

    private void RotateObject()
    {
        if (objectToPlace != null)
        {
            objectToPlace.Rotate();
        }
    }

    private void DestroyObject()
    {
        if (Camera.main == null)
        {
            Debug.LogError("Main Camera not found. Please ensure your camera has the 'MainCamera' tag.");
            return;
        }

        if (WorldGridManager.Instance == null)
        {
            Debug.LogError("WorldGridManager.Instance is null. Is the corresponding GameObject active in the scene?");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);

        if (Physics.Raycast(ray, out hit, 100f, buildingBlockLayer))
        {
            PlaceableObject objToDestroy = hit.collider.GetComponentInParent<PlaceableObject>();

            if (objToDestroy != null)
            {
                WorldGridManager.GridData gridData = WorldGridManager.Instance.GetGridData(objToDestroy.transform.position);

                if (gridData == null)
                {
                    Debug.LogWarning("Grid data is null. The object hit might not be registered in the grid.");
                    return;
                }

                if (gridData.upperObject != null)
                {
                    PlaceableObject objectToDestroyCache = gridData.upperObject;
                    WorldGridManager.Instance.RemoveObject(objectToDestroyCache);
                    Destroy(objectToDestroyCache.gameObject);
                }
                else if (gridData.groundObject != null && gridData.upperObject == null)
                {
                    PlaceableObject objectToDestroyCache = gridData.groundObject;
                    WorldGridManager.Instance.RemoveObject(objectToDestroyCache);
                    Destroy(objectToDestroyCache.gameObject);
                }
            }
            else
            {
                Debug.LogWarning("Raycast hit an object on the BuildingBlocks layer, but it's not a PlaceableObject.");
            }
        }
        else
        {
            Debug.Log("Raycast did not hit any object on the BuildingBlocks layer.");
            if (Physics.Raycast(ray, out hit, 100f))
            {
                Debug.Log("Raycast hit an object, but it's not on the BuildingBlocks layer. The object name is: " + hit.collider.gameObject.name + " and its layer is: " + LayerMask.LayerToName(hit.collider.gameObject.layer));
            }
            else
            {
                Debug.Log("Raycast hit nothing at all.");
            }
        }
    }

    private void CancelPlacement()
    {
        if (objectToPlace != null)
        {
            Destroy(objectToPlace.gameObject);
            objectToPlace = null;
        }
    }

    private BuildingData.PlacementType GetFinalPlacementType(Vector3 snappedPosition, Vector3Int size)
    {
        if (objectToPlace == null) return BuildingData.PlacementType.GroundLevel;

        BuildingData.PlacementType placementType = objectToPlace.BuildingData.placementType;

        if (placementType == BuildingData.PlacementType.GroundLevel)
        {
            return BuildingData.PlacementType.GroundLevel;
        }

        if (placementType == BuildingData.PlacementType.UpperLevel)
        {
            return BuildingData.PlacementType.UpperLevel;
        }

        if (placementType == BuildingData.PlacementType.Either)
        {
            if (WorldGridManager.Instance.AreCellsFreeForUpper(snappedPosition, size))
            {
                return BuildingData.PlacementType.UpperLevel;
            }
            else if (WorldGridManager.Instance.AreCellsFreeForGround(snappedPosition, size))
            {
                return BuildingData.PlacementType.GroundLevel;
            }
        }

        return BuildingData.PlacementType.GroundLevel;
    }

    private bool IsPlacementValid(BuildingData.PlacementType placementType, Vector3 position, Vector3Int size)
    {
        if (placementType == BuildingData.PlacementType.UpperLevel)
        {
            return WorldGridManager.Instance.AreCellsFreeForUpper(position, size);
        }

        if (placementType == BuildingData.PlacementType.GroundLevel)
        {
            return WorldGridManager.Instance.AreCellsFreeForGround(position, size);
        }

        if (placementType == BuildingData.PlacementType.Either)
        {
            return WorldGridManager.Instance.AreCellsFreeForUpper(position, size) || WorldGridManager.Instance.AreCellsFreeForGround(position, size);
        }

        return false;
    }
}