using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineBehavior : MonoBehaviour
{
    public LayerMask ExplosionLayer;

    public LandMineExplosion Explosion;

    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {

        if ((ExplosionLayer | (1<< other.gameObject.layer))== ExplosionLayer)
        {
            Debug.Log("Explode");
            //Explosion
            GetComponent<BoxCollider>().enabled = false;
            Explosion.StartExplosion();
            MakeDamage();
        }
    }

    private void MakeDamage()
    {

    }

}
