using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PW_DirectionsSpawner : MonoBehaviour {

    //spawn a direction element  on one of the four children in a random time
    [Serializable]
    public class Pool
    {
        public string tagname;
        public GameObject prefab;
        public int poolSize;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public float directionWaitMin = 0.3f;
    public float directionWaitMax = 1f;

    private Transform[] spawners;
    private string[] directionQueues;

    private void Start()
    {
        ///fill array with the spawner transforms to randomize spawnpoint of directions later
        spawners = new Transform[transform.childCount];
        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i] = transform.GetChild(i).transform;
        }

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        ///every queue of directions gets filled with direction objects according to its poolsize and added to the dictionary in the end
        foreach (Pool pool in pools)
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

        directionQueues = poolDictionary.Keys.ToArray();
    }

    /// return one object from the queue with the specified tagName from the dictionary, return it and re-add it to its queue for re-use
    public GameObject SpawnNewDirection(string tagName, Vector3 position)
    {
        if (!poolDictionary.ContainsKey(tagName))
        {
            Debug.LogWarning("Pool with tag " + tagName + " doesn't exist, yet you are trying to access it");
            return null;
        }

        GameObject newDirection = poolDictionary[tagName].Dequeue();

        newDirection.SetActive(true);
        newDirection.transform.position = position;

        poolDictionary[tagName].Enqueue(newDirection);
        return newDirection;
    }

    internal void StartSpawnEngine()
    {
        StartCoroutine(SpawnEngine());
    }

    ///started and stopped from InputManager
    private IEnumerator SpawnEngine()
    {
        SpawnNewDirection(directionQueues[UnityEngine.Random.Range(0, 4)], spawners[UnityEngine.Random.Range(0, 4)].position);
        yield return new WaitForSeconds(UnityEngine.Random.Range(directionWaitMin, directionWaitMax));
        StartCoroutine(SpawnEngine());
    }
}
