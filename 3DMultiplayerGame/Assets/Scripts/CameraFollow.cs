﻿using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    public float distance = 5.0f;
    public float height = 3.0f;
    public float damping = 5.0f;
    public bool smoothRotation = true;
    public bool followBehind = true;
    public float rotationDamping = 10.0f;

    void Update()
    {
        if (Car.transform == null)
        {
            return;
        }
        else
        {
            target = Car.transform;
        }
        
        Vector3 wantedPosition;

        followBehind = true;

        if (Input.GetKey(KeyCode.Q))
        {
            followBehind = false;
        }

        if (followBehind)
        {
            wantedPosition = target.TransformPoint(0, height, -distance);
        }
        else
            wantedPosition = target.TransformPoint(0, height, distance);

        transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);

        if (smoothRotation)
        {
            Quaternion wantedRotation = Quaternion.LookRotation(target.position - transform.position, target.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
        }
        else transform.LookAt(target, target.up);
    }
}