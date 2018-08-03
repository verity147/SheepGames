using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PW_DirectionsPooler : MonoBehaviour {

    [Serializable]
    public class Pool
    {
        public string tagname;
        public GameObject prefab;
        public int poolSize;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        ///every queue of directions gets filled with direction objects according to its poolsize and added to the dictionary in the end
        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tagname, objectPool);
        }
    }
}
