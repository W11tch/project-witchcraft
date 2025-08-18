// Located at: Assets/Scripts/UI/DebugToolsUI.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using ProjectWitchcraft.Managers;
using ProjectWitchcraft.Core;

namespace ProjectWitchcraft.UI
{
    public class DebugToolsUI : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private ResourceManager resourceManager;
        [SerializeField] private ResourceDatabase resourceDatabase;

        [Header("UI & Settings")]
        [SerializeField] private GameObject debugPanel;
        [SerializeField] private int amountToAdd = 100;
        [SerializeField] private Toggle flyModeToggle;

        // Add a new field for the destroy mode toggle
        [SerializeField] private Toggle destroyModeToggle;

        private void Start()
        {
            if (debugPanel != null) { debugPanel.SetActive(false); }

            if (flyModeToggle != null)
            {
                flyModeToggle.isOn = true;
                OnFlyModeToggled(true);
                flyModeToggle.onValueChanged.AddListener(OnFlyModeToggled);
            }

            // Setup for the new destroy mode toggle
            if (destroyModeToggle != null)
            {
                destroyModeToggle.isOn = true; // Default to on, as requested
                OnDestroyModeToggled(true); // Fire the initial event to set the state
                destroyModeToggle.onValueChanged.AddListener(OnDestroyModeToggled);
            }
        }

        private void Update()
        {
            if (Keyboard.current.backquoteKey.wasPressedThisFrame) { ToggleDebugPanel(); }
        }

        public void ToggleDebugPanel()
        {
            if (debugPanel != null) { debugPanel.SetActive(!debugPanel.activeSelf); }
        }

        public void OnFlyModeToggled(bool isFlyModeOn)
        {
            EventManager.TriggerEvent(new ToggleFlyModeEvent { IsFlyModeActive = isFlyModeOn });
        }

        // New method to handle the destroy mode toggle's value change
        public void OnDestroyModeToggled(bool isDestroyModeOn)
        {
            EventManager.TriggerEvent(new ToggleDestroyModeEvent { IsDestroyModeActive = isDestroyModeOn });
        }

        public void GiveAllResources()
        {
            if (resourceManager == null || resourceDatabase == null) return;
            foreach (var resourceType in resourceDatabase.AllResources)
            {
                if (resourceType != null) { resourceManager.AddResource(resourceType, amountToAdd); }
            }
        }
    }
}