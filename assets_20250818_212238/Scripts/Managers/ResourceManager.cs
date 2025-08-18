// Located at: Assets/Scripts/Managers/ResourceManager.cs
using System.Collections.Generic;
using UnityEngine;
using ProjectWitchcraft.Core;

namespace ProjectWitchcraft.Managers
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        [Header("Data")]
        [SerializeField] private ResourceDatabase resourceDatabase;

        private Dictionary<ResourceType, int> _resourceAmounts = new Dictionary<ResourceType, int>();

        protected override void Awake()
        {
            base.Awake();
            if (resourceDatabase != null)
            {
                resourceDatabase.Initialize();
            }
            else
            {
                Debug.LogError("ResourceDatabase is not assigned in the ResourceManager Inspector!", this.gameObject);
            }
        }

        private void OnEnable()
        {
            EventManager.AddListener<GatherSaveDataEvent>(OnGatherSaveData);
            EventManager.AddListener<ApplySaveDataEvent>(OnApplySaveData);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<GatherSaveDataEvent>(OnGatherSaveData);
            EventManager.RemoveListener<ApplySaveDataEvent>(OnApplySaveData);
        }

        private void OnGatherSaveData(GatherSaveDataEvent e)
        {
            // FIX: Clear the lists and then pack the dictionary data into them for serialization.
            e.SaveData.resourceNames.Clear();
            e.SaveData.resourceAmounts.Clear();

            foreach (var pair in _resourceAmounts)
            {
                if (pair.Key != null)
                {
                    e.SaveData.resourceNames.Add(pair.Key.name);
                    e.SaveData.resourceAmounts.Add(pair.Value);
                }
            }
        }

        private void OnApplySaveData(ApplySaveDataEvent e)
        {
            var newResourceAmounts = new Dictionary<ResourceType, int>();

            // FIX: Ensure the lists are valid and have the same number of entries.
            if (e.SaveData.resourceNames != null && e.SaveData.resourceAmounts != null && e.SaveData.resourceNames.Count == e.SaveData.resourceAmounts.Count)
            {
                // Unpack the lists back into a dictionary.
                for (int i = 0; i < e.SaveData.resourceNames.Count; i++)
                {
                    string resourceName = e.SaveData.resourceNames[i];
                    int resourceAmount = e.SaveData.resourceAmounts[i];

                    ResourceType resourceType = resourceDatabase.GetResourceByName(resourceName);
                    if (resourceType != null)
                    {
                        newResourceAmounts[resourceType] = resourceAmount;
                    }
                }
            }

            _resourceAmounts = newResourceAmounts;
            EventManager.TriggerEvent(new ResourceChangedEvent());
        }

        public void AddResource(ResourceType resourceType, int amount)
        {
            if (resourceType == null) return;

            if (_resourceAmounts.ContainsKey(resourceType))
            {
                _resourceAmounts[resourceType] += amount;
            }
            else
            {
                _resourceAmounts[resourceType] = amount;
            }

            EventManager.TriggerEvent(new ResourceChangedEvent { ResourceType = resourceType, NewAmount = _resourceAmounts[resourceType] });
        }

        public void RemoveResource(ResourceType resourceType, int amount)
        {
            if (HasResource(resourceType, amount))
            {
                _resourceAmounts[resourceType] -= amount;
                EventManager.TriggerEvent(new ResourceChangedEvent { ResourceType = resourceType, NewAmount = _resourceAmounts[resourceType] });
            }
        }

        public bool HasResource(ResourceType resourceType, int amount)
        {
            if (resourceType == null) return false;
            return _resourceAmounts.TryGetValue(resourceType, out int currentAmount) && currentAmount >= amount;
        }

        public int GetResourceAmount(ResourceType resourceType)
        {
            if (resourceType == null) return 0;
            _resourceAmounts.TryGetValue(resourceType, out int amount);
            return amount;
        }

        public IReadOnlyDictionary<ResourceType, int> GetAllResources()
        {
            return _resourceAmounts;
        }
    }
}