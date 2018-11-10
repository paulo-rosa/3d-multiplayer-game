using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehaviour : MonoBehaviour {

    private GameManager _gameManager;
    public LayerMask CarLayer;

    private void Start ()
    {
        _gameManager = GameManager.Instance;	
	}

    private void OnTriggerEnter(Collider other)
    {
        if(Utils.CompareLayer(CarLayer, other.gameObject.layer))
        {
            _gameManager.SetSpawnPosition(transform);
        }
    }



}
