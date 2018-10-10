using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CarController : NetworkBehaviour
{
    public float velocity = 10;
    public GameObject bulletPrefab;
    public Transform bulletRightSpawn;
    public Transform bulletLeftSpawn;

    public override void OnStartLocalPlayer()
    {
        Car.transform = GetComponent<Transform>();
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

        //Fire
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }

        //Todo - Jump
        //if (Input.GetKey(KeyCode.W))
        //{
        //    transform.GetComponent<Rigidbody>().AddForce(new Vector3(0, 20, 0), ForceMode.Force);
        //}

    }

    [Command]
    void CmdFire()
    {
        // Create the Bullets from the Bullet Prefab
        var bulletLeft = Instantiate(bulletPrefab, bulletLeftSpawn.position, bulletLeftSpawn.rotation);
        var bulletRight = Instantiate(bulletPrefab, bulletRightSpawn.position, bulletRightSpawn.rotation);

        // Add velocity to the bullets
        bulletLeft.GetComponent<Rigidbody>().velocity = bulletLeft.transform.forward * 40;
        bulletRight.GetComponent<Rigidbody>().velocity = bulletRight.transform.forward * 40;

        // Spawn the bullets on the Clients
        NetworkServer.Spawn(bulletLeft);
        NetworkServer.Spawn(bulletRight);

        // Destroy the bullet after 2 seconds
        Destroy(bulletLeft, 2.0f);
        Destroy(bulletRight, 2.0f);
    }
}
