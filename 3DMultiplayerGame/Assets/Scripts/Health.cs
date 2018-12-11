using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{
    public int maxHealth;
    public bool destroyOnDeath;
    public AudioClip ExplosionSound;
    public AudioClip GotShotSound;
    private AudioSource[] _audioSources;

    [SyncVar(hook = "OnUpdateHealth")]
    public int currentHealth;

    public RectTransform healthBar;

    private NetworkStartPosition[] spawnPoints;
    private GameManager _gameManager;
    private GameObject _owner;
    private bool _isAlive = true;

    public event Action OnDie;
    public event Action OnHealthChange;

    private void Awake()
    {
        _audioSources = GetComponents<AudioSource>();
    }

    private void Start()
    {
        if (hasAuthority)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
        _gameManager = GameManager.Instance;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (hasAuthority)
        {
            CmdTakeHealth(amount);
        }
        return;
    }

    [Command]
    private void CmdTakeHealth(int amount)
    {
        RpcTakeHealth(amount);
    }

    [ClientRpc]
    private void RpcTakeHealth(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
    }

    private void OnUpdateHealth(int health)
    {
        if (healthBar == null)
            return;

        currentHealth = health;

        if (OnHealthChange != null)
        {
            OnHealthChange();
        }
    }

    public void OnHealthChanged()
    {
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            _isAlive = false;
            var explosion = GameObject.FindWithTag("Explosion").GetComponent<ExplosionSpawner>();
            PlayExplosionSound();
            explosion.Explode(transform.position);

            if (OnDie != null)
            {
                OnDie();
            }

            if (destroyOnDeath)
            {
                var canvas = GetComponentsInChildren<Canvas>();

                foreach (var item in canvas)
                {
                    item.enabled = false;
                }

                var components = GetComponentsInChildren<MeshRenderer>();

                foreach (var component in components)
                {
                    component.enabled = false;
                }

                Destroy(gameObject, ExplosionSound.length);
            }
            else
            {

            }
        }
        else
        {
            PlayGotShotSound();
        }
    }

    private void UpdateHealthUI()
    {
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        _isAlive = true;
    }

    //Respawn precisa mudar de lugar
    [ClientRpc]
    void RpcRespawn()
    {
        currentHealth = maxHealth;

        if (hasAuthority)
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
        var audioSource = _audioSources.Where(a => a.clip == ExplosionSound).FirstOrDefault();
        audioSource.volume = 1.0f;
        audioSource.clip = ExplosionSound;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayGotShotSound()
    {
        var audioSource = _audioSources.Where(a => a.clip == GotShotSound).FirstOrDefault();
        audioSource.volume = 1.0f;
        audioSource.clip = GotShotSound;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void SetAlive(bool value)
    {
        if (value == true)
        {
            _isAlive = value;
            currentHealth = maxHealth;
        }
    }
}