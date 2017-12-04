using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMoveController : MonoBehaviour
{
    private Transform target;
    public int PointValue = 10;
    public float DamageDone = 0.1f;
    public float Speed = 4;
	// Use this for initialization
	void Start ()
	{

	    target = GameObject.FindGameObjectWithTag("Player").transform;

	}
	
	// Update is called once per frame
	void FixedUpdate () {

        var rotation = Quaternion.FromToRotation(transform.position, target.position);
	    transform.up = target.position - transform.position;
	    transform.position = Vector2.MoveTowards(transform.position, target.transform.position, Speed * Time.deltaTime); transform.up = target.position - transform.position;
    }
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GetComponent<AudioSource>().Play();
            var health = col.collider.gameObject.GetComponent<ObjectHealth>();
            health.TakeDamage(DamageDone);
        }
    }
}
