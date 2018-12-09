﻿using System;
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
    public float _speed;
    public float _turn;
    private CarCollision _carCollision;
    private bool _isFalling = false;
    private Health _carHealth;
    private CannonShooting[] _cannonsShooting;

    public event Action OnHealthChange;

    // Use this for initialization
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _carCollision = GetComponent<CarCollision>();
        Car.transform = GetComponent<Transform>();
        _carHealth = GetComponent<Health>();
        _carHealth.OnHealthChange += _carHealth.OnHealthChanged;
        _cannonsShooting = GetComponentsInChildren<CannonShooting>();
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

    public void Fire()
    {
        foreach (var cannonShooting in _cannonsShooting)
        {
            // Add the time since Update was last called to the timer.
            cannonShooting.timer += Time.deltaTime;

            Vector3 temp = Input.mousePosition;
            temp.z = 10f;
            // Set this to be the distance you want the object to be placed in front of the camera.
            var position = Camera.main.ScreenToWorldPoint(temp);

            if (Input.GetKeyDown(KeyCode.Space) && cannonShooting.timer >= cannonShooting.timeBetweenBullets && Time.timeScale != 0)
            {
                CmdFire(cannonShooting.tag, temp, position);
            }
        }
    }

    [Command]
    private void CmdFire(string objTag, Vector3 mousePosition, Vector3 position)
    {
        RpcFire(objTag, mousePosition, position);
    }

    [ClientRpc]
    private void RpcFire(string objTag, Vector3 mousePosition, Vector3 position)
    {
        var cannons = GetComponentsInChildren<CannonShooting>();

        if (objTag == "cannonCenter")
        {
            cannons.Where(c => c.tag == objTag).FirstOrDefault().ShootCenter(mousePosition, position);
        }
        else
        {
            cannons.Where(c => c.tag == objTag).FirstOrDefault().Shoot();
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
