// Located at: Assets/Scripts/Core/ResourceDatabase.cs
using System.Collections.Generic;
using UnityEngine;

namespace ProjectWitchcraft.Core
{
    [CreateAssetMenu(fileName = "ResourceDatabase", menuName = "Witchcraft/Core/Resource Database")]
    public class ResourceDatabase : ScriptableObject
    {
        public List<ResourceType> AllResources;

        // A dictionary for fast lookups by name.
        private Dictionary<string, ResourceType> _resourceMap;

        public void Initialize()
        {
            _resourceMap = new Dictionary<string, ResourceType>();
            foreach (var resource in AllResources)
            {
                if (resource != null && !_resourceMap.ContainsKey(resource.name))
                {
                    _resourceMap.Add(resource.name, resource);
                }
            }
        }

        public ResourceType GetResourceByName(string resourceName)
        {
            _resourceMap.TryGetValue(resourceName, out var resource);
            return resource;
        }
    }
}