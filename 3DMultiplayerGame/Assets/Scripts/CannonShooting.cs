using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class CannonShooting: MonoBehaviour
{
    public int damagePerShot = 20;                          // The damage inflicted by each bullet.
    public float timeBetweenBullets = 0.15f;                // The time between each shot.
    public float range = 100f;                              // The distance the gun can fire.
    public Cannontype cannonType = Cannontype.staticCannon;
    public Light faceLight;
    public LayerMask shootableMask;
    public float timer { get; set; }                        // A timer to determine when to fire.
    private Ray shootRay = new Ray();                       // A ray from the gun end forwards.
    private RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
    private ParticleSystem gunParticles;                    // Reference to the particle system.
    private LineRenderer gunLine;                           // Reference to the line renderer.
    private AudioSource gunAudio;                           // Reference to the audio source.
    private Light gunLight;                                 // Reference to the light component.
    public float effectsDisplayTime = 0.2f;                 // The proportion of the timeBetweenBullets that the effects will display for.
    private NetworkBehaviour _parentBehaviour;


    void Awake()
    {
        // Create a layer mask for the Shootable layer.

        // Set up the references.
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
        _parentBehaviour = transform.root.GetComponent<NetworkBehaviour>();
        //faceLight = GetComponentInChildren<Light> ();
    }

    public void DisableEffects()
    {
        // Disable the line renderer and the light.
        gunLine.enabled = false;
        faceLight.enabled = false;
        gunLight.enabled = false;
    }
    
    public void Shoot()
    {
        // Reset the timer.
        timer = 0f;

        // Play the gun shot audioclip.
        gunAudio.Play();

        // Enable the lights.
        gunLight.enabled = true;
        faceLight.enabled = true;

        // Stop the particles from playing if they were, then start the particles.
        gunParticles.Stop();
        gunParticles.Play();

        // Enable the line renderer and set it's first position to be the end of the gun.
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        // Perform the raycast against gameobjects on the shootable layer and if it hits something...
        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            // Try and find an EnemyHealth script on the gameobject hit.
            Health enemyHealth = shootHit.collider.GetComponentInParent<Health>();

            // If the EnemyHealth component exist...
            if (enemyHealth != null)
            {
                // ... the enemy should take damage.
                //enemyHealth.TakeDamage(damagePerShot, shootHit.point);
                enemyHealth.TakeDamage(damagePerShot);
            }

            // Set the second position of the line renderer to the point the raycast hit.
            gunLine.SetPosition(1, shootHit.point);
        }
        // If the raycast didn't hit anything on the shootable layer...
        else
        {
            // ... set the second position of the line renderer to the fullest extent of the gun's range.
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }

        // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            // ... disable the effects.
            DisableEffects();
        }
    }

    public void ShootCenter(Vector3 mousePosition, Vector3 position)
    {
        // Reset the timer.
        timer = 0f;
        timer += Time.deltaTime;

        // Play the gun shot audioclip.
        gunAudio.Play();

        // Enable the lights.
        gunLight.enabled = true;
        faceLight.enabled = true;

        // Stop the particles from playing if they were, then start the particles.
        gunParticles.Stop();
        gunParticles.Play();

        // Enable the line renderer and set it's first position to be the end of the gun.
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
        shootRay = Camera.main.ScreenPointToRay(position);

        // Perform the raycast against gameobjects on the shootable layer and if it hits something...
        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            // Try and find an EnemyHealth script on the gameobject hit.
            Health enemyHealth = shootHit.collider.GetComponent<Health>();

            // If the EnemyHealth component exist...
            if (enemyHealth != null)
            {
                // ... the enemy should take damage.
                enemyHealth.TakeDamage(damagePerShot);
            }

            // Set the second position of the line renderer to the point the raycast hit.
            gunLine.SetPosition(1, shootHit.point);
        }
        // If the raycast didn't hit anything on the shootable layer...
        else
        {
            // ... set the second position of the line renderer to the fullest extent of the gun's range.
            gunLine.SetPosition(1, position);
        }


        // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            // ... disable the effects.
            DisableEffects();
        }
    }
}

public enum Cannontype
{
    staticCannon,
    mobileCannon
}