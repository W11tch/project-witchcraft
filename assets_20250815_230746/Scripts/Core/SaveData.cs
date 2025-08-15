using System.Collections.Generic;
using UnityEngine;
using ProjectWitchcraft.Core; // For ResourceType

namespace ProjectWitchcraft.Core
{
    /// <summary>
    /// A serializable class that holds all the data we want to save.
    /// </summary>
    [System.Serializable]
    public class SaveData
    {
        // We will store resources using the name of the ResourceType asset.
        public Dictionary<string, int> resourceAmounts;
        public List<ObjectData> placedObjects;

        public SaveData()
        {
            resourceAmounts = new Dictionary<string, int>();
            placedObjects = new List<ObjectData>();
        }
    }

    /// <summary>
    /// A serializable class that holds the state of a single placed object.
    /// </summary>
    [System.Serializable]
    public class ObjectData
    {
        public string buildingDataName; // The name of the BuildingData asset used to create this object.
        public Vector3 position;
        public Quaternion rotation;
    }
}