using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour {

    public float velocity = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        var direction = Input.GetAxis("Horizontal");
        transform.Rotate(0, direction, 0);

        var speed = Input.GetAxis("Vertical") * velocity;
        transform.position +=  transform.forward * speed * Time.deltaTime;

        if (speed < 0)
        {
            transform.forward = new Vector3(0, 0, speed);
        }
    }
}
