using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOBehaviour : MonoBehaviour {

    public Transform FirstPlayer;
    public float Offset = 20f;
    public float FloatFactor = 4f;
    public float FloatSpeed = 4f;
    public float RotationFactor = 90f;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (Car.transform == null)
        {
            return;
        }
        FirstPlayer = Car.transform;
        FollowPLayer();
	}

    private void FollowPLayer()
    {
        var newPosition = new Vector3(0, 3, 0) + FirstPlayer.position + FirstPlayer.forward * Offset;

        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(FirstPlayer.position, newPosition, Time.deltaTime * 100);

        FloatAnimation();
        RotationAnimation();
    }

    private void FloatAnimation()
    {
        transform.position = transform.position + new Vector3(0, transform.position.y + FloatFactor * Mathf.Sin(FloatSpeed * Time.time), 0);
    }

    private void RotationAnimation()
    {
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * RotationFactor);
    }
}
