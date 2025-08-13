using System.Collections.Generic;
using UnityEngine;

public class WorldGridManager : MonoBehaviour
{
    private static WorldGridManager _instance;
    public static WorldGridManager Instance => _instance;

    // Use a dictionary for an efficient, sparse grid
    private Dictionary<Vector3Int, GridData> gridData;

    public class GridData
    {
        public PlaceableObject groundObject;
        public PlaceableObject upperObject;
    }

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

        gridData = new Dictionary<Vector3Int, GridData>();
    }

    // Maps a world position to a grid cell (e.g., 0.5, 0, 0.5 maps to cell 0, 0, 0)
    public Vector3Int WorldToCell(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x);
        int z = Mathf.FloorToInt(worldPosition.z);
        return new Vector3Int(x, 0, z);
    }

    // Snaps an object's position to the center of a cell
    public Vector3 SnapToGrid(Vector3 worldPosition)
    {
        Vector3Int cellPosition = WorldToCell(worldPosition);
        return new Vector3(cellPosition.x + 0.5f, worldPosition.y, cellPosition.z + 0.5f);
    }

    // Checks if cells are free for a new object
    public bool AreCellsFreeForGround(Vector3 worldPosition, Vector3Int size)
    {
        Vector3Int startCell = WorldToCell(worldPosition);
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.z; z++)
            {
                Vector3Int cell = new Vector3Int(startCell.x + x, 0, startCell.z + z);
                if (gridData.ContainsKey(cell))
                {
                    // Cell is occupied if any object is present
                    if (gridData[cell].groundObject != null || gridData[cell].upperObject != null)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public bool AreCellsFreeForUpper(Vector3 worldPosition, Vector3Int size)
    {
        Vector3Int startCell = WorldToCell(worldPosition);
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.z; z++)
            {
                Vector3Int cell = new Vector3Int(startCell.x + x, 0, startCell.z + z);
                // Cannot place upper-level object if there is no ground object
                if (!gridData.ContainsKey(cell) || gridData[cell].groundObject == null || gridData[cell].upperObject != null)
                {
                    return false;
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
                if (!gridData.ContainsKey(cell))
                {
                    gridData[cell] = new GridData();
                }

                if (placementType == BuildingData.PlacementType.GroundLevel)
                {
                    gridData[cell].groundObject = obj;
                }
                else
                {
                    gridData[cell].upperObject = obj;
                }
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
                    if (gridData[cell].groundObject == obj)
                    {
                        gridData[cell].groundObject = null;
                    }
                    else if (gridData[cell].upperObject == obj)
                    {
                        gridData[cell].upperObject = null;
                    }
                    // Optional: remove cell from dictionary if it's completely empty
                    if (gridData[cell].groundObject == null && gridData[cell].upperObject == null)
                    {
                        gridData.Remove(cell);
                    }
                }
            }
        }
    }

    public GridData GetGridData(Vector3 worldPosition)
    {
        Vector3Int cellPosition = WorldToCell(worldPosition);
        if (gridData.ContainsKey(cellPosition))
        {
            return gridData[cellPosition];
        }
        return null;
    }
}