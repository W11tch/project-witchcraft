// Located at: Assets/Scripts/Managers/InventoryManager.cs
using System.Collections.Generic;
using UnityEngine;
using ProjectWitchcraft.Core;
using System.Linq;

namespace ProjectWitchcraft.Managers
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        // ... (Header variables, properties, Awake, OnEnable/Disable, and Add/Remove/Has/GetItemAmount methods are unchanged) ...
        [Header("Data")]
        [SerializeField] private ItemDatabase itemDatabase;
        [Header("Settings")]
        [Tooltip("The number of slots in the player's hotbar.")]
        [SerializeField] private int hotbarSize = 10;
        [Tooltip("The default number of slots in the main inventory.")]
        [SerializeField] private int inventorySize = 30;
        private List<InventorySlot> _hotbarSlots;
        private List<InventorySlot> _inventorySlots;
        private InventorySlot _heldSlot = new InventorySlot();
        public IReadOnlyList<InventorySlot> HotbarSlots => _hotbarSlots;
        public IReadOnlyList<InventorySlot> InventorySlots => _inventorySlots;
        public InventorySlot HeldSlot => _heldSlot;
        protected override void Awake()
        {
            base.Awake();
            if (itemDatabase != null) itemDatabase.Initialize();
            else Debug.LogError("ItemDatabase is not assigned in the InventoryManager Inspector!", this.gameObject);
            _hotbarSlots = new List<InventorySlot>(hotbarSize);
            _inventorySlots = new List<InventorySlot>(inventorySize);
            for (int i = 0; i < hotbarSize; i++) _hotbarSlots.Add(new InventorySlot());
            for (int i = 0; i < inventorySize; i++) _inventorySlots.Add(new InventorySlot());
        }
        private void OnEnable()
        {
            EventManager.AddListener<GatherSaveDataEvent>(OnGatherSaveData);
            EventManager.AddListener<ApplySaveDataEvent>(OnApplySaveData);
        }
        private void OnDisable()
        {
            EventManager.RemoveListener<GatherSaveDataEvent>(OnGatherSaveData);
            EventManager.RemoveListener<ApplySaveDataEvent>(OnApplySaveData);
        }
        public int AddItem(ItemData itemData, int amount)
        {
            if (itemData == null || amount <= 0) return amount;
            int amountRemaining = amount;
            foreach (var slot in _hotbarSlots.Concat(_inventorySlots).Where(s => !s.IsEmpty && s.item == itemData))
            {
                amountRemaining = AddToStack(slot, amountRemaining);
                if (amountRemaining == 0) break;
            }
            if (amountRemaining > 0)
            {
                foreach (var slot in _hotbarSlots.Concat(_inventorySlots).Where(s => s.IsEmpty))
                {
                    amountRemaining = CreateNewStack(slot, itemData, amountRemaining);
                    if (amountRemaining == 0) break;
                }
            }
            if (amountRemaining < amount)
            {
                EventManager.TriggerEvent(new InventoryChangedEvent());
            }
            return amountRemaining;
        }
        private int AddToStack(InventorySlot slot, int amount)
        {
            int spaceAvailable = slot.item.maxStackSize - slot.quantity;
            int amountToAdd = Mathf.Min(amount, spaceAvailable);
            slot.AddQuantity(amountToAdd);
            return amount - amountToAdd;
        }
        private int CreateNewStack(InventorySlot slot, ItemData item, int amount)
        {
            int amountToAdd = Mathf.Min(amount, item.maxStackSize);
            slot.item = item;
            slot.quantity = amountToAdd;
            return amount - amountToAdd;
        }
        public void RemoveItem(ItemData itemData, int amount)
        {
            if (!HasItem(itemData, amount)) return;
            int amountToRemove = amount;
            foreach (var slot in _inventorySlots.Concat(_hotbarSlots).Where(s => !s.IsEmpty && s.item == itemData))
            {
                int amountToRemoveFromSlot = Mathf.Min(amountToRemove, slot.quantity);
                slot.quantity -= amountToRemoveFromSlot;
                amountToRemove -= amountToRemoveFromSlot;
                if (slot.quantity <= 0) slot.Clear();
                if (amountToRemove == 0) break;
            }
            EventManager.TriggerEvent(new InventoryChangedEvent());
        }
        public bool HasItem(ItemData itemData, int amount)
        {
            return GetItemAmount(itemData) >= amount;
        }
        public int GetItemAmount(ItemData itemData)
        {
            if (itemData == null) return 0;
            return _hotbarSlots.Concat(_inventorySlots)
                .Where(s => !s.IsEmpty && s.item == itemData)
                .Sum(s => s.quantity);
        }

        public void PickupSlotContents(int fromIndex, bool fromIsHotbar)
        {
            if (!_heldSlot.IsEmpty) return;
            var fromList = fromIsHotbar ? _hotbarSlots : _inventorySlots;
            InventorySlot fromSlot = fromList[fromIndex];
            if (fromSlot.IsEmpty) return;
            _heldSlot = new InventorySlot(fromSlot.item, fromSlot.quantity);
            fromSlot.Clear();
            EventManager.TriggerEvent(new InventoryChangedEvent());
        }

        public void DropHeldItemOnSlot(int toIndex, bool toIsHotbar)
        {
            if (_heldSlot.IsEmpty) return;
            var toList = toIsHotbar ? _hotbarSlots : _inventorySlots;
            InventorySlot toSlot = toList[toIndex];
            if (toSlot.IsEmpty)
            {
                toList[toIndex] = new InventorySlot(_heldSlot.item, _heldSlot.quantity);
                _heldSlot.Clear();
            }
            else if (toSlot.item == _heldSlot.item)
            {
                int spaceInToStack = toSlot.item.maxStackSize - toSlot.quantity;
                int amountToMove = Mathf.Min(_heldSlot.quantity, spaceInToStack);
                toSlot.AddQuantity(amountToMove);
                _heldSlot.quantity -= amountToMove;
                if (_heldSlot.quantity <= 0) _heldSlot.Clear();
            }
            else
            {
                InventorySlot temp = new InventorySlot(toSlot.item, toSlot.quantity);
                toList[toIndex] = new InventorySlot(_heldSlot.item, _heldSlot.quantity);
                _heldSlot = temp;
            }
            EventManager.TriggerEvent(new InventoryChangedEvent());
        }

        public void SplitStack(int fromIndex, bool fromIsHotbar)
        {
            if (!_heldSlot.IsEmpty) return;
            var fromList = fromIsHotbar ? _hotbarSlots : _inventorySlots;
            InventorySlot fromSlot = fromList[fromIndex];
            if (fromSlot.IsEmpty || fromSlot.quantity < 2) return;
            int halfAmount = Mathf.CeilToInt(fromSlot.quantity / 2f);
            fromSlot.quantity -= halfAmount;
            _heldSlot = new InventorySlot(fromSlot.item, halfAmount);
            EventManager.TriggerEvent(new InventoryChangedEvent());
        }

        /// <summary>
        /// Places the currently held item into a target slot. Handles merging and swapping.
        /// </summary>
        public void PlaceHeldItem(int toIndex, bool toIsHotbar)
        {
            DropHeldItemOnSlot(toIndex, toIsHotbar);
        }

        /// <summary>
        /// Places a single item from the held stack into a target slot.
        /// </summary>
        public void PlaceOneFromHeldStack(int toIndex, bool toIsHotbar)
        {
            if (_heldSlot.IsEmpty) return;

            var toList = toIsHotbar ? _hotbarSlots : _inventorySlots;
            InventorySlot toSlot = toList[toIndex];

            // If the target slot is empty, create a new stack of 1
            if (toSlot.IsEmpty)
            {
                toList[toIndex] = new InventorySlot(_heldSlot.item, 1);
                _heldSlot.quantity--;
            }
            // If it's the same item and there's space, add 1 to the stack
            else if (toSlot.item == _heldSlot.item && toSlot.quantity < toSlot.item.maxStackSize)
            {
                toSlot.AddQuantity(1);
                _heldSlot.quantity--;
            }

            if (_heldSlot.quantity <= 0) _heldSlot.Clear();

            EventManager.TriggerEvent(new InventoryChangedEvent());
        }

        // ... (Save & Load logic remains the same) ...
        private void OnGatherSaveData(GatherSaveDataEvent e)
        {
            e.SaveData.itemNames.Clear();
            e.SaveData.itemAmounts.Clear();

            foreach (var slot in _hotbarSlots.Concat(_inventorySlots))
            {
                e.SaveData.itemNames.Add(slot.IsEmpty ? "" : slot.item.name);
                e.SaveData.itemAmounts.Add(slot.quantity);
            }
        }
        private void OnApplySaveData(ApplySaveDataEvent e)
        {
            int totalSlots = hotbarSize + inventorySize;
            if (e.SaveData.itemNames == null || e.SaveData.itemNames.Count != totalSlots)
            {
                Debug.LogWarning("Save data mismatch. Inventory could not be loaded.");
                return;
            }

            for (int i = 0; i < hotbarSize; i++)
            {
                LoadSlotData(_hotbarSlots[i], e.SaveData.itemNames[i], e.SaveData.itemAmounts[i]);
            }
            for (int i = 0; i < inventorySize; i++)
            {
                LoadSlotData(_inventorySlots[i], e.SaveData.itemNames[hotbarSize + i], e.SaveData.itemAmounts[hotbarSize + i]);
            }

            EventManager.TriggerEvent(new InventoryChangedEvent());
        }
        private void LoadSlotData(InventorySlot slot, string itemName, int amount)
        {
            if (string.IsNullOrEmpty(itemName) || amount <= 0)
            {
                slot.Clear();
            }
            else
            {
                slot.item = itemDatabase.GetItemByName(itemName);
                slot.quantity = amount;
                if (slot.item == null) slot.Clear();
            }
        }
    }
}