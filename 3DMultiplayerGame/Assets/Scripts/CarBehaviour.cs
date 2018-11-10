using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class CarBehaviour : MonoBehaviour {

    private Health _health;
    private GameManager _gameManager;
    private Rigidbody _rigidBody;

    private void Start()
    {
        _rigidBody = GetComponentInChildren<Rigidbody>();
        _gameManager = GameManager.Instance;
        _health = GetComponent<Health>();
        _health.onDie += PlayerDie;
        transform.rotation = Quaternion.Euler(0, 90, 0);
        _gameManager._player = transform;
    }
    private void OnDestroy()
    {
        _health.onDie -= PlayerDie;
    }

    private void PlayerDie()
    {
        if (_gameManager.Die())
        {
            transform.position = _gameManager._spawnPosition.position;
            transform.rotation = Quaternion.Euler(0, 90, 0); // _gameManager._startPosition.rotation;
            _rigidBody.velocity = Vector3.zero;
        }
        else
        {
            gameObject.SetActive(false);
        }
         
    }
}
