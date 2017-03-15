using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public void Explode()
    {
        var exp = GetComponent<ParticleSystem>();
        exp.Play();
        Destroy(gameObject, exp.main.duration);
    }


    // Grenade explodes on impact.
    private void OnCollisionEnter(Collision coll)
    {
        Explode();
    }

}
