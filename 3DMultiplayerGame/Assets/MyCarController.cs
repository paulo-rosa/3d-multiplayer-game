using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCarController : MonoBehaviour {

    public LayerMask Layer;
    public float colliderSize;

    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _turn;
    [SerializeField]
    private float _maxTorque;
    [SerializeField]
    private Vector3 _jumpForce;
    private Rigidbody _rigidbody;
    private bool _isGrounded;
    private CarCollision _carCollision;

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
            transform.position += transform.forward * _speed * Time.deltaTime;
            return;
        }

        if (Input.GetKey(KeyCode.S) && !_carCollision.BackCollision())
        {
            transform.position += transform.forward * -_speed * Time.deltaTime;
        }
    }

    private void Turn()
    {
        if (!_carCollision.GroundCollision())
            return;
        var torque = Vector3.zero;
        //Debug.Log(torque);

        if (Input.GetKey(KeyCode.A))
        {
            torque = Vector3.up * -_turn;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            torque = Vector3.up * _turn;

        }
        _rigidbody.AddRelativeTorque(torque);
        

    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.F) && _carCollision.GroundCollision())
        {
            _rigidbody.AddForce(_jumpForce);
            Debug.Log("trying junml");
        }
    }






}
