using UnityEngine;

namespace ProjectWitchcraft.Core
{
    [CreateAssetMenu(fileName = "New Resource Type", menuName = "Resources/Resource Type")]
    public class ResourceType : ScriptableObject
    {
        [Header("Resource Properties")]
        [Tooltip("The user-friendly name for this resource (e.g., 'Brown Cube', 'Iron Ore').")]
        public string DisplayName;
        // We can add more properties here later, like an icon or description.
    }
}