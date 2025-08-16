// Located at: Assets/Scripts/UI/ResourceDisplay.cs
using UnityEngine;
using TMPro;
using System.Text;
using ProjectWitchcraft.Managers;
using ProjectWitchcraft.Core;

namespace ProjectWitchcraft.UI
{
    public class ResourceDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI resourceText;

        private void OnEnable()
        {
            // Subscribe to the new event.
            EventManager.AddListener<ResourcesChangedEvent>(OnResourcesChanged);
        }

        private void OnDisable()
        {
            // Unsubscribe from the event.
            EventManager.RemoveListener<ResourcesChangedEvent>(OnResourcesChanged);
        }

        private void Start()
        {
            // Update the display with initial values when the game starts.
            UpdateDisplay();
        }

        // This method is now called by the event, so it doesn't need parameters.
        private void OnResourcesChanged(ResourcesChangedEvent e)
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (resourceText == null) return;

            // Use a StringBuilder for efficient string concatenation.
            StringBuilder sb = new StringBuilder();

            // Get all resources from the ResourceManager.
            var resources = ResourceManager.Instance.GetAllResources();

            foreach (var resource in resources)
            {
                if (resource.Key != null)
                {
                    sb.AppendLine($"{resource.Key.Name}: {resource.Value}");
                }
            }

            resourceText.text = sb.ToString();
        }
    }
}
