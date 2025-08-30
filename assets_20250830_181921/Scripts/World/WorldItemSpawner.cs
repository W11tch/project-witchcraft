// Located at: Assets/Scripts/World/WorldItemSpawner.cs
using ProjectWitchcraft.Core;
using UnityEngine;

namespace ProjectWitchcraft.World
{
    /// <summary>
    /// Listens for item drop events and spawns the corresponding WorldItem prefab in the game world.
    /// </summary>
    public class WorldItemSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject worldItemPrefab;

        private void OnEnable()
        {
            EventManager.AddListener<ItemDroppedInWorldEvent>(OnItemDropped);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<ItemDroppedInWorldEvent>(OnItemDropped);
        }

        private void OnItemDropped(ItemDroppedInWorldEvent e)
        {
            if (worldItemPrefab == null)
            {
                Debug.LogError("WorldItemPrefab is not set on the WorldItemSpawner!");
                return;
            }

            GameObject droppedObject = Instantiate(worldItemPrefab, e.position, Quaternion.identity);
            WorldItem worldItem = droppedObject.GetComponent<WorldItem>();
            worldItem.Initialize(e.itemData, e.quantity);
        }
    }
}