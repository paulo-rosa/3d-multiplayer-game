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
            StartCoroutine(EnableTrigger());
        }
    }

    IEnumerator EnableTrigger()
    {
        yield return new WaitForSeconds(1f);
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