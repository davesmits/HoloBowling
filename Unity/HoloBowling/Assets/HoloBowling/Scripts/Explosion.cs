using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private AudioSource _audioSource;
    private Rigidbody _rigidBody;

    // Use this for initialization
    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
        _rigidBody = GetComponent<Rigidbody>();
    }
    
    // Get notified when collision detected for the Bomb
    void OnCollisionEnter(Collision coll)
    {
        for (int index = 0; index < this.transform.childCount; index++)
        {
            var gameObject = this.transform.GetChild(index).gameObject;
            gameObject.SetActive(false);
        }
        Explode();
    }    

    // Bomb explodes on impact.
    public void Explode()
    {
        _particleSystem.Play();
        _audioSource.Play();
        Destroy(gameObject, _audioSource.clip.length);
    }

}
