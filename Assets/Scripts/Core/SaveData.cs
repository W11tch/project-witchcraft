// Located at: Assets/Scripts/Core/SaveData.cs
using System.Collections.Generic;
using UnityEngine;

namespace ProjectWitchcraft.Core
{
    [System.Serializable]
    public class SaveData
    {
        // FIX: JsonUtility cannot serialize dictionaries directly.
        // We use two lists instead: one for the keys (resource names) and one for the values (amounts).
        public List<string> resourceNames;
        public List<int> resourceAmounts;
        public List<ObjectData> placedObjects;

        public SaveData()
        {
            resourceNames = new List<string>();
            resourceAmounts = new List<int>();
            placedObjects = new List<ObjectData>();
        }
    }

    [System.Serializable]
    public class ObjectData
    {
        public string buildingDataName;
        public Vector3 position;
        public Quaternion rotation;
    }
}