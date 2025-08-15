using UnityEngine;
using TMPro;
using System.Text;
using ProjectWitchcraft.Core;
using ProjectWitchcraft.Managers;

namespace ProjectWitchcraft.UI
{
    public class ResourceDisplay : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI resourceText;

        // We no longer need a direct dependency reference here.

        private void OnEnable()
        {
            // Subscribe to the event when the UI is enabled.
            ResourceManager.OnResourcesChanged += UpdateDisplay;
            // Also, immediately try to update the display in case we missed the initial event.
            UpdateDisplay();
        }

        private void OnDisable()
        {
            // Unsubscribe when the UI is disabled.
            ResourceManager.OnResourcesChanged -= UpdateDisplay;
        }

        // We no longer need a Start() method, as OnEnable handles the initial update.

        private void UpdateDisplay()
        {
            // **THE DEFINITIVE FIX**:
            // We now check if the ResourceManager singleton exists before trying to access it.
            // If it doesn't exist yet (during the first frame of a scene load), we simply do nothing
            // and wait for the OnResourcesChanged event to be fired later.
            if (resourceText == null || ResourceManager.Instance == null)
            {
                // You can set the text to a "Loading..." state here if you want.
                // resourceText.text = "INVENTORY:\nLoading...";
                return;
            }

            StringBuilder sb = new StringBuilder("INVENTORY:\n");

            var allResources = ResourceManager.Instance.GetAllResources();

            if (allResources.Count == 0)
            {
                sb.Append("Empty");
            }
            else
            {
                foreach (var resource in allResources)
                {
                    sb.AppendLine($"- {resource.Key.DisplayName}: {resource.Value}");
                }
            }

            resourceText.text = sb.ToString();
        }
    }
}