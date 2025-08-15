using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using ProjectWitchcraft.BuildingSystem;
using ProjectWitchcraft.Core;
using UnityEngine.SceneManagement;

namespace ProjectWitchcraft.Managers
{
    public class SaveManager : MonoBehaviour
    {
        private string _saveFilePath;
        private static bool _shouldLoad = false;

        #region Singleton
        public static SaveManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _saveFilePath = Path.Combine(Application.persistentDataPath, "savedata.json");
        }
        #endregion

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void SaveGame()
        {
            Debug.Log("Saving game...");
            SaveData data = new SaveData();

            var resourceManager = ResourceManager.Instance;
            var placedObjectsParent = GameObject.Find("Placed Objects")?.transform;

            if (resourceManager == null || placedObjectsParent == null) return;

            var resources = resourceManager.GetAllResources();
            foreach (var resourcePair in resources)
            {
                data.resourceAmounts.Add(resourcePair.Key.name, resourcePair.Value);
            }

            foreach (Transform child in placedObjectsParent)
            {
                if (child.TryGetComponent<PlaceableObject>(out var placedObject) && placedObject.Placed)
                {
                    data.placedObjects.Add(new ObjectData
                    {
                        buildingDataName = placedObject.BuildingData.name,
                        position = placedObject.transform.position,
                        rotation = placedObject.transform.rotation
                    });
                }
            }

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_saveFilePath, json);
            Debug.Log($"Game saved to: {_saveFilePath}");
        }

        public void LoadGame()
        {
            if (File.Exists(_saveFilePath))
            {
                _shouldLoad = true;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                Debug.LogWarning("No save file found.");
            }
        }

        public void ClearSave()
        {
            if (File.Exists(_saveFilePath))
            {
                File.Delete(_saveFilePath);
                Debug.Log("Save file cleared.");
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (_shouldLoad)
            {
                _shouldLoad = false;
                StartCoroutine(PerformLoad());
            }
        }

        // **THE CONTROLLED LOADING ORDER FIX**
        private IEnumerator PerformLoad()
        {
            // Wait one full frame to ensure all Awake() and Start() methods in the new scene have run.
            yield return null;

            var resourceManager = ResourceManager.Instance;
            var worldGridManager = FindFirstObjectByType<WorldGridManager>();
            var placedObjectsParent = GameObject.Find("Placed Objects")?.transform;

            if (resourceManager == null || worldGridManager == null || placedObjectsParent == null) yield break;

            string json = File.ReadAllText(_saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // First, clear the state of the managers.
            resourceManager.ClearResources();

            // Load Resources from the save file.
            foreach (var resourcePair in data.resourceAmounts)
            {
                string resourcePath = $"Core/Resources/{resourcePair.Key}";
                ResourceType resourceType = Resources.Load<ResourceType>(resourcePath);
                if (resourceType != null)
                {
                    resourceManager.AddResource(resourceType, resourcePair.Value);
                }
            }

            // Load Placed Objects from the save file.
            foreach (var objectData in data.placedObjects)
            {
                string buildingDataPath = $"BuildingSystem/BuildingData/{objectData.buildingDataName}";
                BuildingData buildingData = Resources.Load<BuildingData>(buildingDataPath);
                if (buildingData != null)
                {
                    GameObject obj = Instantiate(buildingData.prefab, objectData.position, objectData.rotation, placedObjectsParent);
                    PlaceableObject placeable = obj.GetComponent<PlaceableObject>();
                    placeable.Placed = true;
                    worldGridManager.PlaceObject(placeable, buildingData.placementType);
                }
            }
            Debug.Log("Game loaded.");
        }
    }
}