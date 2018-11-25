using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCarController : MonoBehaviour {

    public LayerMask Layer;

    [SerializeField]
    private float _maxSpeed;
    [SerializeField]
    private float _maxReverseSpeed;
    [SerializeField]
    private float _acceleration = 50;
    [SerializeField]
    private float _maxTurn;
    [SerializeField]
    private Vector3 _jumpForce;
    private Rigidbody _rigidbody;
    private bool _isGrounded;
    private CarCollision _carCollision;
    public float _speed;
    private bool _isFalling = false;
    public float _turn;

    private void Start ()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _carCollision = GetComponent<CarCollision>();
	}

    private void Update()
    {

    }

    private void FixedUpdate ()
    {
        Accelerate();
        Turn();
        Jump();
    }


    private void Accelerate()
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
        else if(!_carCollision.BackCollision() && !_carCollision.FrontCollision())
        {
            _speed = Mathf.MoveTowards(_speed, 0, 2f);
        }
        else if(_carCollision.BackCollision())
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

    private void Turn()
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
        transform.Rotate(torque * Utils.Sign(_speed) *  Time.deltaTime);

    }

    private float CalculateTurn()
    {
        float turn = 0;
        if (_speed >= 0)
            turn = _maxTurn * (_speed * 2.5f / _maxSpeed);
        else
            turn = _maxTurn * (_speed * 2.5f / _maxReverseSpeed);

        _turn = Mathf.Clamp(_turn, _turn, _maxTurn);

        return turn;
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.F) && _carCollision.GroundCollision())
        {
            _rigidbody.AddForce(_jumpForce, ForceMode.Impulse);
            Debug.Log("trying junml");
        }
    }

    private void SetIsFalling(bool value)
    {
        _isFalling = value;
    }
}
