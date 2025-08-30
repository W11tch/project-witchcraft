// Located at: Assets/Scripts/Core/SaveData.cs
using System.Collections.Generic;
using UnityEngine;

namespace ProjectWitchcraft.Core
{
    [System.Serializable]
    public class SaveData
    {
        // We use two lists for serialization because JsonUtility cannot handle dictionaries.
        public List<string> itemNames;       // Renamed from resourceNames
        public List<int> itemAmounts;     // Renamed from resourceAmounts
        public List<ObjectData> placedObjects;

        public SaveData()
        {
            itemNames = new List<string>();
            itemAmounts = new List<int>();
            placedObjects = new List<ObjectData>();
        }
    }

    [System.Serializable]
    public class ObjectData
    {
        public string itemDataName; // Renamed from buildingDataName
        public Vector3 position;
        public Quaternion rotation;
    }
}