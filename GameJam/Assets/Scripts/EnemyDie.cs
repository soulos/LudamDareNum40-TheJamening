using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EnemyDie : MonoBehaviour
{
    public string PoolName;
    public int PointValue = 10;
    private bool isDead = false;

    public BoardManager boardManager = null;

    private void Start()
    {
        var board = GameObject.Find("Board");
        boardManager = board.GetComponent<BoardManager>();
    }

    public void Die()
    {
        if(!isDead)
        {
            isDead = true;
            GameManager.UpdateScore(PointValue);
            var spawn = new Vector3(transform.position.x, transform.position.y, transform.position.z +3);

            ObjectPoolingManager.DestroyPooledObject(PoolName, transform);
            ObjectPoolingManager.GetGameObject("Blood1", spawn, Quaternion.identity);
            
            boardManager?.SomethingDied(this.transform);
        }
    }

    public void Reset()
    {
        isDead = false;
    }
}
