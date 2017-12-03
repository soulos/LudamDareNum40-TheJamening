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
        }
        else if (col.collider.gameObject.CompareTag("Player"))
        {
            // drink
            
        }
    }
}
