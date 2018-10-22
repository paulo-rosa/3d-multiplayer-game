using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineExplosion : MonoBehaviour {

    public LayerMask ExplosionLayer;

    private SphereCollider _collider;
    private float _timeToExplode = 0;
    private float _explosionTime = 2f;
    private float _damageAmount;
    private float _maxRadius = 4;
    private float _maxDamage = 100;
    private float _maxDistanceToDamage = 30f;
    private List<GameObject> _objectsAffected = new List<GameObject>();

    private void Start ()
    {
        _collider = GetComponent<SphereCollider>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if ((ExplosionLayer | (1 << other.gameObject.layer)) == ExplosionLayer)
        {
            var hit = other.GetComponentInParent<Health>();
            if(hit != null)
            {
                if (!_objectsAffected.Contains(other.transform.parent.gameObject))
                {
                    _objectsAffected.Add(other.transform.parent.gameObject);
                    Debug.Log("Give damage");
                    hit.TakeDamage(CalculateDamage(other.transform));
                }
            }
        }
    }

    public void StartExplosion()
    {
        while (_collider.radius < _maxRadius)
        {
            _timeToExplode += Time.deltaTime;
            if(_timeToExplode >= _explosionTime)
            {
                _collider.radius *= 1.1f;
                _timeToExplode = 0;
            }
        }
        if (_collider.radius >= _maxRadius)
        {
            _objectsAffected.Clear();
            Destroy(transform.parent.gameObject, .5f);
        }
    }

    private int CalculateDamage(Transform other)
    {
        var distance = Vector3.Distance(transform.position, other.position);
        Debug.Log("Distance" + distance);
        
        var damage = (int)(((_maxDistanceToDamage - 
            (distance - (transform.lossyScale.z + other.lossyScale.z))) / 
            _maxDistanceToDamage) * _maxDamage);

        if (damage < 0)
            damage = 0;
        Debug.Log(damage);
        return damage;

    }

}
