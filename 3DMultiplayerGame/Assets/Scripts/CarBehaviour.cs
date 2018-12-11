using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CarBehaviour : NetworkBehaviour
{
    public GameObject CarGraphics;
    public Transform PivotPoint;

    private MyCarController _carController;
    private Health _health;
    private IGameManager _gameManager;
    private Rigidbody _rigidBody;
    private PlayerStates _currentState;
    private bool _assigned = false;

    public event Action OnPlayerDied;

    private void Start()
    {
        if (!hasAuthority)
        {
            return;
        }
    }

    private void Assign()
    {
        Camera.main.GetComponent<CameraFollow>().SetTheTarget(this.gameObject);
        Camera.main.GetComponent<CameraFollow>().SetTarget(PivotPoint);

        _carController = GetComponent<MyCarController>();
        _rigidBody = GetComponentInChildren<Rigidbody>();
        _gameManager = GeneralThings.FindGameManager();
        _health = GetComponent<Health>();
        _health.OnDie += PlayerDie;
        transform.rotation = Quaternion.Euler(0, 90, 0);
        _gameManager._player = transform;
        ChangeState(PlayerStates.RESPAWN);
        _assigned = true;
    }

    private void Update()
    {
        if(!_assigned && hasAuthority)
        {
            Assign();
        }
    }

    private void OnDestroy()
    {
        _health.OnDie -= PlayerDie;
    }

    private void PlayerDie()
    {
        ChangeState(PlayerStates.DEAD);

        if (OnPlayerDied != null)
        {
            OnPlayerDied();
        }

    }


    IEnumerator TimeToSpawn()
    {
        yield return new WaitForSeconds(1f);
        //transform.position = _gameManager._spawnPosition.position;
        //transform.rotation = _gameManager._spawnPosition.rotation;
        _rigidBody.velocity = Vector3.zero;
        
        //_health.ResetHealth();
        ChangeState(PlayerStates.RESPAWN);
    }

    #region States
    public void ChangeState(PlayerStates state)
    {
        _currentState = state;
        switch (_currentState)
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
        StartCoroutine(TimeToSpawn());

        //ChangeState(PlayerStates.RESPAWN);
    }

    private void OnRespawnEnter()
    {
        _health.ResetHealth();
        //StartCoroutine(TimeToSpawn());
        ChangeState(PlayerStates.ALIVE);
    }

    public PlayerStates GetState()
    {
        return _currentState;
    }
    #endregion

    public Transform GetPivotPoint()
    {
        return PivotPoint;
    }

    public CarDirection MovingDirection()
    {
       return _carController.GetCarDirection();
    } 
}

public enum PlayerStates
{
    ALIVE,
    DEAD,
    RESPAWN
}