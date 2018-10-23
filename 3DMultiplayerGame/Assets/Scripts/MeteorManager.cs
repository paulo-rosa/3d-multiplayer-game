using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorManager : MonoBehaviour {


    public GameObject Meteor;
    public Transform SpawnPoint;

    private float _timeToSpawn = 4;
    private float _timeCounter = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        _timeCounter += Time.deltaTime;
        if (_timeCounter > _timeToSpawn)
        {
            _timeCounter = 0;
            var go = Instantiate(Meteor, SpawnPoint.position, Quaternion.identity);
            go.GetComponent<MeteorBehaviour>().Init(SpawnPoint.GetComponent<MeteorSpawner>().Direction);
            
        }
	}
}
