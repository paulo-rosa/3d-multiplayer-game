using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GameSound : MonoBehaviour
{
    public AudioClip InGameSound;
    private AudioSource _audioSource;

    private static GameSound _instance;

    public static GameSound Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameSound>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        _audioSource.clip = InGameSound;
        PlayInGameSound();
    }

    public void PlayInGameSound()
    {
        _audioSource.volume = 1f;
        _audioSource.clip = InGameSound;
        _audioSource.loop = true;
        _audioSource.Play();
    }
}