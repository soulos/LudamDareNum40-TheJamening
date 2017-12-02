using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.Scripts;
using UnityEngine;

public class ObjectHealth : MonoBehaviour
{

    public int MaxHealth = 100;

    private int currentHealth = 100;

    public float DamageReduction = 0;
    
	// Use this for initialization
	void Start ()
	{
	    currentHealth = MaxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            // take damage from bullet
            Projectile proj = col.collider.gameObject.GetComponent<Projectile>();
            if (proj!= null)
            {
                
                var damage = DamageReduction * proj.Damage;
                currentHealth -= (int)damage;
                var reflectiveObject = gameObject.GetComponent<ReflectiveObject>();
                if (reflectiveObject != null)
                {
                    reflectiveObject.Reflect(col.relativeVelocity, proj);
                }
                if (currentHealth <= 0)
                {
                    // player died of lead poisoning
                    // play death anim then change state -- look at Animation events
                    if (gameObject.CompareTag("Player"))
                    {
                        GameManager.ChangeState(GameState.DiedBullet);
                    }
                    else
                    {
                        
                    }
                }


            }

        }
        else if (col.collider.gameObject.layer == LayerMask.NameToLayer("Consumable"))
        {
            // get the consumable object and apply the effects   
        }
        else if (col.collider.gameObject.layer == LayerMask.NameToLayer("Badguys"))
        {
            // take damage from zombies
        }
    
    }
}
