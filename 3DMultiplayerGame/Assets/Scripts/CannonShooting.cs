using Assets.Scripts.Multiplayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class CannonShooting : MonoBehaviour
{
    public int damagePerShot = 20;                          // The damage inflicted by each bullet.
    public float timeBetweenBullets = 0.15f;                // The time between each shot.
    public float range = Mathf.Infinity;                              // The distance the gun can fire.
    public Cannontype cannonType = Cannontype.staticCannon;
    public Light faceLight;
    public LayerMask shootableMask;
    public float Timer { get; set; }                        // A timer to determine when to fire.
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
    }

    public void Shoot(bool centerCannon, int shooter, Vector3 worldPosition, Ray centerCannonRay, Vector3 transformPosition)
    {
        // Reset the timer.
        Timer = 0f;

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
        gunLine.SetPosition(0, transformPosition);

        if (centerCannon)
        {
            shootRay = centerCannonRay;
        }
        else
        {
            shootRay.origin = transformPosition;
            shootRay.direction = transform.forward;
        }

        // Perform the raycast against gameobjects on the shootable layer and if it hits something...
        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            var carManager = shootHit.collider.transform.root.GetComponent<MultiplayerCarManager>();

            if (carManager != null && shooter != carManager.PlayerId)
            {
                // Try and find an EnemyHealth script on the gameobject hit.
                Health enemyHealth = shootHit.collider.GetComponentInParent<Health>();

                // If the EnemyHealth component exist...
                if (enemyHealth != null)
                {
                    // ... the enemy should take damage.
                    if (enemyHealth.currentHealth > 0)
                    {
                        enemyHealth.TakeDamage(damagePerShot, shooter);
                    }
                }
            }

            // Set the second position of the line renderer to the point the raycast hit.
            gunLine.SetPosition(1, shootHit.point);
        }
        // If the raycast didn't hit anything on the shootable layer...
        else
        {
            if (centerCannon)
            {
                // ... set the second position of the line renderer to the fullest extent of the gun's range.
                gunLine.SetPosition(1, worldPosition);
            }
            else
            {
                // ... set the second position of the line renderer to the fullest extent of the gun's range.
                gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
            }
        }

        StartCoroutine(DisableEfectsCoroutine());
    }

    public void DisableEffects()
    {
        // Disable the line renderer and the light.
        gunLine.enabled = false;
        faceLight.enabled = false;
        gunLight.enabled = false;
    }

    public IEnumerator DisableEfectsCoroutine()
    {
        yield return new WaitForSeconds(timeBetweenBullets * effectsDisplayTime);
        DisableEffects();
    }

}

public enum Cannontype
{
    staticCannon,
    mobileCannon
}