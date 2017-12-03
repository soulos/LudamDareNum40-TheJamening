using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class ObjectPoolingManager : MonoBehaviour
{

    public ObjectPool[] Pools;
    private static ObjectPoolingManager _instance;
    public static ObjectPoolingManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ObjectPoolingManager>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
        foreach (var pool in Pools)
        {
            pool.Initialize();
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static Transform GetGameObject(string poolName, Vector3 position, Quaternion rotation)
    {
        var pool = Instance.Pools.FirstOrDefault(p => p.PoolName == poolName);
        if (pool != null)
        {
            return pool.GetNextObject(position, rotation);
        }
        else
        {
            Debug.LogError($"No Pool exists with name {poolName}");
            return null;
        }
    }

    public static void DestroyPooledObject(string poolName, Transform pooledItem)
    {
        var pool = Instance.Pools.FirstOrDefault(p => p.PoolName == poolName);
        pool?.DestroyObject(pooledItem);
    }
}

