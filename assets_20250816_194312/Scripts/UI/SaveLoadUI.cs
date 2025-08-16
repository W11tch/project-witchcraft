// Located at: Assets/Scripts/UI/SaveLoadUI.cs
using UnityEngine;
using UnityEngine.UI;
using ProjectWitchcraft.Core;

public class SaveLoadUI : MonoBehaviour
{
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _loadButton;
    [SerializeField] private Button _clearSaveButton;

    private void Start()
    {
        _saveButton.onClick.AddListener(OnSaveButtonClicked);
        _loadButton.onClick.AddListener(OnLoadButtonClicked);
        _clearSaveButton.onClick.AddListener(OnClearSaveButtonClicked);
    }

    private void OnDestroy()
    {
        _saveButton.onClick.RemoveListener(OnSaveButtonClicked);
        _loadButton.onClick.RemoveListener(OnLoadButtonClicked);
        _clearSaveButton.onClick.RemoveListener(OnClearSaveButtonClicked);
    }

    private void OnSaveButtonClicked()
    {
        EventManager.TriggerEvent(new GameSavedEvent());
    }

    private void OnLoadButtonClicked()
    {
        EventManager.TriggerEvent(new GameLoadedEvent());
    }

    private void OnClearSaveButtonClicked()
    {
        EventManager.TriggerEvent(new SaveDataClearedEvent());
    }
}