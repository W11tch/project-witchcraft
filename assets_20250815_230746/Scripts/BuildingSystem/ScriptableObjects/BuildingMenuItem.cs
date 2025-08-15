using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectWitchcraft.BuildingSystem
{
    [CreateAssetMenu(fileName = "New Building Menu Item", menuName = "Building System/Building Menu Item")]
    public class BuildingMenuItem : ScriptableObject
    {
        [Tooltip("The building data for the object to be placed.")]
        public BuildingData BuildingData;

        [Tooltip("The input action that will select this building (e.g., from your Input Actions asset).")]
        public InputActionReference SelectAction;

        // **NEW FIELD**: This will store the name of the key (e.g., "1", "2", "3").
        [Tooltip("The name of the key that triggers this action, as seen in the Input System (e.g., '1', '2', 'a', 'd').")]
        public string KeyIdentifier;
    }
}