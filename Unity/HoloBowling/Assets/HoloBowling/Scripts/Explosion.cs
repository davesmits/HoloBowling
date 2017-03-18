using HoloToolkit.Unity.SpatialMapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private bool _exploded;
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
        if (coll.gameObject.GetComponentInParent<TapToPlace>() != null)
        {
            // Delete all children, so the bomb is hidden when collided
            for (int index = 0; index < this.transform.childCount; index++)
            {
                var gameObject = this.transform.GetChild(index).gameObject;
                gameObject.SetActive(false);
            }

            // Trigger the explosion
            Explode();
        }
    }    

    // Bomb explodes on impact.
    public void Explode()
    {
        if (!_exploded)
        {
            _exploded = true;

            // Start the particle system
            _particleSystem.Play();

            // Start playing the audio clip
            _audioSource.Play();

            // Destroy object after audio clip has finished
            Destroy(gameObject, _audioSource.clip.length);
        }
    }

}
