// Located at: Assets/Scripts/UI/UI_InventorySlot.cs
using ProjectWitchcraft.Core;
using ProjectWitchcraft.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectWitchcraft.UI
{
    public class UI_InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
    {
        [Header("UI Elements")]
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TextMeshProUGUI _quantityText;

        private InventoryManager _inventoryManager;
        private int _slotIndex;
        private bool _isHotbarSlot;

        public void Initialize(InventoryManager manager, int index, bool isHotbar)
        {
            _inventoryManager = manager;
            _slotIndex = index;
            _isHotbarSlot = isHotbar;
        }

        public void UpdateSlot(InventorySlot slot)
        {
            if (slot.IsEmpty)
            {
                _itemIcon.enabled = false;
                _quantityText.enabled = false;
            }
            else
            {
                _itemIcon.sprite = slot.item.Icon;
                _itemIcon.enabled = true;
                _quantityText.text = slot.quantity > 1 ? slot.quantity.ToString() : "";
                _quantityText.enabled = slot.quantity > 1;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.dragging) return;

            // **FIX:** Logic is now complete for all click interactions.
            if (!_inventoryManager.HeldSlot.IsEmpty)
            {
                // If we are holding an item, clicks are for placing.
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    // Left click places the whole stack.
                    _inventoryManager.PlaceHeldItem(_slotIndex, _isHotbarSlot);
                }
                else if (eventData.button == PointerEventData.InputButton.Right)
                {
                    // Right click places one item.
                    _inventoryManager.PlaceOneFromHeldStack(_slotIndex, _isHotbarSlot);
                }
            }
            else
            {
                // If we are not holding an item, right click is for splitting.
                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    _inventoryManager.SplitStack(_slotIndex, _isHotbarSlot);
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_inventoryManager.HeldSlot.IsEmpty)
            {
                _inventoryManager.PickupSlotContents(_slotIndex, _isHotbarSlot);
            }
        }

        public void OnDrag(PointerEventData eventData) { }

        public void OnEndDrag(PointerEventData eventData) { }

        public void OnDrop(PointerEventData eventData)
        {
            _inventoryManager.DropHeldItemOnSlot(_slotIndex, _isHotbarSlot);
        }
    }
}