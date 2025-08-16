// Located at: Assets/Scripts/Managers/ResourceManager.cs
using System.Collections.Generic;
using UnityEngine;
using ProjectWitchcraft.Core;

namespace ProjectWitchcraft.Managers
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        [Header("Debug")]
        [SerializeField] private ResourceType debugResource1;
        [SerializeField] private ResourceType debugResource2;
        [SerializeField] private ResourceType debugResource3;

        private Dictionary<ResourceType, int> _resourceAmounts = new Dictionary<ResourceType, int>();

        private void Start()
        {
            AddResource(debugResource1, 10);
            AddResource(debugResource2, 10);
            AddResource(debugResource3, 10);
        }

        public void AddResource(ResourceType resourceType, int amount)
        {
            if (_resourceAmounts.ContainsKey(resourceType))
            {
                _resourceAmounts[resourceType] += amount;
            }
            else
            {
                _resourceAmounts[resourceType] = amount;
            }
            // Trigger the event to notify listeners (like the UI) that resources have changed.
            EventManager.TriggerEvent(new ResourcesChangedEvent());
        }

        public void RemoveResource(ResourceType resourceType, int amount)
        {
            if (HasResource(resourceType, amount))
            {
                _resourceAmounts[resourceType] -= amount;
                // Trigger the event to notify listeners.
                EventManager.TriggerEvent(new ResourcesChangedEvent());
            }
        }

        public bool HasResource(ResourceType resourceType, int amount)
        {
            return _resourceAmounts.TryGetValue(resourceType, out int currentAmount) && currentAmount >= amount;
        }

        public int GetResourceAmount(ResourceType resourceType)
        {
            _resourceAmounts.TryGetValue(resourceType, out int amount);
            return amount;
        }

        // Add this method so the UI can get all the current resource data.
        public IReadOnlyDictionary<ResourceType, int> GetAllResources()
        {
            return _resourceAmounts;
        }
    }
}
