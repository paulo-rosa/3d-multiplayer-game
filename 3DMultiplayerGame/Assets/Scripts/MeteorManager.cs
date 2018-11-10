using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorManager : MonoBehaviour {


    public GameObject Meteor;
    public Transform SpawnPoint;

    public float _timeToSpawn = 15;
    private float _timeCounter = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        _timeCounter += Time.deltaTime;
        if (_timeCounter > _timeToSpawn)
        {
            var pos = SpawnPoint.position;

            _timeCounter = 0;
            var go = Instantiate(Meteor, new Vector3(Random.Range(pos.x - 3, pos.x + 3), pos.y, Random.Range(pos.z - 3, pos.z + 3)), Quaternion.identity);
            go.GetComponent<MeteorBehaviour>().Init(SpawnPoint.GetComponent<MeteorSpawner>().Direction);
            
        }
	}
}
