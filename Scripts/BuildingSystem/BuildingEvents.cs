using System;
using ProjectWitchcraft.BuildingSystem;

// This using statement is needed because BuildingData is now in a namespace.
// However, to avoid circular dependencies, we will define the namespace for this script first,
// and then add the using statement for BuildingData after we have updated that script.
// For now, let's start fresh.

namespace ProjectWitchcraft.BuildingSystem
{
    public static class BuildingEvents
    {
        public static event Action<BuildingData> OnObjectSelected;
        public static event Action OnObjectPlaced;
        public static event Action OnObjectRotated;
        public static event Action OnObjectDestroyed;
        public static event Action OnPlacementCanceled;

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
}