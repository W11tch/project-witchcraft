// Located at: Assets/Scripts/Managers/GameManager.cs
using ProjectWitchcraft.Core;
using UnityEngine;

namespace ProjectWitchcraft.Managers
{

    public class GameManager : Singleton<GameManager>
    {
        public GameState CurrentState { get; private set; }

        private void Start()
        {
            UpdateState(GameState.Playing);
        }

        public void UpdateState(GameState newState)
        {
            if (CurrentState == newState) return;
            CurrentState = newState;

            switch (newState)
            {
                case GameState.Playing:
                    Time.timeScale = 1f;
                    // We keep the cursor visible for your top-down gameplay
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                case GameState.Paused:
                    Time.timeScale = 0f;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                case GameState.InMenu:
                    Time.timeScale = 1f;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
            }
            EventManager.TriggerEvent(new GameStateChangedEvent { NewState = newState });
        }
        public void PauseGame() => UpdateState(GameState.Paused);
        public void ResumeGame() => UpdateState(GameState.Playing);
        public void EnterMenuMode() => UpdateState(GameState.InMenu);
        public void ExitMenuMode() => UpdateState(GameState.Playing);
    }
}