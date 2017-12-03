using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float RotationSpeed = 10.0f;
    public Transform BulletObject;
    public float MovementSpeed = 10.0f;
    public int BulletLevel = 1;
    public float DispersionAngle = 20f;
    public float ShotDelay = 1.0f;
    public ObjectHealth health;
    private float shotTimer = 0;
	// Use this for initialization
    
	void Start ()
	{
	    health = GetComponent<ObjectHealth>();
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
        GenerateBulletSpray();

    }

    void SpawnForwardBullet()
    {
        ObjectPoolingManager.GetGameObject("Bullets", transform.position + transform.up * 2, transform.rotation);
    }

    void GenerateBulletSpray()
    {
        if (BulletLevel == 1)
        {
            SpawnForwardBullet();
        }
        else
        {
            var counterStart = 0;
            if (BulletLevel % 2 != 0)
            {
                SpawnForwardBullet();
                counterStart++;
            }
            var bullets = BulletLevel - counterStart;
            var startAngle = 0f - (bullets / 2.0f * DispersionAngle);
            for (int loop = 0; loop < bullets; loop++)
            {
                var bullet = ObjectPoolingManager.GetGameObject("Bullets", transform.position + transform.up *2, transform.rotation);
                if (bullet != null)
                {
                    if (startAngle > -0.1f && startAngle <= 0.1f)
                    {
                        startAngle += DispersionAngle;
                    }
                    bullet.eulerAngles += Vector3.forward * startAngle;
                    startAngle += DispersionAngle;
                }
            }
        }
    }
}
