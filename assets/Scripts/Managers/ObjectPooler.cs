// Located at: Assets/Scripts/Managers/ObjectPooler.cs
using System.Collections.Generic;
using UnityEngine;

namespace ProjectWitchcraft.Managers
{
    public class ObjectPooler : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }

        [Header("Pool Configuration")]
        public List<Pool> pools;

        // This dictionary will hold all the queues of pooled objects.
        private Dictionary<string, Queue<GameObject>> _poolDictionary;
        // SUGGESTION: This parent transform helps keep the scene hierarchy clean.
        private Transform _poolParent;

        private void Start()
        {
            _poolDictionary = new Dictionary<string, Queue<GameObject>>();
            // Create a parent object to hold all pooled instances.
            _poolParent = new GameObject("ObjectPool").transform;

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    // Instantiate the object and parent it to our clean hierarchy object.
                    GameObject obj = Instantiate(pool.prefab, _poolParent);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                _poolDictionary.Add(pool.tag, objectPool);
            }
        }

        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!_poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
                return null;
            }

            GameObject objectToSpawn;

            // IMPROVEMENT: Check if the pool is empty.
            if (_poolDictionary[tag].Count > 0)
            {
                // If objects are available, dequeue one as normal.
                objectToSpawn = _poolDictionary[tag].Dequeue();
            }
            else
            {
                // If the pool is empty, dynamically create a new one.
                // This makes the pool flexible and prevents errors if you need more objects than the initial size.
                Debug.LogWarning($"Pool with tag '{tag}' was empty. Creating a new instance.");
                var pool = pools.Find(p => p.tag == tag);
                if (pool != null)
                {
                    objectToSpawn = Instantiate(pool.prefab, _poolParent);
                }
                else
                {
                    // This should not happen if the tag exists in the dictionary.
                    return null;
                }
            }

            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            objectToSpawn.SetActive(true);

            // FIX: The line that incorrectly added the object back to the queue has been removed.

            return objectToSpawn;
        }

        public GameObject SpawnFromPool(string tag)
        {
            return SpawnFromPool(tag, Vector3.zero, Quaternion.identity);
        }

        public void ReturnToPool(GameObject objectToReturn)
        {
            if (objectToReturn == null) return;

            // Derive the tag from the prefab's name. This requires your pooled prefabs
            // to have a name that matches a pool tag in the inspector.
            string tag = objectToReturn.name.Replace("(Clone)", "").Trim();

            if (!_poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"Trying to return object '{objectToReturn.name}' to a non-existent pool. Destroying it instead.");
                Destroy(objectToReturn); // Destroy if it doesn't belong to any pool
                return;
            }

            objectToReturn.SetActive(false);

            // FIX: Correctly add the object back to the queue for reuse.
            _poolDictionary[tag].Enqueue(objectToReturn);
        }
    }
}