using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyDie : MonoBehaviour
{
    public string PoolName;
    public int PointValue = 10;
    private bool isDead = false;
    public void Die()
    {
        
        if(!isDead)
        {
            isDead = true;
            GameManager.UpdateScore(PointValue);
            var spawn = new Vector3(transform.position.x, transform.position.y, transform.position.z +3);

            ObjectPoolingManager.DestroyPooledObject(PoolName, transform);
            ObjectPoolingManager.GetGameObject("Blood1", spawn, Quaternion.identity);
        }
    }

    public void Reset()
    {
        isDead = false;
    }
}
