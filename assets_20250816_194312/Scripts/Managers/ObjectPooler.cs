// Located at: Assets/Scripts/Managers/ObjectPooler.cs
using System.Collections.Generic;
using UnityEngine;
using ProjectWitchcraft.Core; // Added using statement

namespace ProjectWitchcraft.Managers
{
    // Inherit from Singleton<ObjectPooler>
    public class ObjectPooler : Singleton<ObjectPooler>
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }

        [Header("Configuration")]
        public List<Pool> pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary;

        private void Start()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();
                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab, transform);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDictionary.Add(pool.tag, objectPool);
            }
        }

        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
                return null;
            }

            GameObject objectToSpawn = poolDictionary[tag].Dequeue();
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            poolDictionary[tag].Enqueue(objectToSpawn);

            return objectToSpawn;
        }

        public GameObject SpawnFromPool(string tag)
        {
            return SpawnFromPool(tag, Vector3.zero, Quaternion.identity);
        }

        public void ReturnToPool(GameObject objectToReturn)
        {
            objectToReturn.SetActive(false);
        }
    }
}
