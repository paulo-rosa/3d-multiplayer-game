using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UFOController : NetworkBehaviour {

    public Transform FirstPlayer;
    public float Offset = 20f;
    public float FloatFactor = 4f;
    public float FloatSpeed = 4f;
    public float RotationFactor = 90f;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    private float timeToShoot = 1;
    private float shootCounter = 0;
    float alpha = 0f;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (Car.transform == null)
        {
            return;
        }

        shootCounter += Time.deltaTime;

        if (shootCounter > timeToShoot)
        {
            //CmdFire();
            shootCounter = 0;
        }


        FirstPlayer = Car.transform;
        FollowPLayer();
	}

    [Command]
    void CmdFire()
    {
        // Create the Bullets from the Bullet Prefab
        var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        Vector3 shoot = (Car.transform.position - bulletSpawn.position).normalized;

        // Add velocity to the bullets
        bullet.GetComponent<Rigidbody>().velocity = shoot * 40;

        // Spawn the bullets on the Clients
        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

    private void FollowPLayer()
    {
        FloatAnimation();
        //RotationAnimation();
        OrbitAnimation();
    }

    private void FloatAnimation()
    {
        transform.position = transform.position + new Vector3(0, transform.position.y + FloatFactor * Mathf.Sin(FloatSpeed * Time.time), 0);
    }

    private void RotationAnimation()
    {
        //transform.RotateAround(transform.position, transform.up, Time.deltaTime * RotationFactor);
    }

    private void OrbitAnimation()
    {
        var x = FirstPlayer.position.x + FirstPlayer.forward.x * Offset;
        var z = FirstPlayer.position.z + FirstPlayer.forward.z * Offset;

        transform.position = new Vector3(x + (5f * Mathf.Sin(Mathf.Deg2Rad * alpha)),
                                        FirstPlayer.position.y + 10,
                                        z + (2f * Mathf.Cos(Mathf.Deg2Rad * alpha)));
        alpha += 2f;//can be used as speed
    }
}
