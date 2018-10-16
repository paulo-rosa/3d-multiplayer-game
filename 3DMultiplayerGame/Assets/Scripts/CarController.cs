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
    public AudioClip CarSound;
    public AudioClip ShotSound;
    private AudioSource _audioSource;

    public override void OnStartLocalPlayer()
    {
        Car.transform = GetComponent<Transform>();
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        _audioSource.clip = CarSound;
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

        if (speed != 0)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            if (!_audioSource.isPlaying)
            {
                PlayCarSound();
            }
        }
        else
        {
            if (!_audioSource.clip.Equals(ShotSound))
            {
                _audioSource.Stop();
            }
        }

        //Fire
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayShotSound();
            CmdFire();
        }

        //Jump
        if (Input.GetKey(KeyCode.F))
        {
            transform.GetComponent<Rigidbody>().AddForce(new Vector3(0, 20, 0), ForceMode.Impulse);
        }
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

    public void PlayCarSound()
    {
        _audioSource.volume = 0.1f;
        _audioSource.clip = CarSound;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    public void PlayShotSound()
    {
        _audioSource.volume = 0.5f;
        _audioSource.clip = ShotSound;
        _audioSource.loop = false;
        _audioSource.Play();
    }
}
