using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlcoholPickup : MonoBehaviour
{

    public string PoolName;

    public string ExplosionPoolName;

    public int AlcoholAmount = 10;

    public int HealthAmount = 10;
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            // make explode
            ObjectPoolingManager.GetGameObject(ExplosionPoolName, transform.position, Quaternion.identity);
            ObjectPoolingManager.DestroyPooledObject(PoolName,transform);
        }
        else if (col.gameObject.CompareTag("Player"))
        {
            GetComponent<AudioSource>().Play();
            // drink
            ObjectPoolingManager.DestroyPooledObject(PoolName, transform);
            // increase health
            col.gameObject.GetComponent<ObjectHealth>().DrinkAlcohol(AlcoholAmount, HealthAmount);

            // increase alcohol

        }
    }
}
