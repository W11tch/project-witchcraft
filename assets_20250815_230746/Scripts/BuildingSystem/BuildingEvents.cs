using System;
using ProjectWitchcraft.BuildingSystem;

namespace ProjectWitchcraft.BuildingSystem
{
    public static class BuildingEvents
    {
        // This is the only event this class is responsible for.
        public static event Action<BuildingData> OnObjectSelected;

        public static void TriggerObjectSelected(BuildingData data)
        {
            OnObjectSelected?.Invoke(data);
        }
    }
}