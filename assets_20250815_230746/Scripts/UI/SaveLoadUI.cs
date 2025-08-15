using UnityEngine;
using ProjectWitchcraft.Managers;

namespace ProjectWitchcraft.UI
{
    // This script now correctly inherits from MonoBehaviour
    public class SaveLoadUI : MonoBehaviour
    {
        [Header("Dependencies")]
        [Tooltip("Drag the SaveManager GameObject here.")]
        [SerializeField] private SaveManager saveManager;

        public void OnSaveButtonClicked()
        {
            saveManager?.SaveGame();
        }

        public void OnLoadButtonClicked()
        {
            saveManager?.LoadGame();
        }

        public void OnClearSaveButtonClicked()
        {
            saveManager?.ClearSave();
        }
    }
}