using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ZombieMoveController : MonoBehaviour
{
    private Transform target;
    public int PointValue = 10;
    
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
}
