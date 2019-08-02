using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDone : MonoBehaviour
{

    public float Seconds = 2;
    private IEnumerator destroy;
    public string PoolName = "Explosions";
	// Use this for initialization
	void Start () {
        
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Destroy()
     {
         while (true)
         {
             yield return new WaitForSeconds(Seconds);
             ObjectPoolingManager.DestroyPooledObject(PoolName, transform);
         }
     }

    void OnEnable()
    {
        destroy = Destroy();
        StartCoroutine(destroy);
    }


    
}
