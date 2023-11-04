using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private ParticleSystem fireParticles;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        fireParticles.maxParticles -= 20;
    }
}
