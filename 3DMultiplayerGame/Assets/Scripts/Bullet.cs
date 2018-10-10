using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public int bulletDamage = 10;

    void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var health = hit.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(bulletDamage);
        }

        Destroy(gameObject);
    }
}