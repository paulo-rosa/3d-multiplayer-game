using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBehaviour : MonoBehaviour {


    private Rigidbody _rigidbody;
    public LayerMask ExplosionLayer;

	private void Start ()
    {

	}
	
	public void Init(Vector3 dir)
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(dir);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((ExplosionLayer | (1 << collision.gameObject.layer)) == ExplosionLayer)
        {
            //Explosion
            Destroy(gameObject);
        }
    }
}
