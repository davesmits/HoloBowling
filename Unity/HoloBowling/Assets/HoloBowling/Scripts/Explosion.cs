using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private AudioSource _audioSource;
    private Rigidbody _rigidBody;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
        _rigidBody = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        //_rigidBody.AddForce(Vector3.up * 7, ForceMode.Acceleration);
    }

    public void Explode()
    {
        _particleSystem.Play();
        _audioSource.Play();
        Destroy(gameObject, _particleSystem.main.duration);
    }


    // Grenade explodes on impact.
    private void OnCollisionEnter(Collision coll)
    {
        for (int index = 0; index < this.transform.childCount; index++)
        {
            var gameObject = this.transform.GetChild(index).gameObject;
            gameObject.SetActive(false);
        }
        Explode();
    }

}
