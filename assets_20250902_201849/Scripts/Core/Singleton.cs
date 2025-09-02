// Located at: Assets/Scripts/Core/Singleton.cs
using UnityEngine;

/// <summary>
/// A generic base class for creating singletons.
/// Ensures that only one instance of the class exists.
/// </summary>
/// <typeparam name="T">The type of the singleton class.</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T s_instance;
    private static readonly object s_lock = new object();

    /// <summary>
    /// Gets the singleton instance. If it doesn't exist, it will be created.
    /// </summary>
    public static T Instance
    {
        get
        {
            lock (s_lock)
            {
                if (s_instance == null)
                {
                    // Search for an existing instance in the scene using the new API.
                    s_instance = FindAnyObjectByType<T>();

                    // If no instance exists, create a new one.
                    if (s_instance == null)
                    {
                        var singletonObject = new GameObject(typeof(T).Name);
                        s_instance = singletonObject.AddComponent<T>();
                    }
                }
                return s_instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (s_instance != null && s_instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            s_instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}
