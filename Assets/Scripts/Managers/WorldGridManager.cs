// Located at: Assets/Scripts/Managers/WorldGridManager.cs
using ProjectWitchcraft.BuildingSystem;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProjectWitchcraft.Core;

namespace ProjectWitchcraft.Managers
{
    [Serializable]
    public class GridCell
    {
        public PlaceableObject groundObject = null;
        public PlaceableObject upperObject = null;
    }

    public class WorldGridManager : Singleton<WorldGridManager>
    {
        [Header("Configuration")]
        [SerializeField] private float cellSize = 1f;

        private Dictionary<Vector2Int, GridCell> _grid = new Dictionary<Vector2Int, GridCell>();
        private Transform _placedObjectsParent;

        // This is a new method for debug visualization.
        private void OnDrawGizmos()
        {
            if (_grid == null || _grid.Count == 0) return;
            Gizmos.color = Color.yellow;
            foreach (var pair in _grid)
            {
                if (pair.Value.groundObject != null || pair.Value.upperObject != null)
                {
                    Vector3 cellCenter = new Vector3(pair.Key.x * cellSize + (cellSize / 2.0f), 0, pair.Key.y * cellSize + (cellSize / 2.0f));
                    Gizmos.DrawWireCube(cellCenter, new Vector3(cellSize, 0.1f, cellSize));
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            var placementManager = FindAnyObjectByType<PlacementManager>();
            if (placementManager != null)
            {
                _placedObjectsParent = placementManager.PlacedObjectsParent;
            }
        }

        private void OnEnable()
        {
            EventManager.AddListener<GatherSaveDataEvent>(OnGatherSaveData);
            EventManager.AddListener<ApplySaveDataEvent>(OnApplySaveData);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<GatherSaveDataEvent>(OnGatherSaveData);
            EventManager.RemoveListener<ApplySaveDataEvent>(OnApplySaveData);
        }

        private void OnGatherSaveData(GatherSaveDataEvent e)
        {
            e.SaveData.placedObjects.Clear();
            var processedObjects = new HashSet<PlaceableObject>();
            foreach (var cell in _grid.Values)
            {
                if (cell.groundObject != null && processedObjects.Add(cell.groundObject))
                {
                    e.SaveData.placedObjects.Add(new ObjectData { buildingDataName = cell.groundObject.BuildingData.name, position = cell.groundObject.transform.position, rotation = cell.groundObject.transform.rotation });
                }
                if (cell.upperObject != null && processedObjects.Add(cell.upperObject))
                {
                    e.SaveData.placedObjects.Add(new ObjectData { buildingDataName = cell.upperObject.BuildingData.name, position = cell.upperObject.transform.position, rotation = cell.upperObject.transform.rotation });
                }
            }
        }

        private void OnApplySaveData(ApplySaveDataEvent e)
        {
            ClearAllPlacedObjects();
            foreach (var objectData in e.SaveData.placedObjects)
            {
                BuildingData buildingData = Resources.Load<BuildingData>($"BuildingSystem/BuildingData/{objectData.buildingDataName}");
                if (buildingData != null)
                {
                    var placedObject = PlacementManager.Instance.objectPooler.SpawnFromPool(buildingData.poolTag, objectData.position, objectData.rotation).GetComponent<PlaceableObject>();
                    if (placedObject != null)
                    {
                        placedObject.transform.SetParent(_placedObjectsParent);
                        placedObject.GetComponent<VisualsController>()?.SetIsTransparent(false);
                        placedObject.Placed = true;
                        PlaceObject(placedObject, buildingData.Category);
                    }
                }
            }
        }

        private void ClearAllPlacedObjects()
        {
            if (PlacementManager.Instance == null) return;
            foreach (var cell in _grid.Values)
            {
                if (cell.groundObject != null) { PlacementManager.Instance.objectPooler.ReturnToPool(cell.groundObject.gameObject); }
                if (cell.upperObject != null) { PlacementManager.Instance.objectPooler.ReturnToPool(cell.upperObject.gameObject); }
            }
            _grid.Clear();
        }

        public bool IsTileWalkable(Vector3 worldPosition)
        {
            var cell = GetGridData(worldPosition);
            if (cell == null) return false;
            if (cell.groundObject == null || (cell.groundObject.BuildingData.Category != BuildingData.ObjectCategory.Block && cell.groundObject.BuildingData.Category != BuildingData.ObjectCategory.Bridge)) return false;
            if (!cell.groundObject.BuildingData.isWalkable) return false;
            if (cell.upperObject != null && !cell.upperObject.BuildingData.isWalkable) return false;
            return true;
        }

        public bool HasWallBehind(Vector3 worldPosition)
        {
            Vector2Int baseCoord = WorldToGridCoords(worldPosition);
            Vector2Int[] neighbors = { baseCoord + Vector2Int.up, baseCoord + Vector2Int.down, baseCoord + Vector2Int.left, baseCoord + Vector2Int.right };
            foreach (var coord in neighbors)
            {
                if (_grid.TryGetValue(coord, out var cell) && cell.upperObject != null)
                {
                    var category = cell.upperObject.BuildingData.Category;
                    if (category == BuildingData.ObjectCategory.Wall || category == BuildingData.ObjectCategory.Block) return true;
                }
            }
            return false;
        }

        public Vector3 SnapToGridCenter(Vector3 worldPosition)
        {
            float x = Mathf.Floor(worldPosition.x / cellSize) * cellSize + (cellSize / 2.0f);
            float z = Mathf.Floor(worldPosition.z / cellSize) * cellSize + (cellSize / 2.0f);
            return new Vector3(x, 0, z);
        }

        public Vector2Int WorldToGridCoords(Vector3 worldPosition)
        {
            int x = Mathf.FloorToInt(worldPosition.x / cellSize);
            int y = Mathf.FloorToInt(worldPosition.z / cellSize);
            return new Vector2Int(x, y);
        }

        public GridCell GetGridData(Vector3 worldPosition)
        {
            var gridCoords = WorldToGridCoords(worldPosition);
            _grid.TryGetValue(gridCoords, out var cell);
            return cell;
        }

        private GridCell GetOrCreateGridCell(Vector2Int gridCoords)
        {
            if (!_grid.TryGetValue(gridCoords, out var cell))
            {
                cell = new GridCell();
                _grid[gridCoords] = cell;
            }
            return cell;
        }

        public void PlaceObject(PlaceableObject obj, BuildingData.ObjectCategory category)
        {
            var gridCoords = WorldToGridCoords(obj.transform.position);
            var cell = GetOrCreateGridCell(gridCoords);
            if (category == BuildingData.ObjectCategory.Block)
            {
                if (cell.groundObject == null) cell.groundObject = obj;
                else cell.upperObject = obj;
            }
            else if (category == BuildingData.ObjectCategory.Bridge)
            {
                cell.groundObject = obj;
            }
            else
            {
                cell.upperObject = obj;
            }
        }

        public void RemoveObject(PlaceableObject obj)
        {
            var gridCoords = WorldToGridCoords(obj.transform.position);
            if (_grid.TryGetValue(gridCoords, out var cell))
            {
                if (cell.groundObject == obj) cell.groundObject = null;
                if (cell.upperObject == obj) cell.upperObject = null;
            }
        }
    }
}