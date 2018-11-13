using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour {

    public Vector3 Direction;
    public LayerMask CarLayer;

    private MeteorManager _meteorManager;

    private void Start()
    {
        _meteorManager = MeteorManager.Instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(Utils.CompareLayer(CarLayer, other.gameObject.layer))
        {
            GetComponent<BoxCollider>().enabled = false;
            _meteorManager.OnChangeSpawner();
        }
    }

}
