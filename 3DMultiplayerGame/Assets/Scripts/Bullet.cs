using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public int bulletDamage = 10;
    public LayerMask layerMask;
    public LayerMask layerOrigin;

    void OnTriggerEnter(Collider collision)
    {
        if (layerOrigin == collision.gameObject.layer)
        {
            return;
        }

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