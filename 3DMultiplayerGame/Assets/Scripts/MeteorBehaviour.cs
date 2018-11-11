using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeteorBehaviour : MonoBehaviour {


    public LayerMask ExplosionLayer;
    public AudioClip ExplosionSound;

    private Rigidbody _rigidbody;
    private AudioSource[] _audioSources;
    private float _timeToExplode = 10f;
    private float _timeCount = 0;

    private void Awake()
    {
        _audioSources = GetComponents<AudioSource>();
    }

    private void Start ()
    {
        //gravity = gameObject.AddComponent<ConstantForce>();
        //gravity.force = new Vector3(0.0f, -100000f, 0.0f);
    }

    private void Update()
    {
        _timeCount += Time.deltaTime;
        if(_timeCount >= _timeToExplode)
        {
            _timeCount = 0;
            transform.position = new Vector3(847f, 181f, -192f);
            gameObject.SetActive(false);
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
            //var explosion = GameObject.FindWithTag("Explosion").GetComponent<ExplosionSpawner>();
            //PlayExplosionSound();
            //explosion.Explode(transform.position);

            //var components = GetComponentsInChildren<MeshRenderer>();
            //foreach (var component in components)
            //{
            //    component.enabled = false;
            //}

            var hit = collision.gameObject;
            var health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(100);
            }

            Destroy(gameObject, 2f);
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
}
