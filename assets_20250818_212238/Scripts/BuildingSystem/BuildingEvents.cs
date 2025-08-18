// Located at: Assets/Scripts/BuildingSystem/BuildingEvents.cs
using ProjectWitchcraft.BuildingSystem;

/// <summary>
/// This event is triggered when the player selects a new building from the UI.
/// The data includes the BuildingData ScriptableObject for the selected building.
/// </summary>
public struct BuildingSelectedEvent
{
    public BuildingData BuildingData;
}