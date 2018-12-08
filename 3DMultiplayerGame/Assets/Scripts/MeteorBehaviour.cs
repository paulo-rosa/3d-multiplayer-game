﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeteorBehaviour : MonoBehaviour {


    public LayerMask ExplosionLayer;
    public AudioClip ExplosionSound;
    public ParticleSystem Explosion;
    private Rigidbody _rigidbody;
    private AudioSource[] _audioSources;
    private float _timeToExplode = 10f;
    private float _timeCount = 0;
    private Health _health;
    private ScoreGiver _scoreGiver;

    private void Awake()
    {
        _audioSources = GetComponents<AudioSource>();
        _health = GetComponent<Health>();
        _scoreGiver = GetComponent<ScoreGiver>();
    }


    private void OnEnable()
    {
         _health.OnDie += OnMeteorDie;
    }
    //private void OnDisable()
    //{
    //    _health.onDie -= OnMeteorDie;
    //}

    private void OnMeteorDie()
    {
        Debug.Log("morreu");
        DisableObject();
        _health.SetAlive(true);
        _scoreGiver.GiveScore();
    }



    private void Update()
    {
        _timeCount += Time.deltaTime;
        if(_timeCount >= _timeToExplode)
        {
            _timeCount = 0;
            DisableObject();
        }
    }
    public void Init(Vector3 dir)
    {
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().AddForce(dir * 100, ForceMode.Force);
        GetComponent<Rigidbody>().velocity = dir;
        GetComponent<Rigidbody>().angularVelocity = new Vector3(5,5,5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Utils.CompareLayer(ExplosionLayer, collision.gameObject.layer))
        {

            var hit = collision.gameObject;
            var health = hit.GetComponent<Health>();
            
            if (health != null)
            {
                health.TakeDamage(100);
            }

            DisableObject();
        }
    }

    private void DisableObject()
    {
        gameObject.SetActive(false);
        //Explosion.Play();
        transform.position = new Vector3(847f, 181f, -192f);
    }
    public void PlayExplosionSound()
    {
        var audioSource = _audioSources.Where(a => a.clip == ExplosionSound).FirstOrDefault();
        audioSource.volume = 1.0f;
        audioSource.clip = ExplosionSound;
        audioSource.loop = false;
        audioSource.Play();
    }
}
