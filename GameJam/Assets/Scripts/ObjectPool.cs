using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ObjectPool
{
    [SerializeField]
    public string PoolName;
    [SerializeField]
    public Transform Prefab;
    [SerializeField]
    public int CacheCount = 10;

    private readonly Vector3 SpawnPosition = new Vector3(100000, 100000, -1000000);
    private readonly Queue<Transform> ObjectCache = new Queue<Transform>();

    public void Initialize()
    {
        for (int loop = 0; loop < CacheCount; loop++)
        {
            var go = GameObject.Instantiate(Prefab, SpawnPosition, Quaternion.identity);
            go.gameObject.SetActive(false);
            ObjectCache.Enqueue(go);
        }
    }

    public Transform GetNextObject(Vector3 spawnPoint, Quaternion rotation)
    {
        var spawnable = ObjectCache.Dequeue();
        if (spawnable != null)
        {
            spawnable.position = spawnPoint;
            spawnable.rotation = rotation;
            spawnable.gameObject.SetActive(true);
        }
        return spawnable;
    }

    public void DestroyObject(Transform objectToDestroy)
    {
        objectToDestroy.gameObject.SetActive(false);
        objectToDestroy.position = SpawnPosition;
        objectToDestroy.rotation = Quaternion.identity;
        ObjectCache.Enqueue(objectToDestroy);
    }
}

