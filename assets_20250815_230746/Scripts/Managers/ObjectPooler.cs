using System.Collections.Generic;
using UnityEngine;

namespace ProjectWitchcraft.Core
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

        public List<Pool> pools;
        private Dictionary<string, Queue<GameObject>> _poolDictionary;

        // The static Singleton has been removed.

        private void Awake()
        {
            _poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();
                GameObject poolParent = new GameObject($"{pool.tag} Pool");
                poolParent.transform.SetParent(this.transform);

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab, poolParent.transform);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                _poolDictionary.Add(pool.tag, objectPool);
            }
        }

        public GameObject SpawnFromPool(string tag)
        {
            if (!_poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"Pool with tag '{tag}' doesn't exist.");
                return null;
            }

            GameObject objectToSpawn = _poolDictionary[tag].Dequeue();
            objectToSpawn.SetActive(true);
            _poolDictionary[tag].Enqueue(objectToSpawn);

            return objectToSpawn;
        }

        public void ReturnToPool(GameObject obj)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}