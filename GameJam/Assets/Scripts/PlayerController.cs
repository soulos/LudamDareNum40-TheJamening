using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float RotationSpeed = 10.0f;
    public Transform BulletObject;
    public float MovementSpeed = 10.0f;

    public float ShotDelay = 1.0f;

    private float shotTimer = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var movement = Input.GetAxis("Vertical");
	    var rotation = Input.GetAxis("Horizontal");
	    bool shoot = Input.GetButton("Fire1");
	    RotatePlayer(rotation);
	    MovePlayer(movement);
	    shotTimer += Time.deltaTime;
	    if (shoot && shotTimer > ShotDelay)
	    {
	        shotTimer = 0;
            Shoot();
	    }
	}

    void RotatePlayer(float rotation)
    {
        // lefthanded system need to use back to get the correct rotation
        transform.Rotate(Vector3.back * rotation * RotationSpeed * Time.deltaTime);
        
    }

    void MovePlayer(float movement)
    {
        transform.Translate(Vector3.up * movement * MovementSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        // fire bullets
        SpawnBullet();

    }

    Transform SpawnBullet()
    {
        // this should become part of the objectPooler
        return Instantiate(BulletObject, transform.position + Vector3.up, transform.rotation);
    }
}
