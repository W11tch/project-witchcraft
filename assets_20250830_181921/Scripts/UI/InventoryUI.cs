// Located at: Assets/Scripts/UI/InventoryUI.cs
using UnityEngine;
using ProjectWitchcraft.Core;
using ProjectWitchcraft.Managers;

namespace ProjectWitchcraft.UI
{
    public class InventoryUI : MonoBehaviour
    {
        private GameObject _panel;

        private void Awake()
        {
            _panel = this.gameObject;
            _panel.SetActive(false); // Hide on start
            EventManager.AddListener<GameStateChangedEvent>(OnGameStateChanged);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener<GameStateChangedEvent>(OnGameStateChanged);
        }

        private void OnGameStateChanged(GameStateChangedEvent e)
        {
            // Show the panel ONLY when the game state is InMenu
            bool shouldBeActive = (e.NewState == GameState.InMenu);
            _panel.SetActive(shouldBeActive);
        }
    }
}