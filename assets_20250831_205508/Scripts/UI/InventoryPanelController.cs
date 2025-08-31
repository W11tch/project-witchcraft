// Located at: Assets/Scripts/UI/InventoryPanelController.cs
using UnityEngine;
using ProjectWitchcraft.Core;
using ProjectWitchcraft.Managers;

namespace ProjectWitchcraft.UI
{
    public class InventoryPanelController : MonoBehaviour
    {
        private GameObject _panel;

        private void Awake()
        {
            _panel = this.gameObject;
            _panel.SetActive(false);
            EventManager.AddListener<GameStateChangedEvent>(OnGameStateChanged);
            Debug.Log("[InventoryPanelController] Awake: Subscribed to GameStateChangedEvent.");
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener<GameStateChangedEvent>(OnGameStateChanged);
        }

        private void OnGameStateChanged(GameStateChangedEvent e)
        {
            Debug.Log($"[InventoryPanelController] Received event! New state: {e.NewState}");
            bool shouldBeActive = (e.NewState == GameState.InMenu);
            _panel.SetActive(shouldBeActive);
        }
    }
}