using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float Speed = 10;

    public float LifespanInSeconds = 5f;
    private bool isDead = false;

    public float maxRebounds = 5f;

    private float lifetime = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (isActiveAndEnabled)
	    {
	        transform.Translate(Vector3.up * Speed * Time.deltaTime);
	    }

	    if (BulletDied())
	    {
	        gameObject.SetActive(false);
	    }
		
	}

    bool BulletDied()
    {
        lifetime += Time.deltaTime;
        if (lifetime >= LifespanInSeconds)
        {
            lifetime = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    
    
}
