using Assets.Scripts.Multiplayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerCarController : NetworkBehaviour, ICarController
{
    [SerializeField]
    private float _maxSpeed = 70;
    [SerializeField]
    private float _maxReverseSpeed = -30;
    [SerializeField]
    private float _acceleration = 50;
    [SerializeField]
    private float _maxTurn = 30;
    [SerializeField]
    private Vector3 _jumpForce = new Vector3(0, 3, 0);
    private Rigidbody _rigidbody;
    [SyncVar]
    public float _speed;
    public float _turn;
    private CarCollision _carCollision;
    private bool _isFalling = false;
    private Health _carHealth;
    private CannonShooting[] _cannonsShooting;
    private MultiplayerCarManager _carManager;
    public event Action OnHealthChange;
    public float[] GearRatio;//determines how many gears the car has, and at what speed the car shifts to the appropriate gear
    private AudioSource[] _audioSources;
    public AudioClip EngineSound;
    private int gear;//current gear
    private AudioSource _engineAudioSource;

    // Use this for initialization
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _carCollision = GetComponent<CarCollision>();
        Car.transform = GetComponent<Transform>();
        _carHealth = GetComponent<Health>();
        _cannonsShooting = GetComponentsInChildren<CannonShooting>();
        _carManager = GetComponent<MultiplayerCarManager>();
        _carHealth.OnHealthChange += _carHealth.OnHealthChanged;
        _audioSources = GetComponents<AudioSource>();
        _engineAudioSource = _audioSources.Where(a => a.clip == EngineSound).FirstOrDefault();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority)
        {
            return;
        }

        Accelerate();
        Turn();
        Jump();
        Fire();
        FixRotation();
        PlayEngineSound();
    }

    private void FixRotation()
    {
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
        
    }

    public void Accelerate()
    {
        if (Input.GetKey(KeyCode.W) && !_carCollision.FrontCollision())
        {
            if (_speed >= _maxSpeed)
                _speed = _maxSpeed;
            else
                _speed += _acceleration * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S) && !_carCollision.BackCollision())
        {
            if (_speed <= _maxReverseSpeed)
                _speed = _maxReverseSpeed;
            else
                _speed -= _acceleration * Time.deltaTime;
        }
        else if (!_carCollision.BackCollision() && !_carCollision.FrontCollision())
        {
            _speed = Mathf.MoveTowards(_speed, 0, 0.5f);
        }
        else if (_carCollision.BackCollision())
        {
            if (_speed < 0)
                _speed = 0;
        }
        else if (_carCollision.FrontCollision())
        {
            if (_speed > 0)
                _speed = 0;
        }
        else
        {
            _speed = 0;
        }
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    public float CalculateTurn()
    {
        float turn = 0;
        if (_speed >= 0)
            turn = _maxTurn * (_speed * 2.5f / _maxSpeed);
        else
            turn = _maxTurn * (_speed * 2.5f / _maxReverseSpeed);

        _turn = Mathf.Clamp(_turn, _turn, _maxTurn);

        return turn;
    }

    public void Jump()
    {
        if (Input.GetKey(KeyCode.F) && _carCollision.GroundCollision())
        {
            _rigidbody.AddForce(_jumpForce, ForceMode.Impulse);
            Debug.Log("trying junml");
        }
    }

    public void Fire()
    {
        foreach (var cannonShooting in _cannonsShooting)
        {
            // Add the time since Update was last called to the timer.
            cannonShooting.Timer += Time.deltaTime;

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;

            // Set this to be the distance you want the object to be placed in front of the camera.
            var position = Camera.main.ScreenToWorldPoint(mousePos);
            var ray = Camera.main.ScreenPointToRay(mousePos);

            if (Input.GetKeyDown(KeyCode.Space) && cannonShooting.Timer >= cannonShooting.timeBetweenBullets && Time.timeScale != 0)
            {
                var transformPosition = cannonShooting.transform.position;

                cannonShooting.Shoot(cannonShooting.tag == "CannonCenter", _carManager.PlayerId,
                    position, ray, transformPosition);

                CmdFire(cannonShooting.tag, _carManager.PlayerId,
                    position, ray, transformPosition);
            }
        }
    }

    void PlayEngineSound()
    {
        for (int i = 0; i < GearRatio.Length; i++)
        {
            if (GearRatio[i] > _speed)
            {
                break;
            }

            float minGearValue = 0f;
            float maxGearValue = 0f;
            if (i == 0)
            {
                minGearValue = 0f;
            }
            else
            {
                minGearValue = GearRatio[i];
            }

            if (GearRatio.Length > i + 1)
            {
                maxGearValue = GearRatio[i + 1];
            }

            float pitch = ((_speed - minGearValue) / (maxGearValue - minGearValue) + 0.3f * (gear + 1));

            _engineAudioSource.pitch = pitch;

            gear = i;
        }
    }

    [Command]
    private void CmdFire(string objTag, int shooter, Vector3 worldPosition, Ray centerCannonRay, Vector3 transformPosition)
    {
       RpcFire(objTag, shooter, worldPosition, centerCannonRay, transformPosition);
    }

    [ClientRpc]
    private void RpcFire(string objTag, int shooter, Vector3 worldPosition, Ray centerCannonRay, Vector3 transformPosition)
    {
        if (hasAuthority)
        {
            return;
        }

        var cannon = GetComponentsInChildren<CannonShooting>().Where(c => c.tag == objTag).FirstOrDefault();
        cannon.Shoot(cannon.tag == "CannonCenter", shooter, worldPosition, centerCannonRay, transformPosition);
    }

    public void SetIsFalling(bool value)
    {
        _isFalling = value;
    }

    public void Turn()
    {
        var torque = Vector3.zero;

        if (torque != Vector3.zero)
            torque = -torque;

        _turn = CalculateTurn();

        if (Input.GetKey(KeyCode.A) && _speed != 0)
        {
            torque = Vector3.up * -_turn;
        }
        else if (Input.GetKey(KeyCode.D) && _speed != 0)
        {
            torque = Vector3.up * _turn;

        }
        //_rigidbody.AddRelativeTorque(torque);
        transform.Rotate(torque * Utils.Sign(_speed) * Time.deltaTime);
    }
}
