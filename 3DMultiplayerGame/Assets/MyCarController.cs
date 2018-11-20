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


	private void Start ()
    {
        _rigidbody = GetComponent<Rigidbody>();
	}

    private void Update()
    {
        IsGrounded();
        FrontCollider();
        BackCollider();
    }

    private void FixedUpdate ()
    {
        Accelerate();
        Turn();
        Jump();
    }

    private void Accelerate()
    {

        if (Input.GetKey(KeyCode.W) && !FrontCollider()) 
        {
            transform.position += transform.forward * _speed * Time.deltaTime;
            return;
        }

        if (Input.GetKey(KeyCode.S) && !BackCollider())
        {
            transform.position += transform.forward * -_speed * Time.deltaTime;
        }
    }

    private void Turn()
    {
        if (!_isGrounded)
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
        if (Input.GetKey(KeyCode.F) && _isGrounded)
        {
            _rigidbody.AddForce(_jumpForce);
            Debug.Log("trying junml");
        }
    }

    private void IsGrounded()
    {
        var down = transform.TransformDirection(Vector3.down) * colliderSize;
        Debug.DrawRay(transform.position,down , Color.red, .1f);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, down, colliderSize, Layer))
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }

        Debug.Log(_isGrounded);
    }

    private bool FrontCollider()
    {
        Debug.DrawRay(transform.position + new Vector3(2.5f, 0, 5), Vector3.forward, Color.red, 1f);
        Debug.DrawRay(transform.position + new Vector3(-2.5f, 0, 5), Vector3.forward, Color.red, 1f);

        if (Physics.Raycast(transform.position + new Vector3(2.5f, 0, 5), Vector3.forward, colliderSize, Layer))
        {
            return true;
        }
        if (Physics.Raycast(transform.position + new Vector3(-2.5f, 0, 5), Vector3.forward, colliderSize, Layer))
        {
            return true;
        }


        return false;
    }

    private bool BackCollider()
    {
        Debug.DrawRay(transform.position + new Vector3(2.5f, 0, -5), Vector3.forward * -1, Color.red, 1f);
        Debug.DrawRay(transform.position + new Vector3(-2.5f, 0, -5), Vector3.forward * -1, Color.red, 1f);

        if (Physics.Raycast(transform.position + new Vector3(2.5f, 0, -5), Vector3.forward, colliderSize, Layer))
        {
            return true;
        }
        if (Physics.Raycast(transform.position + new Vector3(-2.5f, 0, -5), Vector3.forward, colliderSize, Layer))
        {
            return true;
        }


        return false;
    }
}
