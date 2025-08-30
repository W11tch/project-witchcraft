// Located at: Assets/Scripts/World/WorldItem.cs
using ProjectWitchcraft.Core;
using ProjectWitchcraft.Managers;
using UnityEngine;

namespace ProjectWitchcraft.World
{
    [RequireComponent(typeof(Rigidbody), typeof(SpriteRenderer))]
    public class WorldItem : MonoBehaviour
    {
        // **NEW:** A specific reference to the collider used for pickup detection.
        [Tooltip("Assign the Sphere Collider component from this object here.")]
        [SerializeField] private Collider _pickupTrigger;

        private ItemData _itemData;
        private int _quantity;

        private SpriteRenderer _spriteRenderer;
        private const float PICKUP_DELAY = 0.5f;
        private float _spawnTime;

        private void Awake()
        {
            // **FIX:** Ensure the SPECIFIC pickup collider is a trigger.
            if (_pickupTrigger == null)
            {
                Debug.LogError("Pickup Trigger collider is not assigned on the WorldItem prefab!", this.gameObject);
                return;
            }
            _pickupTrigger.isTrigger = true;

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spawnTime = Time.time;
        }

        public void Initialize(ItemData data, int amount)
        {
            _itemData = data;
            _quantity = amount;
            _spriteRenderer.sprite = data.Icon;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Time.time < _spawnTime + PICKUP_DELAY || !other.CompareTag("Player"))
            {
                return;
            }

            int amountRemaining = InventoryManager.Instance.AddItem(_itemData, _quantity);

            if (amountRemaining == 0)
            {
                Destroy(gameObject);
            }
            else
            {
                _quantity = amountRemaining;
            }
        }
    }
}