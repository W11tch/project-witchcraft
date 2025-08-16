using UnityEngine;
using System.Collections.Generic;
using ProjectWitchcraft.Core;

namespace ProjectWitchcraft.BuildingSystem
{
    [System.Serializable]
    public class ResourceCost
    {
        public ResourceType ResourceType;
        public int Amount;
    }

    [CreateAssetMenu(fileName = "New Building Data", menuName = "Building System/Building Data")]
    public class BuildingData : ScriptableObject
    {
        public enum PlacementType { GroundLevel, UpperLevel, Either }

        [Tooltip("The name of the building as it would appear in the UI.")]
        public string buildingName;
        [Tooltip("The prefab for this building.")]
        public GameObject prefab;
        [Tooltip("How this object can be placed on the grid.")]
        public PlacementType placementType;
        [Tooltip("The tag used to identify this object's pool in the ObjectPooler.")]
        public string poolTag;

        // **NEW FIELD FOR SAVING**
        [Tooltip("A unique ID for saving/loading. This should match the asset's file name.")]
        public string id;

        [Tooltip("A list of all the resources and their amounts required to build this object.")]
        public List<ResourceCost> Cost;

        // This function will automatically set the ID to match the file name in the editor.
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(id))
            {
                id = this.name;
            }
        }
    }
}