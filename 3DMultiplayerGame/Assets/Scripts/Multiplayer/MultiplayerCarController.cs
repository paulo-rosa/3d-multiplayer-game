using System.Collections;
using System.Collections.Generic;
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
    public float _speed;
    public float _turn;
    private CarCollision _carCollision;
    private bool _isFalling = false;

    // Use this for initialization
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _carCollision = GetComponent<CarCollision>();
        Car.transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        Accelerate();
        Turn();
        Jump();
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
            _speed = Mathf.MoveTowards(_speed, 0, 2f);
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
