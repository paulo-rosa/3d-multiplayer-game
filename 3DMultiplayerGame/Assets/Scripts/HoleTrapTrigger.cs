using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HoleTrapTrigger : NetworkBehaviour
{
    bool triggerEnabled = true;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && triggerEnabled)
        {
            triggerEnabled = false;
            DestroyCar(collision);      
        }
    }

    IEnumerator EnableTrigger()
    {
        yield return new WaitForSeconds(5f);
        triggerEnabled = true;
    }

    private void DestroyCar(Collider collision)
    {
        var hit = collision.gameObject;
        var health = hit.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(100);
        }
    }
}