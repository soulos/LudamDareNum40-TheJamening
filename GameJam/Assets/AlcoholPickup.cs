using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlcoholPickup : MonoBehaviour
{

    public string PoolName;

    public string ExplosionPoolName;

    public float AlcoholAmount = 10;

    public float HealthAmount = 10;
    
    void OnCollisionEnter(Collision col)
    {
        if (col.collider.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            // make explode
            ObjectPoolingManager.GetGameObject(ExplosionPoolName, transform.position, Quaternion.identity);
            ObjectPoolingManager.DestroyPooledObject(PoolName,transform);
        }
        else if (col.collider.gameObject.CompareTag("Player"))
        {
            // drink
            ObjectPoolingManager.DestroyPooledObject(PoolName, transform);
            // increase health
            // increase alcohol

        }
    }
}
