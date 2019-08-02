using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class ReflectiveObject : MonoBehaviour
{

    public float ReflectionVariance = 10;

    public void Reflect(ContactPoint2D contact, Transform bullet)
    {
        var minVarience = ReflectionVariance / 2.0f * -1.0f;
        var maxVarience = ReflectionVariance / 2.0f ;
        Vector3 reflectedDirection = Vector3.Reflect(bullet.up, contact.normal);
        var reflectionRotationAmount = Random.Range(minVarience, maxVarience);
        Quaternion rotation = Quaternion.FromToRotation(bullet.up, reflectedDirection);
        bullet.rotation = rotation * transform.rotation;
        bullet.eulerAngles += Vector3.forward * reflectionRotationAmount;
    }
}
