using System.Collections.Generic;
using UnityEngine;
using ProjectWitchcraft.BuildingSystem;

namespace ProjectWitchcraft.Managers
{
    public class WorldGridManager : MonoBehaviour
    {
        private Dictionary<Vector3Int, GridData> gridData;
        private Collider _playerCollider;

        public class GridData
        {
            public PlaceableObject groundObject;
            public PlaceableObject upperObject;
        }

        private void Awake()
        {
            gridData = new Dictionary<Vector3Int, GridData>();
            var player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                _playerCollider = player.GetComponentInChildren<Collider>();
            }
        }

        // **NEW METHOD**: Clears all data from the grid.
        public void ClearGrid()
        {
            gridData.Clear();
            Debug.Log("Grid data cleared.");
        }

        public Vector3Int WorldToCell(Vector3 worldPosition)
        {
            int x = Mathf.FloorToInt(worldPosition.x);
            int z = Mathf.FloorToInt(worldPosition.z);
            return new Vector3Int(x, 0, z);
        }

        public Vector3 SnapToGrid(Vector3 worldPosition)
        {
            Vector3Int cellPosition = WorldToCell(worldPosition);
            return new Vector3(cellPosition.x + 0.5f, worldPosition.y, cellPosition.z + 0.5f);
        }

        public bool IsAreaFree(Vector3 worldPosition, Vector3Int size, BuildingData.PlacementType placementType)
        {
            Vector3Int startCell = WorldToCell(worldPosition);
            for (int x = 0; x < size.x; x++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    Vector3Int cell = new Vector3Int(startCell.x + x, 0, startCell.z + z);

                    if (_playerCollider != null)
                    {
                        Vector3 cellCenter = new Vector3(cell.x + 0.5f, 0, cell.z + 0.5f);
                        if (_playerCollider.bounds.Contains(cellCenter))
                        {
                            return false;
                        }
                    }

                    if (placementType == BuildingData.PlacementType.GroundLevel)
                    {
                        if (gridData.ContainsKey(cell) && (gridData[cell].groundObject != null || gridData[cell].upperObject != null))
                        {
                            return false;
                        }
                    }
                    else // UpperLevel check
                    {
                        if (!gridData.ContainsKey(cell) || gridData[cell].groundObject == null || gridData[cell].upperObject != null)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public void PlaceObject(PlaceableObject obj, BuildingData.PlacementType placementType)
        {
            Vector3Int startCell = WorldToCell(obj.GetStartPosition());
            Vector3Int size = obj.Size;
            for (int x = 0; x < size.x; x++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    Vector3Int cell = new Vector3Int(startCell.x + x, 0, startCell.z + z);
                    if (!gridData.ContainsKey(cell)) gridData[cell] = new GridData();

                    if (placementType == BuildingData.PlacementType.GroundLevel) gridData[cell].groundObject = obj;
                    else gridData[cell].upperObject = obj;
                }
            }
        }

        public void RemoveObject(PlaceableObject obj)
        {
            Vector3Int startCell = WorldToCell(obj.GetStartPosition());
            Vector3Int size = obj.Size;
            for (int x = 0; x < size.x; x++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    Vector3Int cell = new Vector3Int(startCell.x + x, 0, startCell.z + z);
                    if (gridData.ContainsKey(cell))
                    {
                        if (gridData[cell].groundObject == obj) gridData[cell].groundObject = null;
                        else if (gridData[cell].upperObject == obj) gridData[cell].upperObject = null;

                        if (gridData[cell].groundObject == null && gridData[cell].upperObject == null) gridData.Remove(cell);
                    }
                }
            }
        }

        public GridData GetGridData(Vector3 worldPosition)
        {
            Vector3Int cellPosition = WorldToCell(worldPosition);
            if (gridData.ContainsKey(cellPosition)) return gridData[cellPosition];
            return null;
        }
    }
}