// Located at: Assets/Scripts/Core/InteractionEvents.cs
using System.Collections.Generic;

namespace ProjectWitchcraft.Core
{
    /// <summary>
    /// Fired by an interactable object (like a chest) to request its UI be opened.
    /// </summary>
    public struct OpenContainerUIEvent
    {
        public string ContainerName;
        public List<InventorySlot> ContainerInventory;
    }

    /// <summary>
    /// Fired by the PlayerController when an action should close the topmost UI panel.
    /// Listened to by UI controllers like Inventory and Chest UI.
    /// </summary>
    public struct CloseTopUIPanelEvent { }
}