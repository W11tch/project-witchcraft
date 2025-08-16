// Located at: Assets/Scripts/Managers/WorldGridManager.cs
using UnityEngine;
using ProjectWitchcraft.BuildingSystem;
using ProjectWitchcraft.Core;

namespace ProjectWitchcraft.Managers
{
    public class WorldGridManager : Singleton<WorldGridManager>
    {
        [Header("Configuration")]
        [SerializeField] private int gridWidth = 100;
        [SerializeField] private int gridHeight = 100;
        [SerializeField] private float cellSize = 1f;

        private GridCell[,] _grid;

        protected override void Awake()
        {
            base.Awake();
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            _grid = new GridCell[gridWidth, gridHeight];
            for (int x = 0; x < gridWidth; x++)
            {
                for (int z = 0; z < gridHeight; z++)
                {
                    _grid[x, z] = new GridCell();
                }
            }
        }

        public Vector3 SnapToGrid(Vector3 position)
        {
            int x = Mathf.RoundToInt(position.x / cellSize);
            int z = Mathf.RoundToInt(position.z / cellSize);
            return new Vector3(x * cellSize, 0, z * cellSize);
        }

        public void PlaceObject(PlaceableObject obj, BuildingData.PlacementType type)
        {
            Vector3Int gridPos = WorldToGrid(obj.transform.position);
            for (int x = 0; x < obj.Size.x; x++)
            {
                for (int z = 0; z < obj.Size.z; z++)
                {
                    int gridX = gridPos.x + x;
                    int gridZ = gridPos.z + z;
                    if (type == BuildingData.PlacementType.GroundLevel)
                        _grid[gridX, gridZ].groundObject = obj;
                    else
                        _grid[gridX, gridZ].upperObject = obj;
                }
            }
        }

        public void RemoveObject(PlaceableObject obj)
        {
            Vector3Int gridPos = WorldToGrid(obj.transform.position);
            for (int x = 0; x < obj.Size.x; x++)
            {
                for (int z = 0; z < obj.Size.z; z++)
                {
                    int gridX = gridPos.x + x;
                    int gridZ = gridPos.z + z;
                    if (_grid[gridX, gridZ].groundObject == obj)
                        _grid[gridX, gridZ].groundObject = null;
                    if (_grid[gridX, gridZ].upperObject == obj)
                        _grid[gridX, gridZ].upperObject = null;
                }
            }
        }

        // This is the updated method that ignores a specific object during the check.
        public bool IsAreaFree(Vector3 worldPos, Vector3Int size, BuildingData.PlacementType type, PlaceableObject objectToIgnore = null)
        {
            Vector3Int gridPos = WorldToGrid(worldPos);
            for (int x = 0; x < size.x; x++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    int gridX = gridPos.x + x;
                    int gridZ = gridPos.z + z;
                    if (gridX < 0 || gridX >= gridWidth || gridZ < 0 || gridZ >= gridHeight) return false;

                    var cell = _grid[gridX, gridZ];

                    if (type == BuildingData.PlacementType.GroundLevel && cell.groundObject != null && cell.groundObject != objectToIgnore) return false;
                    if (type == BuildingData.PlacementType.UpperLevel && cell.upperObject != null && cell.upperObject != objectToIgnore) return false;
                }
            }
            return true;
        }

        public GridCell GetGridData(Vector3 worldPos)
        {
            Vector3Int gridPos = WorldToGrid(worldPos);
            if (gridPos.x >= 0 && gridPos.x < gridWidth && gridPos.z >= 0 && gridPos.z < gridHeight)
            {
                return _grid[gridPos.x, gridPos.z];
            }
            return null;
        }

        private Vector3Int WorldToGrid(Vector3 worldPos)
        {
            int x = Mathf.RoundToInt(worldPos.x / cellSize);
            int z = Mathf.RoundToInt(worldPos.z / cellSize);
            return new Vector3Int(x, 0, z);
        }

        public class GridCell
        {
            public PlaceableObject groundObject;
            public PlaceableObject upperObject;
        }
    }
}
