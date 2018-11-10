using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineBehaviour : MonoBehaviour
{
    public LayerMask ExplosionLayer;

    public LandMineExplosion Explosion;

    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {

        if (Utils.CompareLayer(ExplosionLayer, other.gameObject.layer))
        {
            Debug.Log("Explode");
            //Explosion
            GetComponent<BoxCollider>().enabled = false;
            Explosion.StartExplosion();
        }
    }


}
