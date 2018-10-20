using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineExplosion : MonoBehaviour {

    public LayerMask ExplosionLayer;

    private SphereCollider _collider;
    private float _timeToExplode = 0;
    private float _explosiontTime = .7f;
    private float _damageAmount;
    private float _maxRadius = 8;
    private float _maxDamage = 100;
    // Use this for initialization

    private void Start ()
    {
        _collider = GetComponent<SphereCollider>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if ((ExplosionLayer | (1 << other.gameObject.layer)) == ExplosionLayer)
        {
            var hit = other.GetComponent<Health>();
            if(hit != null)
            {
                hit.TakeDamage(CalculateDamage(other.transform));
            }
        }
    }

    public void StartExplosion()
    {
        while (_collider.radius < _maxDamage)
        {
            _collider.radius *= 1.1f;
        }

        Destroy(gameObject);
    }

    private int CalculateDamage(Transform other)
    {
        var distance = Vector3.Distance(transform.position, other.position);
        var damage = (int)(((distance * 100)/ _maxRadius) * _maxDamage);
        return damage;
    }
}
