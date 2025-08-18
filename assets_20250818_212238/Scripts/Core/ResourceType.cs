// Located at: Assets/Scripts/Core/ResourceType.cs
using UnityEngine;

namespace ProjectWitchcraft.Core
{
    [CreateAssetMenu(fileName = "New ResourceType", menuName = "Witchcraft/Core/ResourceType")]
    public class ResourceType : ScriptableObject
    {
        [Header("Resource Information")]
        public string Name;
        public Sprite Icon;
    }
}
