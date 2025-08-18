// Located at: Assets/Scripts/UI/ResourceDisplay.cs
using ProjectWitchcraft.Core;
using ProjectWitchcraft.Managers;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProjectWitchcraft.UI
{
    public class ResourceDisplay : MonoBehaviour
    {
        [SerializeField] private ResourceManager resourceManager;
        [SerializeField] private TextMeshProUGUI resourceText;

        private Dictionary<ResourceType, int> _currentResourceAmounts = new Dictionary<ResourceType, int>();

        private void OnEnable()
        {
            // FIX: Subscribe to the correct singular event name.
            EventManager.AddListener<ResourceChangedEvent>(OnResourceChanged);
            InitializeDisplay();
        }

        private void OnDisable()
        {
            // FIX: Unsubscribe from the correct singular event name.
            EventManager.RemoveListener<ResourceChangedEvent>(OnResourceChanged);
        }

        private void InitializeDisplay()
        {
            if (resourceManager == null) return;
            var initialResources = resourceManager.GetAllResources();
            _currentResourceAmounts.Clear();
            foreach (var pair in initialResources)
            {
                _currentResourceAmounts[pair.Key] = pair.Value;
            }
            UpdateDisplayText();
        }

        private void OnResourceChanged(ResourceChangedEvent e)
        {
            // If the event doesn't specify a resource type, it's a signal to refresh everything.
            if (e.ResourceType == null)
            {
                InitializeDisplay();
                return;
            }

            // Otherwise, update the specific resource that changed.
            _currentResourceAmounts[e.ResourceType] = e.NewAmount;
            UpdateDisplayText();
        }

        private void UpdateDisplayText()
        {
            string text = "";
            foreach (var resource in _currentResourceAmounts)
            {
                if (resource.Key != null) // Safety check
                {
                    text += $"{resource.Key.Name}: {resource.Value}\n";
                }
            }
            resourceText.text = text;
        }
    }
}