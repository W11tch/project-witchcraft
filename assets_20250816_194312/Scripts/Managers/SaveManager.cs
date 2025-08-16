// Located at: Assets/Scripts/Managers/SaveManager.cs
using UnityEngine;
using ProjectWitchcraft.Core; // Added using statement

namespace ProjectWitchcraft.Managers
{
    // Inherit from Singleton<SaveManager>
    public class SaveManager : Singleton<SaveManager>
    {
        private void OnEnable()
        {
            EventManager.AddListener<GameSavedEvent>(OnGameSaved);
            EventManager.AddListener<GameLoadedEvent>(OnGameLoaded);
            EventManager.AddListener<SaveDataClearedEvent>(OnSaveDataCleared);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<GameSavedEvent>(OnGameSaved);
            EventManager.RemoveListener<GameLoadedEvent>(OnGameLoaded);
            EventManager.RemoveListener<SaveDataClearedEvent>(OnSaveDataCleared);
        }

        private void OnGameSaved(GameSavedEvent e)
        {
            Debug.Log("Game Saved!");
            // Add your save logic here
        }

        private void OnGameLoaded(GameLoadedEvent e)
        {
            Debug.Log("Game Loaded!");
            // Add your load logic here
        }

        private void OnSaveDataCleared(SaveDataClearedEvent e)
        {
            Debug.Log("Save Data Cleared!");
            // Add your clear save data logic here
        }
    }
}
