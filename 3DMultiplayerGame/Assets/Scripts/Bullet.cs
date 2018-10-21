﻿using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public int bulletDamage = 10;
    public LayerMask layerMask;

    void OnCollisionEnter(Collision collision)
    {
        if (layerMask == (layerMask | (1 << collision.gameObject.layer)))
        {
            //Debug.Log("Bateu" + collision.gameObject.name);
            var hit = collision.gameObject;
            var health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(bulletDamage);
            }

            Destroy(gameObject);
        }
    }
}