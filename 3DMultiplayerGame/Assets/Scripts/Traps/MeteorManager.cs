using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorManager : MonoBehaviour {

    private static MeteorManager _instance;

    public static MeteorManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<MeteorManager>();
            }
            return _instance;
        }
    }

    public GameObject Meteor;
    public float _timeToSpawn = 10;
    public Transform SpawnPointsHolder;

    private int _spawnPointsCount;
    private float _timeCounter = 0;
    private bool _isActive;
    private List<GameObject> _spawnPoints = new List<GameObject>();
    private int _currentId = 0;
    private MeteorPool _meteorPool;

	// Use this for initialization
	void Start ()
    {
        FillTheList();
        _meteorPool = GetComponent<MeteorPool>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        _timeCounter += Time.deltaTime;
        if (_timeCounter > _timeToSpawn)
        {
            var pos = _spawnPoints[_currentId].transform.position;

            _timeCounter = 0;
            //var go = Instantiate(Meteor, new Vector3(UnityEngine.Random.Range(pos.x - 3, pos.x + 3), pos.y, UnityEngine.Random.Range(pos.z - 3, pos.z + 3)), Quaternion.identity);
            //go.GetComponent<MeteorBehaviour>().Init(_spawnPoints[_currentId].GetComponent<MeteorSpawner>().Direction);
            var go = _meteorPool.GetObject();
            go.transform.position = new Vector3(UnityEngine.Random.Range(pos.x - 3, pos.x + 3), pos.y, UnityEngine.Random.Range(pos.z - 3, pos.z + 3));
            go.GetComponent<MeteorBehaviour>().Init(_spawnPoints[_currentId].GetComponent<MeteorSpawner>().Direction);
        }
    }

    public void OnChangeSpawner()
    {
        if(_currentId < _spawnPointsCount)
            _currentId++;
    }

    private void FillTheList()
    {
        _spawnPointsCount = SpawnPointsHolder.childCount;
        for (int i = 0; i < _spawnPointsCount; i++)
        {
            var go = SpawnPointsHolder.GetChild(i).gameObject;
            _spawnPoints.Add(go);
        }
    }


}
