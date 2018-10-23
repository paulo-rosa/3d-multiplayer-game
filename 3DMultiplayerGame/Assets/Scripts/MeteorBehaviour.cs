using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeteorBehaviour : MonoBehaviour {


    private Rigidbody _rigidbody;
    public LayerMask ExplosionLayer;
    public ConstantForce gravity;
    public AudioClip ExplosionSound;
    private AudioSource[] _audioSources;

    private void Awake()
    {
        _audioSources = GetComponents<AudioSource>();
    }

    private void Start ()
    {
        gravity = gameObject.AddComponent<ConstantForce>();
        gravity.force = new Vector3(0.0f, -100000f, 0.0f);
    }
	
	public void Init(Vector3 dir)
    {
        gameObject.GetComponent<Rigidbody>().useGravity = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((ExplosionLayer | (1 << collision.gameObject.layer)) == ExplosionLayer)
        {
            var explosion = GameObject.FindWithTag("Explosion").GetComponent<ExplosionSpawner>();
            PlayExplosionSound();
            explosion.Explode(transform.position);

            var components = GetComponentsInChildren<MeshRenderer>();
            foreach (var component in components)
            {
                component.enabled = false;
            }

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
