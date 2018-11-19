using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCarController : MonoBehaviour {

    public LayerMask Layer;

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


	private void Start ()
    {
        _rigidbody = GetComponent<Rigidbody>();
	}
	
	private void FixedUpdate ()
    {
        Accelerate();
        Turn();
        Debug.Log("Angular velocity: " + _rigidbody.angularVelocity);
    }

    private void Accelerate()
    {
        if (Input.GetKey(KeyCode.W)) 
        {
            transform.position += transform.forward * _speed * Time.deltaTime;
            return;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += transform.forward * -_speed * Time.deltaTime;
        }
    }

    private void Turn()
    {
        float turn = Input.GetAxis("Horizontal");
        var torque = Vector3.up * _turn * turn;
        torque.y = Mathf.Clamp(torque.y, -_maxTorque, _maxTorque);
        Debug.Log(torque);
        _rigidbody.AddRelativeTorque(torque);

    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.LeftShift) && IsGrounded())
        {
            _rigidbody.AddForce(_jumpForce);
        }
    }

    private bool IsGrounded()
    {
        Debug.DrawRay(transform.position, Vector3.down * (.52f), Color.red, 1f);
        
        if(Physics.Raycast(transform.position, Vector3.down, .52f, Layer))
        {
            return false;
        }
        return true;
    }
}
