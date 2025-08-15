using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectWitchcraft.Core
{
    /// <summary>
    /// A generic, project-wide event manager that allows different systems
    /// to communicate without direct dependencies.
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        private Dictionary<string, Action> _eventDictionary;

        #region Singleton
        public static EventManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                Initialize();
            }
        }
        #endregion

        private void Initialize()
        {
            if (_eventDictionary == null)
            {
                _eventDictionary = new Dictionary<string, Action>();
            }
        }

        /// <summary>
        /// Start listening for a specific event.
        /// </summary>
        /// <param name="eventName">The name of the event to listen for.</param>
        /// <param name="listener">The method to be called when the event is triggered.</param>
        public static void StartListening(string eventName, Action listener)
        {
            if (Instance == null) return;

            if (Instance._eventDictionary.TryGetValue(eventName, out Action thisEvent))
            {
                // Add the new listener to the existing event.
                thisEvent += listener;
                Instance._eventDictionary[eventName] = thisEvent;
            }
            else
            {
                // Create a new event and add the listener.
                thisEvent += listener;
                Instance._eventDictionary.Add(eventName, thisEvent);
            }
        }

        /// <summary>
        /// Stop listening for a specific event.
        /// </summary>
        /// <param name="eventName">The name of the event to stop listening for.</param>
        /// <param name="listener">The method that was previously listening.</param>
        public static void StopListening(string eventName, Action listener)
        {
            if (Instance == null) return;

            if (Instance._eventDictionary.TryGetValue(eventName, out Action thisEvent))
            {
                thisEvent -= listener;
                Instance._eventDictionary[eventName] = thisEvent;
            }
        }

        /// <summary>
        /// Trigger a specific event, calling all registered listeners.
        /// </summary>
        /// <param name="eventName">The name of the event to trigger.</param>
        public static void TriggerEvent(string eventName)
        {
            if (Instance == null) return;

            if (Instance._eventDictionary.TryGetValue(eventName, out Action thisEvent))
            {
                thisEvent?.Invoke();
            }
        }
    }
}