// Located at: Assets/Scripts/Core/ResourceEvents.cs
using ProjectWitchcraft.Core;

// FIX: The event struct was named 'ResourcesChangedEvent' (plural) and was empty.
// We are renaming it to 'ResourceChangedEvent' (singular) and adding data fields
// to match what the ResourceManager and ResourceDisplay scripts expect.
public struct ResourceChangedEvent
{
    public ResourceType ResourceType;
    public int NewAmount;
}