using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size = 10;
    }

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDict;

    void Awake()
    {
        Instance = this;
        poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            var queue = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                var obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            poolDict[pool.tag] = queue;
        }
    }

    public GameObject Spawn(string tag, Vector3 pos, Quaternion rot)
    {
        if (!poolDict.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool '{tag}' not found.");
            return null;
        }

        var queue = poolDict[tag];
        GameObject obj;

        if (queue.Count == 0)
        {
            // Expand pool dynamically
            var pool = pools.Find(p => p.tag == tag);
            obj = Instantiate(pool.prefab, transform);
        }
        else
        {
            obj = queue.Dequeue();
        }

        obj.transform.SetPositionAndRotation(pos, rot);
        obj.SetActive(true);
        return obj;
    }

    public void ReturnToPool(string tag, GameObject obj)
    {
        obj.SetActive(false);
        if (poolDict.ContainsKey(tag))
            poolDict[tag].Enqueue(obj);
    }

}
