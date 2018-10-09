using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CarController : NetworkBehaviour
{
    public float velocity = 10;

    public override void OnStartLocalPlayer()
    {
        Car.instance = new Car
        {
            transform = GetComponent<Transform>()
        };
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        var direction = Input.GetAxis("Horizontal");
        transform.Rotate(0, direction, 0);

        var speed = Input.GetAxis("Vertical") * velocity;
        transform.position += transform.forward * speed * Time.deltaTime;

        //Todo - Jump
        //if (Input.GetKey(KeyCode.W))
        //{
        //    transform.GetComponent<Rigidbody>().AddForce(new Vector3(0, 20, 0), ForceMode.Force);
        //}

    }
}
