using System.Collections;
using UnityEngine;

public class CarBehaviour : MonoBehaviour {


    public GameObject CarGraphics;
    public Transform PivotPoint;

    private MyCarController _carController;
    private Health _health;
    private IGameManager _gameManager;
    private Rigidbody _rigidBody;
    private PlayerStates _currentState;

    private void Awake()
    {
        Camera.main.GetComponent<CameraFollow>().SetTheTarget(this.gameObject);
        Camera.main.GetComponent<CameraFollow>().SetTarget(PivotPoint);


    }
    private void Start()
    {
        _carController = GetComponent<MyCarController>();
        _rigidBody = GetComponentInChildren<Rigidbody>();
        _gameManager = GeneralThings.FindGameManager();
        _health = GetComponent<Health>();
        _health.onDie += PlayerDie;
        transform.rotation = Quaternion.Euler(0, 90, 0);
        _gameManager._player = transform;
        ChangeState(PlayerStates.RESPAWN);
    }
    private void OnDestroy()
    {
        _health.onDie -= PlayerDie;
    }

    private void PlayerDie()

    {
        ChangeState(PlayerStates.DEAD);
    }

    IEnumerator TimeToSpawn()
    {
        yield return new WaitForSeconds(1f);
        transform.position = _gameManager._spawnPosition.position;
        transform.rotation = _gameManager._spawnPosition.rotation;
        _rigidBody.velocity = Vector3.zero;
        //_health.ResetHealth();
        ChangeState(PlayerStates.ALIVE);
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
        if (_gameManager.Die())
        {
            ChangeState(PlayerStates.RESPAWN);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnRespawnEnter()
    {
        _health.ResetHealth();
        StartCoroutine(TimeToSpawn());
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