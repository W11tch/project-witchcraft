using System;
using System.Collections.Generic;
using UnityEngine;
using ProjectWitchcraft.Core;

namespace ProjectWitchcraft.Managers
{
    public class ResourceManager : MonoBehaviour
    {
        public static event Action OnResourcesChanged;

        [Header("Starting Resources (For Debugging)")]
        [SerializeField] private ResourceType debugResource1;
        [SerializeField] private int debugAmount1;
        [SerializeField] private ResourceType debugResource2;
        [SerializeField] private int debugAmount2;
        [SerializeField] private ResourceType debugResource3;
        [SerializeField] private int debugAmount3;

        private Dictionary<ResourceType, int> _resourceAmounts;
        private static bool _firstLoadCompleted = false;

        #region Singleton
        public static ResourceManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // **THE ROBUST SINGLETON FIX**: The inventory dictionary is now only created ONCE
            // when this singleton is first created. It will no longer be wiped out on scene load.
            if (_resourceAmounts == null)
            {
                _resourceAmounts = new Dictionary<ResourceType, int>();
            }
        }
        #endregion

        private void Start()
        {
            // Only add starting resources on the very first time the game is launched.
            if (!_firstLoadCompleted)
            {
                _firstLoadCompleted = true;
                if (debugResource1 != null) AddResource(debugResource1, debugAmount1);
                if (debugResource2 != null) AddResource(debugResource2, debugAmount2);
                if (debugResource3 != null) AddResource(debugResource3, debugAmount3);
            }
        }

        public void ClearResources()
        {
            _resourceAmounts.Clear();
            OnResourcesChanged?.Invoke();
        }

        public void AddResource(ResourceType resourceType, int amount)
        {
            if (amount <= 0 || resourceType == null) return;
            if (!_resourceAmounts.ContainsKey(resourceType)) _resourceAmounts[resourceType] = 0;
            _resourceAmounts[resourceType] += amount;
            OnResourcesChanged?.Invoke();
        }

        public bool RemoveResource(ResourceType resourceType, int amount)
        {
            if (amount <= 0 || resourceType == null) return true;
            if (HasResource(resourceType, amount))
            {
                _resourceAmounts[resourceType] -= amount;
                OnResourcesChanged?.Invoke();
                return true;
            }
            return false;
        }

        public bool HasResource(ResourceType resourceType, int amount)
        {
            if (resourceType == null || _resourceAmounts == null) return false;
            return _resourceAmounts.ContainsKey(resourceType) && _resourceAmounts[resourceType] >= amount;
        }

        public int GetResourceAmount(ResourceType resourceType)
        {
            if (resourceType != null && _resourceAmounts != null && _resourceAmounts.ContainsKey(resourceType))
            {
                return _resourceAmounts[resourceType];
            }
            return 0;
        }

        public Dictionary<ResourceType, int> GetAllResources()
        {
            return _resourceAmounts;
        }
    }
}