// Located at: Assets/Scripts/Core/GameEvents.cs
namespace ProjectWitchcraft.Core
{
    // The GameState enum now lives here, in the Core assembly.
    public enum GameState { PreGame, Playing, Paused, InMenu }

    public struct GameStateChangedEvent
    {
        public GameState NewState;
    }
}