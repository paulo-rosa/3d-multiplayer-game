using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class CarController : NetworkBehaviour
{
    public float velocity = 10;
    public GameObject bulletPrefab;
    public Transform bulletRightSpawn;
    public Transform bulletLeftSpawn;
    public Transform bulletCenterSpawn;
    public AudioClip ShotSound;
    public AudioClip EngineSound;
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float handBrakeTorque = 500f;//hand brake value
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    public float[] GearRatio;//determines how many gears the car has, and at what speed the car shifts to the appropriate gear
    public Vector3 centerOfGravity;//car's center of mass offset
    public float maxReverseSpeed = 50f;//top speed for the reverse gear
    public float maxSpeed = 150f;//how fast the vehicle can go

    private float currentSpeed;//read only
    private int gear;//current gear
    Vector3 localCurrentSpeed;
    private AudioSource[] _audioSources;
    private bool reversing;//read only
    private CarBehaviour _carBehaviour;
    private Rigidbody rb;

    public override void OnStartLocalPlayer()
    {
        Car.transform = GetComponent<Transform>();
    }

    private void Awake()
    {
        _audioSources = GetComponents<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += centerOfGravity;
        _carBehaviour = GetComponent<CarBehaviour>();
    }
    
    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        if (_carBehaviour.GetState() != PlayerStates.ALIVE)
            return;

        //Move
        DoMovement();

        //Jump
        if (Input.GetKeyDown(KeyCode.F) && IsGrounded())
        {
            rb.AddForce(new Vector3(0, 8500, 0), ForceMode.Impulse);
        }

        PlayEngineSound();

        currentSpeed = rb.velocity.magnitude * 2.23693629f;//convert currentspeed into MPH

        localCurrentSpeed = transform.InverseTransformDirection(rb.velocity);
    }

    private bool IsGrounded()
    {
        //if (wheels == null || wheels.wheelFL == null || wheels.wheelFR == null || wheels.wheelRL == null || wheels.wheelRR == null)
        //{
        //    return false;
        //}
        //else if(wheels.wheelFL.isGrounded || wheels.wheelFR.isGrounded || wheels.wheelRL.isGrounded || wheels.wheelRR.isGrounded)
        //{
        //    return true;
        //}

        return true;
    }

    private void DoMovement()
    {
        //dont call this function if mobile input is checked in the editor
        float gasMultiplier = 0f;

        if (!reversing)
        {
            if (currentSpeed < maxSpeed)
                gasMultiplier = 1f;
            else
                gasMultiplier = 0f;
        }
        else
        {
            if (currentSpeed < maxReverseSpeed)
                gasMultiplier = 1f;
            else
                gasMultiplier = 0f;
        }

        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * 50.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        //in local space, if the car is travelling in the direction of the -z axis, (or in reverse), reversing will be true
        if (localCurrentSpeed.z < -0.1f)
        {
            reversing = true;
        }
        else
        {
            reversing = false;
        }
    }

    void PlayEngineSound()
    {
        var audioSource = _audioSources.Where(a => a.clip == EngineSound).FirstOrDefault();

        for (int i = 0; i < GearRatio.Length; i++)
        {
            if (GearRatio[i] > currentSpeed)
            {
                break;
            }

            float minGearValue = 0f;
            float maxGearValue = 0f;
            if (i == 0)
            {
                minGearValue = 0f;
            }
            else
            {
                minGearValue = GearRatio[i];
            }

            if (GearRatio.Length > i + 1)
            {
                maxGearValue = GearRatio[i + 1];
            }

            float pitch = ((currentSpeed - minGearValue) / (maxGearValue - minGearValue) + 0.3f * (gear + 1));
            audioSource.pitch = pitch;

            gear = i;
        }
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    void OnGUI()
    {
        //show the GUI for the speed and gear we are on.
        GUI.Box(new Rect(10, 10, 70, 30), "MPH: " + Mathf.Round(currentSpeed));
        if (!reversing)
            GUI.Box(new Rect(10, 70, 70, 30), "Gear: " + (gear + 1));
        if (reversing)//if the car is going backwards display the gear as R
            GUI.Box(new Rect(10, 70, 70, 30), "Gear: R");
    }
}