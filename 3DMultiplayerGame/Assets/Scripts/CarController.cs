using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class CarController : NetworkBehaviour
{
    public WC wheels;
    public float velocity = 10;
    public GameObject bulletPrefab;
    public Transform bulletRightSpawn;
    public Transform bulletLeftSpawn;
    public Transform bulletCenterSpawn;
    public AudioClip ShotSound;
    public AudioClip EngineSound;
    public List<AxleInfo> axleInfoList; // the information about each individual axle
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
        transform.GetComponent<Rigidbody>().centerOfMass += centerOfGravity;
    }
    
    private void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        //Move
        DoMovement();

        ////Fire
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    //PlayShotSound();
        //    //CmdFire();
        //}

        //Jump
        if (Input.GetKey(KeyCode.F))
        {
            transform.GetComponent<Rigidbody>().AddForce(new Vector3(0, 20, 0), ForceMode.Impulse);
        }

        PlayEngineSound();

        currentSpeed = GetComponent<Rigidbody>().velocity.magnitude * 2.23693629f;//convert currentspeed into MPH

        localCurrentSpeed = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
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

        wheels.wheelRR.motorTorque = maxMotorTorque * Input.GetAxis("Vertical") * gasMultiplier;
        wheels.wheelRL.motorTorque = maxMotorTorque * Input.GetAxis("Vertical") * gasMultiplier;

        if (localCurrentSpeed.z < -0.1f && wheels.wheelRL.rpm < 10)
        {//in local space, if the car is travelling in the direction of the -z axis, (or in reverse), reversing will be true
            reversing = true;
        }
        else
        {
            reversing = false;
        }

        wheels.wheelFL.steerAngle = maxSteeringAngle * Input.GetAxis("Horizontal");
        wheels.wheelFR.steerAngle = maxSteeringAngle * Input.GetAxis("Horizontal");

        //pressing space triggers the car's handbrake
        if (Input.GetButton("Jump"))
        {
            wheels.wheelFL.brakeTorque = handBrakeTorque;
            wheels.wheelFR.brakeTorque = handBrakeTorque;
            wheels.wheelRL.brakeTorque = handBrakeTorque;
            wheels.wheelRR.brakeTorque = handBrakeTorque;
        }
        else//letting go of space disables the handbrake
        {
            wheels.wheelFL.brakeTorque = 0f;
            wheels.wheelFR.brakeTorque = 0f;
            wheels.wheelRL.brakeTorque = 0f;
            wheels.wheelRR.brakeTorque = 0f;
        }
    }

    //[Command]
    //void CmdFire()
    //{
    //    // Create the Bullets from the Bullet Prefab
    //    var bulletLeft = Instantiate(bulletPrefab, bulletLeftSpawn.position, bulletLeftSpawn.rotation);
    //    bulletLeft.GetComponent<Bullet>().layerOrigin = gameObject.layer;

    //    var bulletRight = Instantiate(bulletPrefab, bulletRightSpawn.position, bulletRightSpawn.rotation);
    //    bulletRight.GetComponent<Bullet>().layerOrigin = gameObject.layer;

    //    var bulletCenter = Instantiate(bulletPrefab, bulletCenterSpawn.position, bulletCenterSpawn.rotation);
    //    bulletRight.GetComponent<Bullet>().layerOrigin = gameObject.layer;

    //    // Add velocity to the bullets
    //    bulletLeft.GetComponent<Rigidbody>().velocity = bulletLeft.transform.forward * 80;
    //    bulletRight.GetComponent<Rigidbody>().velocity = bulletRight.transform.forward * 80;
    //    bulletCenter.GetComponent<Rigidbody>().velocity = bulletCenter.transform.forward * 80;

    //    // Spawn the bullets on the Clients
    //    NetworkServer.Spawn(bulletLeft);
    //    NetworkServer.Spawn(bulletRight);
    //    NetworkServer.Spawn(bulletCenter);

    //    // Destroy the bullet after 2 seconds
    //    Destroy(bulletLeft, 2.0f);
    //    Destroy(bulletRight, 2.0f);
    //    Destroy(bulletCenter, 2.0f);
    //}

    //public void PlayShotSound()
    //{
    //    var audioSource = _audioSources.Where(a => a.clip == ShotSound).FirstOrDefault();
    //    audioSource.pitch = 1;
    //    audioSource.volume = 0.5f;
    //    audioSource.clip = ShotSound;
    //    audioSource.loop = false;
    //    audioSource.Play();
    //}

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

[System.Serializable]
public class WC
{
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;
}