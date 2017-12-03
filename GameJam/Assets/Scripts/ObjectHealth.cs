﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.Scripts;
using UnityEngine;

public class ObjectHealth : MonoBehaviour
{

    public int MaxHealth = 100;

    private float currentHealth = 100;

    public int MaxAlcohol = 100;

    private float currentAlcohol = 0;

    public float DamageReduction = 0;

    // Use this for initialization
    void Start()
    {
        currentHealth = MaxHealth;
    }

    public void DrinkAlcohol(int alcoholAmount, int healthAmount)
    {
        currentAlcohol += alcoholAmount;
        currentHealth += healthAmount;
        if (currentAlcohol >= MaxAlcohol)
        {
            //Player is munted
            currentHealth = 0;

        }

        if (currentHealth > MaxHealth)
            currentHealth = MaxHealth;
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            // take damage from bullet
            Projectile proj = col.collider.gameObject.GetComponent<Projectile>();
            if (proj != null)
            {

                var damage = DamageReduction * proj.Damage;
                currentHealth -= (int)damage * DamageReduction;
                var reflectiveObject = gameObject.GetComponent<ReflectiveObject>();
                if (reflectiveObject != null)
                {
                    reflectiveObject.Reflect(col.contacts[0], col.collider.transform);
                }

                else
                {
                    ObjectPoolingManager.DestroyPooledObject("Bullets", col.collider.transform);
                    if (currentHealth <= 0 && currentAlcohol < MaxAlcohol)
                    {
                       
                        // player died of lead poisoning
                        // play death anim then change state -- look at Animation events
                        if (gameObject.CompareTag("Player"))
                        {
                            GameManager.ChangeState(GameState.DiedBullet);
                        }
                        else if (gameObject.CompareTag("Enemy"))
                        {
                           gameObject.GetComponent<EnemyDie>().Die();
                        }
                    }
                    else if (currentHealth <= 0 && currentAlcohol >= MaxAlcohol)
                    {
                        if (gameObject.CompareTag("Player"))
                        {
                            GameManager.ChangeState(GameState.DiedAlcohol);
                        }
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

    public float HealthPercent()
    {
        return (float)currentHealth / (float)MaxHealth;
    }

    public float AlcoholPercent()
    {
        return (float)currentAlcohol / (float)MaxAlcohol;
    }
}
