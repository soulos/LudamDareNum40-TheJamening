using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{

    public string ZombieSpawnerName;

    public int PreSpawnAmount = 10;

    public float SpawnSpeed = 3;

	// Use this for initialization
	void Start () {
		
        InvokeRepeating("SpawnItem", SpawnSpeed,SpawnSpeed);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SpawnItem()
    {
        // pick a random direction and spit the item out
        Vector3 spawnPoint = GetRandomPoint();
        ObjectPoolingManager.GetGameObject(ZombieSpawnerName, transform.position + spawnPoint, Quaternion.AngleAxis(0,Vector3.forward));

    }

    void Die()
    {
        CancelInvoke();
    }

    Vector3 GetRandomPoint()
    {
        // we want a random offset based on a local rotation
        var randxPos = Random.Range(-2, 2);
        var randYPos = Random.Range(-2, 2);
        return new Vector3(randxPos, randYPos, 0);

    }
}
