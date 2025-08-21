// Located at: Assets/Scripts/UI/InventoryDisplay.cs
using ProjectWitchcraft.Core;
using ProjectWitchcraft.Managers;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProjectWitchcraft.UI
{
    // This is the final, corrected script that combines the working event listener
    // with your original resource display logic.
    public class InventoryDisplay : MonoBehaviour
    {
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private TextMeshProUGUI inventoryText;

        private Dictionary<ResourceType, int> _currentResourceAmounts = new Dictionary<ResourceType, int>();

        private void OnEnable()
        {
            // Subscribe to both necessary events
            EventManager.AddListener<ResourceChangedEvent>(OnResourceChanged);
            InitializeDisplay();
        }

        private void OnDisable()
        {
            // Unsubscribe from both events
            EventManager.RemoveListener<ResourceChangedEvent>(OnResourceChanged);
        }

        // --- YOUR ORIGINAL RESOURCE DISPLAY LOGIC IS PRESERVED BELOW ---

        private void InitializeDisplay()
        {
            if (inventoryManager == null) return;
            var initialResources = inventoryManager.GetAllResources();
            _currentResourceAmounts.Clear();
            foreach (var pair in initialResources)
            {
                _currentResourceAmounts[pair.Key] = pair.Value;
            }
            UpdateDisplayText();
        }

        private void OnResourceChanged(ResourceChangedEvent e)
        {
            if (e.ResourceType == null)
            {
                InitializeDisplay();
                return;
            }
            _currentResourceAmounts[e.ResourceType] = e.NewAmount;
            UpdateDisplayText();
        }

        private void UpdateDisplayText()
        {
            string text = "";
            foreach (var resource in _currentResourceAmounts)
            {
                if (resource.Key != null)
                {
                    text += $"{resource.Key.Name}: {resource.Value}\n";
                }
            }
            inventoryText.text = text;
        }
    }
}