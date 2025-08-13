using System;

public static class BuildingEvents
{
    public static event Action<BuildingData> OnObjectSelected;
    public static event Action OnObjectPlaced;
    public static event Action OnObjectRotated;
    public static event Action OnObjectDestroyed;
    public static event Action OnPlacementCanceled; // NEW: Event for canceling placement

    public static void TriggerObjectSelected(BuildingData data)
    {
        OnObjectSelected?.Invoke(data);
    }

    public static void TriggerObjectPlaced()
    {
        OnObjectPlaced?.Invoke();
    }

    public static void TriggerObjectRotated()
    {
        OnObjectRotated?.Invoke();
    }

    public static void TriggerObjectDestroyed()
    {
        OnObjectDestroyed?.Invoke();
    }

    public static void TriggerPlacementCanceled()
    {
        OnPlacementCanceled?.Invoke();
    }
}