// Located at: Assets/Scripts/Core/ItemEvents.cs
using ProjectWitchcraft.Core;

/// <summary>
/// Fired whenever the player's inventory changes (items added, removed, or moved).
/// If itemData is null, it signifies a major change requiring a full UI refresh.
/// </summary>
public struct InventoryChangedEvent
{
    public ItemData itemData;
    public int NewAmount;
}