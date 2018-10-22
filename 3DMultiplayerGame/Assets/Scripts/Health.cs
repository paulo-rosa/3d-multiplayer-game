using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{
    public const int maxHealth = 100;
    public bool destroyOnDeath;
    public AudioClip ExplosionSound;
    public AudioClip GotShotSound;
    private AudioSource _audioSource;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    public RectTransform healthBar;

    private NetworkStartPosition[] spawnPoints;
    private GameManager _gameManager;
    private GameObject _owner;

    public event Action onDie;

    void Start()
    {
        if (isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
        _gameManager = GameManager.Instance;
        
    }
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            var explosion = GameObject.FindWithTag("Explosion").GetComponent<ExplosionSpawner>();
            PlayExplosionSound();
            explosion.Explode(transform.position);

            if (destroyOnDeath)
            {
                GetComponentInChildren<Canvas>().enabled = false;
                var components = GetComponentsInChildren<MeshRenderer>();
                foreach (var component in components)
                {
                    component.enabled = false;
                }

                Destroy(gameObject, ExplosionSound.length);
            }
            else
            {
                currentHealth = maxHealth;

                // called on the Server, invoked on the Clients
                RpcRespawn();

                //
                onDie();
                //
            }
        }
        else
        {
            PlayGotShotSound();
        }
    }

    void OnChangeHealth(int currentHealth)
    {
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }

    //Respawn precisa mudar de lugar
    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            // Set the spawn point to origin as a default value
            Vector3 spawnPoint = Vector3.zero;

            // If there is a spawn point array and the array is not empty, pick one at random
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].transform.position;
            }

            // Set the player’s position to the chosen spawn point
            transform.position = spawnPoint;
        }
    }

    public void PlayExplosionSound()
    {
        _audioSource.volume = 1.0f;
        _audioSource.clip = ExplosionSound;
        _audioSource.loop = false;
        _audioSource.Play();
    }

    public void PlayGotShotSound()
    {
        _audioSource.volume = 1.0f;
        _audioSource.clip = GotShotSound;
        _audioSource.loop = false;
        _audioSource.Play();
    }
}