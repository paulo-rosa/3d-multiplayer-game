using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HoleTrapTrigger : NetworkBehaviour
{
    public LayerMask Layer;
    bool triggerEnabled = true;

    private void OnTriggerEnter(Collider collision)
    {
        if (triggerEnabled == false)
            return;

        if (Utils.CompareLayer(Layer, collision.gameObject.layer))
        {
            triggerEnabled = false;
            DestroyCar(collision);      
        }
    }

    IEnumerator EnableTrigger()
    {
        yield return new WaitForSeconds(3f);
        triggerEnabled = true;
    }

    private void DestroyCar(Collider collision)
    {
        var hit = collision.gameObject;
        var health = hit.GetComponentInParent<Health>();
        if (health != null)
        {
            health.TakeDamage(100);
        }
    }
}