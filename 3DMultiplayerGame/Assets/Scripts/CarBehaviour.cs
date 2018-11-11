using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class CarBehaviour : MonoBehaviour {


    public GameObject CarGraphics;

    private Health _health;
    private GameManager _gameManager;
    private Rigidbody _rigidBody;
    private PlayerStates _currentState;

    private void Start()
    {
        _rigidBody = GetComponentInChildren<Rigidbody>();
        _gameManager = GameManager.Instance;
        _health = GetComponent<Health>();
        _health.onDie += PlayerDie;
        transform.rotation = Quaternion.Euler(0, 90, 0);
        _gameManager._player = transform;
        _currentState = PlayerStates.ALIVE;
    }
    private void OnDestroy()
    {
        _health.onDie -= PlayerDie;
    }

    private void PlayerDie()
    {
        if (_gameManager.Die())
        {
            ChangeState(PlayerStates.DEAD);
            StartCoroutine(TimeToSpawn());
        }
        else
        {
            gameObject.SetActive(false);
        }
         
    }

    IEnumerator TimeToSpawn()
    {
        yield return new WaitForSeconds(2f);
        transform.position = _gameManager._spawnPosition.position;
        transform.rotation = _gameManager._spawnPosition.rotation;
        _rigidBody.velocity = Vector3.zero;
        _health.ResetHealth();
        ChangeState(PlayerStates.ALIVE);
    }
    #region
    public void ChangeState(PlayerStates state)
    {
        switch (state)
        {
            case PlayerStates.ALIVE:
                OnAliveEnter();
                break;
            case PlayerStates.DEAD:
                OnDeadEnter();
                break;
            case PlayerStates.RESPAWN:
                OnRespawnEnter();
                break;
        }
    }

    private void OnAliveEnter()
    {
        CarGraphics.SetActive(true);
    }

    private void OnDeadEnter()
    {
        CarGraphics.SetActive(false);
    }

    private void OnRespawnEnter()
    {
    }

    public PlayerStates GetState()
    {
        return _currentState;
    }
    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _health.TakeDamage(20);
        }
    }
}

public enum PlayerStates
{
    ALIVE,
    DEAD,
    RESPAWN
}